using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace notomyk.Models
{
    public class FilterModel
    {

        public FilterModel()
        {
            NewspapersList = new List<int>();
            TagsList = new List<string>();
        }

        public List<int> NewspapersList { get; set; }
        public List<string> TagsList { get; set; }
        public int WhatNews { get; set; }
        public int Period { get; set; }
        public int Page { get; set; }
        public int Remains { get; set; }
    }
}