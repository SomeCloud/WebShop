using System;
using System.Collections.Generic;

namespace WebShop.Models
{
    public partial class Product
    {
        public Product()
        {
            Basket = new HashSet<Basket>();
            Code = new HashSet<Code>();
            Order = new HashSet<Order>();
            ProductGenre = new HashSet<ProductGenre>();
            ProductLanguage = new HashSet<ProductLanguage>();
        }

        public short Id { get; set; }
        public string Name { get; set; }
        public short Cost { get; set; }
        public short Quantity { get; set; }
        public string Description { get; set; }
        public string IconUrl { get; set; }
        public string PictureUrl { get; set; }
        public short? IdDeveloper { get; set; }
        public short? IdPublisher { get; set; }
        public string SystemRequirements { get; set; }

        public virtual Developer IdDeveloperNavigation { get; set; }
        public virtual Publisher IdPublisherNavigation { get; set; }
        public virtual ICollection<Basket> Basket { get; set; }
        public virtual ICollection<Code> Code { get; set; }
        public virtual ICollection<Order> Order { get; set; }
        public virtual ICollection<ProductGenre> ProductGenre { get; set; }
        public virtual ICollection<ProductLanguage> ProductLanguage { get; set; }
    }
}
