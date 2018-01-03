using notomyk.DAL;
using notomyk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace notomyk.Infrastructure
{
    public static class GetAppSettingsValue
    {
        private static NTMContext _db = new NTMContext();
        private static AppSettings _appS = new AppSettings();

        public static string Value(string Key)
        {
            _appS = _db.AppSettings.Where(s => s.Key == Key).FirstOrDefault();
            return _appS.Value;
        }
    }
}