using System.Web;
using System.Web.Optimization;

namespace ImgShareDemo
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new Bundle("~/bundles/base").Include(
                "~/Scripts/jQuery-*",
                "~/Scripts/modernizr-*",
                "~/Scripts/bootstrap.js",
                "~/Scripts/angular.js",
                "~/Scripts/angular-resource.js",
                "~/Scripts/angular-route.js",
                "~/Scripts/angular-sanitize.js",
                "~/Scripts/angular-animate.js",
                "~/Scripts/ng-file-upload.min.js",
                "~/Scripts/angular-ui/ui-bootstrap.js",
                "~/Scripts/angular-ui/ui-bootstrap-tpls.js",
                "~/Scripts/lodash.core.min.js",
                "~/Scripts/lodash.min.js"));

            bundles.Add(new Bundle("~/bundles/app").Include(
                "~/Scripts.AngularJs/Home/App.js"
                ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
        }
    }
}
