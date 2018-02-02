using notomyk.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace notomyk.Controllers
{
    public class AdminCommentsController : Controller
    {
        NTMContext db = new NTMContext();
        // GET: AdminUserTable
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult IndexUserID(string id)
        {
            ViewBag.UserID = id;
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult IndexNewsID(int id)
        {
            ViewBag.NewsID = id;
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult GetComments(int newsID, string userID)
        {
            var n = db.Comment.AsQueryable();

            if (newsID != 0)
            {
                n = n.Where(c => c.tbl_NewsID == newsID);
            }
            else if (userID != "")
            {
                n = db.Comment.Where(c => c.ApplicationUser.Id == userID);
            }

            var comments = n.Select(x => new {
                x.ApplicationUser.UserName,
                x.IsReported,
                x.IsActive,                
                //x.Comment,
                x.tbl_NewsID
            }).ToList();

            return Json(new { data = comments }, JsonRequestBehavior.AllowGet);
        }
    }
}