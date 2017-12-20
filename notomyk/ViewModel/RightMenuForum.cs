using notomyk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace notomyk.ViewModel
{
    public class RightMenuForum
    {
        public ICollection<ForumTopic> Topics { get; set; }
    }
}