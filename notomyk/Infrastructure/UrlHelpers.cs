using System;
using System.Collections.Generic;
using System.Configuration;
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

        public static string FOFheaderLogo(this UrlHelper helper)
        {
            var fofLogosPath = ConfigurationManager.AppSettings["FOFlogos"];
            var fileName = "fofheaderlogo.png";
            var path = Path.Combine(fofLogosPath, fileName);
            
            return helper.Content(path);
        }

        public static string FOFfooterLogo(this UrlHelper helper)
        {
            var fofLogosPath = ConfigurationManager.AppSettings["FOFlogos"];
            var fileName = "foffooterlogo.png";
            var path = Path.Combine(fofLogosPath, fileName);

            return helper.Content(path);
        }

        public static string FOFiconLight(this UrlHelper helper)
        {
            var fofLogosPath = ConfigurationManager.AppSettings["FOFlogos"];
            var fileName = "foficonlight.png";
            var path = Path.Combine(fofLogosPath, fileName);

            return helper.Content(path);
        }

        public static string FOFiconDark(this UrlHelper helper)
        {
            var fofLogosPath = ConfigurationManager.AppSettings["FOFlogos"];
            var fileName = "foficondark.png";
            var path = Path.Combine(fofLogosPath, fileName);

            return helper.Content(path);
        }

        public static string FOFiconDarkW(this UrlHelper helper)
        {
            var fofLogosPath = ConfigurationManager.AppSettings["FOFlogos"];
            var fileName = "foficondarkW.png";
            var path = Path.Combine(fofLogosPath, fileName);

            return helper.Content(path);
        }

        public static string FOFiconJustF(this UrlHelper helper)
        {
            var fofLogosPath = ConfigurationManager.AppSettings["FOFlogos"];
            var fileName = "foficonlightNoBorder.png";
            var path = Path.Combine(fofLogosPath, fileName);

            return helper.Content(path);
        }

    }
}