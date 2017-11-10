using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace notomyk.Models
{
    public class ForumPost
    {
        public int ID { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public DateTime DateAdd { get; set; }

        public DateTime DateModify { get; set; }
        public bool IsActive { get; set; }
        public int RepliesN { get; set; }
        public int ParentID { get; set; }

        public int UserID { get; set; }
        public int ForumTopicID { get; set; }

    }
}