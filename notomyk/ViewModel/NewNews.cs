using notomyk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace notomyk.ViewModel
{
    public class NewNews
    {
        public string UrlLink { get; set; }

        public tbl_Comment Comment { get; set; }
    }
}