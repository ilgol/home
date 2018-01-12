using System.Web;
using System.Web.Optimization;

namespace Diploma
{
    public class BundleConfig
    {
        // Дополнительные сведения об объединении см. на странице https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-3.2.1.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                      "~/Scripts/main.js",
                      "~/Scripts/moment.js",
                      "~/Scripts/moment-with-locales.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/main.css",
                      "~/admin-lte/css/AdminLTE.min.css"));
        }
    }
}
