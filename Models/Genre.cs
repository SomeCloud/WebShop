using System;
using System.Collections.Generic;

namespace WebShop.Models
{
    public partial class Genre
    {
        public Genre()
        {
            ProductGenre = new HashSet<ProductGenre>();
        }

        public short Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ProductGenre> ProductGenre { get; set; }
    }
}
