using notomyk.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace notomyk.Infrastructure
{
    public static class tagsToViews
    {
        public static List<string> ReturnTags(List<string> tags)
        {            
            int counter = 0;
            int limit = Convert.ToInt16(ConfigurationManager.AppSettings["TagStrignLenght"]);
            List<string> finalTags = new List<string>();

            foreach (var tag in tags)
            {
                counter += tag.Count();

                if (counter < limit)
                {
                    finalTags.Add(tag);
                }
                else
                {
                    finalTags.Add("...");
                    return finalTags;
                }

            }
            return finalTags;
        }
    }
}