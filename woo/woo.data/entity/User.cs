﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace woo.data
{
    public class User
    {
        public int Id { get; set; }
        public virtual ICollection<Event> Events { get; set; }

        public virtual ICollection<Activity> Activities { get; set; }
    }
}
