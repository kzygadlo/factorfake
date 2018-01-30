using NLog;
using notomyk.DAL;
using notomyk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace notomyk.Infrastructure
{
    public class cApp
    {
        private static Logger FOFlog = LogManager.GetCurrentClassLogger();
        static NTMContext db = new NTMContext();
        // This is the only time System.Configuration.ConfigurationManager.AppSettings is called.
        // The appSetting ApplicationEnv is in machine.config and will be one of the values “Dev”, “Test”, “QA”, “Prod” or “DR” 
        static readonly cConfig _config = new cConfig();

        public static Dictionary<string, string> AppSettings
        {
            get
            {
                return _config.AppSettings;
            }
        }

        public static List<AppSettings> GetSettings()
        {
            FOFlog.Info(string.Format("Get Settings from AppSettings table"));
            var result = db.AppSettings.ToList();
            return result;
        }
    }
}