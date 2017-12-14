using Microsoft.AspNet.Identity;
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
                        _CommentsLimitNumber = int.Parse(ConfigurationManager.AppSettings["CommentsLimitAdmin"]);
                        break;
                    case "Moderator":
                        _CommentsLimitNumber = int.Parse(ConfigurationManager.AppSettings["CommentsLimitModerator"]);
                        break;
                    case "User":
                        _CommentsLimitNumber = int.Parse(ConfigurationManager.AppSettings["CommentsLimitUser"]);
                        break;
                }
            }
            else
            {
                _CommentsLimitNumber = 1;
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
                    _CommentsTimeDelay = double.Parse(ConfigurationManager.AppSettings["CommentsAddDelayAdmin"]);
                    break;
                case "Moderator":
                    _CommentsTimeDelay = double.Parse(ConfigurationManager.AppSettings["CommentsAddDelayModerator"]);
                    break;
                case "User":
                    _CommentsTimeDelay = double.Parse(ConfigurationManager.AppSettings["CommentsAddDelayUser"]);
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
            return _CommentsTimeDelay;
        }

        public void CommentAdded(ApplicationUser user, NTMContext db)
        {
            user.CommentsCounter++;
            user.LastCommentAdded = DateTime.UtcNow;
            db.SaveChanges();
        }
    }
}