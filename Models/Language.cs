using System;
using System.Collections.Generic;

namespace WebShop.Models
{
    public partial class Language
    {
        public Language()
        {
            ProductLanguage = new HashSet<ProductLanguage>();
        }

        public short Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ProductLanguage> ProductLanguage { get; set; }
    }
}
