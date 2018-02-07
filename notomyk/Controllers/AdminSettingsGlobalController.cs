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
    public class AdminSettingsGlobalController : Controller
    {
        NTMContext db = new NTMContext();


        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            ViewBag.AdminSettingsGlobalClass = "active";
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult GetSettings(string type)
        {
            var settingsGlobal = db.AppSettingsGlobal
                .OrderBy(o => o.order)
                .Select(x => new
                {
                    x.ID,
                    x.Key,
                    x.Value,
                    x.Description
                }).ToList();

            return Json(new { data = settingsGlobal }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Save(int ID)
        {
            if (db.AppSettingsGlobal.Any(s => s.ID == ID))
            {
                var settingGlobal = db.AppSettingsGlobal.Where(s => s.ID == ID).FirstOrDefault();
                return View(settingGlobal);
            }
            else
            {
                var settingNew = new AppSettingsGlobal();
                return View(settingNew);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Save(AppSettingsGlobal settingsGlobal)
        {
            bool status = false;
            if (ModelState.IsValid)
            {
                if (settingsGlobal.ID != 0)
                {
                    var s = db.AppSettingsGlobal.Where(x => x.ID == settingsGlobal.ID).FirstOrDefault();

                    s.Value = settingsGlobal.Value;
                    s.Description = settingsGlobal.Description;
                    db.SaveChanges();
                    HttpRuntime.Cache.Remove("appSettingsGlobal");
                    status = true;
                }
                else {

                    if (settingsGlobal.Key != "" && settingsGlobal.Value)
                    {
                        db.AppSettingsGlobal.Add(settingsGlobal);
                        db.SaveChanges();
                        HttpRuntime.Cache.Remove("appSettingsGlobal");
                        status = true;
                    }
                    else {
                        return RedirectToAction("Index", "Error", new { errorMessage = ErrorMessage.FieldsRequired });
                    }
                }

            }

            if (status)
            {
                return RedirectToAction("Index", "AdminSettingsGlobal");
            }
            else
            {
                return RedirectToAction("Index", "Error", new { errorMessage = ErrorMessage.AdminTableSaveFailed });
            }

        }
    }
}