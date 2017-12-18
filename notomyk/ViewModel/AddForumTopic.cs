using notomyk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace notomyk.ViewModel
{
    public class AddForumTopic
    {
        public List<ForumCategory> Categories { get; set; }
        public int CategoryID { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
    }
}