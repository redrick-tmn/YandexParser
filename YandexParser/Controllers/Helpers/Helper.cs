using System.Web.Mvc;

namespace YandexParser.Controllers.Helpers
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