
using Microsoft.AspNet.Identity;
using notomyk.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace notomyk.Models
{
    public class logLastActivity
    {
        NTMContext db = new NTMContext();
        ApplicationUser user = new ApplicationUser();

        public logLastActivity(string userID)
        {
            user = db.Users.Where(u => u.Id == userID).FirstOrDefault();
            user.LastActivity = DateTime.UtcNow;
            db.SaveChanges();
        }
    }
}