using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace notomyk.Models
{
    public class Comments
    {
        public tbl_Comment Comment { get; set; }
        public ApplicationUser User { get; set; }
    }
}