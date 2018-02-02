using notomyk.DAL;
using notomyk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace notomyk.Controllers
{
    public class AdminNewsController : Controller
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
            ViewBag.NewspaperID = 0;
            ViewBag.UserID = id;
            return View("Index");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult IndexNewspaperID(int id)
        {
            ViewBag.NewsID = id;
            return View("Index");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult GetNewses(int newspaperID, string userID)
        {
            var n = db.News.AsQueryable();

            if (newspaperID != 0)
            {
                n = n.Where(c => c.tbl_NewspaperID == newspaperID);
            }
            else if (userID != "")
            {
                n = db.News.Where(c => c.ApplicationUser.Id == userID);
            }

            var newses = n.Select(x => new
            {
                x.Newspaper.NewspaperName,
                x.ApplicationUser.UserName,
                x.Description,
                x.IsReported,
                x.IsActive,
                x.tbl_NewsID
            }).ToList();

            return Json(new { data = newses }, JsonRequestBehavior.AllowGet);
        }
    }
}