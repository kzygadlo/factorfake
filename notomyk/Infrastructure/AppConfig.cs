using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using notomyk.DAL;
using notomyk.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace notomyk.Infrastructure
{
    public class AppConfig
    {
        #region Paths


        public static string _newspaperIconsFolder { get; private set; }

        public static string IconFolder
        {
            get {
                return ConfigurationManager.AppSettings["UserIconsFolder"];
            }
        }

        public static string NewspaperIconsFolder_ICO
        {
            get
            {
                return ConfigurationManager.AppSettings["NewspapersIconsFolder_ICO"];
            }
        }

        public static string NewspaperIconsFolder_250
        {
            get
            {
                return ConfigurationManager.AppSettings["NewspapersIconsFolder_250"];
            }
        }

        public static string NewspaperIconsFolder_wide
        {
            get
            {
                return ConfigurationManager.AppSettings["NewspapersIconsFolder_wide"];
            }
        }

#endregion Paths



        public static string UserLogoLink(string userID)
        {
            var imgPath = System.Configuration.ConfigurationManager.AppSettings["UserIconsFolder"] + userID + ".png";
            var imgUrl = System.Configuration.ConfigurationManager.AppSettings["UserIconsFolder"] + "user_icon_default.jpg";

            if (File.Exists(System.Web.HttpContext.Current.Server.MapPath(imgPath)))
            {
                imgUrl = imgPath + "?time=" + DateTime.Now.ToString();
            }

            return imgUrl;
        }

        public static Boolean isAdminUser()
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = HttpContext.Current.User.Identity;
                NTMContext context = new NTMContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());
                if (s[0].ToString() == "Admin")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }  


    }
}