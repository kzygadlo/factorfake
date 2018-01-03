using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace notomyk.Infrastructure
{
    public static class myNews
    {
        public static bool isNew(DateTime dateAdd)
        {
            DateTime currentDate = DateTime.UtcNow;
            TimeSpan diff = currentDate - dateAdd;

            if (diff.TotalHours < Convert.ToDouble(GetAppSettingsValue.Value("NewNewsHours")))
            {
                return true;
            }
            return false;
        }
    }
}