﻿using Microsoft.AspNet.Identity;
using NLog;
using notomyk.DAL;
using notomyk.Infrastructure;
using notomyk.Models;
using notomyk.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace notomyk.Controllers
{

    public class MainController : Controller
    {

        public ActionResult WhatNewspaper(string id)
        {

            if (db.Newspaper.Any(n => n.NewspaperName.Contains(id) && n.Colection_Newses.Where(c => c.IsActive == true).Count() > 0))
            {
                //var newspapersList = db.Newspaper.Where(n => n.NewspaperName.Contains(id) && n.Colection_Newses.Where(c => c.IsActive == true).Count() > 0).ToList();
                return RedirectToAction("Index", new { f = id });
            }

            return RedirectToAction("Index");
        }

        private NTMContext db = new NTMContext();
        private static Logger FOFlog = LogManager.GetCurrentClassLogger();
        public ActionResult Index(int mainPage = 1, string f = "", string tag = "")
        {

            if (!cAppGlobal.IsAllowed("SiteEnabled"))
            {
                return RedirectToAction("Index", "Error", new { errorMessage = ErrorMessageGlobal.SiteEnabled });
            }

            if (Request.IsAuthenticated)
            {
                logLastActivity la = new logLastActivity(User.Identity.GetUserId());
            }

            int numberOfVotes = Convert.ToInt32(cApp.AppSettings["MinNumberVotesForMainPage"]);
            List<string> newspaperList = new List<string>();
            List<Tag> tagList = new List<Tag>();

            if (mainPage == 1)
            {
                newspaperList = db.Newspaper
                 .Where(n => n.Colection_Newses.Where(c => c.IsActive == true && c.VoteLogs.Count > numberOfVotes).Count() > 0 && n.IsActive == true)
                 .OrderBy(o => o.NewspaperName)
                 .Select(s => s.NewspaperName)
                 .Distinct()
                 .ToList();

                tagList = db.Tag
                    .Where(t => t.ListOfNews.Any(n => n.News.IsActive == true && n.News.VoteLogs.Count > numberOfVotes))
                    .OrderByDescending(o => o.ListOfNews.Count).ToList();

                ViewBag.Start = "active";
            }
            else
            {
                newspaperList = db.Newspaper
                 .Where(n => n.Colection_Newses.Where(c => c.IsActive == true && c.VoteLogs.Count < numberOfVotes).Count() > 0 && n.IsActive == true)
                 .OrderBy(o => o.NewspaperName)
                 .Select(s => s.NewspaperName)
                 .Distinct()
                 .ToList();

                tagList = db.Tag
                    .Where(t => t.ListOfNews.Any(n => n.News.IsActive == true && n.News.VoteLogs.Count < numberOfVotes))
                    .OrderByDescending(o => o.ListOfNews.Count).ToList();

                ViewBag.WaitingRoom = "active";
            }

            if (f != "")
            {
                ViewBag.Default = ReturnListOfNewspapers(f, ref newspaperList);
                ViewBag.MainPage = 2;
            }
            else
            {
                ViewBag.MainPage = mainPage;
            }

            if (tag != "")
            {
                ViewBag.DefaultTag = db.Tag.Where(t => t.TagName == tag).Select(s => s.ID).FirstOrDefault() ;
            }


            var fofUrl1 = ConfigurationManager.AppSettings["UrlAddress"];

            ViewBag.ogTitle = "FF | Fakt or Fake";
            ViewBag.fbButtonUrl = fofUrl1;
            ViewBag.ogDescription = "Strona poświęcona weryfikowaniu wiadomości pod kątem ich wiarygodności bądź tendencyjności.";
            ViewBag.ogImage = imgUrl("/Images/Social/og-image.png", fofUrl1);

            Filters vm = new Filters();
            vm.Newspapers = newspaperList;
            vm.Categories = tagList;

            return View(vm);
        }

        private string ReturnListOfNewspapers(string newspaper, ref List<string> ListNewspapers)
        {

            string result = "";
            var nList = db.Newspaper.Where(n => n.NewspaperName.Contains(newspaper)).Select(s => s.NewspaperName).ToList();

            foreach (var n in nList)
            {
                result = result + n + ",";

                if (!ListNewspapers.Any(x => x.Contains(n)))
                {
                    ListNewspapers.Add(n);
                }
            }

            if (result.Length > 1)
            {
                result = result.Remove(result.Length - 1);
            }

            return result;

        }

        [HttpPost]
        public JsonResult Get(FilterModel filter)
        {
            List<tbl_News> listOfNews = GetNewsList(filter).ToList();

            var finalList = listOfNews.Select(x => new
            {
                CommentsNumber = Convert.ToInt32(x.Collection_Comments.Where(n => n.IsActive == true && n.Parenttbl_CommentID == null).Count()) + Convert.ToInt32(x.Collection_Comments.Where(n => n.IsActive == true && n.Parenttbl_CommentID != null && n.Parent.IsActive == true).Count()),
                x.ApplicationUser,
                x.ArticleLink,
                x.DateAdd,
                x.Description,
                x.EventsTags,
                x.IsActive,
                x.Newspaper,
                x.PictureLink,
                x.tbl_NewsID,
                x.tbl_NewspaperID,
                x.Title,
                x.UserId,
                x.VoteLogs,
                x.Visitors,
                RatingValue = x.RatingValue(),
                RatingClass = x.IsFMF(),
                Tags = tagsToViews.ReturnTags(x.EventsTags.OrderByDescending(o => o.Tags.ListOfNews.Count)
                        .Select(e => e.Tags.TagName)
                        .Take(5)
                        .ToList())
            });

            UrlHelper u = new UrlHelper(this.ControllerContext.RequestContext);
            string url = u.Action("News", "Main", new { ID = "id" });

            UrlHelper p = new UrlHelper(this.ControllerContext.RequestContext);
            string iconP = p.NewspaperIconPath_250("path");

            return Json(finalList.Select(x => new
            {
                newsID = x.tbl_NewsID,
                urlActionLink = url.Replace("id", x.tbl_NewsID.ToString()),
                newspaperPictureLink = iconP.Replace("path", x.Newspaper.NewspaperIconLink),
                newsPictureLink = x.PictureLink,
                newsTitle = myEncoding.ReplaceSign(x.Title),
                //newsDescription = myEncoding.ReplaceSign(x.Description),
                newsDescription = HttpUtility.HtmlDecode(x.Description),
                numberOfVisitors = x.Visitors,
                numberOfComments = x.CommentsNumber,
                dateAdded = GetTimeAgo.CalculateDateDiff(x.DateAdd),
                ratingClass = x.RatingClass,
                ratingValue = x.RatingValue,
                faktValue = x.VoteLogs.Where(v => v.Vote == 1).Count(),
                fakeValue = x.VoteLogs.Where(v => v.Vote == -1).Count(),
                manipulatedValue = x.VoteLogs.Where(v => v.Vote == 2).Count(),
                remainingRows = filter.Remains,
                tagList = x.Tags
            }), JsonRequestBehavior.AllowGet);
        }

        public IQueryable<tbl_News> GetNewsList(FilterModel filter)
        {
            var result = db.News.Where(n => n.IsActive == true && n.Newspaper.IsActive == true).AsQueryable();
            int votingValue = Convert.ToInt32(cApp.AppSettings["FilterVoting"]);
            int commentValue = Convert.ToInt32(cApp.AppSettings["FilterComments"]);
            int visitorsValue = Convert.ToInt32(cApp.AppSettings["FilterVisitors"]);

            if (filter != null)
            {
                if (filter.NewspapersList.Count > 0)
                {
                    result = result.Where(n => filter.NewspapersList.Contains(n.Newspaper.NewspaperName));
                }
                if (filter.TagsList.Count > 0)
                {
                    result = result.Where(n => n.EventsTags.Any(a => filter.TagsList.Contains(a.TagID)));
                }
                if (filter.WhatNews != 0)
                {
                    switch (filter.WhatNews)
                    {
                        case 1: //Top Fakt
                            result = result.Where(n => n.VoteLogs.Where(v => v.Vote == 1).Count() + n.VoteLogs.Where(v => v.Vote == -1).Count() >= votingValue
                                && (n.VoteLogs.Where(v => v.Vote == -1).Count() == 0 || n.VoteLogs.Where(v => v.Vote == 1).Count() / n.VoteLogs.Where(v => v.Vote == -1).Count() > 2));
                            break;
                        case 2: //Top Fake
                            result = result.Where(n => n.VoteLogs.Where(v => v.Vote == 1).Count() + n.VoteLogs.Where(v => v.Vote == -1).Count() >= votingValue
                                && (n.VoteLogs.Where(v => v.Vote == 1).Count() == 0 || n.VoteLogs.Where(v => v.Vote == -1).Count() / n.VoteLogs.Where(v => v.Vote == 1).Count() > 2));
                            break;
                        case 3: //Top Comments
                            result = result.Where(n => n.Collection_Comments.Where(c => c.IsActive == true).Count() >= commentValue).OrderByDescending(o => o.Collection_Comments.Where(c => c.IsActive == true).Count());
                            break;
                        case 4: //Top Visits
                            result = result.Where(n => n.Visitors >= visitorsValue).OrderByDescending(o => o.Visitors);
                            break;
                        case 5: //Top Manipulated
                            result = result
                                .Where(n => n.VoteLogs.Where(v => v.Vote == 2).Count() + n.VoteLogs.Where(v => v.Vote == 1).Count() + n.VoteLogs.Where(v => v.Vote == -1).Count() >= votingValue)
                                .Where(n => n.VoteLogs.Where(v => v.Vote == 2).Count() > n.VoteLogs.Where(v => v.Vote == 1).Count())
                                .Where(n => n.VoteLogs.Where(v => v.Vote == 2).Count() > n.VoteLogs.Where(v => v.Vote == -1).Count());
                            break;
                    }
                }

                if (filter.Period != 0)
                {
                    DateTime dateForFilter = new DateTime();
                    switch (filter.Period)
                    {
                        case 1: //Today
                            dateForFilter = DateTime.UtcNow.AddHours(-24);
                            result = result.Where(n => n.DateAdd > dateForFilter);
                            break;
                        case 2: //Last week
                            dateForFilter = DateTime.UtcNow.AddDays(-7);
                            result = result.Where(n => n.DateAdd > dateForFilter);
                            break;
                        case 3: //Last month
                            dateForFilter = DateTime.UtcNow.AddMonths(-1);
                            result = result.Where(n => n.DateAdd > dateForFilter);
                            break;
                    }
                }
            }

            int numberOfVotesforMainPage = Convert.ToInt32(cApp.AppSettings["MinNumberVotesForMainPage"]);

            if (filter.MainPage == 1)
            {
                result = result.Where(r => r.VoteLogs.Count() > numberOfVotesforMainPage);
            }
            else if (filter.MainPage == 0)
            {
                result = result.Where(r => r.VoteLogs.Count() <= numberOfVotesforMainPage);
            }


            filter.Remains = result.Count() - 10 - filter.Page * 10;

            if (filter.WhatNews == 0 || filter.WhatNews == 1 || filter.WhatNews == 2 || filter.WhatNews == 5)
            {
                result = result.OrderByDescending(r => r.DateAdd);
            }

            result = result.Skip(filter.Page * 10).Take(10);

            return result;
        }

        public ActionResult News(int ID, int nPage = 3)
        {
            if (!cAppGlobal.IsAllowed("SiteEnabled"))
            {
                return RedirectToAction("Index", "Error", new { errorMessage = ErrorMessageGlobal.SiteEnabled });
            }

            if (!Request.IsAuthenticated)
            {
                ViewBag.popupMsg = "Musisz byc zalogowany aby oddać głos.";
            }
            else
            {
                logLastActivity la = new logLastActivity(User.Identity.GetUserId());
            }


            ViewBag.NewsID = ID;

            var currentUserID = User.Identity.GetUserId();
            int whatVote = 0;
            var isVoted = db.VoteLog.Where(v => v.tbl_NewsID == ID && v.UserId == currentUserID).FirstOrDefault();

            if (isVoted != null)
            {
                whatVote = isVoted.Vote;
            }

            var singleNews = db.News.Where(n => n.tbl_NewsID == ID && n.IsActive == true && n.Newspaper.IsActive == true).FirstOrDefault();

            if (singleNews == null)
            {
                return RedirectToAction("Index", "Main");
            }

            if (!singleNews.IsReported)
            {
                ViewBag.ReportedClass = "hidden";
            }

            var leftNews = db.News
                .Where(n => n.tbl_NewspaperID == singleNews.tbl_NewspaperID && n.IsActive == true && n.Newspaper.IsActive == true)
                .OrderByDescending(n => n.DateAdd)
                .Take(5)
                .ToList();

            ICollection<string> listOfTags = (from t in db.Tag
                                              join e in db.EventTag on t.ID equals e.TagID
                                              where e.tbl_NewsID == singleNews.tbl_NewsID
                                              orderby t.ListOfNews.Count descending
                                              select t.TagName).ToList();

            string CommaSeparatedTags = myTags.TagsBuilder(listOfTags);


            if (Request.Cookies[string.Format("HasVisited:{0}", ID)] == null)
            {
                HttpCookie cookie = new HttpCookie(string.Format("HasVisited:{0}", ID), "true");
                cookie.Expires = DateTime.UtcNow.AddDays(30);
                Response.Cookies.Add(cookie);

                singleNews.Visitors++;
                db.SaveChanges();
            }

            var vm = new NewsDetail();

            var fofUrl1 = "https://www.faktorfake.pl";
            var fofUrl2 = Url.Action("News", "Main", new { ID = ID });

            var fbButtonUrl = string.Concat(HttpContext.Request.Url.AbsoluteUri.Where(u => !char.IsWhiteSpace(u)));

            if (fbButtonUrl.IndexOf("?") > 0)
            {
                ViewBag.fbButtonUrl = fbButtonUrl.Substring(0, fbButtonUrl.IndexOf("?"));
            }
            else
            {
                ViewBag.fbButtonUrl = fbButtonUrl;
            }
            

            //ViewBag.fbButtonUrl = string.Concat("https://www.facebook.com/sharer/sharer.php?u=https%3A%2F%2Fwww.faktorfake.pl", fofUrl2, "%2F&quote=");
            ViewBag.twitterButtonUrl = string.Concat("http://twitter.com/share?url=https://faktorfake.pl", fofUrl2, "&hashtags=fakenews,faktorfake");

            ViewBag.ogTitle = string.Concat(singleNews.Newspaper.NewspaperName, " | ", singleNews.Title);
            ViewBag.ogDescription = singleNews.Description;

            ViewBag.ogImage = imgUrl(singleNews.PictureLink, fofUrl1);

            var commNumber = singleNews.Collection_Comments.Where(n => n.IsActive == true && n.Parenttbl_CommentID == null).Count() + singleNews.Collection_Comments.Where(n => n.IsActive == true && n.Parenttbl_CommentID != null && n.Parent.IsActive == true).Count();
            ViewBag.CommNum = commNumber;

            ViewBag.PageN = nPage;

            vm.SingleNews = singleNews;
            vm.LeftNews = leftNews;
            vm.CommaSeparatedTags = CommaSeparatedTags;

            vm.WhatClass = NewsClass(singleNews.IsFMF(), whatVote);
            return View(vm);
        }


        public NewsCssClasses NewsClass(int IsFMF, int IsVoted)
        {

            var result = new NewsCssClasses("basic");
            switch (IsFMF)
            {
                case 1:
                    result.FaktClass = "";
                    break;
                case -1:
                    result.FakeClass = "";
                    break;
                case 2:
                    result.ManipulatedClass = "";
                    break;
            }
            switch (IsVoted)
            {
                case -1:
                    result.FakeVotedClass = "BGredColorLight";
                    break;
                case 1:
                    result.FaktVotedClass = "BGgreenColorLight";
                    break;
                case 2:
                    result.ManipulatedVotedClass = "BGgreyColorLight";
                    break;
            }
            return result;
        }

        public string imgUrl(string url, string rootUrl)
        {
            string result = url;

            if (url.Contains("http") == true)
            {
                return url;
            }
            else
            {
                return string.Concat(rootUrl, url);
            }
        }

        public ActionResult AllNews()
        {

            var AllN = (from x in db.News
                        where x.IsActive == true
                        orderby x.DateAdd descending
                        select x).ToList();

            var vm = new NewsList() { News = AllN };

            return View("Index", vm);
        }


        [ChildActionOnly]
        public ActionResult RightMenu(int newsID = 0, int topicID = 0, int nPage = 2)
        {

            var faktN = db.News
                .Where(n => n.VoteLogs.Where(v => v.Vote == 1).Count() >= n.VoteLogs.Where(v => v.Vote == -1).Count())
                .Where(n => n.Newspaper.IsActive == true)
                .Where(n => n.IsActive == true)
                .OrderByDescending(n => n.VoteLogs.Where(v => v.Vote == 1).Count() - n.VoteLogs.Where(v => v.Vote == -1).Count())
                .Take(10)
                .ToList();

            var fakeN = db.News
                .Where(n => n.VoteLogs.Where(v => v.Vote == -1).Count() >= n.VoteLogs.Where(v => v.Vote == 1).Count())
                .Where(n => n.Newspaper.IsActive == true)
                .Where(n => n.IsActive == true)
                .OrderByDescending(n => n.VoteLogs.Where(v => v.Vote == -1).Count() - n.VoteLogs.Where(v => v.Vote == 1).Count())
                .Take(10)
                .ToList();

            var manipulatedN = db.News
                .Where(n => n.VoteLogs.Where(v => v.Vote == 2).Count() >= n.VoteLogs.Where(v => v.Vote == 1).Count())
                .Where(n => n.VoteLogs.Where(v => v.Vote == 2).Count() >= n.VoteLogs.Where(v => v.Vote == -1).Count())
                .Where(n => n.Newspaper.IsActive == true)
                .Where(n => n.IsActive == true)
                .OrderByDescending(n => n.VoteLogs.Where(v => v.Vote == 2).Count())
                .Take(10)
                .ToList();

            var comments = db.News
                .Where(n => n.IsActive == true && n.Newspaper.IsActive == true)
                .OrderByDescending(n => n.Collection_Comments.Where(c => c.IsActive == true && c.Parenttbl_CommentID == null).Count() + n.Collection_Comments.Where(c => c.IsActive == true && c.Parenttbl_CommentID != null && c.Parent.IsActive == true).Count())
                .Take(10)
                .ToList();

            var visitors = db.News
                .Where(n => n.IsActive == true && n.Newspaper.IsActive == true)
                .OrderByDescending(n => n.Visitors)
                .Take(10)
                .ToList();

            List<ApplicationUser> uR = new List<ApplicationUser>();

            uR = db.Users.ToList();

            var userR = uR.OrderByDescending(o => o.Reputation()).Take(5).ToList();

            var usersNews = (from x in db.Users
                             orderby x.tbl_News.Count descending
                             select x).Take(5).ToList();

            var usersComm = (from x in db.Users
                             orderby x.Comments.Where(c => c.IsActive == true).Count() descending
                             select x).Take(5).ToList();

            var vm = new RightMenuModel()
            {
                FakeNews = fakeN,
                ManipulatedNews = manipulatedN,
                FaktNews = faktN,
                VisitedNews = visitors,
                CommentedNews = comments,
                UsersRep = userR,
                UsersNews = usersNews,
                UsersComm = usersComm
            };

            ViewBag.NewsID = newsID;
            ViewBag.TopicID = topicID;
            ViewBag.PageN = nPage;

            return PartialView(vm);
        }

        [ChildActionOnly]
        public ActionResult RightMenuForum(int topicID = 0)
        {
            RightMenuForum model = new RightMenuForum();
            model.Topics = db.ForumTopic.Where(t => t.OnMainPage != null && t.IsActive == true).OrderBy(o => o.OnMainPage).ToList();

            ViewBag.TopicID = topicID;

            return PartialView(model);
        }
    }
}