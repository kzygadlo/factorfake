﻿using notomyk.Infrastructure;
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

        int value = Convert.ToInt32(cApp.AppSettings["FilterVoting"]);

        public tbl_News()
        {
            IsActive = true;
        }

        [Key]
        public int tbl_NewsID { get; set; } /*PK*/
        [Required(ErrorMessage = "Tytuł nie może być pusty.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Link do artykułu nie może być pusty.")]
        public string ArticleLink { get; set; }
        public string PictureLink { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateAdd { get; set; }
        [Required(ErrorMessage = "Komentarz nie może być pusty.")]
        public string Description { get; set; }
        public int tbl_NewspaperID { get; set; }
        public int Visitors { get; set; }
        public virtual tbl_Newspaper Newspaper { get; set; }
        public bool IsReported { get; set; }

        [ForeignKey("ApplicationUser")] //as User Id is not present in target table therefore we must indicate to which table 'UserID' is referenced as PK
        public string UserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual ICollection<tbl_Comment> Collection_Comments { get; set; }
        public virtual ICollection<EventTag> EventsTags { get; set; }
        public virtual ICollection<VoteLog> VoteLogs { get; set; }


        public int RatingValue()
        {
            int Fakt = this.VoteLogs.Where(v => v.Vote == 1).Count();
            int Fake = this.VoteLogs.Where(v => v.Vote == -1).Count();

            return Fakt - Fake;
        }

        public int IsFMF()
        {
            int Fakt = this.VoteLogs.Where(v => v.Vote == 1).Count();
            int Fake = this.VoteLogs.Where(v => v.Vote == -1).Count();
            int Manipulated = this.VoteLogs.Where(v => v.Vote == 2).Count();

            if (Fakt > Fake && Fakt > Manipulated)
            {
                if (Fakt + Fake + Manipulated >= value)
                {
                    return 1;
                }
            }
            else if (Manipulated > Fakt && Manipulated > Fake)
            {
                if (Fakt + Fake + Manipulated >= value)
                {
                    return 2;
                }
            }
            else if (Fake > Fakt && Fake > Manipulated)
            {
                if (Fakt + Fake + Manipulated >= value)
                {
                    return -1;
                }
            }
            return 0;
        }

        public string LeftMenuIcon(int fakt, int manipulated, int fake)
        {
            if (fakt > fake && fakt > manipulated)
            {
                return "smile";
            }
            else if (manipulated > fakt && manipulated > fake)
            {
                return "meh";
            }
            else if (fake > fakt && fake > manipulated)
            {
                return "frown";
            }
            else
            {
                return "minus";
            }
        }

        public string LeftMenuColor(int fakt, int manipulated, int fake)
        {
            if (fakt > fake && fakt > manipulated)
            {
                return "green";
            }
            else if (manipulated > fakt && manipulated > fake)
            {
                return "grey";
            }
            else if (fake > fakt && fake > manipulated)
            {
                return "red";
            }
            else
            {
                return "grey";
            }
        }

        public int LeftMenuRating(int fakt, int manipulated, int fake)
        {
            if (fakt > fake && fakt > manipulated)
            {
                return fakt;
            }
            else if (manipulated > fakt && manipulated > fake)
            {
                return manipulated;
            }
            else if (fake > fakt && fake > manipulated)
            {
                return fake;
            }
            else
            {
                return 0;
            }
        }
    }
}