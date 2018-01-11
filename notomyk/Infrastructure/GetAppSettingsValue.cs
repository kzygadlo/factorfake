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
        private static NTMContext _db = null;
        private static AppSettings _appS = null;
        static GetAppSettingsValue()
        {
            _appS = new AppSettings();
            _db = new NTMContext();
        }   

        public static string Value(string Key)
        {
            _appS = _db.AppSettings.Where(s => s.Key == Key).FirstOrDefault();
            return _appS.Value;
        }
    }
}