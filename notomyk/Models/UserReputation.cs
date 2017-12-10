using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace notomyk.Models
{
    public class UserReputation
    {
        public string Id { get; set;}
        public string UserName { get; set; }
        public int RepPoints { get; set; }
        public int Pcomments { get; set; }
        public int Ncomments { get; set; }
        public int Acomments { get; set; }
    }
}