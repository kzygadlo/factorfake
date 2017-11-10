using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using notomyk.DAL;
using notomyk.Infrastructure;
using notomyk.Models;
using notomyk.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace notomyk.Controllers
{
    public class MainController : Controller
    {
        private NTMContext db = new NTMContext();


        public ActionResult Index()
        {
            var newspaperList = (from n in db.Newspaper
                                 where n.Colection_Newses.Count > 0
                                 orderby n.NewspaperName ascending
                                 select n).ToList();

            var tagList = (from t in db.Tag
                           orderby t.TagVotes descending
                           select t).ToList();

            Filters vm = new Filters();
            vm.Newspapers = newspaperList;
            vm.Categories = tagList;

            return View(vm);
        }

        [HttpPost]
        public JsonResult Get(FilterModel filter)
        {
            try
            {
                var listOfNews = GetNewsList(filter).ToList();

                var finalList = listOfNews.Select(x => new
                {
                    x.Collection_Comments,
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
                    Fakt = x.VoteLogs.Where(v => v.Vote == true).Count(),
                    Fake = x.VoteLogs.Where(v => v.Vote == false).Count()
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
                        newsTitle = MyEncoding.ReplaceSign(x.Title),
                        newsDescription = MyEncoding.ReplaceSign(x.Description),
                        numberOfVisitors = x.Visitors, //to edit
                        numberOfComments = x.Collection_Comments.Count, //to edit
                        dateAdded = GetTimeAgo.CalculateDateDiff(x.DateAdd),
                        ratingClass = Rating.RatingClass(x.Fakt, x.Fake),
                        ratingValue = Rating.RatingValue(x.Fakt, x.Fake),
                        faktValue = x.Fakt,
                        fakeValue = x.Fake,
                        remainingRows = filter.Remains,
                        tagList = "gospodarka | finanse | polityka"

                    }), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false });
            }

        }

        public IQueryable<tbl_News> GetNewsList(FilterModel filter)
        {
            var result = db.News.AsQueryable();
            int value = Convert.ToInt32(ConfigurationManager.AppSettings["VotingRate"].ToString());

            if (filter != null)
            {
                if (filter.NewspapersList.Count > 0)
                {
                    result = result.Where(n => filter.NewspapersList.Contains(n.Newspaper.tbl_NewspaperID));
                }
                if (filter.TagsList.Count > 0)
                {
                    result = result;
                }
                if (filter.WhatNews != 0)
                {
                    switch (filter.WhatNews)
                    {
                        case 1: //Top Fakt
                            result = result.Where(n => n.VoteLogs.Where(v => v.Vote == true).Count() + n.VoteLogs.Where(v => v.Vote == false).Count() >= value
                                && (n.VoteLogs.Where(v => v.Vote == false).Count() == 0 || n.VoteLogs.Where(v => v.Vote == true).Count() / n.VoteLogs.Where(v => v.Vote == false).Count() > 2));
                            break;
                        case 2: //Top Fake
                            result = result.Where(n => n.VoteLogs.Where(v => v.Vote == true).Count() + n.VoteLogs.Where(v => v.Vote == false).Count() >= value
                                && (n.VoteLogs.Where(v => v.Vote == true).Count() == 0 || n.VoteLogs.Where(v => v.Vote == false).Count() / n.VoteLogs.Where(v => v.Vote == true).Count() > 2));
                            break;
                        case 3: //Top Comments
                            result = result.Where(n => n.Collection_Comments.Count > value);
                            break;
                        case 4: //Top Visits
                            result = result;
                            break;
                    }
                }
            }
            filter.Remains = result.Count() - 10 - filter.Page * 10;
            result = result.OrderByDescending(r => r.DateAdd).Skip(filter.Page * 10).Take(10);

            return result;
        }

        public ActionResult News(int ID)
        {

            ViewBag.NewsID = ID;

            var currentUserID = User.Identity.GetUserId();
            int whatVote = 0;
            var isVoted = db.VoteLog.Where(v => v.tbl_NewsID == ID && v.UserId == currentUserID).FirstOrDefault();

            if (isVoted != null)
            {
                whatVote = isVoted.Vote ? 1 : -1;
            }

            tbl_News singleNews = (from x in db.News
                                   where x.tbl_NewsID == ID
                                   select x).First();


            ICollection<tbl_News> leftNews = (from x in db.News
                                              where x.tbl_NewspaperID == singleNews.tbl_NewspaperID
                                              orderby x.DateAdd descending
                                              select x).Take(5).ToList();

            ICollection<string> listOfTags = (from t in db.Tag
                                              join e in db.EventTag on t.ID equals e.TagID
                                              where e.tbl_NewsID == singleNews.tbl_NewsID
                                              orderby t.TagVotes descending
                                              select t.TagName).ToList();

            string CommaSeparatedTags = NewsMethodes.TagsBuilder(listOfTags);


            if (Request.Cookies[string.Format("HasVisited:{0}", ID)] == null)
            {
                HttpCookie cookie = new HttpCookie(string.Format("HasVisited:{0}", ID), "true");
                cookie.Expires = DateTime.UtcNow.AddDays(30);
                Response.Cookies.Add(cookie);

                singleNews.Visitors++;
                db.SaveChanges();
            }

            var vm = new NewsDetail();

            vm.SingleNews = singleNews;
            vm.LeftNews = leftNews;
            vm.CommaSeparatedTags = CommaSeparatedTags;
            vm.WhatVote = whatVote;

            return View(vm);

        }

        public ActionResult AllNews()
        {
            var AllN = (from x in db.News
                        orderby x.DateAdd descending
                        select x).ToList();
            var vm = new NewsList() { News = AllN };

            return View("Index", vm);
        }


        [ChildActionOnly]
        public ActionResult RightMenu()
        {

            var faktN = (from x in db.News
                         where x.VoteLogs.Where(n => n.Vote == true).Count() >= x.VoteLogs.Where(n => n.Vote == false).Count()
                         orderby ((decimal)(x.VoteLogs.Where(n => n.Vote == true).Count() - x.VoteLogs.Where(n => n.Vote == false).Count())) descending
                         select x
                         ).Take(10).ToList();

            var fakeN = (from x in db.News
                         where x.VoteLogs.Where(n => n.Vote == true).Count() <= x.VoteLogs.Where(n => n.Vote == false).Count()
                         orderby ((decimal)(x.VoteLogs.Where(n => n.Vote == true).Count() - x.VoteLogs.Where(n => n.Vote == false).Count())) ascending
                         select x
                         ).Take(10).ToList();

            var comments = (from x in db.News
                            orderby x.Collection_Comments.Count descending
                            select x
                            ).Take(10).ToList();

            var visitors = (from x in db.News
                            orderby x.Visitors descending
                            select x
                             ).Take(10).ToList();

            var usersRep = (from x in db.Users
                            orderby x.tbl_Comment.Count descending
                            select x).Take(5).ToList();

            var usersNews = (from x in db.Users
                             orderby x.tbl_News.Count descending
                             select x).Take(5).ToList();

            var usersComm = (from x in db.Users
                             orderby x.tbl_Comment.Count descending
                             select x).Take(5).ToList();

            var vm = new RightMenuModel() { FakeNews = fakeN, FaktNews = faktN, VisitedNews = visitors, CommentedNews = comments, UsersRep = usersRep, UsersNews = usersNews, UsersComm = usersComm };

            return PartialView(vm);
        }


    }
}