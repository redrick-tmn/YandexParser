using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using YandexParser.Models;

namespace YandexParser.Controllers
{
    public class HomeController : Controller
    {
        private readonly QueryModel mQueryModel = new QueryModel();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Chart() 
        {
            ViewBag.Sites = mQueryModel.Sites;
            return View();
        }

        public ActionResult Queries()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Search(string query)
        {

            var results = Helpers.YandexParser.ParseQuery(query);

         //     mQueryModel.Database.ExecuteSqlCommand("DELETE FROM QueryResults");
         //       mQueryModel.Database.ExecuteSqlCommand("DELETE FROM Sites");
         //       mQueryModel.Database.ExecuteSqlCommand("DELETE FROM Queries"); 
            var queryObj = new Query
                {
                    Date = DateTime.Now,
                    QueryText = query
                };
            mQueryModel.Queries.Add(queryObj);
            var viewResult = new List<QueryResult>();
            foreach (var result in results)
            {
                var url = result.Url as string;
                var site = mQueryModel.Sites.FirstOrDefault(x => x.Url == url) ?? new Site { Url = result.Url };
                var queryResult = new QueryResult
                    {
                    Site = site,
                    Query = queryObj,
                    Title = result.Title,
                    Position = result.Position,
                    Description = result.Description
                };
                mQueryModel.QueryResults.Add(queryResult);
                viewResult.Add(queryResult);
                mQueryModel.SaveChanges();
            }
            return PartialView(viewResult);
        }

        public JsonResult QueriesJson()
        {
            var queryResults = mQueryModel.Sites.Join(mQueryModel.QueryResults, site => site.Id, result => result.SiteId,
                (site, result) => new
                {
                    result.QueryId,
                    site.Url,
                    result.Position,
                    result.Title
                });
            var joinResult = queryResults.Join(mQueryModel.Queries, result => result.QueryId, query => query.Id,
                                            (result, query) => new
                                                {
                                                query.Id,
                                                result.Position,
                                                result.Url,
                                                result.Title,
                                                query.QueryText,
                                                query.Date
                                                });
            return Json(joinResult, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            { 
                mQueryModel.Dispose(); 
            }
            base.Dispose(disposing);
        }

    }
}
