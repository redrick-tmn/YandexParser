using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Web;
using HtmlAgilityPack;
using YandexParser.Models;

namespace YandexParser.Controllers
{
    public class YandexParser
    {
        private const string SearchQueryUri = @"http://yandex.ru/yandsearch?text={0}";
        private const string MozillaUserAgent = @"Mozilla/5.0 (Windows; I; Windows NT 5.1; ru; rv:1.9.2.13) Gecko/20100101 Firefox/4.0";

        public static IEnumerable<QueryResult> ParseQuery(string query)
        {
            var webClient = new WebClient();
            webClient.Headers.Add("user-agent", MozillaUserAgent);
            var pageHtml = webClient.DownloadString(string.Format(SearchQueryUri, Uri.UnescapeDataString(query)));
            var pageHtmlDoument = new HtmlDocument();
            pageHtmlDoument.LoadHtml(pageHtml);
            var items = pageHtmlDoument.DocumentNode.SelectNodes("//div[@class='b-body-items']/ol/li[not(contains(@class, 'z-images'))]/h2[@class='b-serp-item__title']");
            if (items == null)
                return null;

            var result = new List<QueryResult>();            
            foreach (var item in items)
            {
                var position = item.SelectSingleNode(item.XPath + "/b[@class='b-serp-item__number']");
                var link = item.SelectSingleNode(item.XPath + "/a[@class='b-serp-item__title-link']");

                if (position == null || link == null)
                    continue;

                var rawUrl = link.Attributes["href"];
                
                var title = link.InnerText;
               
                int positionValue;
                Uri uri;

                if (rawUrl == null || !Uri.TryCreate(rawUrl.Value, UriKind.Absolute, out uri) ||
                    string.IsNullOrEmpty(title) || !int.TryParse(position.InnerText, out positionValue))
                    continue;

                result.Add(new QueryResult()
                    {
                        Position = positionValue,
                        Title = title,
                        Url = uri.Host
                    });
            }
            return result;
        }
    }
}