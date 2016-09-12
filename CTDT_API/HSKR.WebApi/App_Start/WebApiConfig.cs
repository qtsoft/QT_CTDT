﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;

namespace CTDT.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
            var cors = new EnableCorsAttribute("*","Origin, Content-Type, Accept",
                                                   "GET, PUT, POST, DELETE, OPTIONS");
            config.EnableCors();

        
            // Web API routes
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
            name: "CTDTApi",
            routeTemplate: "api/{controller}/{action}/{id}",
            defaults: new { id = RouteParameter.Optional });

            config.Routes.MapHttpRoute(
           name: "DefaultApi",
           routeTemplate: "api/{controller}/{id}",
           defaults: new { id = RouteParameter.Optional }
       );

            var formatters = GlobalConfiguration.Configuration.Formatters;
            formatters.Remove(formatters.XmlFormatter); 
        }
    }
}