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
            var newspaperList = db.Newspaper
                 .Where(n => n.Colection_Newses.Where(c => c.IsActive == true).Count() > 0)
                 .OrderBy(o => o.NewspaperName)
                 .Select(s => s.NewspaperName)
                 .Distinct()
                 .ToList();

            var tagList = db.Tag
                .Where(t => t.ListOfNews.Any(n => n.News.IsActive == true))
                .OrderByDescending(o => o.ListOfNews.Count).ToList();

            Filters vm = new Filters();
            vm.Newspapers = newspaperList;
            vm.Categories = tagList;

            return View(vm);
        }

        [HttpPost]
        public JsonResult Get(FilterModel filter)
        {
            List<tbl_News> listOfNews = GetNewsList(filter).ToList();

            var finalList = listOfNews.Select(x => new
            {
                CommentsNumber = Convert.ToInt32(x.Collection_Comments.Where(n => n.IsActive == true && n.Parenttbl_CommentID == null).Count()) + +Convert.ToInt32(x.Collection_Comments.Where(n => n.IsActive == true && n.Parenttbl_CommentID != null && n.Parent.IsActive == true).Count()),
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
                Fake = x.VoteLogs.Where(v => v.Vote == false).Count(),
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
                    ratingClass = Rating.RatingClass(x.Fakt, x.Fake),
                    ratingValue = Rating.RatingValue(x.Fakt, x.Fake),
                    faktValue = x.Fakt,
                    fakeValue = x.Fake,
                    remainingRows = filter.Remains,
                    tagList = x.Tags
                }), JsonRequestBehavior.AllowGet);
        }

        public IQueryable<tbl_News> GetNewsList(FilterModel filter)
        {
            var result = db.News.Where(n => n.IsActive == true).AsQueryable();
            int votingValue = Convert.ToInt32(ConfigurationManager.AppSettings["FilterVoting"]);
            int commentValue = Convert.ToInt32(ConfigurationManager.AppSettings["FilterComments"]);
            int visitorsValue = Convert.ToInt32(ConfigurationManager.AppSettings["FilterVisitors"]);

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
                            result = result.Where(n => n.VoteLogs.Where(v => v.Vote == true).Count() + n.VoteLogs.Where(v => v.Vote == false).Count() >= votingValue
                                && (n.VoteLogs.Where(v => v.Vote == false).Count() == 0 || n.VoteLogs.Where(v => v.Vote == true).Count() / n.VoteLogs.Where(v => v.Vote == false).Count() > 2));
                            break;
                        case 2: //Top Fake
                            result = result.Where(n => n.VoteLogs.Where(v => v.Vote == true).Count() + n.VoteLogs.Where(v => v.Vote == false).Count() >= votingValue
                                && (n.VoteLogs.Where(v => v.Vote == true).Count() == 0 || n.VoteLogs.Where(v => v.Vote == false).Count() / n.VoteLogs.Where(v => v.Vote == true).Count() > 2));
                            break;
                        case 3: //Top Comments
                            result = result.Where(n => n.Collection_Comments.Where(c => c.IsActive == true).Count() > commentValue).OrderByDescending(o => o.Collection_Comments.Where(c => c.IsActive == true).Count());
                            break;
                        case 4: //Top Visits
                            result = result.Where(n => n.Visitors > visitorsValue).OrderByDescending(o => o.Visitors);
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
            filter.Remains = result.Count() - 10 - filter.Page * 10;

            if (filter.WhatNews == 0 || filter.WhatNews == 1 || filter.WhatNews == 2)
            {
                result = result.OrderByDescending(r => r.DateAdd);
            }

            result = result.Skip(filter.Page * 10).Take(10);

            return result;
        }

        public ActionResult News(int ID)
        {
            if (!Request.IsAuthenticated)
            {
                ViewBag.popupMsg = "Musisz byc zalogowany aby oddać głos.";
            }

            ViewBag.NewsID = ID;

            var currentUserID = User.Identity.GetUserId();
            int whatVote = 0;
            var isVoted = db.VoteLog.Where(v => v.tbl_NewsID == ID && v.UserId == currentUserID).FirstOrDefault();

            if (isVoted != null)
            {
                whatVote = isVoted.Vote ? 1 : -1;
            }


            var singleNews = db.News.Where(n => n.tbl_NewsID == ID && n.IsActive == true).FirstOrDefault();

            if (singleNews == null)
            {
                return RedirectToAction("Index", "Main");
            }

            var leftNews = db.News
                .Where(n => n.tbl_NewspaperID == singleNews.tbl_NewspaperID && n.IsActive == true)
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

            var fofUrl1 = "http://www.faktorfake.pl";
            var fofUrl2 = Url.Action("News", "Main", new { ID = ID });

            ViewBag.fbButtonUrl = fofUrl1 + fofUrl2;

            ViewBag.ogTitle = "FAKTORFAKE : " + singleNews.Title;
            ViewBag.ogDescription = singleNews.Description;
            ViewBag.ogImage = fofUrl1 + "/Images/Logos/250/" + singleNews.Newspaper.NewspaperIconLink;

            var commNumber = singleNews.Collection_Comments.Where(n => n.IsActive == true && n.Parenttbl_CommentID == null).Count() + singleNews.Collection_Comments.Where(n => n.IsActive == true && n.Parenttbl_CommentID != null && n.Parent.IsActive == true).Count();
            ViewBag.CommNum = commNumber;

            vm.SingleNews = singleNews;
            vm.LeftNews = leftNews;
            vm.CommaSeparatedTags = CommaSeparatedTags;
            vm.WhatVote = whatVote;

            return View(vm);

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
        public ActionResult RightMenu()
        {

            var faktN = db.News
                .Where(n => n.VoteLogs.Where(v => v.Vote == true).Count() >= n.VoteLogs.Where(v => v.Vote == false).Count())
                .Where(n => n.IsActive == true)
                .OrderByDescending(n => n.VoteLogs.Where(v => v.Vote == true).Count() - n.VoteLogs.Where(v => v.Vote == false).Count())
                .Take(10)
                .ToList();

            var fakeN = db.News
                .Where(n => n.VoteLogs.Where(v => v.Vote == false).Count() >= n.VoteLogs.Where(v => v.Vote == true).Count())
                .Where(n => n.IsActive == true)
                .OrderByDescending(n => n.VoteLogs.Where(v => v.Vote == false).Count() - n.VoteLogs.Where(v => v.Vote == true).Count())
                .Take(10)
                .ToList();

            var comments = db.News
                .Where(n => n.IsActive == true)
                .OrderByDescending(n => n.Collection_Comments.Where(c => c.IsActive == true && c.Parenttbl_CommentID == null).Count() + n.Collection_Comments.Where(c => c.IsActive == true && c.Parenttbl_CommentID != null && c.Parent.IsActive == true).Count())
                .Take(10)
                .ToList();

            var visitors = db.News
                .Where(n => n.IsActive == true)
                .OrderByDescending(n => n.Visitors)
                .Take(10)
                .ToList();

            int MinCommentsForReputation = int.Parse(ConfigurationManager.AppSettings["MinCommentsForReputation"]);

            var userR = db.Users.Select(
                o => new UserReputation
                {
                    Id = o.Id,
                    UserName = o.UserName,
                    Pcomments = o.tbl_Comment.Where(c => c.IsActive == true && c.Fakt > c.Fake && (c.Fakt + c.Fake) > MinCommentsForReputation).Count(),
                    Acomments = o.tbl_Comment.Where(c => c.IsActive == true && (c.Fakt + c.Fake) > MinCommentsForReputation).Count(),
                    Reputation = (o.tbl_Comment.Where(c => c.IsActive == true && (c.Fakt + c.Fake) > MinCommentsForReputation).Count() == 0 ? 0 : o.tbl_Comment.Where(c => c.IsActive == true && c.Fakt > c.Fake && (c.Fakt + c.Fake) > MinCommentsForReputation).Count() / (1.0 * o.tbl_Comment.Where(c => c.IsActive == true && (c.Fakt + c.Fake) > MinCommentsForReputation).Count()))
                }
                ).OrderByDescending(o => o.Reputation)
                .Take(5).ToList();

            var usersNews = (from x in db.Users
                             orderby x.tbl_News.Count descending
                             select x).Take(5).ToList();

            var usersComm = (from x in db.Users
                             orderby x.tbl_Comment.Count descending
                             select x).Take(5).ToList();

            

            var vm = new RightMenuModel() { 
                FakeNews = fakeN, 
                FaktNews = faktN, 
                VisitedNews = visitors, 
                CommentedNews = comments, 
                UsersRep = userR, 
                UsersNews = usersNews, 
                UsersComm = usersComm 
            };

            return PartialView(vm);
        }

        [ChildActionOnly]
        public ActionResult RightMenuForum()
        {
            RightMenuForum model = new RightMenuForum(); 
            model.Topics = db.ForumTopic.Where(t => t.OnMainPage != null).OrderBy(o => o.OnMainPage).ToList();

            return PartialView(model);
        }
    }
}