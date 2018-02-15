using notomyk.DAL;
using notomyk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Configuration;
using System.Security.Claims;
using System.Web.UI;
using Microsoft.AspNet.Identity.EntityFramework;

namespace notomyk.Infrastructure
{
    public class NewsValidator
    {
        ApplicationUser _user = new ApplicationUser();
        public string WhatRole;
        int _NewsLimitNumber;
        public bool EmailConfirmed;

        public NewsValidator()
        {
        
        }

        public NewsValidator(ApplicationUser user, NTMContext db)
        {
            if (CheckIfCounterToReset(user.LastNewsAdded) == true)
            {
                user.NewsCounter = 0;
                db.SaveChanges();
            }
            //_User = user;
            _user.Id = user.Id;
            EmailConfirmed = user.EmailConfirmed;
            _user.NewsCounter = user.NewsCounter;
            _user.LastCommentAdded = DateTime.UtcNow;
            _user.LastNewsAdded = DateTime.UtcNow;

            var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            WhatRole = um.GetRoles(_user.Id).FirstOrDefault();

        }

        private bool CheckIfCounterToReset(DateTime? nLastAdded)
        {
            if (nLastAdded.HasValue)
            {
                if (nLastAdded.Value.Date == DateTime.Today)
                {
                    return false;
                }
            }            
            return true;
        }

        public int IfExceededNewsNumber()
        {
            if (EmailConfirmed)
            {
                switch (WhatRole)
                {
                    case "Admin":
                        _NewsLimitNumber = Convert.ToInt32(cApp.AppSettings["NewsLimitAdmin"]);
                        break;
                    case "Moderator":
                        _NewsLimitNumber = Convert.ToInt32(cApp.AppSettings["NewsLimitModerator"]);
                        break;
                    case "User":
                        _NewsLimitNumber = Convert.ToInt32(cApp.AppSettings["NewsLimitUser"]);
                        break;
                }
            }
            else
            {
                _NewsLimitNumber = Convert.ToInt32(cApp.AppSettings["NewsLimitNotConfirmed"]);
            }

            if (_user.NewsCounter < _NewsLimitNumber)
            {
                return 0;
            }
            return _NewsLimitNumber;
        }

        public void NewsAdded(ApplicationUser user, NTMContext db)
        {
            user.NewsCounter++;
            user.LastNewsAdded = DateTime.UtcNow;
            user.LastActivity = DateTime.UtcNow;
            db.SaveChanges();
        }

        public bool UrlForbidden(string url, NTMContext db)
        {
            var homeURL = new Uri(url);
            string host = homeURL.Host;

            if (host.Substring(0, 4) == "www.")
            {
                host = host.Substring(4);
            }

            if (db.BlackList.Any(b => b.url == host))
            {
                return true;
            }
            return false;
        }
      
    }
}