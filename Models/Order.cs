using System;
using System.Collections.Generic;

namespace WebShop.Models
{
    public partial class Order
    {
        public short Number { get; set; }
        public int IdUser { get; set; }
        public short Quantity { get; set; }
        public short IdProduct { get; set; }
        public bool Done { get; set; }

        public virtual Product IdProductNavigation { get; set; }
        public virtual User IdUserNavigation { get; set; }
    }
}
