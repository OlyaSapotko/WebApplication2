using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.Models;

namespace WebApplication2.Entity
{
    public class Comment
    {
        public int id { get; set; }
        public int ItemId { get; set; }
        public string ApplicationUserId { get; set; }
        public string text { get; set; }
        public virtual Item Item { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}