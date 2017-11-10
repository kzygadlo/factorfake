using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace notomyk.Models
{
    public class tbl_News
    {
        public tbl_News()
        {
            IsActive = true;
        }

        [Key]
        public int tbl_NewsID { get; set; } /*PK*/
        [Required]
        public string Title { get; set; }
        [Required]
        public string ArticleLink { get; set; }
        public string PictureLink { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateAdd { get; set; }
        [Required]
        public string Description { get; set; }
        public int tbl_NewspaperID { get; set; }
        public int Visitors { get; set; }
        public virtual tbl_Newspaper Newspaper { get; set; }


        [ForeignKey("ApplicationUser")] //as User Id is not present in target table therefore we must indicate to which table 'UserID' is referenced as PK
        public string UserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual ICollection<tbl_Comment> Collection_Comments { get; set; }
        public virtual ICollection<EventTag> EventsTags { get; set; }
        public virtual ICollection<VoteLog> VoteLogs { get; set; }
    }
}