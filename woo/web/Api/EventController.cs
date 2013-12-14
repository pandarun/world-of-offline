using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using web.Models;
using web.Models.Event;
using woo.data.entity;

namespace web.Api
{
    public class EventController : BaseApiController
    {

        IReadSearchUtils _readSerachUtils;
        ISearchUtils _searchUtils;

        public EventController(IReadSearchUtils readSerachUtils, ISearchUtils searchUtils)
        {
            _readSerachUtils = readSerachUtils;
            _searchUtils = searchUtils;
        }

        // GET api/<controller>
        public IEnumerable<Event> Get()
        {
            return DataContext.Event;
        }

        public IEnumerable<Event> Get(string eventText, int skip, int take)
        {
            int[] eventIds = _readSerachUtils.FindEvents(eventText, skip, take);

            return   from e in DataContext.Event
                      where eventIds.Contains(e.Id)
                      select e;
            
        }

        // GET api/<controller>/5
        public IEnumerable<Event> Get(int eventId)
        {
                return from e in DataContext.Event
                        where e.Id == eventId
                        select e;
        }

        //events for user
        public IEnumerable<Event> Get(int userId, int skip, int take)
        {
            int[] eventIds = _readSerachUtils.FindEventsForUser(userId, skip, take);
            return from e in DataContext.Event
                   where eventIds.Contains(e.Id)
                   select e;
        }

        // create event
        public void Post([FromBody]Event e)
        {
            DataContext.Event.Add(e);
            DataContext.SaveChanges();
            _searchUtils.IndexEvent(e, e.Users.ToArray());
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
            _searchUtils.IndexEvent(event_, event_.Users.ToArray());
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