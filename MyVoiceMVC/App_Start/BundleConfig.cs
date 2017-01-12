using System.Web;
using System.Web.Optimization;

namespace MyVoiceMVC
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"
                        ));

            //// Use the development version of Modernizr to develop with and learn from. Then, when you're
            //// ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js",
            //          "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                "~/Scripts/jQuery/jquery-3.1.0.min.js",
                "~/Scripts/Bootstrap/bootstrap.min.js",
                //"~/Scripts/Plugins/jquery.scrollbar/jquery.scrollbar.min.js",
                "~/Scripts/Plugins/toastr.js",
                //"~/Scripts/Theme/theme.js",
                //"~/Scripts/Theme/theme.layout.js",
                "~/Scripts/App/Custom.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/app/js").Include(
                //"~/Scripts/jQuery/jquery-ui.min.js",
                //"~/Scripts/Angular/angular.min.js",
                //"~/Scripts/Angular/angular-ui/ui-bootstrap-tpls.min.js",
                //"~/Scripts/Angular/angular-touch.min.js", //fixes ipad... http://stackoverflow.com/questions/27423437/angular-ng-click-function-not-called-on-touchscreen-devices
                //                                          //"~/Scripts/Angular/angular-route.min.js",
                //"~/Scripts/Angular/angular-http-loader.min.js",
                //"~/Scripts/Angular/angular-confirm.js",
                //"~/Scripts/Angular/angular-upload.min.js",

                //"~/Scripts/App/Phrases.js",
                //"~/Scripts/App/Services/PhrasesService.js",
                //"~/Scripts/App/Services/ApiService.js",
                //"~/Scripts/App/Services/SoundService.js",
                //"~/Scripts/App/Filters/PhrasesFilter.js",
                //"~/Scripts/App/Components/Phrases/PhrasesComponent.js",
                //"~/Scripts/App/Components/User/UserComponent.js",
                //"~/Scripts/App/Components/PhraseList/phraseListComponent.js",
                //"~/Scripts/App/Components/CategorySidebar/categorySidebarComponent.js",
                //"~/Scripts/App/Components/EditPhrase/editPhraseComponent.js",
                //"~/Scripts/App/Directives/onFileChangeDirective.js",
                //"~/Scripts/App/Components/EditCategory/editCategoryComponent.js",
                //"~/Scripts/App/Components/Volume/VolumeComponent.js"
                
                "~/Scripts/Angular/angular.min.js",
                "~/Scripts/Angular/angular-ui/ui-bootstrap-tpls.min.js",
                "~/Scripts/Angular/angular-ui/ui-router.min.js",
                "~/Scripts/Angular/angular-touch.min.js",
                "~/Scripts/Angular/angular-http-loader.min.js",
                "~/Scripts/Angular/angular-confirm.js",
                "~/Scripts/Angular/angular-upload.min.js",
                "~/Scripts/App/MyVoice.js",
                "~/Scripts/App/Services/PhrasesService.js",
                "~/Scripts/App/Services/ApiService.js",
                "~/Scripts/App/Services/SoundService.js",
                "~/Scripts/App/Services/UserService.js",
                "~/Scripts/App/Filters/PhrasesFilter.js",
                "~/Scripts/App/Components/Phrases/PhrasesComponent.js",
                "~/Scripts/App/Components/User/UserComponent.js",
                "~/Scripts/App/Components/PhraseList/phraseListComponent.js",
                "~/Scripts/App/Components/CategorySidebar/categorySidebarComponent.js",
                "~/Scripts/App/Components/EditPhrase/editPhraseComponent.js",
                "~/Scripts/App/Components/EditCategory/editCategoryComponent.js",
                "~/Scripts/App/Components/Login/LoginComponent.js",
                "~/Scripts/App/Directives/onFileChangeDirective.js"

                ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                
                "~/Content/Bootstrap/bootstrap.min.css",
                "~/Content/Theme/layout.css",
                "~/Content/Theme/components-md.css",
                "~/Content/Theme/todo.css",
                "~/Content/Theme/blue.css",
                "~/Content/Site.css",
                "~/Content/FontAwesome/font-awesome.min.css",
                "~/Content/Theme/simple-line-icons.min.css",
                "~/Content/Theme/bootstrap-switch.min.css",
                "~/Content/Theme/toastr.css",
                "~/Content/custom.css",
                "~/Content/Theme/login.css"

                ));

            bundles.Add(new StyleBundle("~/Content/app/css").Include(
                //"~/Scripts/jQuery/jquery-ui.min.css",
                //"~/Scripts/Angular/angular-ui/ng-sortable.min.css"
                //"~/Scripts/Plugins/jquery.scrollbar/jquery.scrollbar.css"
                ));

            bundles.Add(new StyleBundle("~/Content/LoginRegister").Include(
                "~/Content/Theme/login.css"
                ));


            // force minification for development
            // There is no realese version on production 
            // must be forced to minify
            BundleTable.EnableOptimizations = true;
        }
    }
}
