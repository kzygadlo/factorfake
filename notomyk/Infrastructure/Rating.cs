using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace notomyk.Infrastructure
{
    public class Rating
    {
        public static int RatingValue(int Fakt, int Fake)
        {
            return Fakt - Fake;
        }

        public static int RatingClass(int Fakt, int Fake)
        {
            int value = Convert.ToInt32(ConfigurationManager.AppSettings["FilterVoting"]);

            if (Fakt > Fake)
            {
                if (Fakt + Fake >= value && (Fake == 0 || Fakt / Fake > 2))
                {
                    return 2; 
                }

                return 1;
            }
            else if (Fakt < Fake)
            {
                if (Fakt + Fake >= value && (Fakt == 0 || Fake / Fakt > 2))
                {
                    return 4;
                }
                return 3;
            }
            
            return 0;
        }

    }
}