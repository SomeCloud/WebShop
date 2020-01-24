using System;
using System.Collections.Generic;

namespace WebShop.Models
{
    public partial class ProductLanguage
    {
        public short IdLanguage { get; set; }
        public short IdProduct { get; set; }

        public virtual Language IdLanguageNavigation { get; set; }
        public virtual Product IdProductNavigation { get; set; }
    }
}
