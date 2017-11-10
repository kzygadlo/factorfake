using notomyk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace notomyk.ViewModel
{
    public class Filters
    {
        public List<tbl_Newspaper> Newspapers { get; set; }
        public List<Tag> Categories { get; set; }
    }
}