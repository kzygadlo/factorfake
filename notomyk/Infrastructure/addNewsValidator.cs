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
    public class addNewsValidator
    {

        ApplicationUser _user = new ApplicationUser();
        public string WhatRole;
        int _NewsLimitNumber;
        public bool EmailConfirmed;

        public addNewsValidator()
        {
        
        }

        public addNewsValidator(ApplicationUser user, NTMContext db)
        {
            //_User = user;
            _user.Id = user.Id;
            EmailConfirmed = user.EmailConfirmed;
            _user.NewsCounter = user.NewsCounter;
            _user.LastCommentAdded = DateTime.UtcNow;
            _user.LastNewsAdded = DateTime.UtcNow;

            var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            WhatRole = um.GetRoles(_user.Id).FirstOrDefault();


            if (CheckIfCounterToReset(user.LastNewsAdded) == true)
            {
                user.NewsCounter = 0;
                db.SaveChanges();
            }
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
            if (_user.EmailConfirmed)
            {
                switch (WhatRole)
                {
                    case "Admin":
                        _NewsLimitNumber = int.Parse(ConfigurationManager.AppSettings["NewsLimitAdmin"]);
                        break;
                    case "Moderator":
                        _NewsLimitNumber = int.Parse(ConfigurationManager.AppSettings["NewsLimitModerator"]);
                        break;
                    case "User":
                        _NewsLimitNumber = int.Parse(ConfigurationManager.AppSettings["NewsLimitUser"]);
                        break;
                }
            }
            else
            {
                _NewsLimitNumber = 1;
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
            db.SaveChanges();
        }
      
    }
}