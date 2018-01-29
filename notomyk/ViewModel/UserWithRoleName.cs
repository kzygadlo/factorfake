using notomyk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace notomyk.ViewModel
{
    public class UserWithRoleName
    {
        public ApplicationUser User { get; set; }
        public string RoleName { get; set; }
    }
}