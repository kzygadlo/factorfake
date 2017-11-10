using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace notomyk.Models
{
    public class GetTimeAgo
    {
        public static string CalculateDateDiff(DateTime strDate)
        {
            string strTime = string.Empty;
            if (IsDate(Convert.ToString(strDate)))
            {
                TimeSpan t = DateTime.UtcNow - Convert.ToDateTime(strDate);
                double deltaSeconds = t.TotalSeconds;

                double deltaMinutes = deltaSeconds / 60.0f;
                int minutes;

                if (deltaSeconds < 5)
                {
                    return "Teraz";
                }
                else if (deltaSeconds < 60)
                {
                    return Math.Floor(deltaSeconds) + " sekund temu";
                }
                else if (deltaSeconds < 120)
                {
                    return "Minutę temu";
                }
                else if (deltaMinutes < 60)
                {
                    return Math.Floor(deltaMinutes) + " minut temu";
                }
                else if (deltaMinutes < 120)
                {
                    return "Godzinę temu";
                }
                else if (deltaMinutes < (24 * 60))
                {
                    minutes = (int)Math.Floor(deltaMinutes / 60);
                    return minutes + " godzin temu";
                }
                else if (deltaMinutes < (24 * 60 * 2))
                {
                    return "Wczoraj";
                }
                else if (deltaMinutes < (24 * 60 * 7))
                {
                    minutes = (int)Math.Floor(deltaMinutes / (60 * 24));
                    return minutes + " dni temu";
                }
                else if (deltaMinutes < (24 * 60 * 14))
                {
                    return "Tydzień temu";
                }
                else if (deltaMinutes < (24 * 60 * 31))
                {
                    minutes = (int)Math.Floor(deltaMinutes / (60 * 24 * 7));
                    return minutes + " tygodni temu";
                }
                else if (deltaMinutes < (24 * 60 * 61))
                {
                    return "Miesiąc temu";
                }
                else if (deltaMinutes < (24 * 60 * 365.25))
                {
                    minutes = (int)Math.Floor(deltaMinutes / (60 * 24 * 30));
                    return minutes + " miesięcy temu";
                }
                else if (deltaMinutes < (24 * 60 * 731))
                {
                    return "W zeszłym roku";
                }

                minutes = (int)Math.Floor(deltaMinutes / (60 * 24 * 365));
                return minutes + " lat temu";
            }
            else
            {
                return "";
            }
        }
        public static bool IsDate(string o)
        {
            DateTime tmp;
            return DateTime.TryParse(o, out tmp);
        } 
    }
}