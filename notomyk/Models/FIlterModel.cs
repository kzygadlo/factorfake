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
            NewspapersList = new List<string>();
            TagsList = new List<int>();
            MainPage = true;
        }

        public List<string> NewspapersList { get; set; }
        public List<int> TagsList { get; set; }
        public int WhatNews { get; set; }
        public int Period { get; set; }
        public int Page { get; set; }
        public int Remains { get; set; }
        public bool MainPage { get; set; }
    }
}