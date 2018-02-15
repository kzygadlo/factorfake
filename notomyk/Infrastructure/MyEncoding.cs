using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace notomyk.Infrastructure
{
    public class myEncoding
    {
        public static string ReplaceSign(string input)
        {
            if (input != null)
            {
              
                if (input.IndexOf("&amp;quot;") > 0)
                {
                    input = input.Replace("&amp;quot;", "'");
                }

                if (input.IndexOf("&#34;") > 0)
                {
                    input = input.Replace("&#34;", "\"");
                }

                if (input.IndexOf("&#8222;") > 0)
                {
                    input = input.Replace("&#8222;", "\"");
                }

                if (input.IndexOf("&#8221;") > 0)
                {
                    input = input.Replace("&#8221;", "\"");
                }

                if (input.IndexOf("&quot;") > 0)
                {
                    input = input.Replace("&quot;", "\"");
                }

                if (input.IndexOf("&#347;") > 0)
                {
                    input = input.Replace("&#347;", "ś");
                }

                if (input.IndexOf("&#346;") > 0)
                {
                    input = input.Replace("&#346;", "Ś");
                }

                if (input.IndexOf("&#39;") > 0)
                {
                    input = input.Replace("&#39;", "'");
                }

                if (input.IndexOf("&#039;") > 0)
                {
                    input = input.Replace("&#039;", "'");
                }
            }

            return input;
        }
    }
}