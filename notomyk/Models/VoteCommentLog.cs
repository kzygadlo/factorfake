using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace notomyk.Models
{
    public class VoteCommentLog
    {
        public VoteCommentLog()
        {
            Active = true;
        }

        public int VoteCommentLogID { get; set; }
        public int tbl_CommentID { get; set; }       

        public bool Vote { get; set; }
        public bool Active { get; set; }

        [ForeignKey("ApplicationUser")] //as User Id is not present in target table therefore we must indicate to which table 'UserID' is referenced as PK
        public string UserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual tbl_Comment Comment { get; set; }

    }
}