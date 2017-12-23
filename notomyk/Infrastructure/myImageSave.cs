using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Hosting;

namespace notomyk.Infrastructure
{
    public static class myImageSave
    {
        private static string _link;
        private static string _imgName;
        private static string _path;
        private static string _fullPath;
        private static string _pathToReturn;

        public static string Link(string link)
        {
            _link = link;
            if (IfHttps(_link) == false)
            {
                _link = SaveImage(_link);
            }
            return _link;
        }

        private static bool IfHttps(string link)
        {
            if (link.Substring(0, 5).ToLower() == "https")
            {
                return true;
            }
            else
            {
                return false;
            }            
        }

        private static string SaveImage(string link)
        {
            _imgName = Guid.NewGuid().ToString() + ".png";
            _path = ConfigurationManager.AppSettings["NewsImages"];
            _fullPath = HostingEnvironment.MapPath(_path) + _imgName;

            var directory = new DirectoryInfo(HostingEnvironment.MapPath(ConfigurationManager.AppSettings["NewsImages"]));
            if (directory.Exists == false)
            {
                directory.Create();
            }

            using (WebClient client = new WebClient())
            {
                client.DownloadFile(link, _fullPath);
            }

            _pathToReturn = _path.Replace("~","") + _imgName;

            return _pathToReturn;
        }
    }
}