using notomyk.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using notomyk.Models;
using System.Collections;

namespace notomyk.Controllers
{
    public class VotingController : Controller
    {
        // GET: Voting
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Vote(bool whatVote, int ID)
        {

            if (!User.Identity.IsAuthenticated)
            {
                return Json(new { msg = "Aby dodac glos musisz byc zalogowany." });
            }

            //checking if is already voted
            using (NTMContext db = new NTMContext())
            {
                var currentUserID = User.Identity.GetUserId();
                var isVoted = (dynamic)null;

                isVoted = db.VoteCommentLog.Where(c => c.tbl_CommentID == ID && c.UserId == currentUserID).FirstOrDefault();

                if (isVoted != null)
                {
                    if (isVoted.Vote == whatVote)
                    {
                        return Json(new { result = 0 });
                    }
                    else
                    {
                        var comment = db.Comment.Where(c => c.tbl_CommentID == ID).FirstOrDefault();
                        if (whatVote)
                        {
                            comment.Fakt++;
                            comment.Fake--;
                        }
                        else
                        {
                            comment.Fakt--;
                            comment.Fake++;
                        }


                        isVoted.Vote = whatVote;
                        isVoted.Timestamp = DateTime.UtcNow;
                        db.SaveChanges();
                        return Json(new { result = whatVote ? 2 : -2 });
                    }
                }
                else
                {
                    var singleVote = (dynamic)null;

                    singleVote = new VoteCommentLog();
                    singleVote.tbl_CommentID = ID;
                    singleVote.UserId = currentUserID;
                    singleVote.Vote = whatVote;
                    singleVote.Timestamp = DateTime.UtcNow;

                    var comment = db.Comment.Where(c => c.tbl_CommentID == ID).FirstOrDefault();
                    if (whatVote)
                    {
                        comment.Fakt++;
                    }
                    else
                    {
                        comment.Fake++;
                    }

                    db.VoteCommentLog.Add(singleVote);

                    db.SaveChanges();
                    return Json(new { result = whatVote ? 1 : -1 });
                }
            }
        }
    }
}