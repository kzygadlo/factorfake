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
            ForumMain model = new ForumMain();

            model.Categories = db.ForumCategory.OrderBy(o => o.Order).ToList();
            model.Topics = db.ForumTopic.OrderBy(o => o.DateAdd).ToList();

            return View(model);
        }

        public ActionResult Topic(int ID)
        {
            var singleTopic = db.ForumTopic.Where(t => t.ID == ID).FirstOrDefault();
            return View(singleTopic);
        }

        [HttpGet]
        public ActionResult Add(int category)
        {
            if (User.IsInRole("Admin") || User.IsInRole("Moderator"))
            {
                ViewBag.catID = category;
                return View();    
            }
            else {
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

        public ActionResult Post(int id)
        {
            return View("Index");
        }
    }
}