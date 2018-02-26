using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace notomyk
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute(
            name: "NewspaperName",
            url: "gazeta/{id}"
            , defaults: new { controller = "Main", action = "WhatNewspaper" }
            );

            routes.MapRoute(
            name: "PortalName",
            url: "portal/{id}"
            , defaults: new { controller = "Main", action = "WhatNewspaper" }
            );

            routes.MapRoute(
            name: "Default",
            url: "{controller}/{action}/{id}",
            defaults: new { controller = "Main", action = "Index", id = 0 });
        }

        public class NotEqual : IRouteConstraint
        {
            private string _match = String.Empty;

            public NotEqual(string match)
            {
                _match = match;
            }

            public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
            {
                return String.Compare(values[parameterName].ToString(), _match, true) != 0;
            }
        }
    }
}
