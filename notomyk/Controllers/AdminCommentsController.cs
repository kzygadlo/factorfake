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
    public class AdminCommentsController : Controller
    {
        NTMContext db = new NTMContext();
        // GET: AdminUserTable
        [Authorize(Roles = "Admin")]
        public ActionResult Index(int id)
        {
            ViewBag.NewsID = 0;
            ViewBag.UserID = "";
            ViewBag.ParentID = id;
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult IndexUserID(string id)
        {
            ViewBag.NewsID = 0;
            ViewBag.UserID = id;
            ViewBag.ParentID = 0;
            return View("Index");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult IndexNewsID(int id)
        {
            ViewBag.NewsID = id;
            ViewBag.ParentID = 0;
            return View("Index");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult GetComments(int newsID, string userID, int parentID)
        {
            var n = db.Comment.AsQueryable();

            if (newsID != 0)
            {
                n = n.Where(c => c.tbl_NewsID == newsID);
            }
            else if (userID != "")
            {
                n = db.Comment.Where(c => c.ApplicationUser.Id == userID);
            }

            if (parentID != 0)
            {
                n = n.Where(c => c.Parenttbl_CommentID == parentID);
            }

            var comments = n.Select(x => new {
                x.ApplicationUser.UserName,
                x.Comment,
                x.Fakt,
                x.Fake,
                x.IsReported,
                x.IsActive,
                x.DateAdd,
                Child = x.Children.Count,
                x.tbl_CommentID
            }).ToList();

            var commentsFinal = comments.Select(x => new {
                x.UserName,
                x.Comment,
                x.Fakt,
                x.Fake,
                x.IsReported,
                x.IsActive,                
                Date = ConvertToString.Date(x.DateAdd),
                Child = string.Concat(x.tbl_CommentID,";", x.Child),
                x.tbl_CommentID
            }).ToList();

            return Json(new { data = commentsFinal }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Save(int id)
        {
            if (id != 0)
            {
                var comment = db.Comment.Where(c => c.tbl_CommentID == id).FirstOrDefault();
                return View(comment);
            }
            return View("Index");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Save(tbl_Comment comm)
        {
            bool status = false;
            if (ModelState.IsValid)
            {
                if (comm.tbl_CommentID != 0)
                {
                    var c = db.Comment.Where(d => d.tbl_CommentID == comm.tbl_CommentID).FirstOrDefault();

                    c.Comment = comm.Comment;
                    c.IsActive = comm.IsActive;
                    c.IsReported = comm.IsReported;
                    db.SaveChanges();
                    status = true;
                }
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors);

            if (status)
            {
                return RedirectToAction("Index", "AdminComments");
            }
            else
            {
                return RedirectToAction("Index", "Error", new { errorMessage = ErrorMessage.AdminTableSaveFailed });
            }

        }
    }
}