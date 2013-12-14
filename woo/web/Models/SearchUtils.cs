using core.Business;
using ServiceBusHub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using woo.data.entity;

namespace web.Models
{
    public interface ISearchUtils
    {
        void IndexEvent(Event ev, woo.data.entity.User[] us);
    }

    public class SearchUtils : ISearchUtils
    {
        private readonly IHub _hub;

        public SearchUtils(IHub hub)
        {
            _hub = hub;
        }

        public class EventSearchIndex
        {
            public int Id { get; set; }
            public string Summary { get; set; }

            public long Time { get; set; }

            public string Users { get; set; }
        }

        public void IndexEvent(Event ev, woo.data.entity.User[] us)
        {
            var eventIndex = new EventSearchIndex
            {
                Id = ev.Id,
                Summary = ev.Summary,
                Time = ev.Since.ToBinary(),
                Users = string.Join(" ", us.Select(u => u.Id.ToString()))
            };

            var genericSearchItem = new GenericSearchItem(eventIndex);
            _hub.SendToTopicAsync("lucene", "any", genericSearchItem);
        }
    }
}