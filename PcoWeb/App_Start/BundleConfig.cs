using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace PcoWeb
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            const string KendoVersion = "2014.1.318";

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/kendoui").Include(
                        "~/Scripts/kendo/" + KendoVersion + "/kendo.web.min.js",
                        "~/Scripts/kendo/" + KendoVersion + "/cultures/kendo.culture.de-DE.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            Bundle css = new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/bootstrap-theme.css",
                "~/Content/sticky-footer.css",
                "~/Content/site.css",
                "~/Content/Live.css");

            css.Include("~/Content/kendo/" + KendoVersion + "/kendo.common.min.css", new CssRewriteUrlTransform());
            css.Include("~/Content/kendo/" + KendoVersion + "/kendo.common-bootstrap.min.css", new CssRewriteUrlTransform());
            css.Include("~/Content/kendo/" + KendoVersion + "/kendo.default.min.css", new CssRewriteUrlTransform());

            bundles.Add(css);
        }
    }
}