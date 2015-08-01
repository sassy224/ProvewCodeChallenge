using System.Web;
using System.Web.Optimization;

namespace Proview.CodeChallenge.Web
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            //base
            bundles.Add(new ScriptBundle("~/bundles/base-lib-js")
                .Include(
                        "~/bower_components/jquery/dist/jquery.js",
                        "~/bower_components/bootstrap/dist/js/bootstrap.js",
                        "~/bower_components/angular/angular.js",
                //"~/bower_components/angular-i18n/angular-locale_en.js",
                        "~/bower_components/tr-ng-grid/trNgGrid.js",
                        "~/bower_components/angular-route/angular-route.js",
                        "~/bower_components/angular-bootstrap/ui-bootstrap.js",
                        "~/bower_components/angular-bootstrap/ui-bootstrap-tpls.js",
                        "~/bower_components/mathjs/dist/math.min.js"
                ));
            bundles.Add(new StyleBundle("~/Content/base-lib-css")
                .Include("~/bower_components/bootstrap/dist/css/bootstrap.css", new CssRewriteUrlTransform())
                .Include("~/bower_components/tr-ng-grid/trNgGrid.min.css", new CssRewriteUrlTransform())
                );

            //proview
            bundles.Add(new ScriptBundle("~/bundles/proview-js")
                .IncludeDirectory("~/Scripts/", "*.js", false)
                        );
            bundles.Add(new StyleBundle("~/Content/proview-css").Include(
                "~/Content/*.css"
                ));
        }

    }
}