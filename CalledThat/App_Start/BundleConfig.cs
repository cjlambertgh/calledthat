using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace CalledThat.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            var jsDir = "~/Scripts/startmin-javascript/";
            var mainBundle = new ScriptBundle("~/bundles/main").Include(
                $"{jsDir}jquery.min.js",
                $"{jsDir}bootstrap.min.js",
                $"{jsDir}metisMenu.min.js",
                $"{jsDir}startmin.js",
                $"{jsDir}jquery-bootpag.min.js"
                );

            mainBundle.Orderer = new AsIsBundleOrderer();
            bundles.Add(mainBundle);
            bundles.Add(new ScriptBundle("~/bundles/menutheme").Include(
                $"{jsDir}metisMenu.min.js",
                $"{jsDir}startmin.js"
                ));

            bundles.Add(new StyleBundle("~/bundles/styles").Include(
                "~/Content/bootstrap.min.css",
                "~/Content/metisMenu.min.css",
                "~/Content/timeline.min.css",
                "~/Content/startmin.min.css",
                "~/Content/morris.min.css",
                "~/Content/font-awesome.min.css"
                ));

        }
    }

    class AsIsBundleOrderer : IBundleOrderer
    {
        public IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> files)
        {
            return files;
        }
    }
}
 