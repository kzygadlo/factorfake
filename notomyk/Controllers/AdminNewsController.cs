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
        public ActionResult GetNewses()
        {
            var newses = db.News.Select(x => new
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