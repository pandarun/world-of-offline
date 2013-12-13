using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using core.Business;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Microsoft.ServiceBus;
using ServiceBusHub;

namespace web.Controllers
{
    public class SearchController : ApiController
    {
        private readonly IHub _hub;
        private readonly ISearchReaderService _searchReaderService;

        public SearchController(IHub hub, ISearchReaderService searchReaderService)
        {
            _hub = hub;
            _searchReaderService = searchReaderService;
        }

        public object Get(int id)
        {
            var query = new BooleanQuery();
            
            query.Add(new Lucene.Net.Search.WildcardQuery(new Term("name", id.ToString() + "*")), Occur.MUST);
            query.Add(new TermQuery(new Term("id", "web.Controllers.SearchPutController+Model " + id.ToString())), Occur.SHOULD);

            SearchResult searchResult = _searchReaderService.GetSearchResult(query, new QueryWrapperFilter(query), 0, 20, new Sort());
            return searchResult;
        }
    }
}
