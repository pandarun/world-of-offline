using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace woo.data.entity
{
    public class User
    {
        public int Id { get; set; }
        public string UniqueName { get; set; }
        public string Provider { get; set; }
        public string ImageUrl { get; set; }
        public virtual ICollection<Event> Events { get; set; }

        public virtual ICollection<Activity> Activities { get; set; }
    }
}
