using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace woo.data
{
    public class WooDataContext : DbContext
    {

        public WooDataContext() : base("WooDataContext") { }
        public DbSet<User> User { get; set; }
        public DbSet<Message> Message { get; set; }
        public DbSet<Event> Event { get; set; }

        public DbSet<Activity> Activity { get; set; }
    }
}
