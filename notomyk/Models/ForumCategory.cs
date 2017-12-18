using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace notomyk.Models
{
    public class ForumCategory
    {
        public ForumCategory()
        {
            this.ForumTopics = new List<ForumTopic>();
        }

        public int ID { get; set; }
        public string CategoryName { get; set; }
        
        public virtual ICollection<ForumTopic> ForumTopics { get; set; }
    }
}