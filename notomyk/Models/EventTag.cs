using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace notomyk.Models
{
    public class EventTag
    {
        public int ID { get; set; }
        public int tbl_NewsID { get; set; }
        public int TagID { get; set; }

        public virtual tbl_News News { get; set; }
        public virtual Tag Tags { get; set; }

    }
}