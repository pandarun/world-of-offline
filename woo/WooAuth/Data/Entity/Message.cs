using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WooAuth.data.entity
{
    public class Message
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public int EventId { get; set; }

        public virtual Event Event { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; }
    }
}
