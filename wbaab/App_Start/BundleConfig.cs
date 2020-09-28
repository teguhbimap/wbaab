using System.Web;
using System.Web.Optimization;

namespace wbaab
{
    public class BundleConfig
    {
        //// For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        //public static void RegisterBundles(BundleCollection bundles)
        //{
        //    bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
        //                "~/Scripts/jquery-{version}.js"));

        //    bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
        //                "~/Scripts/jquery-ui-{version}.js"));

        //    bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
        //                "~/Scripts/jquery.unobtrusive*",
        //                "~/Scripts/jquery.validate*"));

        //    // Use the development version of Modernizr to develop with and learn from. Then, when you're
        //    // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
        //    bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
        //                "~/Scripts/modernizr-*"));

        //    bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

        //    bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
        //                "~/Content/themes/base/jquery.ui.core.css",
        //                "~/Content/themes/base/jquery.ui.resizable.css",
        //                "~/Content/themes/base/jquery.ui.selectable.css",
        //                "~/Content/themes/base/jquery.ui.accordion.css",
        //                "~/Content/themes/base/jquery.ui.autocomplete.css",
        //                "~/Content/themes/base/jquery.ui.button.css",
        //                "~/Content/themes/base/jquery.ui.dialog.css",
        //                "~/Content/themes/base/jquery.ui.slider.css",
        //                "~/Content/themes/base/jquery.ui.tabs.css",
        //                "~/Content/themes/base/jquery.ui.datepicker.css",
        //                "~/Content/themes/base/jquery.ui.progressbar.css",
        //                "~/Content/themes/base/jquery.ui.theme.css"));
        //    bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
        //                "~/Scripts/modernizr-*"));

        //    bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
        //              "~/Scripts/bootstrap.js"));

        //    bundles.Add(new StyleBundle("~/Content/css").Include(
        //              "~/Content/bootstrap.css",
        //              "~/Content/site.css"));

        //    bundles.Add(new StyleBundle("~/wallboard/css").Include(
        //              "~/Content/endless/bootstrap/css/bootstrap.min.css",
        //              "~/Content/endless/css/font-awesome.min.css",
        //              "~/Content/endless/css/pace.css",
        //              "~/Content/endless/css/colorbox/colorbox.css",
        //              "~/Content/endless/css/morris.css",

        //              "~/Content/endless/css/jcarousel.responsive.css",

        //              "~/Content/endless/css/endless.min.css",
        //              "~/Content/endless/css/endless-skin.css"
        //              ));

        //    bundles.Add(new StyleBundle("~/admin/css").Include(
        //              "~/Content/endless/bootstrap/css/bootstrap.min.css",
        //              "~/Content/endless/css/font-awesome.min.css",
        //              "~/Content/endless/css/pace.css",

        //              "~/Content/endless/css/jquery.dataTables_themeroller.css",
        //              "~/Content/endless/css/gritter/jquery.gritter.css",

        //              "~/Content/endless/css/endless.min.css",
        //              "~/Content/endless/css/endless-skin.css"
        //              ));

        //    bundles.Add(new ScriptBundle("~/wallboard/js").Include(
        //              "~/Content/endless/bootstrap/js/bootstrap.js",
        //              "~/Content/endless/js/jquery.flot.min.js",
        //              "~/Content/endless/js/rapheal.min.js",
        //              "~/Content/endless/js/morris.min.js",
        //              "~/Content/endless/js/jquery.colorbox.min.js",
        //              "~/Content/endless/js/jquery.sparkline.min.js",
        //              "~/Content/endless/js/uncompressed/pace.js",
        //              "~/Content/endless/js/jquery.popupoverlay.min.js",
        //              "~/Content/endless/js/jquery.slimscroll.min.js",
        //              "~/Content/endless/js/modernizr.min.js",
        //              "~/Content/endless/js/jquery.cookie.min.js",
        //              "~/Content/endless/js/endless/endless_dashboard.js",
        //              "~/Content/endless/js/endless/endless.js",

        //              "~/Content/endless/js/jquery.jcarousel.min.js",
        //              "~/Content/endless/js/jquery.resize.min.js",

