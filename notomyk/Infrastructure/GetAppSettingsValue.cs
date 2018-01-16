using notomyk.DAL;
using notomyk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace notomyk.Infrastructure
{
    public static class GetAppSettingsValue
    {
        public static string Value(string Key)
        {
            switch (Key)
            {
                case "CommentsLimitAdmin":
                    return "999";

                case "CommentsLimitModerator":
                    return "999";

                case "CommentsLimitUser":
                    return "20";

                case "CommentsLimitNotConfirmed":
                    return "5";

                case "CommentsAddDelayAdmin":
                    return "0";

                case "CommentsAddDelayModerator":
                    return "0";

                case "CommentsAddDelayUser":
                    return "30";

                case "NewsLimitAdmin":
                    return "999";

                case "NewsLimitModerator":
                    return "999";
                  
                case "NewsLimitUser":
                    return "10";
                   
                case "NewsLimitNotConfirmed":
                    return "1";                   

                case "NewNewsHours":
                    return "48";
                  
                case "FilterVoting":
                    return "4";
                    
                case "FilterVisitors":
                    return "2";
                    
                case "FilterComments":
                    return "2";
                    
                case "TagStrignLenght":
                    return "42";
                    
                case "MinCommentsForReputation":
                    return "2";
                   
                case "MinNumberVotes":
                    return "2";
            
                default:
                    return "1";

            }

        }
    }
}