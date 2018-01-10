using notomyk.DAL;
using notomyk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace notomyk.Infrastructure
{
    public class addLoginValidator
    {
        private int _LoginLimitAttempts;
        private double _LoginTimeDelay;
        public double timeToGO;

        public ApplicationUser _User = new ApplicationUser();
        NTMContext db = new NTMContext();

        public addLoginValidator(ApplicationUser user)
        {
            if (user != null)
            {
                _User = db.Users.Where(u => u.Email == user.Email).FirstOrDefault();
            }

            _LoginLimitAttempts = 3;
            _LoginTimeDelay = 60;
        }


        public bool IfExceededLoginAttempts()
        {

            if (_User.LoginAttempts > 3)
            {
                return true;
            }

            return false;
        }

        public double WhetherDelayTimeHasPassed()
        {
            double timeDiff;

            if (_User.LastLoginAttempt.HasValue)
            {
                timeDiff = (DateTime.UtcNow - _User.LastLoginAttempt.Value).TotalSeconds;
            }
            else
            {
                timeDiff = 9999;
            }


            if (timeDiff > _LoginTimeDelay)
            {
                return 0;
            }

            timeToGO = _LoginTimeDelay - timeDiff;

            return timeDiff;
        }

        public void WrongLogin()
        {
            _User.LoginAttempts++;
            _User.LastLoginAttempt = DateTime.UtcNow;
            db.SaveChanges();
        }

        public void SuccessfulLogin()
        {
            _User.LoginAttempts = 0;
            db.SaveChanges();
        }
    }
}