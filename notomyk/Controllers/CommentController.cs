using notomyk.Infrastructure;
using notomyk.Models;
using notomyk.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using notomyk.DAL;
using System.Data.Entity;
using System.Web.Security;

namespace notomyk.Controllers
{
    public class CommentController : Controller
    {

        // GET: Comment
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult Get(int newsID)
        {
            try
            {
                using (NTMContext db = new NTMContext())
                {

                    if (!Request.IsAuthenticated)
                    {
                        ViewBag.popupMsg = "zaloguj sie";
                    }

                    var CommentsList = db.Comment.Include(c => c.Children)
                                          .Where(c => c.tbl_NewsID == newsID && c.Parenttbl_CommentID == null && c.IsActive == true)
                                          .Select(s => new
                                          {
                                              s.tbl_CommentID,
                                              s.Comment,
                                              s.DateAdd,
                                              s.ApplicationUser.Id,
                                              s.ApplicationUser.UserName,
                                              s.VoteCommentLogs,
                                              children = s.Children.Count()
                                          })
                                          .ToList();

                    return Json(CommentsList.Select(x => new
                    {
                        com = x.Comment,
                        cid = x.tbl_CommentID,
                        date = GetTimeAgo.CalculateDateDiff(x.DateAdd),
                        userN = x.UserName,
                        userL = Url.Content(AppConfig.UserLogoLink(x.Id)),
                        faktV = x.VoteCommentLogs.Where(c => c.Vote == true).Count(),
                        fakeV = x.VoteCommentLogs.Where(c => c.Vote == false).Count(),
                        repliesV = x.children
                    }), JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { success = false });
            }

        }

        public JsonResult GetReplies(int parentID)
        {
            //try
            //{
            using (NTMContext db = new NTMContext())
            {
                var CommentsList = db.Comment.Where(c => c.Parenttbl_CommentID == parentID).Select(
                    s => new
                    {
                        s.tbl_CommentID,
                        s.Comment,
                        s.DateAdd,
                        s.ApplicationUser.Id,
                        s.ApplicationUser.UserName,
                        s.VoteCommentLogs
                    }
                    ).ToList();

                return Json(CommentsList.Select(x => new
                {
                    com = x.Comment,
                    cid = x.tbl_CommentID,
                    date = GetTimeAgo.CalculateDateDiff(x.DateAdd),
                    userN = x.UserName,
                    userL = Url.Content(AppConfig.UserLogoLink(x.Id)),
                    faktV = x.VoteCommentLogs.Where(c => c.Vote == true).Count(),
                    fakeV = x.VoteCommentLogs.Where(c => c.Vote == false).Count(),
                }), JsonRequestBehavior.AllowGet);
            }
            //}
            //catch
            //{
            //    return Json(new { success = false });
            //}

        }

        [HttpPost]
        public ActionResult Add(string CommentText, int NewsID, int parentID = 0)
        {
            if (Request.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        using (NTMContext db = new NTMContext())
                        {
                            var comment = new tbl_Comment();

                            comment.Comment = CommentText;
                            comment.DateAdd = DateTime.UtcNow;
                            comment.UserId = User.Identity.GetUserId();
                            comment.tbl_NewsID = NewsID;

                            if (parentID != 0)
                            {
                                comment.Parenttbl_CommentID = parentID;
                            }


                            var user = db.Users.Where(u => u.Id == comment.UserId).FirstOrDefault();
                            //var news = db.News.Where(n => n.tbl_NewsID == NewsID).FirstOrDefault();

                            //user.commentsNumber++;
                            //news.Comments++;

                            db.Comment.Add(comment);
                            db.SaveChanges();

                            return Json(new
                            {
                                success = true,
                                com = comment.Comment,
                                cid = comment.tbl_CommentID,
                                date = GetTimeAgo.CalculateDateDiff(comment.DateAdd),
                                userN = user.UserName,
                                userL = Url.Content(AppConfig.UserLogoLink(user.Id))
                            });
                        }
                    }
                    catch
                    {
                        return Json(new { success = false });
                    }
                }
            }
            else
            {
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("News", "Main", new { ID = NewsID }) });
            }
            return null;
        }

        [HttpPost]
        public ActionResult Remove(int commentID)
        {

            if (User.IsInRole("Admin") || User.IsInRole("Moderator") || myUser.IsCommentAuthor(commentID, User.Identity.GetUserId()))
            {
                using (NTMContext db = new NTMContext())
                {
                    var comment = db.Comment.Where(c => c.tbl_CommentID == commentID).FirstOrDefault();

                    comment.IsActive = false;
                    db.SaveChanges();

                    return Json(new { Success = true });
                }

            }
            else
            {
                return Json(new
                {
                    Success = false,
                    ResultMsg = "Nie masz uprawnień aby usunąć ten komentarz."
                });
            }


        }
    }
}