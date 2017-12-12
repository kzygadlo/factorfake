using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace notomyk.Models
{
    public class Tag
    {
        public int ID { get; set; }
        public string TagName { get; set; }
        public virtual ICollection<EventTag> ListOfNews { get; set; }
    }
        
}