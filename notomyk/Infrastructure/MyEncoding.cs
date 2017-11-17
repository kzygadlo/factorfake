using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace notomyk.Infrastructure
{
    public class MyEncoding
    {
        public static string ReplaceSign(string input)
        {
            if (input != null)
            {
                if (input.IndexOf("&#34;") > 0)
                {
                    input = input.Replace("&#34;", "\"");
                }
                else
                {
                    input = input.Replace("&quot;", "\"");
                }           
            
            }
            
            return input;
        }
    }
}