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
using NLog;

namespace notomyk.Controllers
{
    public class NewsController : Controller
    {
        private NTMContext db = new NTMContext();
        private static Logger FOFlog = LogManager.GetCurrentClassLogger();
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
                    var newsValidator = new NewsValidator(_User);

                    var valResult = newsValidator.IfExceededNewsNumber();

                    if (valResult == 0)
                    {
                        if (string.IsNullOrWhiteSpace(newN.UrlLink))
                        {
                            return RedirectToAction("Index", "Error", new { errorMessage = ErrorMessage.NewsEmptyLink });
                        }

                        var news = new tbl_News();
                        var metaDataFromUrl = NewsMethodes.GetMetaDataFromUrl(newN.UrlLink);
                        var homeUrl = NewsMethodes.GetHomeURL(newN.UrlLink);
                        var URL = new Uri(newN.UrlLink);

                        if (newsValidator.UrlForbidden(newN.UrlLink))
                        {
                            FOFlog.Error(string.Format("User: {0} tried to add news from forbidden domain: {1}", _User.UserName, newN.UrlLink));
                            return RedirectToAction("Index", "Error", new { errorMessage = string.Format("Podany link pochodzi z domeny: {0}, która jest na naszej czarnej liście. Jeżeli uważasz, że ta domena jest bezpieczna - skontaktuj się z nami.", URL.Host) });
                        }

                        if (string.IsNullOrWhiteSpace(metaDataFromUrl.Description))
                        {
                            FOFlog.Error(string.Format("User: {0} tried to add news with empty description: {1}", _User.UserName, newN.UrlLink));
                            return RedirectToAction("Index", "Error", new { errorMessage = string.Format(ErrorMessage.NewsNoDescription, URL.Host) });
                        }

                        if (string.IsNullOrWhiteSpace(metaDataFromUrl.Title))
                        {
                            FOFlog.Error(string.Format("User: {0} tried to add news with empty title: {1}", _User.UserName, newN.UrlLink));
                            return RedirectToAction("Index", "Error", new { errorMessage = string.Format(ErrorMessage.NewsNoTitle, URL.Host) });
                        }

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

                            addNewspaper.NewspaperName = myNewspaper.GetNewspaperName(homeUrl);
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

                        if (string.IsNullOrEmpty(metaDataFromUrl.ImageUrl))
                        {
                            news.PictureLink = "/Images/Utility/defaultImage.jpg";
                        }
                        else
                        {
                            news.PictureLink = myImageSave.Link(metaDataFromUrl.ImageUrl.ToString());
                        }

                        db.News.Add(news);

                        db.SaveChanges();

                        myTags.AddTags(news.tbl_NewsID, metaDataFromUrl.Keywords);

                        newsValidator.NewsAdded(_User);

                        FOFlog.Info(string.Format("User: {0} added news ID: {1}", news.ApplicationUser.UserName, news.tbl_NewsID));

                        return RedirectToAction("News", "Main", new { id = news.tbl_NewsID });
                    }
                    else
                    {
                        string eMessage = string.Format("Przekroczyłeś dzienną dostępną liczbę dodawanych newsów.\n\n Limit dla Twojej roli {0} wynosi: {1}.", StringTranslate.ReturnRoleName(newsValidator.WhatRole), valResult);
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

        }

        [HttpPost]
        public JsonResult Remove(int newsID)
        {            
            if (User.IsInRole("Admin") || User.IsInRole("Moderator") || myUser.IsNewsAuthor(newsID, User.Identity.GetUserId()))
            {
                if (myUser.IsNewsAuthor(newsID, User.Identity.GetUserId()) && !User.IsInRole("Admin") && !User.IsInRole("Moderator"))
                {
                    if (db.Comment.Any(c => c.tbl_NewsID == newsID && c.IsActive == true))
                    {
                        return Json(new
                        {
                            Success = false,
                            errMessage = ErrorMessage.NewsRemoveHasComment
                        });
                    }
                }
                using (NTMContext db = new NTMContext())
                {

                    var newsToDelete = db.News.Where(n => n.tbl_NewsID == newsID).FirstOrDefault();
                    newsToDelete.IsActive = false;

                    db.SaveChanges();

                    FOFlog.Info(string.Format("User: {0} removed newsID: {1}", User.Identity.Name, newsID));

                    //return RedirectToAction("Index", "Main");

                    return Json(new { Success = true, redirectUrl = Url.Action("Index", "Main") });
                }
            }
            return Json(new
            {
                Success = false,
                errMessage = "Nie masz uprawnień aby usunąć tego newsa."
            });
        }

        [HttpPost]
        public JsonResult Report(int newsID, bool ToReport)
        {
            if (Request.IsAuthenticated)
            {
                var news = db.News.Where(n => n.tbl_NewsID == newsID).FirstOrDefault();
                news.IsReported = ToReport;
                db.SaveChanges();


                FOFlog.Info(string.Format("User: {0} reported newsID: {1}", User.Identity.Name, newsID));
                return Json(new
                {
                    Success = true,
                    errMessage = "News został zgłoszony."
                });

            }
            else
            {
                return Json(new
                {
                    Success = false,
                    errMessage = "Tylko zalogowani użytkownicy mogą zgłaszać newsy."
                });
            }
        }
    }
}


