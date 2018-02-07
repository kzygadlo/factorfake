using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace notomyk.Models
{
    public class GetTimeAgo
    {

        public static string CalculateDateDiff(DateTime? strDate)
        {
            string strTime = string.Empty;

            if (strDate == null)
            {
                return "brak danych";
            }
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
                    return minutes + " godz. temu";
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
                    return minutes + " tyg. temu";
                }
                else if (deltaMinutes < (24 * 60 * 61))
                {
                    return "Miesiąc temu";
                }
                else if (deltaMinutes < (24 * 60 * 365.25))
                {
                    minutes = (int)Math.Floor(deltaMinutes / (60 * 24 * 30));
                    return minutes + " mies. temu";
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

        public static string CalculateDateDiffAhead(DateTime? strDate)
        {
            string strTime = string.Empty;

            if (strDate == null)
            {
                return "brak danych";
            }
            if (IsDate(Convert.ToString(strDate)))
            {
                TimeSpan t = Convert.ToDateTime(strDate) - DateTime.UtcNow;
                double deltaSeconds = t.TotalSeconds;

                double deltaMinutes = deltaSeconds / 60.0f;
                int minutes;

                if (deltaSeconds < 5)
                {
                    return " kilka sekund";
                }
                else if (deltaSeconds < 60)
                {
                    return Math.Floor(deltaSeconds) + " sekund(y)";
                }
                else if (deltaSeconds < 120)
                {
                    return "minutę";
                }
                else if (deltaMinutes < 60)
                {
                    return Math.Floor(deltaMinutes) + " minut(y)";
                }
                else if (deltaMinutes < 120)
                {
                    return "godzinę";
                }
                else if (deltaMinutes < (24 * 60))
                {
                    minutes = (int)Math.Floor(deltaMinutes / 60);
                    return minutes + " godzin(y)";
                }
                else if (deltaMinutes < (24 * 60 * 2))
                {
                    return "24 godziny";
                }
                else if (deltaMinutes < (24 * 60 * 7))
                {
                    minutes = (int)Math.Floor(deltaMinutes / (60 * 24));
                    return minutes + " dni";
                }
                else if (deltaMinutes < (24 * 60 * 14))
                {
                    return "tydzień";
                }
                else if (deltaMinutes < (24 * 60 * 31))
                {
                    minutes = (int)Math.Floor(deltaMinutes / (60 * 24 * 7));
                    return minutes + " tygodni(e)";
                }
                else if (deltaMinutes < (24 * 60 * 61))
                {
                    return "miesiąc.";
                }
                else if (deltaMinutes < (24 * 60 * 365.25))
                {
                    minutes = (int)Math.Floor(deltaMinutes / (60 * 24 * 30));
                    return minutes + " miesięcy";
                }
                else if (deltaMinutes < (24 * 60 * 731))
                {
                    return "rok";
                }

                minutes = (int)Math.Floor(deltaMinutes / (60 * 24 * 365));
                return minutes + " lat(a)";
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

        public static int IsOnline(DateTime? date)
        {
            TimeSpan t = DateTime.UtcNow - Convert.ToDateTime(date);
            double deltaMinutes = t.TotalMinutes;

            if (deltaMinutes < 20)
            {
                return 1;
            }
            else if (deltaMinutes < 1440)
            {
                return 0;
            }
            else
            {
                return -1;
            }
        }
    }
}