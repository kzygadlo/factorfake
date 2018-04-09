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
    public class AdminNewsController : Controller
    {
        NTMContext db = new NTMContext();
        // GET: AdminUserTable
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            ViewBag.AdminNewsClass = "active";
            ViewBag.NewspaperID = 0;
            ViewBag.UserID = "";
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult IndexUserID(string id)
        {
            ViewBag.AdminNewsClass = "active";
            ViewBag.NewspaperID = 0;
            ViewBag.UserID = id;
            return View("Index");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult IndexNewspaperID(int id)
        {
            ViewBag.AdminNewsClass = "active";
            ViewBag.NewspaperID = id;
            ViewBag.UserID = 0;
            return View("Index");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult GetNewses(int newspaperID, string userID)
        {
            var n = db.News.OrderByDescending(o => o.DateAdd).AsQueryable();

            if (newspaperID != 0)
            {
                n = n.Where(c => c.tbl_NewspaperID == newspaperID);
            }
            else if (userID != "")
            {
                n = db.News.Where(c => c.ApplicationUser.Id == userID);
            }

            var newses = n.Select(x => new
            {
                x.Newspaper.NewspaperName,
                x.Title,
                x.IsReported,
                x.IsActive,
                x.DateAdd,
                x.Visitors,
                faktValue = x.VoteLogs.Where(v => v.Vote == 1).Count(),
                fakeValue = x.VoteLogs.Where(v => v.Vote == -1).Count(),
                Comm = x.Collection_Comments.Count,
                x.tbl_NewsID
            }).ToList();

            var newsesFinal = newses.Select(x => new
            {
                x.NewspaperName,
                x.Title,
                x.IsReported,
                x.IsActive,
                DateAdd = ConvertToString.Date(x.DateAdd),
                x.Visitors,
                x.faktValue,
                x.fakeValue,
                Comm = string.Concat(x.tbl_NewsID, ";", x.Comm),
                x.tbl_NewsID
            }).ToList();

            return Json(new { data = newsesFinal }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Save(int id)
        {
            if (id != 0)
            {
                var news = db.News.Where(c => c.tbl_NewsID == id).FirstOrDefault();
                return View(news);
            }
            return View("Index");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Save(tbl_News news)
        {
            bool status = false;
            if (ModelState.IsValid)
            {
                if (news.tbl_NewsID != 0)
                {
                    var n = db.News.Where(x => x.tbl_NewsID == news.tbl_NewsID).FirstOrDefault();

                    n.Title = news.Title;
                    n.Description = news.Description;
                    n.ArticleLink = news.ArticleLink;
                    n.PictureLink = news.PictureLink;
                    n.IsReported = news.IsReported;
                    n.IsActive = news.IsActive;

                    db.SaveChanges();
                    status = true;
                }
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors);

            if (status)
            {
                return RedirectToAction("Index", "AdminNews");
            }
            else
            {
                return RedirectToAction("Index", "Error", new { errorMessage = ErrorMessage.AdminTableSaveFailed });
            }

        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Remove(int ID)
        {
            if (db.News.Any(n => n.tbl_NewsID == ID))
            {
                var news = db.News.Where(n => n.tbl_NewsID == ID).FirstOrDefault();
                return View(news);
            }

            return RedirectToAction("Index", "Error", new { errorMessage = ErrorMessage.ItemDoesntExist });

        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Remove(tbl_News news)
        {
            bool status = false;
            news.Description = "";
            news.Title = "";
            news.ArticleLink = "";

            if (ModelState.IsValid)
            {
                if (db.News.Any(n => n.tbl_NewsID == news.tbl_NewsID))
                {
                    var newsToDelete = db.News.Where(n => n.tbl_NewsID == news.tbl_NewsID).FirstOrDefault();
                    db.News.Remove(newsToDelete);
                    db.SaveChanges();
                    status = true;
                }
                else {
                    return RedirectToAction("Index", "Error", new { errorMessage = ErrorMessage.ItemDoesntExist });
                }
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors);

            if (status)
            {
                return RedirectToAction("Index", "AdminNews");
            }
            else
            {
                return RedirectToAction("Index", "Error", new { errorMessage = ErrorMessage.AdminTableSaveFailed });
            }

        }
    }
}