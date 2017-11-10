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
        public static string ProgressBarStyle(int Fakt, int Fake)
        {
            string FaktColor = "rgba(92, 184, 92, 0)";
            string FakeColor = "rgba(217, 83, 79, 0)";
            //string RightColor = "rgba(255, 255, 255, 1)";
            string RightColor = "rgba(220, 220, 220, 0);";
            string Color;
            int Percent = Math.Abs(FFpercentages(Fakt, Fake));

            if (Fakt > Fake)
            {
                Color = FaktColor;
            }
            else
            {
                Color = FakeColor;
            }

            return string.Format("background: linear-gradient(to right, {0} 0%,{0} 0%,{0} {1}%, {2} {1}%, {2} 100%)", Color, Percent, RightColor);
        }

        public static int FFpercentages(int Fakt, int Fake)
        {
            return (int)(((decimal)(Fakt - Fake) / (Fakt + Fake)) * 100);
        }

        //public static string UserIcon(string UserIconName)
        //{
        //    var iconFolder = AppConfig.IconFolder;
        //    var userIconName = UserIconName ?? "user_icon_default.jpg";
        //    return  Regex.Replace(Path.Combine(iconFolder, userIconName), @"~", "");
        //}

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