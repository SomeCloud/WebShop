using System;
using System.Collections.Generic;

namespace WebShop.Models
{
    public partial class Developer
    {
        public Developer()
        {
            Product = new HashSet<Product>();
        }

        public short Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Product> Product { get; set; }
    }
}
