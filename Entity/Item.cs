using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Entity
{
    public class Item
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int collectionItId { get; set; }
        public virtual CollectionIt CollectionIt { get; set; }

        public virtual List<LikeItem> LikeItems { get; set; }
        public virtual List<Comment> Comments { get; set; }


        public Item()
        {
            Comments = new List<Comment>();
            LikeItems = new List<LikeItem>();
        }
    }
}