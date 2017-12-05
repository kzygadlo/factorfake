using notomyk.DAL;
using notomyk.Infrastructure;
using notomyk.Models;
using notomyk.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using System.Data.Entity;

namespace notomyk.Controllers
{
    public class NewsController : Controller
    {
        private NTMContext db = new NTMContext();
        //GET: Add
        public ActionResult Add()
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Main");
            }
            else
            {
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("Index", "Main") });
            }
        }

        [HttpPost]
        public ActionResult Add(NewNews newN)
        {
            if (Request.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    var _uID = User.Identity.GetUserId();
                    var _User = db.Users.Where(u => u.Id == _uID).FirstOrDefault();
                    var newsValidator = new addNewsValidator(_User, db);

                    var valResult = newsValidator.IfExceededNewsNumber();

                    if (valResult == 0)
                    {
                        var news = new tbl_News();
                        var metaDataFromUrl = NewsMethodes.GetMetaDataFromUrl(newN.UrlLink);
                        var homeUrl = NewsMethodes.GetHomeURL(newN.UrlLink);

                        var newsID = db.News.Where(n => n.ArticleLink == newN.UrlLink).Select(n => n.tbl_NewsID).FirstOrDefault();
                        if (newsID != 0)
                        {
                            return RedirectToAction("News", "Main", new { id = newsID });
                        }

                        var newspaperId = db.Newspaper.Where(n => n.NewspaperLink == homeUrl).Select(n => n.tbl_NewspaperID).FirstOrDefault();
                        if (newspaperId == 0)
                        {
                            var addNewspaper = new tbl_Newspaper();
                            addNewspaper.NewspaperLink = homeUrl;

                            addNewspaper.NewspaperName = string.IsNullOrEmpty(metaDataFromUrl.SiteName) ? homeUrl : metaDataFromUrl.SiteName;
                            addNewspaper.NewspaperIconLink = "default.jpg";
                            db.Newspaper.Add(addNewspaper);
                            db.SaveChanges();
                            news.tbl_NewspaperID = addNewspaper.tbl_NewspaperID;
                        }
                        else
                        {
                            news.tbl_NewspaperID = newspaperId;
                        }

                        news.ArticleLink = newN.UrlLink;
                        news.DateAdd = DateTime.UtcNow;
                        news.UserId = User.Identity.GetUserId();
                        news.Title = string.IsNullOrEmpty(metaDataFromUrl.Title) ? metaDataFromUrl.SiteName : myEncoding.ReplaceSign(metaDataFromUrl.Title);
                        news.Description = myEncoding.ReplaceSign(metaDataFromUrl.Description);
                        news.PictureLink = metaDataFromUrl.ImageUrl;

                        db.News.Add(news);

                        db.SaveChanges();

                        myTags.AddTags(news.tbl_NewsID, metaDataFromUrl.Keywords);

                        newsValidator.NewsAdded(_User,db);

                        return RedirectToAction("News", "Main", new { id = news.tbl_NewsID });
                    }
                    else
                    {
                        string eMessage = string.Format("Przekroczyłeś dzienną dostępną liczbę dodawanych newsów.\n\n Limit dla Twojej roli {0} wynosi: {1}.", newsValidator.WhatRole, valResult);
                        if (newsValidator.EmailConfirmed == false)
                        {
                            eMessage += "\n\n Twoje konto nie zostało aktywowane. Po jego aktywowaniu liczba dopuszczalnych newsów się zwiększy.";
                        }
                        return RedirectToAction("Index", "Error", new { errorMessage = eMessage });
                    }
                }
            }
            else
            {
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("Index", "Main") });
            }
            return null;

        }

        [HttpPost]
        public ActionResult NewsFakt(int newsID)
        {

            if (Request.IsAuthenticated)
            {
                var news = db.News.FirstOrDefault(n => n.tbl_NewsID == newsID);
                //news.Fakt++;
                db.SaveChanges();

                return RedirectToAction("News", "Main", new { id = newsID });
            }
            else
            {
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("News", "Main", new { id = newsID }) });
            }

            return null;
        }

        [HttpPost]
        public ActionResult NewsFake(int newsID)
        {

            if (Request.IsAuthenticated)
            {
                var news = db.News.FirstOrDefault(n => n.tbl_NewsID == newsID);

                //news.Fake++;
                db.SaveChanges();

                return RedirectToAction("News", "Main", new { id = newsID });
            }
            else
            {
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("News", "Main", new { id = newsID }) });
            }

            return null;
        }

        [HttpPost]
        public JsonResult Remove(int newsID)
        {
            //if (myUser.IsNewsAuthor(newsID, User.Identity.GetUserId()))
            if (User.IsInRole("Admin"))
            {
                using (NTMContext db = new NTMContext())
                {

                    var newsToDelete = db.News.Where(n => n.tbl_NewsID == newsID).FirstOrDefault();
                    newsToDelete.IsActive = false;

                    db.SaveChanges();

                    RedirectToAction("Index", "Main");

                    return Json(new { Success = true, redirectUrl = Url.Action("Index", "Main") });
                }
            }
            return Json(new { Success = false, ResultMsg = "Nie masz uprawnień aby usunąć tego newsa." });
        }

    }
}


