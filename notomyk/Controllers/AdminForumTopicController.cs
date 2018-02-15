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
    public class AdminForumTopicController : Controller
    {
        NTMContext db = new NTMContext();
        // GET: AdminUserTable
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            ViewBag.AdminForumTopicClass = "active";
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult GetForumTopics()
        {
            var topics = db.ForumTopic.Select(x => new
            {
                x.ApplicationUser.UserName,
                x.Description,
                x.IsActive,
                x.ID
            }).ToList();

            return Json(new { data = topics }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Save(int ID)
        {
            if (db.ForumTopic.Any(t => t.ID == ID))
            {
                var topic = db.ForumTopic.Where(t => t.ID == ID).FirstOrDefault();
                return View(topic);
            }
            return View("Index");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Save(ForumTopic topic)
        {
            bool status = false;
            if (ModelState.IsValid)
            {
                if (topic.ID != 0)
                {
                    var singleTopic = db.ForumTopic.Where(t => t.ID == topic.ID).FirstOrDefault();

                    singleTopic.Subject = topic.Subject;
                    singleTopic.IsActive = topic.IsActive;
                    singleTopic.OnMainPage = topic.OnMainPage;
                    singleTopic.IsReported = topic.IsReported;
                    
                    db.SaveChanges();
                    status = true;
                }
            }

            if (status)
            {
                return RedirectToAction("Index", "AdminForumTopic");
            }
            else
            {
                return RedirectToAction("Index", "Error", new { errorMessage = ErrorMessage.GeneralError });
            }

        }
    }
}