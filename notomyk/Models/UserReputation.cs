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
        public int Pcomments { get; set; }
        public int Acomments { get; set; }
        public double Reputation { get; set; }
        public DateTime? lastActivity { get; set; }
    }
}