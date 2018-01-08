using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace notomyk.Models
{
    public class BlackList
    {
        public int ID { get; set; }
        [Index]
        [StringLength(400)]
        public string url { get; set; }
    }
}