﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace YummyCures.Models
{
    public class Content
    {
        public int ContentID { get; set; }
        public DateTime ContentCreatedDate { get; set; }

        public string Description { get; set; }


        public ApplicationUser User { get; set; }
        public string UserID { get; set; }

        public ContentType ContentType { get; set; }
        public int ContentTypeID { get; set; }

        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        public string ContentBody { get; set; }

        public string PreviewUrl { get; set; }

        public string ThumbNailUrl { get; set; }

        public List<Ingredient> Ingredients { get; set; }
        public List<Tag> Tags { get; set; }

        public List<Comment> Comments { get; set; }
    }
}