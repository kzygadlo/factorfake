using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace notomyk.Infrastructure
{
    public static class myNewspaper
    {
        public static string GetNewspaperName(string link)
        {
            if (link.Substring(0, 4) == "www.")
            {
                return link.Substring(4, link.Length - 4);
            }
            return link;
        }
    }
}