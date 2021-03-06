﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using notomyk.DAL;
using notomyk.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace notomyk.Infrastructure
{
    public class addCommentValidator
    {
        public string WhatRole;
        public bool EmailConfirmed;
        int _CommentsLimitNumber;
        double _CommentsTimeDelay;
        public double timeToGO;

        ApplicationUser _User = new ApplicationUser();

        public addCommentValidator()
        {
        }

        public addCommentValidator(ApplicationUser user, NTMContext db)
        {

            if (CheckIfCounterToReset(user.LastCommentAdded) == true)
            {
                user.CommentsCounter = 0;
                db.SaveChanges();
            }

            _User.Id = user.Id;
            EmailConfirmed = user.EmailConfirmed;

            _User.CommentsCounter = user.CommentsCounter;
            _User.LastCommentAdded = user.LastCommentAdded;

            var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            WhatRole = um.GetRoles(_User.Id).FirstOrDefault();            
        }

        private bool CheckIfCounterToReset(DateTime? cLastAdded)
        {
            if (cLastAdded.HasValue)
            {
                if (cLastAdded.Value.Date == DateTime.Today)
                {
                    return false;
                }
            }
            return true;
        }

        public int IfExceededCommentsNumber()
        {
            if (EmailConfirmed)
            {
                switch (WhatRole)
                {
                    case "Admin":
                        _CommentsLimitNumber = Convert.ToInt32(cApp.AppSettings["CommentsLimitAdmin"]);
                        break;
                    case "Moderator":
                        _CommentsLimitNumber = Convert.ToInt32(cApp.AppSettings["CommentsLimitModerator"]);
                        break;
                    case "User":
                        _CommentsLimitNumber = Convert.ToInt32(cApp.AppSettings["CommentsLimitUser"]);
                        break;
                }
            }
            else
            {
                _CommentsLimitNumber = Convert.ToInt32(cApp.AppSettings["CommentsLimitNotConfirmed"]);
            }

            if (_User.CommentsCounter < _CommentsLimitNumber)
            {
                return 0;
            }
            return _CommentsLimitNumber;
        }

        public double WhetherDelayTimeHasPassed()
        {
            switch (WhatRole)
            {
                case "Admin":
                    _CommentsTimeDelay = Convert.ToDouble(cApp.AppSettings["CommentsAddDelayAdmin"]);
                    break;
                case "Moderator":
                    _CommentsTimeDelay = Convert.ToDouble(cApp.AppSettings["CommentsAddDelayModerator"]);
                    break;
                default:
                    _CommentsTimeDelay = Convert.ToDouble(cApp.AppSettings["CommentsAddDelayUser"]);
                    break;
            }

            double timeDiff;

            if (_User.LastCommentAdded.HasValue)
            {
                timeDiff = (DateTime.UtcNow - _User.LastCommentAdded.Value).TotalSeconds;
            }
            else
            {
                timeDiff = 9999;
            }


            if (timeDiff > _CommentsTimeDelay)
            {
                return 0;
            }

            timeToGO = _CommentsTimeDelay - timeDiff;

            return _CommentsTimeDelay;
        }

        public void CommentAdded(ApplicationUser user, NTMContext db)
        {
            user.CommentsCounter++;
            user.LastCommentAdded = DateTime.UtcNow;
            user.LastActivity = DateTime.UtcNow;
            db.SaveChanges();
        }
    }
}