using notomyk.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace notomyk.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index(string errorMessage)
        {
            ViewBag.Message = errorMessage;
            return View();
        }

        public ActionResult NotFound()
        {
            //Response.StatusCode = 404;  //you may want to set this to 200
            return RedirectToAction("Index", new { errorMessage = "Strona pod podanym adresem nie istnieje w naszym serwisie." });
        }
        //public ActionResult error404()
        //{
        //    return RedirectToAction("Index", new { errorMessage = "Strona pod podanym adresem nie istnieje w naszym serwisie." });
        //}

        public ActionResult Default()
        {
            return RedirectToAction("Index", new { errorMessage = ErrorMessage.GeneralError});
        }
    }
}