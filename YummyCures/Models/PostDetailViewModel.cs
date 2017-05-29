using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YummyCures.Models
{
    public class PostDetailViewModel
    {
        public Post Post { get; set; }
        public Comment NewComment { get; set; }
    }
}