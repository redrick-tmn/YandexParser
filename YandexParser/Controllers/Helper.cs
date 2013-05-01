using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace YandexParser.Controllers
{
    public static class Helper
    {
        public static bool IsCurrent(this HtmlHelper html, string action, string controller)
        {
            var routeValues = html.ViewContext.RouteData.Values;
            var currentAction = routeValues["action"].ToString();
            var currentController = routeValues["controller"].ToString();
            return currentAction == action && currentController == controller;
        }
    }
}