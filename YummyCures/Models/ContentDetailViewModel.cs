using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YummyCures.Models
{
    public class ContentDetailViewModel
    {
        public Content Content { get; set; }
        public Comment NewComment { get; set; }
    }
}