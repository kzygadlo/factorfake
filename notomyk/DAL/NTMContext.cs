using Microsoft.AspNet.Identity.EntityFramework;
using notomyk.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace notomyk.DAL
{
    public class NTMContext: IdentityDbContext<ApplicationUser>
    {
        public NTMContext() : base("CSlocal")
        {
            
        }

        static NTMContext()
        {
            //Database.SetInitializer(new NTMInitializer());
        }

        public static NTMContext Create()
        {
            return new NTMContext();
        }

        public DbSet<tbl_Newspaper> Newspaper { get; set; }
        public DbSet<tbl_News> News { get; set; }
        public DbSet<tbl_Comment> Comment { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<EventTag> EventTag { get; set; }
        public DbSet<VoteLog> VoteLog { get; set; }
        public DbSet<VoteCommentLog> VoteCommentLog { get; set; }
        public DbSet<ForumTopic> ForumTopic { get; set; }
        public DbSet<ForumCategory> ForumCategory { get; set; }
        public DbSet<ForumPost> ForumPost { get; set; }
    }
}