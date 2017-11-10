using notomyk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace notomyk.ViewModel
{
    public class NewsList
    {
        public IEnumerable<tbl_News> News { get; set; }
        public IEnumerable<tbl_Newspaper> Newspaper { get; set; }
    }
}