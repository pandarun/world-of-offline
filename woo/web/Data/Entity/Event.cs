using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace woo.data
{
    public class Event
    {
        public int Id { get; set; }
        public DateTime Since { get; set; }
        public DateTime To { get; set; }

        public string Summary { get; set; }

        public virtual ICollection<User> Users { get; set; }

        public virtual ICollection<Message> Messages { get; set; }

    }
}
