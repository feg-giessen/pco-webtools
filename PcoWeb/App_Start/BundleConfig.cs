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
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/kendoui").Include(
                        "~/Scripts/kendo/2013.3.1119/kendo.web.min.js",
                        "~/Scripts/kendo/2013.3.1119/cultures/kendo.culture.de-DE.min.js"));

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

            css.Include("~/Content/kendo/2013.3.1119/kendo.common.min.css", new CssRewriteUrlTransform());
            css.Include("~/Content/kendo/2013.3.1119/kendo.common-bootstrap.min.css", new CssRewriteUrlTransform());
            css.Include("~/Content/kendo/2013.3.1119/kendo.default.min.css", new CssRewriteUrlTransform());

            bundles.Add(css);
        }
    }
}