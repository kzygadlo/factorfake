﻿using notomyk.Infrastructure;
using notomyk.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using notomyk.DAL;
using System.Data.Entity;

namespace notomyk.Controllers
{
    public class CommentController : Controller
    {
        private NTMContext db = new NTMContext();

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult Get(int newsID, int filter)
        {
            using (NTMContext db = new NTMContext())
            {
                if (!Request.IsAuthenticated)
                {
                    ViewBag.popupMsg = "zaloguj sie";
                }

                var uName = User.Identity.GetUserId();

                var CFiltered = CommentListFiltered(newsID, filter);
                var CommentsList = CFiltered
                                      .Select(s => new
                                      {
                                          ApplicationUser = s.ApplicationUserAutor,
                                          s.tbl_CommentID,
                                          comment = s.IsActive == true ? s.Comment : "",
                                          s.DateAdd,
                                          s.ApplicationUserAutor.Id,
                                          s.ApplicationUserAutor.UserName,
                                          s.VoteCommentLogs,
                                          children = s.Children.Where(c => c.IsActive == true).Count(),
                                          voted = s.VoteCommentLogs.Where(v => v.UserId == uName).FirstOrDefault(),
                                          whatVoteForNews = s.ApplicationUserAutor.VotingLogs.Where(n => n.tbl_NewsID == newsID).Select(x => x.Vote).FirstOrDefault(),
                                          commentBasicClass = s.IsActive == true ? "" : "hidden",
                                          commentRemovedClass = s.IsActive == true ? "hidden" : "",
                                          reportedClass = s.IsReported == true ? "" : "hidden"
                                      })
                                      .ToList();

                return Json(CommentsList.Select(x => new
                {                    
                    com = x.comment,
                    cid = x.tbl_CommentID,
                    date = GetTimeAgo.CalculateDateDiff(x.DateAdd),
                    userN = x.UserName,
                    userL = Url.Content(AppConfig.UserLogoLink(x.Id)),
                    userPositiveCoummentCount = 1,
                    userCommentsCount = 10,
                    userReputation = 20,
                    faktV = x.VoteCommentLogs.Where(c => c.Vote == true).Count(),
                    fakeV = x.VoteCommentLogs.Where(c => c.Vote == false).Count(),
                    repliesV = x.children,
                    voteForComment = ifVoted(x.voted),
                    positiveCommentsNumber = x.ApplicationUser.PostitiveCommentsCount(),
                    allCommentsNumber = x.ApplicationUser.AllCommentsCount(),
                    reputationPoints = x.ApplicationUser.Reputation(),
                    whatVote = x.whatVoteForNews,
                    commentBasicClass = x.commentBasicClass,
                    commentRemovedClass = x.commentRemovedClass,
                    reportedClass = x.reportedClass
                }), JsonRequestBehavior.AllowGet);
            }
        }

        public static int ifVoted(VoteCommentLog voted)
        {
            int result = 1;

            if (voted != null)
            {
                if (voted.Vote == true)
                {
                    result = 1;
                }
                else
                {
                    result = 0;
                }
            }
            else
            {
                result = -1;
            }

            return result;
        }

        public IEnumerable<tbl_Comment> CommentListFiltered(int newsID, int filter)
        {
            var result = db.Comment.Include(i => i.Children).Where(c => (c.IsActive == true || c.Children.Count > 0) && c.tbl_NewsID == newsID && c.Parenttbl_CommentID == null).ToList();
            
            return CommentsSorting(result,filter);
        }

        public IEnumerable<tbl_Comment> CommentsSorting(IEnumerable<tbl_Comment> comments, int filter)
        {
            switch (filter)
            {
                case 0: // by date added
                    comments = comments.OrderBy(o => o.DateAdd);
                    break;
                case 1: // by author reputation
                    comments = comments.OrderBy(o => o.ApplicationUserAutor.Reputation());
                    break;
                case 2: // by votes
                    comments = comments.OrderBy(o => o.VoteCommentLogs.Where(v => v.Vote == true).Count() - o.VoteCommentLogs.Where(v => v.Vote == false).Count());
                    break;
                case 3:
                    break;
            }

            return comments;
        }


