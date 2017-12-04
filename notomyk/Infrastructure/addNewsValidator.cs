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
        private NTMContext db = new NTMContext();
        private ApplicationUser _User = new ApplicationUser();
        
        private string _uID;
        public string WhatRole;
        public bool eConfirmed;

        private int _NewsLimitNumber;
        private int _NewsDelayInSec;

        private DateTime _CurrentDate;

        public addNewsValidator(ApplicationUser user)
        {
            _User = user;
            var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));  
            WhatRole = um.GetRoles(_User.Id).FirstOrDefault();
            eConfirmed = _User.EmailConfirmed;

            if (CheckIfCounterToReset(_User.LastNewsAdded) == true)
            {
                var userToUpdate = db.Users.Where(u => u.Id == _User.Id).FirstOrDefault();
                userToUpdate.NewsCounter = 0;
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
            if (_User.EmailConfirmed)
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

            if (_User.NewsCounter < _NewsLimitNumber)
            {
                return 0;
            }
            return _NewsLimitNumber;
        }

        public void NewsAdded()
        {
            var userToUpdate = db.Users.Where(u => u.Id == _User.Id).FirstOrDefault();
            userToUpdate.NewsCounter++;
            userToUpdate.LastNewsAdded = DateTime.UtcNow;
            db.SaveChanges();
        }
      
    }
}