using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using core.Business;
using Lucene.Net.Index;
using Lucene.Net.Search;

namespace web.Models
{
    public interface IReadSearchUtils
    {
        int[] FindEvents(string q, int skip, int take, bool strict = false);
        int[] FindEventsForUser(int user, int skip, int take, bool strict = false);
    }

    public class ReadSearchUtils : IReadSearchUtils
    {
        private readonly ISearchReaderService _readerService;

        public ReadSearchUtils(ISearchReaderService readerService)
        {
            _readerService = readerService;
        }

        public int[] FindEvents(string q, int skip, int take, bool strict = false)
        {
            var type = typeof (SearchUtils.EventSearchIndex);

            var query = new BooleanQuery();

            query.Add(new Lucene.Net.Search.WildcardQuery(new Term("id", type.FullName + "*")), Occur.MUST);
            query.Add(new WildcardQuery(new Term("summary", q + "*")), strict ? Occur.MUST : Occur.SHOULD);

            SearchResult searchResult = _readerService.GetSearchResult(query, new QueryWrapperFilter(query), skip, take);
            return searchResult
                .Links
                .Select(link => link.Link.Split(' ')[1])
                .Select(id => int.Parse(id))
                .ToArray();
        }

        public int[] FindEventsForUser(int user, int skip, int take, bool strict = false)
        {
            var type = typeof(SearchUtils.EventSearchIndex);

            var query = new BooleanQuery();

            query.Add(new Lucene.Net.Search.WildcardQuery(new Term("id", type.FullName + "*")), Occur.MUST);
            query.Add(new TermQuery(new Term("users", user.ToString())), strict ? Occur.MUST : Occur.SHOULD);

            var searchResult = _readerService.GetSearchResult(query, new QueryWrapperFilter(query), skip, take);
            return searchResult
                .Links
                .Select(link => link.Link.Split(' ')[1])
                .Select(id => int.Parse(id))
                .ToArray();
        }
    }
}