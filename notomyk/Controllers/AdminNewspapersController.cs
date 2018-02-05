using notomyk.DAL;
using notomyk.Infrastructure;
using notomyk.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Hosting;
using System.Web.Mvc;
using static notomyk.Controllers.ManageController;

namespace notomyk.Controllers
{
    public class AdminNewspapersController : Controller
    {
        // GET: AdminNewspapers
        NTMContext db = new NTMContext();
        // GET: AdminUserTable
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            ViewBag.AdminNewspapersClass = "active";
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult GetNewspapers()
        {
            var newspapers = db.Newspaper.Select(n => new
            {
                n.NewspaperName,
                n.NewspaperLink,
                n.NewspaperIconLink,
                n.IsActive,
                News = string.Concat(n.tbl_NewspaperID, ";", n.Colection_Newses.Count),
                n.tbl_NewspaperID
            }).ToList();

            return Json(new { data = newspapers }, JsonRequestBehavior.AllowGet);

        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Save(int id)
        {
            if (id != 0)
            {
                var newspaper = db.Newspaper.Where(n => n.tbl_NewspaperID == id).FirstOrDefault();
                return View(newspaper);
            }
            return View("Index");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Save(tbl_Newspaper newspaper)
        {
            bool status = false;
            if (ModelState.IsValid)
            {
                if (newspaper.tbl_NewspaperID != 0)
                {
                    var n = db.Newspaper.Where(x => x.tbl_NewspaperID == newspaper.tbl_NewspaperID).FirstOrDefault();

                    n.NewspaperName = newspaper.NewspaperName;
                    n.NewspaperLink = newspaper.NewspaperLink;
                    n.NewspaperIconLink = newspaper.NewspaperIconLink;
                    n.IsActive = newspaper.IsActive;

                    db.SaveChanges();
                    status = true;
                }
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors);

            if (status)
            {
                return RedirectToAction("Index", "AdminNewspapers");
            }
            else
            {
                return RedirectToAction("Index", "Error", new { errorMessage = ErrorMessage.AdminTableSaveFailed });
            }

        }

        [HttpPost]
        public ActionResult UploadLogo(HttpPostedFileBase file, int newspaperID)
        {
            if (file != null && file.ContentLength > 0)
            {
                var photo = new WebImage(file.InputStream);
                var newspaperName = GetNewspaperName(newspaperID);

                var a = AddPhoto(
                    photo, 
                    250, 
                    ConfigurationManager.AppSettings["NewspapersIconsFolder_250"], 
                    GetNewspaperName(newspaperID), 
                    file.FileName,
                    newspaperName
                    );         
                
                var b = AddPhoto(
                    photo, 
                    50, 
                    ConfigurationManager.AppSettings["NewspapersIconsFolder_ICO"], 
                    GetNewspaperName(newspaperID), 
                    file.FileName,
                    newspaperName
                    );

                if (a && b) { UpdateLogoName(newspaperID, newspaperName); }
            }
            return RedirectToAction("Save", new { id = newspaperID });
        }
        public bool AddPhoto(WebImage photo, int size, string path, string newspaperName,string fileName, string logoName)
        {
            
            photo.Resize(width: size, height: size, preserveAspectRatio: true, preventEnlarge: true);


            var fileExt = Path.GetExtension(fileName);
            var fnm = logoName + ".png";

            if (fileExt.ToLower().EndsWith(".png") || fileExt.ToLower().EndsWith(".jpg") || fileExt.ToLower().EndsWith(".gif"))// Important for security if saving in webroot
            {
                var filePath = HostingEnvironment.MapPath(path) + fnm;
                var directory = new DirectoryInfo(HostingEnvironment.MapPath(path));
                if (directory.Exists == false)
                {
                    directory.Create();
                }
                ViewBag.FilePath = filePath.ToString();

                photo.Save(filePath, "png", false);
                return true;
            }
            else
            {
                return false;
            }
        }

        public string GetNewspaperName(int newspaperID)
        {
            var newspaper = db.Newspaper.Where(n => n.tbl_NewspaperID == newspaperID).FirstOrDefault();
            var result = newspaper.NewspaperName.Split('.');
            return string.Concat("logo_", result[0]);
        }

        public void UpdateLogoName(int newspaperID, string logoName)
        {
            var newspaper = db.Newspaper.Where(n => n.tbl_NewspaperID == newspaperID).FirstOrDefault();
            newspaper.NewspaperIconLink = string.Concat(logoName, ".png") ;
            db.SaveChanges();
        }
    }


}