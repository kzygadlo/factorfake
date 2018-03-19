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
    public class VotingNewsController : Controller
    {
        // GET: Voting
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Vote(int whatVote, int ID)
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

                isVoted = db.VoteLog.Where(v => v.tbl_NewsID == ID && v.UserId == currentUserID).FirstOrDefault();

                if (isVoted != null)
                {
                    if (isVoted.Vote == whatVote)
                    {
                        isVoted.Vote = 0;
                        db.SaveChanges();
                        var votes = db.VoteLog.Where(v => v.tbl_NewsID == ID);
                        return Json(new
                        {
                            result = 0,
                            faktVote = votes.Where(v => v.Vote == 1).Count(),
                            fakeVote = votes.Where(v => v.Vote == -1).Count(),
                            manipulatedVote = votes.Where(v => v.Vote == 2).Count()
                        });
                    }
                    else
                    {
                        isVoted.Vote = whatVote;
                        isVoted.Timestamp = DateTime.UtcNow;
                        db.SaveChanges();

                        var votes = db.VoteLog.Where(v => v.tbl_NewsID == ID);
                        return Json(new
                        {
                            result = whatVote,
                            faktVote = votes.Where(v => v.Vote == 1).Count(),
                            fakeVote = votes.Where(v => v.Vote == -1).Count(),
                            manipulatedVote = votes.Where(v => v.Vote == 2).Count()
                        });
                    }
                }
                else
                {
                    var singleVote = (dynamic)null;

                    singleVote = new VoteLog();
                    singleVote.tbl_NewsID = ID;
                    singleVote.UserId = currentUserID;
                    singleVote.Vote = whatVote;
                    singleVote.Timestamp = DateTime.UtcNow;
                    db.VoteLog.Add(singleVote);

                    db.SaveChanges();

                    var votes = db.VoteLog.Where(v => v.tbl_NewsID == ID);
                    return Json(new
                    {
                        result = whatVote,
                        faktVote = votes.Where(v => v.Vote == 1).Count(),
                        fakeVote = votes.Where(v => v.Vote == -1).Count(),
                        manipulatedVote = votes.Where(v => v.Vote == 2).Count()
                    });
                }
            }
        }
    }
}