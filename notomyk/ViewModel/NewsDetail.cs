using notomyk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace notomyk.ViewModel
{
    public class NewsDetail
    {
        public tbl_News SingleNews { get; set; }
        public IEnumerable<tbl_News> LeftNews { get; set; }
        public string CommaSeparatedTags { get; set; }
        public int WhatVote { get; set; }
    }
}