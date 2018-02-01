using notomyk.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace notomyk.Controllers
{
    public class AdminForumCategoryController : Controller
    {
        NTMContext db = new NTMContext();
        // GET: AdminUserTable
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult GetForumCategories()
        {
            var categories = db.ForumCategory.Select(x => new
            {
                x.CategoryName,
                x.IconClass,
                x.Order,
                HowManyTopics = x.ForumTopics.Count,
                x.ID
            }).ToList();

            return Json(new { data = categories }, JsonRequestBehavior.AllowGet);
        }
    }
}