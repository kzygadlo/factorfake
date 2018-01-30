using notomyk.DAL;
using System.Linq;
using System.Web.Mvc;
using notomyk.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using notomyk.Infrastructure;
using notomyk.ViewModel;
using System;

namespace notomyk.Controllers
{
    public class AdminUserTableController : Controller
    {
        NTMContext db = new NTMContext();
        // GET: AdminUserTable
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult GetUsers()
        {

            DateTime date;
            
            //var newspapers = db.Newspaper.Include(c => c.Colection_Newses).ToList();
            var users = db.Users.
                OrderBy(o => o.UserName).
                Select(x => new
                {                   
                    x.UserName,
                    x.Email,
                    x.EmailConfirmed,
                    RoleName = db.Roles.FirstOrDefault(r => r.Id == x.Roles.FirstOrDefault().RoleId).Name,               
                    x.Id,
                    
                }).ToList();

            return Json(new { data = users }, JsonRequestBehavior.AllowGet);

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
                    user.RoleName = db.Roles.FirstOrDefault(r => r.Id == roleID ).Name;
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
                    u.UserName = _u.User.UserName;
                    u.Email = _u.User.Email;
                    u.EmailConfirmed = _u.User.EmailConfirmed;

                    string oldRoleName = "";
                    if (u.Roles.Count > 0)
                    {
                        string RoleID = u.Roles.FirstOrDefault().RoleId;
                        oldRoleName = db.Roles.FirstOrDefault(r => r.Id == RoleID ).Name;
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
                return RedirectToAction("Index", "Error", new { errorMessage = ErrorMessage.AdminTableSaveFailed});
            }

        }
    }
}