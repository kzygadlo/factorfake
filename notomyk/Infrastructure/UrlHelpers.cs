using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace notomyk.Infrastructure
{
    public static class UrlHelpers
    {
        public static string NewspaperIconPath_ICO(this UrlHelper helper, string newspaperIconFilename)
        {
            var newspaperIconFolder = AppConfig.NewspaperIconsFolder_ICO;
            var path = Path.Combine(newspaperIconFolder, newspaperIconFilename);
            return helper.Content(path);
        }

        public static string NewspaperIconPath_250(this UrlHelper helper, string newspaperIconFilename)
        {
            var newspaperIconFolder = AppConfig.NewspaperIconsFolder_250;
            var path = Path.Combine(newspaperIconFolder, newspaperIconFilename);
            return helper.Content(path);
        }


        public static string NewspaperIconPath_wide(this UrlHelper helper, string newspaperIconFilename)
        {
            var newspaperIconFolder = AppConfig.NewspaperIconsFolder_wide;
            var path = Path.Combine(newspaperIconFolder, newspaperIconFilename);
            return helper.Content(path);
        }

    }
}