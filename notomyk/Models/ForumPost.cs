using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace notomyk.Models
{
    public class ForumPost
    {
        public ForumPost()
        {
            IsActive = true;
        }

        public int ID { get; set; }
        public virtual ForumPost Parent { get; set; }
        public virtual ICollection<ForumPost> Children { get; set; }

        [Required(ErrorMessage="Komentarz nie może być pusty.")]
        public string Content { get; set; }
        public bool IsActive { get; set; }

        public int Fakt { get; set; }
        public int Fake { get; set; }
        public DateTime DateAdd { get; set; }
        public DateTime DateModify { get; set; }
        public bool IsReported { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ForumTopic Topic { get;set;}

    }
}