using notomyk.DAL;
using notomyk.Models;
using notomyk.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace notomyk.Controllers
{
    public class ForumController : Controller
    {
        private NTMContext db = new NTMContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Topic(int ID)
        {
            var singleTopic = db.ForumTopic.Where(t => t.ID == ID).FirstOrDefault();
            return View(singleTopic);
        }

        [HttpGet]
        public ActionResult Add(int category = 1)
        {
            ViewBag.catID = category;
            return View();        
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

        public ActionResult Post(int id)
        {
            return View("Index");
        }
    }
}