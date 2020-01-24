using System;
using System.Collections.Generic;

namespace WebShop.Models
{
    public partial class Comment
    {
        public short Id { get; set; }
        public string Text { get; set; }
        public DateTime? Date { get; set; }
    }
}