        //              "~/Content/endless/js/chart.js/2.8.0/chart.bundle.min.2.8.0.js"
        //              ));

        //    bundles.Add(new ScriptBundle("~/admin/js").Include(
        //              "~/Content/endless/bootstrap/js/bootstrap.js",

        //              "~/Content/endless/js/pace.min.js",
        //              "~/Content/endless/js/jquery.popupoverlay.min.js",
        //              "~/Content/endless/js/jquery.slimscroll.min.js",
        //              "~/Content/endless/js/modernizr.min.js",
        //              "~/Content/endless/js/jquery.cookie.min.js",

        //              "~/Content/endless/js/jquery.dataTables.min.js",
        //              "~/Content/endless/js/jquery.gritter.min.js",

        //              "~/Content/endless/js/endless/endless.js"
        //              ));

        //    bundles.Add(new ScriptBundle("~/endless/jquery").Include(
        //             "~/Content/endless/js/jquery-1.10.2.min.js"));
        //}
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css")
                      .Include("~/Content/site.css"));

            bundles.Add(new StyleBundle("~/wallboard/css")
                .Include("~/Content/endless/bootstrap/css/bootstrap.min.css")
                .Include("~/Content/endless/css/font-awesome.min.css")
                .Include("~/Content/endless/css/pace.css")
                .Include("~/Content/endless/css/colorbox/colorbox.css")
                .Include("~/Content/endless/css/morris.css")
                .Include("~/Content/endless/css/jcarousel.responsive.css")
                .Include("~/Content/endless/css/endless.min.css")
                .Include("~/Content/endless/css/endless-skin.css"));

            bundles.Add(new StyleBundle("~/admin/css")
                .Include("~/Content/endless/bootstrap/css/bootstrap.min.css")
                .Include("~/Content/endless/css/font-awesome.min.css")
                .Include("~/Content/endless/css/pace.css")

                .Include("~/Content/endless/css/jquery.dataTables_themeroller.css")
                .Include("~/Content/endless/css/gritter/jquery.gritter.css")

                .Include("~/Content/endless/css/endless.min.css")
                .Include("~/Content/endless/css/endless-skin.css"
                      ));

            bundles.Add(new ScriptBundle("~/wallboard/js").Include(
                      "~/Content/endless/bootstrap/js/bootstrap.js",
                      "~/Content/endless/js/jquery.flot.min.js",
                      "~/Content/endless/js/rapheal.min.js",
                      "~/Content/endless/js/morris.min.js",
                      "~/Content/endless/js/jquery.colorbox.min.js",
                      "~/Content/endless/js/jquery.sparkline.min.js",
                      "~/Content/endless/js/uncompressed/pace.js",
                      "~/Content/endless/js/jquery.popupoverlay.min.js",
                      "~/Content/endless/js/jquery.slimscroll.min.js",
                      "~/Content/endless/js/modernizr.min.js",
                      "~/Content/endless/js/jquery.cookie.min.js",
                      "~/Content/endless/js/endless/endless_dashboard.js",
                      "~/Content/endless/js/endless/endless.js",

                      "~/Content/endless/js/jquery.jcarousel.min.js",
                      "~/Content/endless/js/jquery.resize.min.js",

                      "~/Content/endless/js/chart.js/2.8.0/chart.bundle.min.2.8.0.js"
                      ));

            bundles.Add(new ScriptBundle("~/admin/js").Include(
                      "~/Content/endless/bootstrap/js/bootstrap.js",

                      "~/Content/endless/js/pace.min.js",
                      "~/Content/endless/js/jquery.popupoverlay.min.js",
                      "~/Content/endless/js/jquery.slimscroll.min.js",
                      "~/Content/endless/js/modernizr.min.js",
                      "~/Content/endless/js/jquery.cookie.min.js",

                      "~/Content/endless/js/jquery.dataTables.min.js",
                      "~/Content/endless/js/jquery.gritter.min.js",

                      "~/Content/endless/js/endless/endless.js"
                      ));

            bundles.Add(new ScriptBundle("~/endless/jquery").Include(
                     "~/Content/endless/js/jquery-1.10.2.min.js"));
        }
    }
}