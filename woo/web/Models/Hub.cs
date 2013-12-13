using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ServiceBusHub
{
    public enum BrokeredMessageResults
    {
        NoMatter = 0,
        Ack,
        Reject,
        Dead
    }

    public class GenericPayloadDeliverInfo
    {
        public GenericPayload Payload { get; set; }
        public BrokeredMessage BrokeredMessage { get; set; }
    }

    public class GenericPayload
    {
        public string RoutingKey { get; set; }
        public string Payload { get; set; }

        public T GetBody<T>()
        {
            return JsonConvert.DeserializeObject<T>(Payload);
        }
    }

    public interface IHub
    {
        Task<IDisposable> SubscribeWithRoutingKey(string topicName, string subscriptionName, Func<GenericPayloadDeliverInfo, BrokeredMessageResults> message, ReceiveMode receiveMode = ReceiveMode.ReceiveAndDelete);
        Task SendToTopicAsync<T>(string topicName, string routingKey, T payload);
        Task<IDisposable> CreateSubscription(string topic, string subscriptionName);
    }

    public sealed class Hub : IHub
    {
        private readonly string _connectionString;

        public Hub(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Task<IDisposable> SubscribeWithRoutingKey(string topicName, string subscriptionName, Func<GenericPayloadDeliverInfo, BrokeredMessageResults> message, ReceiveMode receiveMode = ReceiveMode.ReceiveAndDelete)
        {
            return Subscribe(topicName,
                subscriptionName,
                brokeredMessage =>
                {
                    var body = new GenericPayloadDeliverInfo { BrokeredMessage = brokeredMessage, Payload = brokeredMessage.GetBody<GenericPayload>() };
                    return message(body);
                }, receiveMode);
        }

        private Task<IDisposable> Subscribe(string topicName, string subscriptionName, Func<BrokeredMessage, BrokeredMessageResults> message, ReceiveMode receiveMode = ReceiveMode.ReceiveAndDelete)
        {
            var sc = GetFactoryAsync()
                .ContinueWith(t => t.Result.CreateSubscriptionClient(topicName, subscriptionName, receiveMode))
                .ContinueWith(st =>
                {
                    st.Result.OnMessageAsync(m =>
                    {
                        Trace(topicName, "Got message " + JsonConvert.SerializeObject(m.MessageId));

                        return Task.Run(() => message(m))
                            .ContinueWith(t =>
                            {
                                var result = t.IsFaulted || t.IsCanceled || t.Result == BrokeredMessageResults.Dead ?
                                                 BrokeredMessageResults.Dead :
                                                 t.Result == BrokeredMessageResults.Reject ?
                                                     BrokeredMessageResults.Reject :
                                                     BrokeredMessageResults.Ack;

                                Trace(topicName, "Result for message " + m.MessageId + " is " + result);

                                return result == BrokeredMessageResults.Dead ?
                                           m.DeadLetterAsync() :
                                           result == BrokeredMessageResults.Reject ?
                                               m.AbandonAsync() :
                                               m.CompleteAsync();
                            })
                            .Unwrap();
                    });

                    return st.Result;
                })
                .ContinueWith(t => new Subscription(t) as IDisposable);

            return sc;
        }

        private class Subscription : IDisposable
        {
            private volatile SubscriptionClient _subscription;

            public Subscription(Task<SubscriptionClient> sTask)
            {
                sTask.ContinueWith(t => _subscription = t.Result);
            }

            public void Dispose()
            {
                var s = _subscription;
                s.Close();
            }
        }

        public Task SendToTopicAsync<T>(string topicName, string routingKey, T payload)
        {
            var json = JsonConvert.SerializeObject(payload, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver(), ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

            return SendToTopicAsync(topicName, new BrokeredMessage(new GenericPayload
            {
                RoutingKey = routingKey,
                Payload = json
            }));
        }

        private void Trace(string topic, string message)
        {
            if (!topic.ToLower().Contains("logs"))
            {
                NLog.LogManager.GetCurrentClassLogger().Trace(message);
            }
        }
        private Task SendToTopicAsync(string topicName, BrokeredMessage message)
        {
            return GetFactoryAsync()
                .ContinueWith(t => GetSender(t.Result, topicName))
                .Unwrap()
                .ContinueWith(t =>
                {
                    Trace(topicName, string.Format("Sending message to topic {0}", topicName));

                    return t.Result.SendAsync(message);
                })
                .Unwrap();
        }

        private readonly ConcurrentDictionary<string, Task<MessageSender>> _senders = new ConcurrentDictionary<string, Task<MessageSender>>();
        Task<MessageSender> GetSender(MessagingFactory factory, string topicName)
        {
            var sender = _senders.GetOrAdd(topicName, factory.CreateMessageSenderAsync);

            return sender
                .ContinueWith(t =>
                {
                    if (t.IsFaulted || t.Result.IsClosed)
                    {
                        sender = factory.CreateMessageSenderAsync(topicName);
                        _senders[topicName] = sender;
                    }

                    return _senders[topicName];
                })
                .Unwrap();
        }

        private volatile MessagingFactory _currentFactory;
        private TaskCompletionSource<MessagingFactory> _factoryCompletionSource;

        public Task<IDisposable> CreateSubscription(string topic, string subscriptionName)
        {
            var manager = NamespaceManager.CreateFromConnectionString(_connectionString);

            return manager.CreateSubscriptionAsync(topic, subscriptionName)
                          .ContinueWith(created => new DisposableTemprorarySubscription(manager, created.Result) as IDisposable);
        }

        private class DisposableTemprorarySubscription : IDisposable
        {
            private NamespaceManager manager;
            private SubscriptionDescription created;

            public DisposableTemprorarySubscription(NamespaceManager manager, SubscriptionDescription created)
            {
                this.manager = manager;
                this.created = created;
            }

            public void Dispose()
            {
                manager.DeleteSubscription(created.TopicPath, created.Name);
            }
        }

        Task<MessagingFactory> GetFactoryAsync()
        {
            var factory = _currentFactory;

            if (factory == null || factory.IsClosed)
            {
                var tcs = System.Threading.Interlocked.CompareExchange(ref _factoryCompletionSource, new TaskCompletionSource<MessagingFactory>(), null);

                if (tcs == null)
                {
                    tcs = _factoryCompletionSource;

                    if (factory != null)
                    {
                        factory.Close();
                    }

                    factory = MessagingFactory.CreateFromConnectionString(_connectionString);

                    _currentFactory = factory;

                    tcs.SetResult(_currentFactory);

                    System.Threading.Interlocked.CompareExchange(ref _factoryCompletionSource, null, tcs);
                }

                return tcs.Task;
            }

            return Task.FromResult(factory);
        }

    }

}