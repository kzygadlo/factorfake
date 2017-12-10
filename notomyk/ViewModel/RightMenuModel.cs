using notomyk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace notomyk.ViewModel
{
    public class RightMenuModel
    {
        public IEnumerable<tbl_News> FakeNews { get; set; }
        public IEnumerable<tbl_News> FaktNews { get; set; }
        public IEnumerable<tbl_News> CommentedNews { get; set; }
        public IEnumerable<tbl_News> VisitedNews { get; set; }
        public IEnumerable<UserReputation> UsersRep { get; set; }
        public IEnumerable<ApplicationUser> UsersComm { get; set; }
        public IEnumerable<ApplicationUser> UsersNews { get; set; }
    }
}
