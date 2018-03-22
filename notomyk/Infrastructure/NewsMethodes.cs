using HtmlAgilityPack;
using notomyk.DAL;
using notomyk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;


namespace notomyk.Infrastructure
{
    public static class NewsMethodes
    {
        public static bool CheckIfISO_889_2(string url)
        {
            var web = new HtmlWeb()
            {
                PreRequest = request =>
                {
                    request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                    return true;
                }
            };

            var document = web.Load(url);

            var metaTags = document.DocumentNode.SelectNodes("//meta");


            if (metaTags != null)
            {
                foreach (var tag in metaTags)
                {
                    var tagCharset = tag.Attributes["charset"];

                    if (tagCharset != null)
                    {
                        if (tagCharset.Value.ToLower() == "iso-8859-2")
                        {
                            return true;
                        }
                    }

                }
            }
            return false;
        }


        public static bool CheckIfISO_windows_1250(string url)
        {
            var web = new HtmlWeb()
            {
                PreRequest = request =>
                {
                    request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                    return true;
                }
            };

            var document = web.Load(url);

            var metaTags = document.DocumentNode.SelectNodes("//meta");


            if (metaTags != null)
            {

                foreach (var tag in metaTags)
                {
                    var tagCharset = tag.Attributes["charset"];

                    if (tagCharset != null)
                    {
                        if (tagCharset.Value.ToLower() == "utf-8")
                        {
                            return true;
                        }
                    }

                }
            }
            return false;
        }

        public static MetaInformation GetMetaDataFromUrl(string url)
        {

            //
            var web = new HtmlWeb()
            {
                PreRequest = request =>
                {
                    request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                    return true;
                }
            };

            HttpDownloader downloadWrapper = new HttpDownloader(url, null, null);


            HtmlDocument htmlDocument = new HtmlDocument();

            htmlDocument.LoadHtml(downloadWrapper.GetPage());

            var metaTags = htmlDocument.DocumentNode.SelectNodes("//meta");


            MetaInformation metaInfo = new MetaInformation();
            if (metaTags != null)
            {
                int matchCount = 0;
                metaInfo.Keywords = "";
                foreach (var tag in metaTags)
                {

                    var tagCharset = tag.Attributes["charset"];
                    var tagName = tag.Attributes["name"];
                    var tagContent = tag.Attributes["content"];
                    var tagProperty = tag.Attributes["property"];
                    if (tagName != null && tagContent != null)
                    {
                        switch (tagName.Value.ToLower())
                        {
                            case "title":
                                metaInfo.Title = tagContent.Value;
                                matchCount++;
                                break;
                            case "description":
                                metaInfo.Description = tagContent.Value;
                                matchCount++;
                                break;
                            case "twitter:title":
                                metaInfo.Title = string.IsNullOrEmpty(metaInfo.Title) ? tagContent.Value : metaInfo.Title;
                                matchCount++;
                                break;
                            case "twitter:description":
                                metaInfo.Description = string.IsNullOrEmpty(metaInfo.Description) ? tagContent.Value : metaInfo.Description;
                                matchCount++;
                                break;
                            case "keywords":
                                metaInfo.Keywords = tagContent.Value;
                                matchCount++;
                                break;
                            case "news_keywords":
                                metaInfo.Keywords = tagContent.Value;
                                matchCount++;
                                break;
                            case "twitter:image":
                                metaInfo.ImageUrl = string.IsNullOrEmpty(metaInfo.ImageUrl) ? tagContent.Value : metaInfo.ImageUrl;
                                matchCount++;
                                break;
                            case "og:title":
                                metaInfo.Title = string.IsNullOrEmpty(metaInfo.Title) ? tagContent.Value : metaInfo.Title;
                                matchCount++;
                                break;
                            case "og:description":
                                metaInfo.Description = string.IsNullOrEmpty(metaInfo.Description) ? tagContent.Value : metaInfo.Description;
                                matchCount++;
                                break;
                            case "og:image":
                                metaInfo.ImageUrl = string.IsNullOrEmpty(metaInfo.ImageUrl) ? tagContent.Value : metaInfo.ImageUrl;
                                matchCount++;
                                break;
                            case "og:site_name":
                                metaInfo.SiteName = string.IsNullOrEmpty(metaInfo.SiteName) ? (tagContent.Value).ToString().Replace("www.", "").Replace(".pl", "") : metaInfo.SiteName;
                                matchCount++;
                                break;
                        }
                    }
                    else if (tagProperty != null && tagContent != null)
                    {
                        switch (tagProperty.Value.ToLower())
                        {
                            case "og:title":
                                metaInfo.Title = string.IsNullOrEmpty(metaInfo.Title) ? tagContent.Value : metaInfo.Title;
                                matchCount++;
                                break;
                            case "og:description":
                                metaInfo.Description = string.IsNullOrEmpty(metaInfo.Description) ? tagContent.Value : metaInfo.Description;
                                matchCount++;
                                break;
                            case "og:image":
                                metaInfo.ImageUrl = string.IsNullOrEmpty(metaInfo.ImageUrl) ? tagContent.Value : metaInfo.ImageUrl;
                                matchCount++;
                                break;
                            case "og:site_name":
                                metaInfo.SiteName = string.IsNullOrEmpty(metaInfo.SiteName) ? (tagContent.Value).ToString().Replace("www.", "").Replace(".pl", "") : metaInfo.SiteName;
                                matchCount++;
                                break;
                        }
                    }
                }
                metaInfo.HasData = matchCount > 0;
            }
            return metaInfo;


        }

        public static string GetHomeURL(string url)
        {
            var HomeURL = new Uri(url);

            if (HomeURL.Host.Substring(0, 4) != "www.")
            {
                return string.Concat("www.", HomeURL.Host);

            }
            else
            {
                return HomeURL.Host;
            }

        }

        public static string MonthName(this DateTime value)
        {
            var MonthNum = value.Month;
            var MonthName = string.Empty;

            switch (MonthNum)
            {
                case 1:
                    MonthName = "Sty";
                    break;

                case 2:
                    MonthName = "Lut";
                    break;

                case 3:
                    MonthName = "Mar";
                    break;

                case 4:
                    MonthName = "Kwi";
                    break;

                case 5:
                    MonthName = "Maj";
                    break;

                case 6:
                    MonthName = "Cze";
                    break;
                case 7:
                    MonthName = "Lip";
                    break;

                case 8:
                    MonthName = "Sie";
                    break;

                case 9:
                    MonthName = "Wrz";
                    break;
                case 10:
                    MonthName = "Paz";
                    break;

                case 11:
                    MonthName = "Lis";
                    break;

                case 12:
                    MonthName = "Gru";
                    break;
            }

            return MonthName;
        }

        public static string ShowRepliesString(int repliesNum)
        {
            if (repliesNum > 0)
            {
                return string.Format("Pokaż wszystkie {0}", repliesNum);
            }
            else
            {
                return "";
            }
        }
        
    }
}
