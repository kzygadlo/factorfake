using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace notomyk.Infrastructure
{
    public static class StringTranslate
    {
        public static string ReturnRoleName(string userRole)
        {

            if (userRole == "User")
            {
                return "Użytkownik";
            }

            return userRole;
        }
    }
}