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
using System.Configuration;

namespace notomyk.Controllers
{
    public class CommentController : Controller
    {
        private int MinCommentsForReputation = Convert.ToInt16(ConfigurationManager.AppSettings["MinCommentsForReputation"]);

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
                                          s.tbl_CommentID,
                                          s.Comment,
                                          s.DateAdd,
                                          s.ApplicationUser.Id,
                                          s.ApplicationUser.UserName,
                                          s.VoteCommentLogs,
                                          children = s.Children.Where(c => c.IsActive == true).Count(),
                                          voted = s.VoteCommentLogs.Where(v => v.UserId == uName).FirstOrDefault(),
                                          positiveCommentsCount = s.ApplicationUser.tbl_Comment.Where(c => c.IsActive == true && c.Fakt > c.Fake && (c.Fakt + c.Fake) > MinCommentsForReputation).Count(),
                                          commentsCount = s.ApplicationUser.tbl_Comment.Where(c => c.IsActive == true && (c.Fakt + c.Fake) > MinCommentsForReputation).Count()
                                      })
                                      .ToList();

                return Json(CommentsList.Select(x => new
                {
                    com = x.Comment,
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
                    positiveCommentsNumber = x.positiveCommentsCount,
                    allCommentsNumber = x.commentsCount,
                    reputationPoints = ReputationLogic.ReputationPercentage(x.positiveCommentsCount, x.commentsCount)
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

        public IQueryable<tbl_Comment> CommentListFiltered(int newsID, int filter)
        {
            var result = db.Comment.Include(i => i.Children).Where(c => c.IsActive == true && c.tbl_NewsID == newsID && c.Parenttbl_CommentID == null).AsQueryable();
            switch (filter)
            {
                case 0: // by date added
                    result = result.OrderBy(o => o.DateAdd);
                    break;
                case 1: // by author reputation
                    result = result.OrderBy(o => o.ApplicationUser.tbl_Comment.Where(c => c.IsActive == true && (c.Fakt + c.Fake) > MinCommentsForReputation).Count() == 0 ? 0 : o.ApplicationUser.tbl_Comment.Where(c => c.IsActive == true && c.Fakt > c.Fake && (c.Fakt + c.Fake) > MinCommentsForReputation).Count() / (1.0 * o.ApplicationUser.tbl_Comment.Where(c => c.IsActive == true && (c.Fakt + c.Fake) > MinCommentsForReputation).Count()));
                    break;
                case 2: // by votes
                    result = result.OrderBy(o => o.VoteCommentLogs.Where(v => v.Vote == true).Count() - o.VoteCommentLogs.Where(v => v.Vote == false).Count());
                    break;
                case 3:
                    break;
            }
            return result;
        }

        public JsonResult GetReplies(int parentID)
        {
            using (NTMContext db = new NTMContext())
            {
                
                var CommentsList = db.Comment.Where(c => c.Parenttbl_CommentID == parentID && c.IsActive == true).Select(
                    s => new
                    {
                        s.tbl_CommentID,
                        s.Comment,
                        s.DateAdd,
                        s.ApplicationUser.Id,
                        s.ApplicationUser.UserName,
                        s.VoteCommentLogs,
                        positiveCommentsCount = s.ApplicationUser.tbl_Comment.Where(c => c.IsActive == true && c.Fakt > c.Fake && (c.Fakt + c.Fake) > MinCommentsForReputation).Count(),
                        commentsCount = s.ApplicationUser.tbl_Comment.Where(c => c.IsActive == true && (c.Fakt + c.Fake) > MinCommentsForReputation).Count()                                      
                    }
                    ).OrderBy(o => o.DateAdd)
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
                    positiveCommentsNumber = x.positiveCommentsCount,
                    allCommentsNumber = x.commentsCount,
                    reputationPoints = ReputationLogic.ReputationPercentage(x.positiveCommentsCount, x.commentsCount)
                }), JsonRequestBehavior.AllowGet);
            }
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
                        var commentValidator = new addCommentValidator(_User, db);

                        var valResultN = commentValidator.IfExceededCommentsNumber();
                        if (valResultN == 0)
                        {
                            var valResultD = commentValidator.WhetherDelayTimeHasPassed();

                            if (valResultD == 0)
                            {
                                var comment = new tbl_Comment();
                                comment.Comment = CommentText;
                                comment.DateAdd = DateTime.UtcNow;
                                comment.UserId = _uID;
                                comment.tbl_NewsID = NewsID;

                                if (parentID != 0)
                                {
                                    comment.Parenttbl_CommentID = parentID;
                                }

                                var user = db.Users.Where(u => u.Id == comment.UserId).FirstOrDefault();
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
                                    positiveCommentsNumber = comment.ApplicationUser.tbl_Comment.Where(c => c.IsActive == true && c.Fakt > c.Fake && (c.Fakt + c.Fake) > MinCommentsForReputation).Count(),
                                    allCommentsNumber = comment.ApplicationUser.tbl_Comment.Where(c => c.IsActive == true && (c.Fakt + c.Fake) > MinCommentsForReputation).Count(),
                                    reputationPoints = ReputationLogic.ReputationPercentage(comment.ApplicationUser.tbl_Comment.Where(c => c.IsActive == true && c.Fakt > c.Fake && (c.Fakt + c.Fake) > MinCommentsForReputation).Count(), comment.ApplicationUser.tbl_Comment.Where(c => c.IsActive == true && (c.Fakt + c.Fake) > MinCommentsForReputation).Count())
                                });                                
                            }
                            else
                            {
                                return Json(
                                    new
                                    {
                                        success = false,
                                        errHeader = string.Format("Filtr anty-spamowy."),
                                        errMessage = string.Format("Musisz odczekać {0} sekund aby dodać nowy komentarz.", valResultD)
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