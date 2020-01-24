using System;
using System.Collections.Generic;

namespace WebShop.Models
{
    public partial class Code
    {
        public short Id { get; set; }
        public string Key { get; set; }
        public short? IdProduct { get; set; }

        public virtual Product IdProductNavigation { get; set; }
    }
}
