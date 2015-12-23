using System.Web;
using System.Web.Optimization;

namespace guahao
{
    public class BundleConfig
    {
        // 有关绑定的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            // 生产准备时，请使用 http://modernizr.com 上的生成工具来仅选择所需的测试。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/slides").Include(
                      "~/Scripts/slides.min.jquery.js",
                      "~/Scripts/slider.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquerypaging").Include(
          "~/Scripts/jquery.paging.js"));


            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css"
                      ,"~/Content/site.css"
                      , "~/Content/portal.base.min.css"
                      , "~/Content/portal.basic.min.css"
                      , "~/Content/portal.hdd.min.css"
                      , "~/Content/portal.content.min.css"
                      , "~/Content/style.min.css"
                      , "~/Content/animate.min.css"
                      //,"~/Content/slider.css"
                      ,"~/Content/stylesheet.css"
                      , "~/Content/jquery.paging.css"
                      //,"~/Content/datepicker.css"
                      ));
        }
    }
}
