using notomyk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace notomyk.ViewModel
{
    public class ForumMain
    {
        public IEnumerable<ForumCategory> Categories {get;set;}
        public IEnumerable<ForumTopic> Topics { get; set; }
    }
}