using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace woo.data.entity
{
    public class Activity
    {
        public int Id { get; set; }
        public DateTime Since { get; set; }

        public DateTime To { get; set; }

        public int UserId { get; set; }

        public string Text { get; set; }

        public virtual User User { get; set; }
    }
}
