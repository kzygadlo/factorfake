using NLog;
using notomyk.DAL;
using notomyk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace notomyk.Infrastructure
{
    public class cAppGlobalSettings
    {
        private static Logger FOFlog = LogManager.GetCurrentClassLogger();

        NTMContext db = new NTMContext();
        public Dictionary<string, string> SettingsDictionary = new Dictionary<string, string>();

        public static Dictionary<string, string> AppGlobalSettings
        {
            get
            {                
                if (HttpRuntime.Cache["appSettingsGlobal"] != null)
                {
                    //FOFlog.Info("Settings taken from Cache");
                    return (Dictionary<string, string>)HttpRuntime.Cache["appSettingsGlobal"];
                }
                cAppGlobalSettings settings = new cAppGlobalSettings();
                settings.AddSettingsToDict();
                return settings.SettingsDictionary;
            }
        }

        public void AddSettingsToDict()
        {
            foreach (var settingGlobal in SettingsGlobalFromDB())
            {
                SettingsDictionary.Add(settingGlobal.Key, Convert.ToString(Convert.ToInt32(settingGlobal.Value)));
            }

            HttpRuntime.Cache["appSettingsGlobal"] = SettingsDictionary;
        }

        public List<AppSettingsGlobal> SettingsGlobalFromDB()
        {
            FOFlog.Info("SettingsGlobal taken from DB");
            var settingsGlobal = db.AppSettingsGlobal.ToList();
            return settingsGlobal;
        }

    }
}
