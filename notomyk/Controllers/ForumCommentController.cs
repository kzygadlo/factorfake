using notomyk.DAL;
using notomyk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace notomyk.Controllers
{
    public class ForumCommentController : Controller
    {
        private NTMContext db = new NTMContext();
        // GET: ForumComment
        public ActionResult Add(int ID, string comm )
        {
            var comment = new ForumPost();
            comment.Content = comm;
            
            return Json(new { success = true });
        }
    }
}