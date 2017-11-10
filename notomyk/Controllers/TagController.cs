using notomyk.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace notomyk.Controllers
{
    public class TagController : Controller
    {
        // GET: Tag
        public ActionResult Add(int newsID, string tagsList)
        {

            if (Request.IsAuthenticated)
            {
                NewsMethodes.AddTags(newsID, tagsList);
            }
            else
            {
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("News", "Main", new { ID = newsID }) });
            }

            return null;
        }
    }
}