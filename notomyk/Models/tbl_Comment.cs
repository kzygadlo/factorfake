using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace notomyk.Models
{
    public class tbl_Comment
    {
        public tbl_Comment()
        {
            IsActive = true;
        }

        public int tbl_CommentID { get; set; }
        public int? Parenttbl_CommentID { get; set; }
        public virtual tbl_Comment Parent { get; set; }
        public virtual ICollection<tbl_Comment> Children { get; set; }

        [Required(ErrorMessage="Komentarz nie może być pusty.")]
        public string Comment { get; set; }
        public bool IsActive { get; set; }
        public int Fakt { get; set; }
        public int Fake { get; set; }
        public DateTime DateAdd { get; set; }

        public int tbl_NewsID { get; set; }
        public bool IsReported { get; set; }

        
        public string ApplicationUserAutor_Id { get; set; }

        public string NewContent { get; set; }
        public string EditorName { get; set; }        
        public DateTime? EditDate { get; set; }


        public virtual tbl_News Newses { get; set; }

        //[ForeignKey("ApplicationUserEditor_Id")]
        //public virtual ApplicationUser ApplicationUserEditor { get; set; }
        [ForeignKey("ApplicationUserAutor_Id")]
        public virtual ApplicationUser ApplicationUserAutor { get; set; }


        public virtual ICollection<VoteCommentLog> VoteCommentLogs { get; set; }
    }
}