﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using HtmlAgilityPack;

namespace YandexParser.Controllers.Helpers
{
    public class YandexParser
    {
        private const string SearchQueryUri = @"http://yandex.ru/yandsearch?text={0}";
      //  private const string MozillaUserAgent = @"Mozilla/5.0 (Windows; I; Windows NT 5.1; ru; rv:1.9.2.13) Gecko/20100101 Firefox/4.0";

        public static IEnumerable<dynamic> ParseQuery(string query)
        {
#if DEBUG
            var pageHtml = File.ReadAllText(HttpContext.Current.Server.MapPath("~/Content/yandex.html"));
#else 
            var webClient = new WebClient
                {
                    Encoding = Encoding.GetEncoding("utf-8")
                };
            // webClient.Headers.Add("user-agent", MozillaUserAgent);
            var pageHtml = webClient.DownloadString(string.Format(SearchQueryUri, Uri.UnescapeDataString(query)));
#endif
            var pageHtmlDoument = new HtmlDocument();
            pageHtmlDoument.LoadHtml(pageHtml);
            var items = pageHtmlDoument.DocumentNode.SelectNodes("//div[@class='b-body-items']/ol/li[not(contains(@class, 'z-images'))]");
            if (items == null)
                return null;

            var result = new List<dynamic>();            
            foreach (var item in items)
            {
                var position = item.SelectSingleNode(item.XPath + "/h2[@class='b-serp-item__title']/b[@class='b-serp-item__number']");
                var link = item.SelectSingleNode(item.XPath + "/h2[@class='b-serp-item__title']/a[@class='b-serp-item__title-link']");
                var description = item.SelectNodes(item.XPath + "/div[@class='b-serp-item__text']");
                if (position == null || link == null || description == null)
                    continue;

                var rawUrl = link.Attributes["href"];
                var title = link.InnerText;
                var desc = description.Aggregate(new StringBuilder(), (current, d) => current.AppendLine(d.InnerText)).ToString();

                int positionValue;
                Uri uri;

                if (rawUrl == null || 
                    !Uri.TryCreate(rawUrl.Value, UriKind.Absolute, out uri) ||
                    string.IsNullOrEmpty(title) || 
                    string.IsNullOrEmpty(desc) ||
                    !int.TryParse(position.InnerText, out positionValue))
                    continue;

                result.Add(new 
                    {
                        Position = positionValue,
                        Title = HttpUtility.HtmlDecode(title),
                        Url = (uri.Scheme + "://" + uri.Host).ToLower(),
                        Description = HttpUtility.HtmlDecode(desc)
                    });
            }
            
            return result;
        }
    }
}