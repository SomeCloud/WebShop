using System;
using System.Collections.Generic;

namespace WebShop.Models
{
    public partial class Comments
    {
        public short IdProduct { get; set; }
        public short IdUserClientComment { get; set; }
        public short IdCommentClientComment { get; set; }
    }
}
