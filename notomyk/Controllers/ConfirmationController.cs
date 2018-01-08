using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace notomyk.Controllers
{
    public class ConfirmationController : Controller
    {
        // GET: Confirmation
        public ActionResult Index(string title, string message)
        {
            ViewBag.Title = title;
            ViewBag.Message = message;
            return View();
        }
    }
}