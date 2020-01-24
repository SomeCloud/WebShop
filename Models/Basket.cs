using System;
using System.Collections.Generic;

namespace WebShop.Models
{
    public partial class Basket
    {
        public int IdUser { get; set; }
        public short IdProduct { get; set; }
        public short? Status { get; set; }

        public virtual Product IdProductNavigation { get; set; }
        public virtual User IdUserNavigation { get; set; }
    }
}
