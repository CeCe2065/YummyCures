using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YummyCures.Models
{
    public class Tag
    {
        public int TagID { get; set; }
        public string Description { get; set; }
        /*
         A tag has a list posts.
         A post has a list of tags.
         */

        public List<Content> Contents { get; set; }
    }
}