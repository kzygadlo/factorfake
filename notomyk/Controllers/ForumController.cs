using notomyk.DAL;
using notomyk.Models;
using notomyk.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using notomyk.Infrastructure;
using System.Configuration;

namespace notomyk.Controllers
{
    public class ForumController : Controller
    {
        private NTMContext db = new NTMContext();

        public ActionResult Index()
        {
            if (!cAppGlobal.IsAllowed("SiteEnabled"))
            {
                return RedirectToAction("Index", "Error", new { errorMessage = ErrorMessageGlobal.SiteEnabled });
            }

            if (Request.IsAuthenticated)
            {
                logLastActivity la = new logLastActivity(User.Identity.GetUserId());
            }

            var model = new ForumMain()
            {
                Categories = db.ForumCategory.OrderBy(o => o.Order).ToList(),
                Topics = db.ForumTopic.Where(t => t.IsActive == true).OrderBy(o => o.DateAdd).ToList()
            };

            ViewBag.Forum = "active";
            return View(model);
        }

        public ActionResult Topic(int ID)
        {
            if (Request.IsAuthenticated)
            {
                logLastActivity la = new logLastActivity(User.Identity.GetUserId());
            }
            var singleTopic = db.ForumTopic.Where(t => t.ID == ID).FirstOrDefault();

            if (Request.Cookies[string.Format("HasVisitedTopic:{0}", ID)] == null)
            {
                HttpCookie cookie = new HttpCookie(string.Format("HasVisitedTopic:{0}", ID), "true");
                cookie.Expires = DateTime.UtcNow.AddDays(30);
                Response.Cookies.Add(cookie);

                singleTopic.Visitors++;
                db.SaveChanges();
            }

            var fofUrl1 = ConfigurationManager.AppSettings["UrlAddress"];
 
            ViewBag.ogImage = imgUrl("/Images/Social/og-image.png", fofUrl1);
            var fbButtonUrl = HttpContext.Request.Url.AbsoluteUri;

            if (fbButtonUrl.IndexOf("?") > 0)
            {
                ViewBag.fbButtonUrl = fbButtonUrl.Substring(0, fbButtonUrl.IndexOf("?"));
            }
            else
            {
                ViewBag.fbButtonUrl = fbButtonUrl;
            }
            

            return View(singleTopic);
        }


        public string imgUrl(string url, string rootUrl)
        {
            string result = url;

            if (url.Contains("http") == true)
            {
                return url;
            }
            else
            {
                return string.Concat(rootUrl, url);
            }
        }

        [HttpGet]
        public ActionResult Add(int category)
        {
            if (User.IsInRole("Admin") || User.IsInRole("Moderator"))
            {
                ViewBag.catID = category;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Error", new { errorMessage = "Obecnie tylko moderatorzy mogą dodawać artykuły na forum." });
            }
        }

        [HttpPost]
        public ActionResult Add(int catID, string sub, string desc)
        {
            if (Request.IsAuthenticated)
            {
                var newTopic = new ForumTopic();
                var category = db.ForumCategory.Where(c => c.ID == catID).FirstOrDefault();
                newTopic.ForumCategory = category;
                newTopic.DateAdd = DateTime.UtcNow;
                newTopic.UserId = User.Identity.GetUserId();
                newTopic.Subject = sub;
                newTopic.Description = HttpUtility.HtmlDecode(desc);

                db.ForumTopic.Add(newTopic);

                db.SaveChanges();

                return RedirectToAction("Topic", "Forum", new { ID = newTopic.ID });
            }
            else
            {
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("Add", "Forum") });
            }

        }

        [HttpGet]
        public ActionResult Edit(int ID)
        {
            var model = db.ForumTopic.Where(t => t.ID == ID).FirstOrDefault();
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(int ID, int catID, string sub, string desc)
        {
            if (Request.IsAuthenticated)
            {
                var singleTopic = db.ForumTopic.Where(t => t.ID == ID).FirstOrDefault();

                singleTopic.ForumCategory.ID = catID;
                singleTopic.Subject = sub;

                singleTopic.Description = HttpUtility.HtmlDecode(desc);

                db.SaveChanges();
                return RedirectToAction("Topic", "Forum", new { ID = singleTopic.ID });
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpGet]
        public ActionResult Remove(int ID)
        {
            if (Request.IsAuthenticated)
            {
                if (User.IsInRole("Admin") || User.IsInRole("Moderator"))
                {
                    var article = db.ForumTopic.Where(t => t.ID == ID).FirstOrDefault();
                    article.IsActive = false;
                    db.SaveChanges();

                    return RedirectToAction("Index", "Forum");
                }
                else
                {
                    return RedirectToAction("Index", "Error", new { errorMessage = "Obecnie tylko moderatorzy mogą usuwać artykuły na forum." });
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
    }
}