        public JsonResult GetReplies(int parentID)
        {
            var uName = User.Identity.GetUserId();

            var CommentsList = db.Comment.Where(c => c.Parenttbl_CommentID == parentID && c.IsActive == true).Select(
                s => new
                {
                    ApplicationUser = s.ApplicationUserAutor,
                    s.tbl_CommentID,
                    comment = s.IsActive == true ? s.Comment : "",
                    s.DateAdd,
                    s.ApplicationUserAutor.Id,
                    s.ApplicationUserAutor.UserName,
                    s.VoteCommentLogs,
                    voted = s.VoteCommentLogs.Where(v => v.UserId == uName).FirstOrDefault(),
                    reportedClass = s.IsReported == true ? "" : "hidden"
                }
                ).OrderByDescending(o => o.DateAdd)
                 .ToList();

            return Json(CommentsList.Select(x => new
            {
                com = x.comment,
                cid = x.tbl_CommentID,
                date = GetTimeAgo.CalculateDateDiff(x.DateAdd),
                userN = x.UserName,
                userL = Url.Content(AppConfig.UserLogoLink(x.Id)),
                faktV = x.VoteCommentLogs.Where(c => c.Vote == true).Count(),
                fakeV = x.VoteCommentLogs.Where(c => c.Vote == false).Count(),
                voteForComment = ifVoted(x.voted),
                positiveCommentsNumber = x.ApplicationUser.PostitiveCommentsCount(),
                allCommentsNumber = x.ApplicationUser.AllCommentsCount(),
                reputationPoints = x.ApplicationUser.Reputation(),
                reportedClass = x.reportedClass
            }), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Add(string CommentText, int NewsID, int parentID = 0)
        {
            if (Request.IsAuthenticated)
            {                
                if (ModelState.IsValid)
                {
                    using (NTMContext db = new NTMContext())
                    {
                        var _uID = User.Identity.GetUserId();
                        var _User = db.Users.Where(u => u.Id == _uID).FirstOrDefault();

                        if (_User.IsBanned()) {
                            return Json(
                                    new
                                    {
                                        success = false,
                                        errHeader = string.Format("Blokada konta."),
                                        errMessage = string.Format("Konto użytkownika {0} zostało zablokowane na {1}", _User.UserName, GetTimeAgo.CalculateDateDiffAhead(_User.LockoutEndDateUtc))
                                    });
                        }

                        var commentValidator = new addCommentValidator(_User, db);
                                                
                        var valResultN = commentValidator.IfExceededCommentsNumber();
                        if (valResultN == 0)
                        {
                            var valResultD = commentValidator.WhetherDelayTimeHasPassed();

                            if (valResultD == 0)
                            {
                                var comment = new tbl_Comment();
                                var userReference = db.Users.Where(u => u.Id == _uID).FirstOrDefault();
                                comment.Comment = CommentText;
                                comment.DateAdd = DateTime.UtcNow;
                                comment.ApplicationUserAutor = userReference;
                                comment.tbl_NewsID = NewsID;

                                if (parentID != 0)
                                {
                                    comment.Parenttbl_CommentID = parentID;
                                }

                                var user = db.Users.Where(u => u.Id == comment.ApplicationUserAutor.Id).FirstOrDefault();
                                db.Comment.Add(comment);
                                db.SaveChanges();

                                commentValidator.CommentAdded(_User, db);

                                return Json(new
                                {
                                    success = true,
                                    com = comment.Comment,
                                    cid = comment.tbl_CommentID,
                                    date = GetTimeAgo.CalculateDateDiff(comment.DateAdd),
                                    userN = user.UserName,
                                    userL = Url.Content(AppConfig.UserLogoLink(user.Id)),
                                    positiveCommentsNumber = comment.ApplicationUserAutor.PostitiveCommentsCount(),
                                    allCommentsNumber = comment.ApplicationUserAutor.AllCommentsCount(),
                                    reputationPoints = comment.ApplicationUserAutor.Reputation()
                                });
                            }
                            else
                            {
                                return Json(
                                    new
                                    {
                                        success = false,
                                        errHeader = string.Format("Filtr anty-spamowy."),
                                        errMessage = string.Format("Musisz odczekać jeszcze {0} sekund aby dodać nowy komentarz.", Convert.ToInt16(commentValidator.timeToGO))
                                    });
                                // time dealy 
                            }
                        }
                        else
                        {
                            if (commentValidator.EmailConfirmed == true)
                            {
                                return Json(
                                    new
                                    {
                                        success = false,
                                        errHeader = string.Format("Przekroczona dzienna liczba dodanych komentarzy ({0}) dla Twojej roli.", valResultN),
                                        errMessage = string.Format("Jutro blokada zostanie zdjęta..")
                                    });
                            }
                            else
                            {
                                return Json(
                                        new
                                        {
                                            success = false,
                                            errHeader = string.Format("Przekroczona liczba komentarzy dla Twojej roli."),
                                            errMessage = string.Format("Aktywuj swoje konto aby móc dodawać większą ilość komentarzy.")
                                        });
                            }
                            // total number of comments exceeded
                        }
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

                    return Json(new
                    {
                        success = true,
                        childComments = comment.Children.Where(c => c.IsActive == true).Count()
                    });
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
        public JsonResult Report(int commentID, bool ToReport)
        {
            if (Request.IsAuthenticated)
            {
                var comment = db.Comment.Where(c => c.tbl_CommentID == commentID).FirstOrDefault();
                comment.IsReported = ToReport;
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
    }
}