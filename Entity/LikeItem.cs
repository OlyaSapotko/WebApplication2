using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Entity
{
    public class LikeItem
    {
        public int id { get; set; }
        public string ApplicationUserId { get; set; }

        public int ItemId { get; set; }

        public virtual Item Item { get; set; }

    }
}