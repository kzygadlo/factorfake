using notomyk.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace notomyk.Controllers
{
    public class AdminVotingNewsController : Controller
    {
        NTMContext db = new NTMContext();
        // GET: AdminUserTable
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {

            ViewBag.AdminVotingNewsClass = "active";
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult GetNewsVotes()
        {
            var newsVotes = db.VoteLog.Select(x => new
            {
                x.ApplicationUser.UserName,
                x.tbl_NewsID,
                x.Vote,
                x.VoteLogID
            }).ToList();

            return Json(new { data = newsVotes }, JsonRequestBehavior.AllowGet);
        }
    }
}