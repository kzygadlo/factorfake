using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using notomyk.DAL;
using Microsoft.AspNet.Identity;
using notomyk.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using notomyk.Infrastructure;

namespace notomyk.Controllers
{
    public class RoleController : Controller
    {

        NTMContext context;

        public RoleController()
        {
            context = new NTMContext();
        }
        // GET: Role
        public ActionResult Index()
        {
            NTMContext db = new NTMContext();

            if (User.Identity.IsAuthenticated)
            {
                if (AppConfig.isAdminUser() == false)
                {
                    return RedirectToAction("Index", "Main");
                }
            }
            else
            {
                return RedirectToAction("Index", "Main");
            }

            var Roles = db.Roles.ToList();
            return View(Roles);   
        }

        public ActionResult Create()
        {
            if (User.Identity.IsAuthenticated)
            {

                if (AppConfig.isAdminUser() == false)
                {
                    return RedirectToAction("Index", "Main");
                }
            }
            else
            {
                return RedirectToAction("Index", "Main");
            }

            var Role = new IdentityRole();
            return View(Role);
        }


        [HttpPost]
        public ActionResult Create(IdentityRole Role)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (AppConfig.isAdminUser() == false)
                {
                    return RedirectToAction("Index", "Main");
                }
            }
            else
            {
                return RedirectToAction("Index", "Main");
            }

            context.Roles.Add(Role);
            context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}