using notomyk.DAL;
using notomyk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace notomyk.Infrastructure
{
    public class Account
    {
        static NTMContext db = new NTMContext();


        public static bool UserExists(string userID)
        {
            if (db.Users.Any(u => u.Id == userID))
            {
                return true;
            }
            return false;
        }
    }
}