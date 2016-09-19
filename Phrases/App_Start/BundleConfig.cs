using System.Web;
using System.Web.Optimization;

namespace Phrases
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            //// Use the development version of Modernizr to develop with and learn from. Then, when you're
            //// ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js",
            //          "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/main").Include(
                "~/Scripts/jQuery/jquery-3.1.0.min.js",
                "~/Scripts/Bootstrap/bootstrap.min.js",
                "~/Scripts/Plugins/jquery.slimscroll.js",
                "~/Scripts/Plugins/js.cookie.min.js", //??
                "~/Scripts/Plugins/js.cookie-2.1.2.min.js", //??
                "~/Scripts/Plugins/jquery.blockui.min.js",
                "~/Scripts/Plugins/bootstrap-switch.min.js",
                "~/Scripts/Plugins/toastr.js",
                "~/Scripts/Theme/theme.js",
                "~/Scripts/Theme/theme.layout.js",
                "~/Scripts/App/Custom.js"));

            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                "~/Scripts/jQuery/jquery-ui.min.js",
                "~/Scripts/Angular/angular.min.js",
                "~/Scripts/Angular/angular-touch.min.js",
                "~/Scripts/Angular/angular-route.min.js",
                "~/Scripts/Angular/angular-ui/ui-bootstrap-tpls.min.js",
                "~/Scripts/Angular/angular-ui/ui-sortable.min.js",
                "~/Scripts/Angular/angular-confirm.js",
                "~/Scripts/Angular/angular-soundmanager2.js",
                "~/Scripts/Angular/angular-bootstrap-switch.js",
                "~/Scripts/Angular/rzslider.js",

                "~/Scripts/App/Phrases.js",
                "~/Scripts/App/Services/PhrasesService.js",
                "~/Scripts/App/Services/ApiService.js",
                "~/Scripts/App/Filters/PhrasesFilter.js",
                "~/Scripts/App/Components/Phrases/PhrasesComponent.js",
                "~/Scripts/App/Components/User/UserComponent.js",
                "~/Scripts/App/Components/PhraseList/phraseListComponent.js",
                "~/Scripts/App/Components/EditPhrase/editPhraseComponent.js",
                "~/Scripts/App/Components/EditCategory/editCategoryComponent.js",
                "~/Scripts/App/Components/Volume/VolumeComponent.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Scripts/jQuery/jquery-ui.min.css",
                "~/Content/Bootstrap/bootstrap.min.css",
                "~/Content/Theme/layout.css",
                "~/Content/Theme/components-md.css",
                "~/Content/Theme/plugins-md.css",
                "~/Content/Theme/todo.css",
                "~/Content/Theme/blue.css",
                "~/Content/Theme/rzslider.css",
                "~/Content/Site.css",
                "~/Content/FontAwesome/font-awesome.min.css",
                "~/Content/Theme/simple-line-icons.min.css",
                "~/Content/Theme/bootstrap-switch.min.css",
                "~/Content/Theme/toastr.css",
                "~/Content/custom.css"));

            bundles.Add(new StyleBundle("~/Content/LoginRegister").Include(
                "~/Content/Theme/login.css"));


            // force minification for development
            // There is no realese version on production 
            // must be forced to minify
            BundleTable.EnableOptimizations = false;
        }
    }
}
