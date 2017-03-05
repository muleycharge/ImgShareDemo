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
                "~/Scripts/jQuery-{version}.js",
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


            bundles.Add(new Bundle("~/bundles/home/app").Include(
                "~/Scripts/ng-file-upload.js",
                "~/Scripts.AngularJs/Home/app.js",
                "~/Scripts.AngularJs/Shared/ContentEditable.directive.js",
                "~/Scripts.AngularJs/Shared/ChipListing.directive.js",
                "~/Scripts.AngularJs/Shared/ChipDrop.directive.js",
                "~/Scripts.AngularJs/Home/ImageLibrary/ImageCard.directive.js",
                "~/Scripts.AngularJs/Home/ImageLibrary/ImageLibrary.controller.js",
                "~/Scripts.AngularJs/Home/ImageLibrary/ImageList.directive.js",
                "~/Scripts.AngularJs/Home/ImageLibrary/ImageListItem.directive.js",
                "~/Scripts.AngularJs/Shared/Tag.service.js",
                "~/Scripts.AngularJs/Shared/Asset.service.js"
            ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/site.css",
                      "~/Content/font-awesome.min.css"));
        }
    }
}
