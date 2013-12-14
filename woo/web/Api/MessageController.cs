using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using woo.data.entity;

namespace web.Api
{
    public class MessageController : BaseApiController
    {
        // GET api/<controller>
        public IEnumerable<Message> Get(int eventId)
        {
            return from m in DataContext.Message
                   where m.EventId == eventId
                   select m;
         }

        //// GET api/<controller>/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}