using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace notomyk.Models
{
    public class ForumTopic
    {
        public ForumTopic()
        {
            this.IsActive = true;
        }

        public int ID { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public DateTime DateAdd { get; set; }
        public bool IsActive { get; set; }
        public int Visitors { get; set; }
        public bool IsReported { get; set; }

        [ForeignKey("ApplicationUser")] //as User Id is not present in target table therefore we must indicate to which table 'UserID' is referenced as PK        public string UserId { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual ForumCategory ForumCategory { get; set; }
    }
}