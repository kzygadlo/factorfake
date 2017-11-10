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

        public JsonResult Vote(bool whatVote, int ID, bool newsVote = true)
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

                if (newsVote == true)
                {
                    isVoted = db.VoteLog.Where(v => v.tbl_NewsID == ID && v.UserId == currentUserID).FirstOrDefault();
                }
                else
                {
                    isVoted = db.VoteCommentLog.Where(c => c.tbl_CommentID == ID && c.UserId == currentUserID).FirstOrDefault();
                }

                if (isVoted != null)
                {
                    if (isVoted.Vote == whatVote)
                    {
                        return Json(new {result = 0 });
                    }
                    else
                    {
                        isVoted.Vote = whatVote;
                        db.SaveChanges();
                        return Json(new {result = whatVote? 2 : -2 });
                    }
                }
                else
                {
                    var singleVote = (dynamic)null; 
                    if (newsVote == true)
                    {
                        singleVote = new VoteLog();
                        singleVote.tbl_NewsID = ID;
                        singleVote.UserId = currentUserID;
                        singleVote.Vote = whatVote;
                        db.VoteLog.Add(singleVote);
                    }
                    else
                    {
                        singleVote = new VoteCommentLog();
                        singleVote.tbl_CommentID = ID;
                        singleVote.UserId = currentUserID;
                        singleVote.Vote = whatVote;
                        db.VoteCommentLog.Add(singleVote);
                    }

                    db.SaveChanges();
                    return Json(new {result = whatVote ? 1 : -1 });
                }
            }
            return Json("ok");
        }
    }
}