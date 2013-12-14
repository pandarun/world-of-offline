using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using woo.data.entity;

namespace web.Api
{
    public class EventController : BaseApiController
    {
        // GET api/<controller>
        public IEnumerable<Event> Get()
        {
            return DataContext.Event;
        }

        public Event Get(string eventText)
        {
            return DataContext.Event.FirstOrDefault();
        }

        // GET api/<controller>/5
        public Event Get(int id)
        {
            return (from e in DataContext.Event
                   where e.Id == id
                   select e).FirstOrDefault();
        }

        // create event
        public void Post([FromBody]Event e)
        {
            DataContext.Event.Add(e);
        }

        /// <summary>
        /// add user to event
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="eventId"></param>
        public void Post([FromBody]int userId, [FromBody]int eventId)
        {
            User user = (from u in DataContext.User
                         where u.Id == userId
                         select u).FirstOrDefault();
            Event event_ = (from e in DataContext.Event
                            where e.Id == eventId
                            select e).FirstOrDefault();

            event_.Users.Add(user);
            user.Events.Add(event_);
            DataContext.SaveChanges();

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