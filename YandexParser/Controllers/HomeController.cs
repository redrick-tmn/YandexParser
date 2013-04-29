using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HtmlAgilityPack;
using YandexParser.Models;

namespace YandexParser.Controllers
{
    public class HomeController : Controller
    {

        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Search(string query)
        {
            var results = YandexParser.ParseQuery(query);
            return PartialView(results);
        }

    }
}
