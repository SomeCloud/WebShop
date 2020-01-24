using System;
using System.Collections.Generic;

namespace WebShop.Models
{
    public partial class ProductGenre
    {
        public short IdGenre { get; set; }
        public short IdProduct { get; set; }

        public virtual Genre IdGenreNavigation { get; set; }
        public virtual Product IdProductNavigation { get; set; }
    }
}
