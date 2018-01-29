using notomyk.DAL;
using notomyk.Infrastructure;
using notomyk.Models;
using notomyk.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace notomyk.Controllers
{
    public class AdminController : Controller
    {
        private NTMContext db = new NTMContext();
        // GET: Admin

        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                {
                    var model = new AdminModel();
                    model.AppSettings = db.AppSettings.ToList();
                    return View(model);
                }
                else
                {
                    return RedirectToAction("Index", "Error", new { errorMessage = ErrorMessage.YouAreNotAdmin });
                }               

            }
            else
            {
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("Index", "Admin") });
            }
        }


        public ActionResult UserRoles()
        {
            if (Request.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                {
                    var model = new AdminModel();
                    model.Users = db.Users.ToList();
                    return View(model);

                    
                }
                else
                {
                    return RedirectToAction("Index", "Error", new { errorMessage = ErrorMessage.YouAreNotAdmin });
                }
            }
            else
            {
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("Index", "Admin") });
            }
            
        }

        [HttpPost]
        public ActionResult AppSettings(string key, string value)
        {

            var setting = db.AppSettings.Where(s => s.Key == key).FirstOrDefault();
            setting.Value = value;
            db.SaveChanges();

            return RedirectToAction("AppSettings");
        }


        
    }
}