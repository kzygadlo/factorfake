using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using notomyk.DAL;
using notomyk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using notomyk.Infrastructure;


namespace notomyk.Controllers
{
    public class UserController : Controller
    {
        // GET: User

        [Authorize]
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {                
                var user = User.Identity;
                ViewBag.Name = user.Name;
                ViewBag.displayMenu = "No";

                if (AppConfig.isAdminUser())
                {
                    ViewBag.displayMenu = "Yes";
                    return View();
                }
                return RedirectToAction("Index", "Main");
            }
            else
            {
                return RedirectToAction("Index", "Main");
            }
        }

        public ActionResult UserDetails(string ID)
        {
            return View("UserDetails");
        }

    }
}