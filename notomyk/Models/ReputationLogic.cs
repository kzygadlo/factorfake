using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace notomyk.Models
{
    public static class ReputationLogic
    {       
        private static double _result;

        public static int ReputationPercentage(int pComments, int aComments)
        {
            if (aComments > 0)
            {
                _result = (Convert.ToDouble(pComments) / Convert.ToDouble(aComments)) * 100;
            }
            else
            {
                _result = 0;
            }
            return Convert.ToInt16(_result);
        }

    }
}