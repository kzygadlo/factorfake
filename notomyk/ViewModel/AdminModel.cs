using notomyk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace notomyk.ViewModel
{
    public class AdminModel
    {
        public IEnumerable<AppSettings> AppSettings { get; set; }
        public IEnumerable<ApplicationUser> Users { get; set; }
    }
}