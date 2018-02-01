using notomyk.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace notomyk.Controllers
{
    public class AdminForumPostController : Controller
    {
        NTMContext db = new NTMContext();
        // GET: AdminUserTable
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult GetForumPosts()
        {
            var posts = db.ForumPost.Select(x => new {
                x.ApplicationUser.UserName,
                x.Content,
                x.IsReported,
                x.ID
            }).ToList();

            return Json(new { data = posts }, JsonRequestBehavior.AllowGet);
        }
    }
}