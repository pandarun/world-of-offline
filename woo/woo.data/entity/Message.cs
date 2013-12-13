using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace woo.data
{
    public class Message
    {
        public int Id { get; set; }

        public int EventId { get; set; }

        public virtual Event Event { get; set; }
    }
}
