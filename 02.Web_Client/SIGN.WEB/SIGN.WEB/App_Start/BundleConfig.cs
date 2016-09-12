﻿using System.Web;
using System.Web.Optimization;

namespace SIGN.WEB
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();

            // Add angularJS plug-in.
            bundles.Add(new ScriptBundle("~/bundlesangularjs").Include(
                "~/Scripts/angular.min.js",
                "~/Scripts/angular-route.min.js",
                "~/Scripts/angular-animate.min.js",
                "~/Scripts/angular-sanitize.min.js"));

            bundles.Add(new ScriptBundle("~/bundlesjquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundlesjqueryval").Include(
                        "~/Scripts/jquery.validate.min.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
        }
    }
}