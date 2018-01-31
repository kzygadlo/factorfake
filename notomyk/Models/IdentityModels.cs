using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel;
using System;
using notomyk.DAL;
using System.Linq;
using notomyk.Infrastructure;

namespace notomyk.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        private int MinNumberVotes = Convert.ToInt32(cApp.AppSettings["MinNumberVotes"]);
        private int MinCommentsForReputation = Convert.ToInt32(cApp.AppSettings["MinCommentsForReputation"]);


        public int NewsCounter { get; set; }
        public DateTime? LastNewsAdded { get; set; }
        public int CommentsCounter { get; set; }
        public DateTime? LastCommentAdded { get; set; }
        public int LoginAttempts { get; set; }
        public DateTime? LastLoginAttempt { get; set; }
        public DateTime? AccountCreateDate { get; set; }
        public DateTime? LastActivity { get; set; }

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

        public int Reputation()
        {

            double _result;

            DateTime? lastActivity = this.LastActivity;
            int LastActDiff = 0;

            if (lastActivity.HasValue)
            {
                LastActDiff = (DateTime.UtcNow - (DateTime)lastActivity).Days;
            }


            int aComments = AllCommentsCount();
            int pComments = PostitiveCommentsCount();

            if (aComments >= MinCommentsForReputation)
            {
                _result = ((Convert.ToDouble(pComments) / Convert.ToDouble(aComments)) * 100) - LastActDiff / 2;                
            }
            else
            {
                _result = 0;
            }
            return Convert.ToInt16(_result);

        }

        public int AllCommentsCount()
        {
            return this.tbl_Comment.Where(c => c.IsActive == true && (c.Fakt + c.Fake) >= MinNumberVotes).Count();
        }

        public int PostitiveCommentsCount()
        {
            return this.tbl_Comment.Where(c => c.IsActive == true && c.Fakt > c.Fake && (c.Fakt + c.Fake) >= MinNumberVotes).Count();
        }
    }
}