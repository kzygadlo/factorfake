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
        public ActionResult GetComments()
        {
            var comments = db.Comment.Select(x => new {
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