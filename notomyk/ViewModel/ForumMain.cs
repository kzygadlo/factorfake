﻿using notomyk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace notomyk.ViewModel
{
    public class ForumMain
    {
        public ICollection<ForumCategory> Categories {get;set;}
        public ICollection<ForumTopic> Topics { get; set; }
    }
}