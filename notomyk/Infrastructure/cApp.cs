using NLog;
using notomyk.DAL;
using notomyk.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace notomyk.Infrastructure
{
    public class cApp
    {
        private static Logger FOFlog = LogManager.GetCurrentClassLogger();

        NTMContext db = new NTMContext();
        public Dictionary<string, string> SettingsDictionary = new Dictionary<string, string>();

        public static Dictionary<string, string> AppSettings
        {
            get
            {                
                if (HttpRuntime.Cache["appSettings"] != null)
                {
                    //FOFlog.Info("Settings taken from Cache");
                    return (Dictionary<string, string>)HttpRuntime.Cache["appSettings"];
                }
                cApp settings = new cApp();
                settings.AddSettingsToDict();
                return settings.SettingsDictionary;
            }
        }

        public void AddSettingsToDict()
        {
            foreach (var setting in SettingsFromDB())
            {
                SettingsDictionary.Add(setting.Key, setting.Value);
            }

            HttpRuntime.Cache["appSettings"] = SettingsDictionary;

            //HttpRuntime.Cache.Add(
            //    "appSettings",
            //    SettingsDictionary,
            //    null,
            //    System.Web.Caching.Cache.NoAbsoluteExpiration,
            //    System.Web.Caching.Cache.NoSlidingExpiration,      
            //    System.Web.Caching.CacheItemPriority.Default,
            //    null
            //    );
        }

        public List<AppSettings> SettingsFromDB()
        {
            FOFlog.Info("Settings taken from DB");
            var settings = db.AppSettings.ToList();
            return settings;
        }
    }
}
