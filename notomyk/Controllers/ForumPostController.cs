using notomyk.DAL;
using notomyk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using notomyk.Infrastructure;

namespace notomyk.Controllers
{
    public class ForumPostController : Controller
    {
        private NTMContext db = new NTMContext();
        // GET: ForumComment

        [HttpGet]
        public JsonResult Get(int TopicID, int? filter)
        {
            var userID = User.Identity.GetUserId();
            var postList = db.ForumPost.Where(x => x.Topic.ID == TopicID && x.IsActive == true && x.Parent == null).ToList();

            return Json(postList.Select(x => new
            {
                postID = x.ID,
                post = x.Content,
                dateAdd = GetTimeAgo.CalculateDateDiff(x.DateAdd),
                userName = x.ApplicationUser.UserName,
                userLogoLink = Url.Content(AppConfig.UserLogoLink(x.ApplicationUser.Id)),
                repliesNumber = x.Children.Where(c => c.IsActive == true).Count()
            }), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Add(int TopicID, string CommentText, int ParentID = 0)
        {
            if (Request.IsAuthenticated)
            {
                var post = new ForumPost();
                var userID = User.Identity.GetUserId();
                var Topic = db.ForumTopic.Where(t => t.ID == TopicID).FirstOrDefault();
                var activeUser = db.Users.Where(u => u.Id == userID).FirstOrDefault();

                post.ApplicationUser = activeUser;
                post.Topic = Topic;
                post.Content = CommentText;
                post.DateAdd = DateTime.UtcNow;
                post.DateModify = DateTime.UtcNow;

                if (ParentID != 0)
                {
                    var parent = db.ForumPost.Where(p => p.ID == ParentID).FirstOrDefault();
                    post.Parent = parent;
                }

                db.ForumPost.Add(post);
                db.SaveChanges();

                return Json(new
                {
                    success = true,
                    postID = post.ID,
                    post = post.Content,
                    dateAdd = GetTimeAgo.CalculateDateDiff(post.DateAdd),
                    userName = post.ApplicationUser.UserName,
                    userLogoLink = Url.Content(AppConfig.UserLogoLink(userID))
                });
            }
            else
            {
                return Json(new
                {
                    success = false,
                    errHeader = string.Format("Dodawanie komentarza."),
                    errMessage = string.Format("Tylko zalogowani użytkownicy mogą dodawać komentarze.")
                });
            }
        }

        [HttpPost]
        public ActionResult Remove(int postID)
        {
            if (User.IsInRole("Admin") || User.IsInRole("Moderator") || myUser.IsPostAuthor(postID, User.Identity.GetUserId()))
            {
                using (NTMContext db = new NTMContext())
                {
                    var post = db.ForumPost.Where(p => p.ID == postID).FirstOrDefault();
                    post.IsActive = false;
                    db.SaveChanges();

                    return Json(new { success = true });
                }
            }
            else
            {
                return Json(new
                {
                    success = false,
                    errHeader = string.Format("Usuwanie komentarza."),
                    errMessage = string.Format("Nie masz uprawnień aby usunąć ten komentarz. Tylko moderatorzy oraz autorzy komentarza mogą go usunąć.")
                });
            }
        }

        [HttpPost]
        public JsonResult Report(int postID, bool ToReport)
        {
            if (Request.IsAuthenticated)
            {
                var post = db.ForumPost.Where(p => p.ID == postID).FirstOrDefault();
                post.IsReported = ToReport;
                db.SaveChanges();

                return Json(new
                {
                    Success = true,
                    errMessage = "Komentarz został zgłoszony."
                });
            }
            else
            {
                return Json(new
                {
                    Success = false,
                    errMessage = "Tylko zalogowani użytkownicy mogą zgłaszać komentarze."
                });
            }
        }

        [HttpGet]
        public JsonResult GetReplies(int parentID)
        {
            var replies = db.ForumPost.Where(p => p.IsActive == true && p.Parent.ID == parentID).ToList();

            return Json(replies.Select(x => new { 
                        postID = x.ID,
                        post = x.Content,
                        dateAdd = GetTimeAgo.CalculateDateDiff(x.DateAdd),
                        logoName = Url.Content(AppConfig.UserLogoLink(x.ApplicationUser.Id)),
                        userName = x.ApplicationUser.UserName
            }), JsonRequestBehavior.AllowGet);
        }
    }
}