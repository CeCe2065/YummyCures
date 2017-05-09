using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YummyCures.Models
{
    public class Comment
    {
        public int CommentID { get; set; }

        public Content Content { get; set; }
        public int ContentID { get; set; }


        public DateTime CommentCreatedDate { get; set; }
        public string FirstName { get; set; }
        public string EmailAddress { get; set; }
        public string CommentUrl { get; set; }
        public string CommentBody { get; set; }

    }
}