using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using web.Models.User;
using woo.data.entity;

namespace web.Api
{
    public class UserController : BaseApiController
    {
        // GET api/<controller>
        public IEnumerable<User> Get()
        {
            return DataContext.User;
        }

        public object Get(UserGetModel m)
        {
            if (m.EventId != null)
            {
                Event event_ = (from e in DataContext.Event
                                where e.Id == m.EventId
                                select e).FirstOrDefault();
                return event_.Users;
            }

            return (from u in DataContext.User
                    where u.Id == m.Id
                    select u).FirstOrDefault();

            //if(eventId)
        }


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