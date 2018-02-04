using notomyk.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace notomyk.Controllers
{
    public class AdminBlackListController : Controller
    {
        NTMContext db = new NTMContext();
        // GET: AdminUserTable
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            ViewBag.AdminBlackListClass = "active";
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult GetBlackList()
        {
            var blackList = db.BlackList.Select(b => new
            {
                b.url,
                b.ID
            }).ToList();

            return Json(new { data = blackList }, JsonRequestBehavior.AllowGet);
        }
    }
}