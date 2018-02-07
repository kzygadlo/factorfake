using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NLog;
using notomyk.DAL;
using notomyk.Infrastructure;
using notomyk.Models;
using notomyk.ViewModel;
using System;
using System.Linq;
using System.Web.Mvc;

namespace notomyk.Controllers
{
    public class AdminUserTableController : Controller
    {
        private static Logger FOFlog = LogManager.GetCurrentClassLogger();
        NTMContext db = new NTMContext();
        // GET: AdminUserTable
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            ViewBag.AdminUserTableClass = "active";
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult GetUsers()
        {

            //var newspapers = db.Newspaper.Include(c => c.Colection_Newses).ToList();
            var users = db.Users.
                OrderBy(o => o.UserName).
                Select(x => new
                {
                    x.UserName,
                    x.Email,
                    x.EmailConfirmed,
                    RoleName = db.Roles.FirstOrDefault(r => r.Id == x.Roles.FirstOrDefault().RoleId).Name,
                    Comm = x.Comments.Count,
                    News = x.tbl_News.Count,
                    x.LastActivity,
                    x.LockoutEnabled,
                    x.LockoutEndDateUtc,
                    x.Id
                }).ToList();


            var filtereUsers = users.Select(y => new
            {
                y.UserName,
                y.Email,
                y.EmailConfirmed,
                y.RoleName,
                LastActivity = ConvertToString.Date(y.LastActivity),
                BanTo = ConvertToString.Date(y.LockoutEndDateUtc),
                Comm = string.Concat(y.Id, ";", y.Comm),
                News = string.Concat(y.Id, ";", y.News),
                y.Id
            });

            return Json(new
            {
                data = filtereUsers
            }, JsonRequestBehavior.AllowGet);

        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Save(string ID)
        {
            UserWithRoleName user = new UserWithRoleName();
            if (ID != "0")
            {
                user.User = db.Users.Where(u => u.Id == ID).FirstOrDefault();

                if (user.User.Roles.Count > 0)
                {
                    string roleID = user.User.Roles.FirstOrDefault().RoleId;
                    user.RoleName = db.Roles.FirstOrDefault(r => r.Id == roleID).Name;
                }
                else
                {
                    user.RoleName = "User";
                }
                return View(user);
            }

            return View("Index");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Save(UserWithRoleName _u)
        {
            bool status = false;
            if (ModelState.IsValid)
            {

                if (_u.User.Id != "")
                {
                    var u = db.Users.Where(s => s.Id == _u.User.Id).FirstOrDefault();

                    if (db.Users.Any(x => x.Email == _u.User.Email && x.Id != _u.User.Id))
                    {
                        return RedirectToAction("Index", "Error", new { errorMessage = ErrorMessage.EmailIsTaken });
                    }

                    if (db.Users.Any(x => x.UserName == _u.User.UserName && x.Id != _u.User.Id))
                    {
                        return RedirectToAction("Index", "Error", new { errorMessage = ErrorMessage.UserNameIsTaken });
                    }

                    u.UserName = _u.User.UserName;
                    u.Email = _u.User.Email;
                    u.EmailConfirmed = _u.User.EmailConfirmed;
                    u.LockoutEnabled = _u.User.LockoutEnabled;

                    string oldRoleName = "";
                    if (u.Roles.Count > 0)
                    {
                        string RoleID = u.Roles.FirstOrDefault().RoleId;
                        oldRoleName = db.Roles.FirstOrDefault(r => r.Id == RoleID).Name;
                    }

                    //var oldRoleName = db.Roles.FirstOrDefault(r => r.Id == oldRoleID).Name;
                    if (oldRoleName != _u.RoleName)
                    {
                        var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new NTMContext()));

                        if (oldRoleName != "")
                        {
                            userManager.RemoveFromRole(_u.User.Id, oldRoleName);
                        }
                        userManager.AddToRole(_u.User.Id, _u.RoleName);
                    }
                    db.SaveChanges();
                    status = true;
                }
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors);

            if (status)
            {
                return View("Index");
            }
            else
            {
                return RedirectToAction("Index", "Error", new { errorMessage = ErrorMessage.AdminTableSaveFailed });
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Ban(int whatBan, string userID)        {
            
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new NTMContext()));

            var user = userManager.FindById(userID);
            if (user.LockoutEnabled)
            {
                switch (whatBan)
                {
                    case 0:
                        user.LockoutEndDateUtc = null;
                        break;
                    case 1:
                        user.LockoutEndDateUtc = DateTime.UtcNow.AddDays(1);
                        break;
                    case 2:
                        user.LockoutEndDateUtc = DateTime.UtcNow.AddDays(7);
                        break;
                    case 3:
                        user.LockoutEndDateUtc = DateTime.UtcNow.AddYears(2).AddDays(1);
                        break;
                }
            }

            //db.SaveChanges();
            userManager.Update(user);
            return RedirectToAction("Save", "AdminUserTable", new { ID = userID});
        }
    }
}