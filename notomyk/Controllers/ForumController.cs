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

        [HttpGet]
        public ActionResult Add()
        {
            var AddComment = new AddForumTopic();
            AddComment.Categories = db.ForumCategory.ToList();

            return View(AddComment);        
        }

        [HttpPost]
        public JsonResult Add(int catID, string sub, string desc)
        {
            if (Request.IsAuthenticated)
            {
                var newTopic = new ForumTopic();
                var category = db.ForumCategory.Where(c => c.ID == catID).FirstOrDefault();
                newTopic.ForumCategory = category;
                newTopic.DateAdd = DateTime.UtcNow;
                newTopic.UserId = User.Identity.GetUserId();
                newTopic.Subject = sub;
                newTopic.Description = desc;
                db.ForumTopic.Add(newTopic);
                db.SaveChanges();

                return Json(new
                {
                    success = true,
                    errHeader = string.Format("Wpis został dodany."),
                    errMessage = string.Format(string.Format("Kagegoria: {0}, Tytuł: {1}", category.CategoryName, sub))
                });
            }
            else
            {
                return Json(new
                {
                    success = false,
                    errHeader = string.Format("Dodawanie Wpisu."),
                    errMessage = string.Format("Nie masz uprawnień aby umieścić nowy wpis.")
                }); 
            }            
            
        }

        public ActionResult Post(int id)
        {
            return View("Index");
        }
    }
}