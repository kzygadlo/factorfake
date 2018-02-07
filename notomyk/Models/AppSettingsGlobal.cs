using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace notomyk.Models
{
    public class AppSettingsGlobal
    {
        public int ID { get; set; }
        public string Key { get; set; }
        public bool Value { get; set; }
        public int order { get; set; }
        public string Description { get; set; }
    }
}