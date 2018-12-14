using System.Web;
using System.Web.Optimization;

namespace WebApplication6
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.UseCdn = true;

            bundles.Add(new ScriptBundle("~/bundles/scripts").Include(
                       "~/Scripts/*.min.js",
                       "~/Scripts/*.js"
                        ));

            bundles.Add(new ScriptBundle("~/bubles/bootstrapJS").
                IncludeDirectory("~/Scripts/bootstrapJS", "*.js"));

            bundles.Add(new StyleBundle("~/bubles/bootstrap").
                IncludeDirectory("~/Content/bootstrap", "*.css"));
    

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/*.css"
                      ));
        }
    }
}
