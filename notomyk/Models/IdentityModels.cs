using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel;
using System;

namespace notomyk.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public int NewsCounter { get; set; }
        public DateTime? LastNewsAdded { get; set; }
        public int CommentsCounter { get; set; }
        public DateTime? LastCommentAdded { get; set; }
        public int LoginAttempts { get; set; }
        public DateTime? LastLoginAttempt { get; set; }

        public virtual ICollection<VoteLog> VotingLogs { get; set; }

        public virtual ICollection<tbl_News> tbl_News { get; set; }

        public virtual ICollection<tbl_Comment> tbl_Comment { get; set; }
        //public virtual tbl_Comment tbl_Comment { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}