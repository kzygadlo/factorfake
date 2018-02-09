using notomyk.DAL;
using notomyk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace notomyk.Controllers
{
    public class AdminBlackListController : Controller
    {
        NTMContext db = new NTMContext();
        // GET: AdminUserTable
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            ViewBag.AdminBlackListClass = "active";
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult SaveAdd(BlackList blackList)
        {
            if (db.BlackList.Any(b => b.url == blackList.url))
            {
                ViewBag.Result = "Podany url juz jest dodany.";
            }
            else
            {
                var blackListNew = new BlackList();
                blackListNew.url = blackList.url;
                db.BlackList.Add(blackListNew);
                db.SaveChanges();

                ViewBag.Result = "Dodano";
            }

            return View("Index");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult SaveRemove(BlackList blackList)
        {
            if (db.BlackList.Any(b => b.url == blackList.url))
            {
                var bl = db.BlackList.Where(b => b.url == blackList.url).FirstOrDefault();
                db.BlackList.Remove(bl);
                db.SaveChanges();

                ViewBag.Result = string.Format("Usunieto: {0}", blackList.url);
            }
            else
            {
                ViewBag.Result = "Nie istnieje taki url w BlackList";
            }

            return View("Index");
        }
    }
}