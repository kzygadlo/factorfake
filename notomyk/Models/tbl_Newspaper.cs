using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace notomyk.Models
{
    public class tbl_Newspaper
    {

        public int tbl_NewspaperID { get; set; } 
        public string NewspaperName { get; set; }
        public string NewspaperLink { get; set; }
        public string NewspaperIconLink { get; set; }

        public virtual ICollection<tbl_News> Colection_Newses { get; set; }
    }
}