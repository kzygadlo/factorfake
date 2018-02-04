using notomyk.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace notomyk.Controllers
{
    public class AdminForumTopicController : Controller
    {
        NTMContext db = new NTMContext();
        // GET: AdminUserTable
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            ViewBag.AdminForumTopicClass = "active";
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult GetForumTopics()
        {
            var topics = db.ForumTopic.Select(x => new
            {
                x.ApplicationUser.UserName,
                x.Description,
                x.IsActive,
                x.ID
            }).ToList();

            return Json(new { data = topics }, JsonRequestBehavior.AllowGet);
        }
    }
}