using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace notomyk.Models
{
    public class ForumTopic
    {
        public int ID { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public DateTime DateAdd { get; set; }
        public bool IsActive { get; set; }        
        public int VisitorsN { get; set; }
        public int CommentsN { get; set; }
        public int UserID { get; set; }
        public int CategoryID { get; set; }
    }
}