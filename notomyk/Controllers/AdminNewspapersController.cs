using notomyk.DAL;
using notomyk.Infrastructure;
using notomyk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace notomyk.Controllers
{
    public class AdminNewspapersController : Controller
    {
        // GET: AdminNewspapers
        NTMContext db = new NTMContext();
        // GET: AdminUserTable
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            ViewBag.AdminNewspapersClass = "active";
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult GetNewspapers()
        {
            var newspapers = db.Newspaper.Select(n => new {                
                n.NewspaperName,
                n.NewspaperLink,
                n.NewspaperIconLink,
                n.IsActive,
                News = string.Concat(n.tbl_NewspaperID, ";", n.Colection_Newses.Count),
                n.tbl_NewspaperID
            }).ToList();

            return Json(new { data = newspapers }, JsonRequestBehavior.AllowGet);

        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Save(int id)
        {
            if (id != 0)
            {
                var newspaper = db.Newspaper.Where(n => n.tbl_NewspaperID == id).FirstOrDefault();
                return View(newspaper);
            }
            return View("Index");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Save(tbl_Newspaper newspaper)
        {
            bool status = false;
            if (ModelState.IsValid)
            {
                if (newspaper.tbl_NewspaperID!= 0)
                {
                    var n = db.Newspaper.Where(x => x.tbl_NewspaperID == newspaper.tbl_NewspaperID).FirstOrDefault();

                    n.NewspaperName = newspaper.NewspaperName;
                    n.NewspaperLink = newspaper.NewspaperLink;
                    n.NewspaperIconLink = newspaper.NewspaperIconLink;
                    n.IsActive = newspaper.IsActive;

                    db.SaveChanges();
                    status = true;
                }
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors);

            if (status)
            {
                return RedirectToAction("Index", "AdminNewspapers");
            }
            else
            {
                return RedirectToAction("Index", "Error", new { errorMessage = ErrorMessage.AdminTableSaveFailed });
            }

        }
    }
}