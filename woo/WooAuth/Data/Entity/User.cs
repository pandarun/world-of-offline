using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WooAuth.data.entity
{
    public class User
    {
        public int Id { get; set; }
        public string USerName { get; set; }
        public string AvatarPic { get; set; }

        public string OAuthLogin { get; set; }

        public virtual ICollection<Event> Events { get; set; }

        public virtual ICollection<Activity> Activities { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
    }
}
