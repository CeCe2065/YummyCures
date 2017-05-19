using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YummyCures.Models
{
    public class ContentIndexViewModel
    {
        public List<Content> Videos { get; set; }
        public List<Content> Recipes { get; set; }
        public List<Content> Articles { get; set; }
    }
}