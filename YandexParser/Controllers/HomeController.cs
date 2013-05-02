using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HtmlAgilityPack;
using YandexParser.Models;
using System.Web.UI.DataVisualization;

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

        //
        // GET: /Queries/

        public ActionResult Queries()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Search(string query)
        {

            var results = YandexParser.ParseQuery(query);
            using (var queryModel = new QueryModel())
            {
              //  queryModel.Database.ExecuteSqlCommand("DELETE FROM QueryResults");
              //  queryModel.Database.ExecuteSqlCommand("DELETE FROM Queries"); 
                var queryObj = new Query()
                    {
                        Date = DateTime.Now,
                        QueryText = query
                    };
                queryModel.Queries.Add(queryObj);
                foreach (var queryResult in results)
                {
                    queryResult.Query = queryObj;
                    queryModel.QueryResults.Add(queryResult);
                }
                queryModel.SaveChanges();
            }
            return PartialView(results);
        }

        public JsonResult QueriesJson()
        {
            using (var queryModel = new QueryModel())
            {
                var joinResult = queryModel.QueryResults.Join(queryModel.Queries, result => result.QueryId, query => query.Id,
                                             (result, query) => new
                                                 {
                                                    result.Id,
                                                    result.Position,
                                                    result.Url,
                                                    result.Title,
                                                    query.QueryText,
                                                    query.Date
                                                 });
                return Json(joinResult.ToArray(), JsonRequestBehavior.AllowGet);
            }
        }

    }
}
