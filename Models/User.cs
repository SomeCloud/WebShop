using System;
using System.Collections.Generic;

namespace WebShop.Models
{
    public partial class User
    {
        public User()
        {
            Basket = new HashSet<Basket>();
            Order = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string EMail { get; set; }
        public string Password { get; set; }
        public string IconUrl { get; set; }

        public virtual ICollection<Basket> Basket { get; set; }
        public virtual ICollection<Order> Order { get; set; }
    }
}
