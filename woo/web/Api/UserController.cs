using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
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

        // GET api/<controller>/5
        public User Get(int id)
        {
            return (from u in DataContext.User
                   where u.Id == id
                   select u).FirstOrDefault();
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