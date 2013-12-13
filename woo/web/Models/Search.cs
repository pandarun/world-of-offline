using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using ServiceBusHub;
using Lucene.Net.Search;
using Lucene.Net.Store;
using System.Globalization;
using System.Timers;
using Lucene.Net.Store.Azure;
using Filter = Lucene.Net.Search.Filter;
using Version = Lucene.Net.Util.Version;

namespace core.Business
{
    public interface ISearchReaderService : IDisposable
    {
        SearchResult GetSearchResult(Query query, Filter filter, int skip, int take, Sort sort);
    }

    public interface ISearchWriterService : IDisposable
    {
        bool TryEnter();
        void Release();

        void Update(object item);
        void Remove(object item);
    }

    public class GenericSearchItem
    {
        private static readonly Dictionary<string, Type> TypeMap = AppDomain.CurrentDomain.GetAssemblies()
                                                                    .SelectMany(a => a.GetTypes())
                                                                    .GroupBy(t => t.FullName)
                                                                    .ToDictionary(grouping => grouping.Key, grouping => grouping.First());

        public GenericSearchItem(object body)
        {
            Body = body;
            Type = body.GetType().FullName;
        }

        public string Type { get; set; }
        public object Body { get; set; }

        public object GetBody()
        {
            var body = JsonConvert.SerializeObject(Body);
            return JsonConvert.DeserializeObject(body, TypeMap[Type]);
        }
    }

    public class SearchResultItem
    {
        public string Link { get; set; }
        public float Score { get; set; }
    }
    public class SearchResult
    {
        public SearchResultItem[] Links { get; set; }
        public int Total { get; set; }
        public int ElapsedMilliseconds { get; set; }
    }

    internal class SearchSettings
    {
        public const Lucene.Net.Util.Version LuceneVersion = Lucene.Net.Util.Version.LUCENE_30;

        // ReSharper disable InconsistentNaming
        public const string Field_ID = "id";
        public const string Field_Permissions = "permissions";
        public const string Field_Parent = "parent";
        // ReSharper restore InconsistentNaming
    }

    public class SearchReaderService : ISearchReaderService
    {
        private readonly Directory _luceneDirectory;
        private readonly IndexReader _luceneReader;
        private readonly IndexSearcher _luceneSearcher;

        public SearchReaderService(Directory luceneDirectory)
        {
            _luceneDirectory = luceneDirectory;
            _luceneReader = IndexReader.Open(_luceneDirectory, true);
            _luceneSearcher = new IndexSearcher(_luceneReader);
        }

        public void Dispose()
        {
            _luceneSearcher.Dispose();
            _luceneReader.Dispose();
        }

        public SearchResult GetSearchResult(Query query, Filter filter, int skip, int take, Sort sort)
        {
            var s = Stopwatch.StartNew();
            var searchResult = _luceneSearcher.Search(query, filter, skip + take);
            s.Stop();
            return new SearchResult
            {
                Links = searchResult.ScoreDocs
                                    .Skip(skip)
                                    .Take(take)
                                    .Select(storeDoc => new SearchResultItem { Link = _luceneSearcher.Doc(storeDoc.Doc).GetField(SearchSettings.Field_ID).StringValue, Score = storeDoc.Score })
                                    .ToArray(),
                Total = searchResult.TotalHits,
                ElapsedMilliseconds = (int)s.ElapsedMilliseconds
            };
        }

    }

    public class SearchWriterService : ISearchWriterService
    {
        private const string ObjectTermPattern = "{0} {1}";

        private readonly Directory _luceneDirectory;
        private IndexWriter _luceneWriter;

        private volatile bool _hasLease;

        private readonly bool _enableNearRealTimeCache;
        private readonly Timer _timer;
        private readonly object _sync = new object();
        private volatile int _entriesToCommit = 0;

        public SearchWriterService(Directory luceneDirectory, bool enableNearRealTimeCache = false)
        {
            _luceneDirectory = luceneDirectory;
            _enableNearRealTimeCache = enableNearRealTimeCache;

            if (enableNearRealTimeCache)
            {
                _timer = new Timer(TimeSpan.FromSeconds(10).TotalMilliseconds);
                _timer.Elapsed += TimerOnElapsed;
                _timer.Start();
            }
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            lock (_sync)
            {
                if (_entriesToCommit > 0)
                {
                    _luceneWriter.Commit();
                    _entriesToCommit = 0;
                }
            }
        }

