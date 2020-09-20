using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Entity
{
    public class CollectionIt
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string ApplicationUserId { get; set; }

        public virtual List<Item> Items { get; set; }

        public CollectionIt()
        {
            Items = new List<Item>();
        }
    }
}