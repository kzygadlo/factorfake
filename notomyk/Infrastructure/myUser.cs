﻿using notomyk.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace notomyk.Infrastructure
{
    public static class myUser
    {
        public static bool IsCommentAuthor(int commID, string userID)
        {
            using (NTMContext db = new NTMContext())
            {

                if (db.Comment.Any(c => c.tbl_CommentID == commID && c.ApplicationUserAutor.Id == userID))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsNewsAuthor(int newsID, string userID)
        {
            using (NTMContext db = new NTMContext())
            {

                if (db.News.Any(n => n.tbl_NewsID == newsID && n.UserId == userID))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsPostAuthor(int postID, string userID)
        {
            using (NTMContext db = new NTMContext())
            {

                if (db.ForumPost.Any(p => p.ID == postID && p.ApplicationUser.Id == userID))
                {
                    return true;
                }
            }
            return false;
        }

    }
}