﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using YandexParser.App_Start;
using YandexParser.Controllers;
using YandexParser.Controllers.Helpers;
using YandexParser.Models;

namespace YandexParser
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Database.SetInitializer<QueryModel>(new DbInitializer());
            Database.DefaultConnectionFactory = new SqlCeConnectionFactory(
                "System.Data.SqlServerCe.4.0",
                @"|DataDirectory|",
                @"Data Source=|DataDirectory|\Database.sdf");
        }
    }
}