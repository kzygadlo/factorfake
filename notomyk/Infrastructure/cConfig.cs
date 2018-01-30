using NLog;
using notomyk.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace notomyk.Infrastructure
{
    public class cConfig
    {
        
        public Dictionary<string, string> AppSettings = new Dictionary<string, string>();

        public cConfig()
        {
            // the cApp.DAL is our data access layer and this just calls the stored proc and returns a table.
            foreach (AppSettings aS in cApp.GetSettings())
            {                
                AppSettings.Add(aS.Key, aS.Value);
            }
        }
    }
}