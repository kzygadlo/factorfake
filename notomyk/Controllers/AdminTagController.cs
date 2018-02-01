using notomyk.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace notomyk.Controllers
{
    public class AdminTagController : Controller
    {
        NTMContext db = new NTMContext();
        // GET: AdminUserTable
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult GetTags()
        {
            var tags = db.Tag.Select(x => new
            {
                x.TagName,
                HowManyTimesUsed = x.ListOfNews.Count,
                x.ID
            }).ToList();

            return Json(new { data = tags }, JsonRequestBehavior.AllowGet);
        }
    }
}