using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace notomyk.Infrastructure
{
    public class ConvertToString
    {
        public static string Date(DateTime? date)
        {
            if (date is null)
            {
                return "";
            }
            else
            {
                return String.Format("{0:s}", date);
            }           
        }
    }
}