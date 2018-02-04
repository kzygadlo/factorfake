using notomyk.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace notomyk.Controllers
{
    public class AdminVotingCommentController : Controller
    {
        NTMContext db = new NTMContext();
        // GET: AdminUserTable
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            ViewBag.AdminVotingCommentClass = "active";
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult GetCommentVotes()
        {
            var commentVotes = db.VoteCommentLog.Select(x => new
            {
                x.ApplicationUser.UserName,
                x.tbl_CommentID,
                x.Vote,
                x.VoteCommentLogID
            }).ToList();

            return Json(new { data = commentVotes }, JsonRequestBehavior.AllowGet);
        }
    }
}