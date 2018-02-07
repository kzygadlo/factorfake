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
    public class AdminSettingsController : Controller
    {
        NTMContext db = new NTMContext();


        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            ViewBag.AdminSettingsClass = "active";
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult GetSettings(string type)
        {
            var settings = db.AppSettings
                .Select(x => new {
                    x.ID,
                    x.Key,
                    x.Value,
                    x.Description
                }).ToList();

            return Json(new { data = settings }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Save(int ID)
        {
            if (db.AppSettings.Any(s => s.ID == ID))
            {
                var setting = db.AppSettings.Where(s => s.ID == ID).FirstOrDefault();
                return View(setting);
            }
            return View("Index");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Save(AppSettings settings)
        {
            bool status = false;
            if (ModelState.IsValid)
            {

                if (settings.ID != 0)
                {
                    var s = db.AppSettings.Where(x => x.ID == settings.ID).FirstOrDefault();

                    int value;
                    bool result = Int32.TryParse(settings.Value, out value);

                    if (result)
                    {
                        s.Value = settings.Value;
                        s.Description = settings.Description;
                        db.SaveChanges();
                        HttpRuntime.Cache.Remove("appSettings");
                        status = true;
                    }
                    else
                    {
                        return RedirectToAction("Index", "Error", new { errorMessage = ErrorMessage.ValueIsNotInt });
                    }                    
                }
            }

            if (status)
            {
                return RedirectToAction("Index", "AdminSettings");
            }
            else
            {
                return RedirectToAction("Index", "Error", new { errorMessage = ErrorMessage.AdminTableSaveFailed });
            }

        }
    }
}