        public bool TryEnter()
        {
            if (_hasLease)
            {
                return true;
            }

            try
            {
                _luceneWriter = new IndexWriter(_luceneDirectory, new StandardAnalyzer(SearchSettings.LuceneVersion), Lucene.Net.Index.IndexWriter.MaxFieldLength.UNLIMITED);
                _hasLease = true;

                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Release()
        {
            if (_hasLease)
            {
                if (_enableNearRealTimeCache)
                {
                    _timer.Stop();
                    _timer.Dispose();

                    TimerOnElapsed(null, null);
                }

                _hasLease = false;

                _luceneWriter.Dispose();
                _luceneWriter = null;
            }
        }

        internal static Lucene.Net.Documents.Document GetDocument(object source)
        {
            var type = source.GetType();
            var sourceId = (int)type.GetProperties().First(p => p.Name.ToLower() == SearchSettings.Field_ID).GetValue(source);
            var id = string.Format(ObjectTermPattern, type.FullName, sourceId);

            var document = new Lucene.Net.Documents.Document();

            document.Add(new Field(SearchSettings.Field_ID, id, Field.Store.YES, Field.Index.NOT_ANALYZED));

            foreach (var prop in type.GetProperties().Where(p => p.Name.ToLower() != SearchSettings.Field_ID))
            {
                if (prop.PropertyType == typeof(DateTime))
                {
                    document.Add(new NumericField(prop.Name.ToLower(), Field.Store.NO, true).SetLongValue(((DateTime)prop.GetValue(source)).ToBinary()));
                }

                if (prop.PropertyType == typeof(int))
                {
                    document.Add(new NumericField(prop.Name.ToLower(), Field.Store.NO, true).SetIntValue((int)prop.GetValue(source)));
                }

                if (prop.PropertyType == typeof(double))
                {
                    document.Add(new NumericField(prop.Name.ToLower(), Field.Store.NO, true).SetDoubleValue((double)prop.GetValue(source)));
                }

                if (prop.PropertyType == typeof(string))
                {
                    var value = (string)prop.GetValue(source);

                    if (!string.IsNullOrEmpty(value))
                    {
                        document.Add(new Field(prop.Name.ToLower(), value, Field.Store.NO, Field.Index.ANALYZED));
                    }
                }
            }

            return document;
        }

        internal static Lucene.Net.Index.Term GetTerm(object source)
        {
            var type = source.GetType();
            var sourceId = (int)type.GetProperties().First(p => p.Name.ToLower() == SearchSettings.Field_ID).GetValue(source);
            var id = string.Format(ObjectTermPattern, type.FullName, sourceId);

            var term = new Term(SearchSettings.Field_ID, id);

            return term;
        }

        public void Update(object item)
        {
            if (!TryEnter())
            {
                return;
            }

            _luceneWriter.UpdateDocument(GetTerm(item), GetDocument(item));

            if (_enableNearRealTimeCache)
            {
                lock (_sync)
                {
                    _entriesToCommit++;
                }
            }
            else
            {
                _luceneWriter.Commit();
            }
        }

        public void Remove(object item)
        {
            if (!TryEnter())
            {
                return;
            }

            _luceneWriter.DeleteDocuments(GetTerm(item));

            if (_enableNearRealTimeCache)
            {
                lock (_sync)
                {
                    _entriesToCommit++;
                }
            }
            else
            {
                _luceneWriter.Commit();
            }
        }

        public void Dispose()
        {
            Release();
        }
    }


    public interface IIntermediateReaderService : IDisposable
    {
    }

    public class IntermediateReaderService : IIntermediateReaderService
    {
        private readonly bool _enableNearRealTimeCache;
        private IDisposable _termprorarySubscription;
        private IndexWriter _writer;
        private IDisposable _subscription;
        private const int CacheInSeconds = 5;
        private static readonly object Sync = new object();
        private static int HasEntitites = 0;
        private Timer _timer;

        class DiposableArray : IDisposable
        {
            private readonly IEnumerable<IDisposable> _disposables;

            public DiposableArray(IEnumerable<IDisposable> disposables)
            {
                _disposables = disposables;
            }

            public void Dispose()
            {
                if (_disposables != null)
                {
                    foreach (var disposable in _disposables)
                    {
                        disposable.Dispose();
                    }
                }
            }
        }

        public IntermediateReaderService(AzureDirectory masterDirectory, IHub hub, Directory cacheDirectory, bool enableNearRealTimeCache = false)
        {
            _enableNearRealTimeCache = enableNearRealTimeCache;
            var topicName = "lucene";
            var subscriptionName = "i_" + Guid.NewGuid().ToString().Replace("-", string.Empty);
            
            hub.CreateSubscription(topicName, subscriptionName)
               .ContinueWith(t =>
               {
                   _termprorarySubscription = t.Result;

                   Directory.Copy(masterDirectory, cacheDirectory, false);

                   _writer = new IndexWriter(cacheDirectory, new StandardAnalyzer(Version.LUCENE_30), Lucene.Net.Index.IndexWriter.MaxFieldLength.UNLIMITED);

                   var subscriptions = Enumerable.Range(0, 10).Select(_ => hub.SubscribeWithRoutingKey(topicName, subscriptionName, Message)).ToArray();

                   return Task.WhenAll(subscriptions)
                       .ContinueWith(_ => new DiposableArray(subscriptions.Select(s => s.Result)) as IDisposable);
               })
               .Unwrap()
               .ContinueWith(t =>
               {
                   _subscription = t.Result;

                   if (_enableNearRealTimeCache)
                   {
                       _timer = new Timer(CacheInSeconds*1000);
                       _timer.Elapsed += TimerOnElapsed;
                       _timer.Start();
                   }
               })
               .Wait();

        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            lock (Sync)
            {
                if (HasEntitites > 0)
                {
                    _writer.Commit();
                    HasEntitites = 0;
                }
            }
        }

        private BrokeredMessageResults Message(GenericPayloadDeliverInfo genericPayloadDeliverInfo)
        {
            var body = genericPayloadDeliverInfo.Payload.GetBody<GenericSearchItem>().GetBody();

            _writer.UpdateDocument(SearchWriterService.GetTerm(body), SearchWriterService.GetDocument(body));
            if (!_enableNearRealTimeCache)
            {
                _writer.Commit();
            }
            else
            {
                lock (Sync)
                {
                    HasEntitites++;
                }
            }

            return BrokeredMessageResults.NoMatter;
        }

        public void Dispose()
        {
            if (_subscription != null)
            {
                _subscription.Dispose();
            }

            if (_writer != null)
            {
                _writer.Dispose();
            }

            if (_termprorarySubscription != null)
            {
                _termprorarySubscription.Dispose();
            }

            if (_timer != null)
            {
                _timer.Dispose();
            }
        }
    }
}

