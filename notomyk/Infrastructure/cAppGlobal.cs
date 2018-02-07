using NLog;
using notomyk.DAL;
using notomyk.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace notomyk.Infrastructure
{
    public class cAppGlobal
    {
        private static Logger FOFlog = LogManager.GetCurrentClassLogger();

        NTMContext db = new NTMContext();
        public bool ResultFromDB = true;

        public static bool IsAllowed(string what)
        {
            cAppGlobal global = new cAppGlobal();
            global.CheckIfSettingTrue(what);
            return global.ResultFromDB;
        }

        public void CheckIfSettingTrue(string key)
        {
            string dictValue;
            if (cAppGlobalSettings.AppGlobalSettings.TryGetValue(key, out dictValue))
            {
                int value;
                if (int.TryParse(dictValue, out value))
                {
                    if (value == 0)
                    {
                        ResultFromDB = false;
                    }                    
                }
            }           
        }
    }
}
