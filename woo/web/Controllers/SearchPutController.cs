using System.Threading.Tasks;
using core.Business;
using ServiceBusHub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace web.Controllers
{
    public class SearchPutController : ApiController
    {
        private readonly IHub _hub;

        public SearchPutController(IHub hub)
        {
            _hub = hub;
        }

        public class Model
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
        public async Task<Model> Get(int id)
        {
            var model = new Model
            {
                Id = id,
                Name = id.ToString()
            };

            await _hub.SendToTopicAsync("lucene", "any", new GenericSearchItem(model));


            return model;
        }
    }
}
