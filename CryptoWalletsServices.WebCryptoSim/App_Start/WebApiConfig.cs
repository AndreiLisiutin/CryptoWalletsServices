﻿using CryptoWalletsServices.WebCryptoSim.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace CryptoWalletsServices.WebCryptoSim
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
			config.Filters.Add(new BaseExceptionHandleAttribure());

			config.MapHttpAttributeRoutes();

			// Route to index.html
			config.Routes.MapHttpRoute(
				name: "Index",
				routeTemplate: "{id}.html",
				defaults: new { id = "index" });

			config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
		}
    }
}
