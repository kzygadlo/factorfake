using notomyk.DAL;
using notomyk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using notomyk.Infrastructure;

namespace notomyk.Controllers
{
    public class ReplyController : Controller
    {
        private NTMContext db = new NTMContext();

        [HttpGet]
        public JsonResult Get(int commID)
        {
            try
            {
                using (NTMContext db = new NTMContext())
                {
                    //var ReplyLIst = db.Reply.Where(c => c.tbl_CommentID == commID).ToList();
                    //var ReplyList = (
                    //    from r in db.Reply
                    //    join u in db.Users on r.UserId equals u.Id
                    //    where r.tbl_CommentID == commID
                    //    select new
                    //    {
                    //        r.tbl_CommentReplyID,
                    //        r.Comment,
                    //        r.DateAdd,
                    //        u.Id,
                    //        u.UserName,
                    //        r.Fakt,
                    //        r.Fake
                    //    }).ToList();

                    //return Json(ReplyList.Select(x => new
                    //{
                    //    rep = x.Comment,
                    //    rid = x.tbl_CommentReplyID,
                    //    date = GetTimeAgo.CalculateDateDiff(x.DateAdd),
                    //    userN = x.UserName,
                    //    userL = Url.Content(AppConfig.UserLogoLink(x.Id)),
                    //    faktV = x.Fakt,
                    //    fakeV = x.Fake
                    //}), JsonRequestBehavior.AllowGet);

                    return Json("");
                }
            }
            catch
            {
                return Json(new { success = false });
            }

        }

        // GET: Reply
        [HttpPost]
        public ActionResult Add(string ReplyText, int CommentID)
        {
            if (Request.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        //using (NTMContext db = new NTMContext())
                        //{
                        //    var reply = new tbl_CommentReply();

                        //    reply.Comment = ReplyText;
                        //    reply.DateAdd = DateTime.UtcNow;
                        //    reply.UserId = User.Identity.GetUserId();
                        //    reply.tbl_CommentID = CommentID;

                        //    var user = db.Users.Where(u => u.Id == reply.UserId).FirstOrDefault();
                        //    var comment = db.Comment.Where(c => c.tbl_CommentID == CommentID).FirstOrDefault();

                        //    db.Reply.Add(reply);

                        //    user.commentsNumber++;
                        //    //comment.Replies++;

                        //    db.SaveChanges();

                        //    return Json(new
                        //    {
                        //        success = true,
                        //        rep = reply.Comment,
                        //        rid = reply.tbl_CommentReplyID,
                        //        date = GetTimeAgo.CalculateDateDiff(reply.DateAdd),
                        //        userN = user.UserName,
                        //        userL = Url.Content(AppConfig.UserLogoLink(user.Id))
                        //    });
                        //}
                    }
                    catch
                    {
                        return Json(new { success = false });
                    }
                }

            }
            return null;
        }

        [HttpPost]
        public ActionResult Remove(int replyID)
        {
            //using (NTMContext db = new NTMContext())
            //{
            //    var commentID = db.Reply.Where(r => r.tbl_CommentReplyID == replyID).FirstOrDefault();
            //    var comment = db.Comment.Where(c => c.tbl_CommentID == commentID.tbl_CommentID).FirstOrDefault();

            //    //comment.Replies--;
            //    db.SaveChanges();
            //}

            //using (NTMContext db = new NTMContext())
            //{
            //    var Reply = new tbl_CommentReply { tbl_CommentReplyID = replyID };
            //    db.Entry(Reply).State = EntityState.Deleted;

            //    db.SaveChanges();
            //}

            //return Json(new { Success = true });

            return Json("");
        }
    }
}