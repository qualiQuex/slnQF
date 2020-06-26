using System.Web.Optimization;

namespace slnQF
{
    public class BundleConfig
    {
        // Para obtener más información sobre Bundles, visite http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.UseCdn = true;
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js", 

                "~/Scripts/jquery-2.2.3.min.js"
                
                   
                    ,
                            "~/Scripts/jquery-ui-1114.js",
                                "~/Scripts/jquery-ui.min1.12.js"/*,
                                    "~/Scripts/jquer191y.min.js"*/
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));


            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
               //         "~/Scripts/bootstrap.js",
               "~/Content/AdminLTE/bootstrap/js/bootstrap.min.js",
                      "~/Scripts/respond.js"));


            bundles.Add(new ScriptBundle("~/bundles/AdminLTE", "http://cdnjs.cloudflare.com/ajax/libs/raphael/2.1.0/raphael-min.js").Include(
                
"~/Content/AdminLTE/plugins/morris/morris.min.js",
"~/Content/AdminLTE/plugins/sparkline/jquery.sparkline.min.js",
"~/Content/AdminLTE/plugins/jvectormap/jquery-jvectormap-1.2.2.min.js",
"~/Content/AdminLTE/plugins/jvectormap/jquery-jvectormap-world-mill-en.js",
"~/Content/AdminLTE/plugins/jqueryKnob/jquery.knob.js",
"~/Content/AdminLTE/plugins/bootstrap-wysihtml5/bootstrap3-wysihtml5.all.min.js",
"~/Content/AdminLTE/plugins/iCheck/icheck.min.js",
"~/Content/AdminLTE/plugins/datepicker/bootstrap-datepicker.js",
"~/Content/AdminLTE/plugins/AdminLTE/app.min.js",
"~/Content/AdminLTE/plugins/datatables/jquery.dataTables.min.js", 
"~/Content/AdminLTE/plugins/dateOrder.js",
"~/Content/AdminLTE/charts/js/jquery.mockjax.min.js",
"~/Content/AdminLTE/charts/js/jquery.orgchart.js",
"~/Scripts/jquery.unobtrusive-ajax.min.js",
"~/Content/SweetAlert/sweetalert-dev.js",
"~/Content/Switch/bootstrap-switch.min.js",
"~/Content/PrintJS/print.min.js",
"~/Content/spin.min.js"
));

            bundles.Add(new StyleBundle("~/Content/css").Include(
           "~/Content/AdminLTE/bootstrap/css/bootstrap.min.css",  
"~/Content/AdminLTE/plugins/morris/morris.css",
"~/Content/AdminLTE/plugins/jvectormap/jquery-jvectormap-1.2.2.css",
"~/Content/AdminLTE/plugins/bootstrap-wysihtml5/bootstrap3-wysihtml5.min.css",
"~/Content/AdminLTE/plugins/AdminLTE/skins/_all-skins.css",
"~/Content/AdminLTE/plugins/estilos/font-awesome.min.css",
"~/Content/AdminLTE/plugins/estilos/ionicons.min.css",
"~/Content/AdminLTE/plugins/datepicker/datepicker3.css",
"~/Content/AdminLTE/plugins/AdminLTE/AdminLTE.min.css",
"~/Content/AdminLTE/plugins/datatables/dataTables.bootstrap.css",
"~/Content/AdminLTE/plugins/datatables/jquery.dataTables.min.css",
"~/Content/AdminLTE/charts/css/jquery.orgchart.css",
"~/Content/AdminLTE/charts/css/style_OrgChart.css",
"~/Content/AdminLTE/charts/css/style_ul.css",
"~/Content/Web/jquery-ui-smoothness.css",
"~/Content/SweetAlert/sweetalert.css",
"~/Content/Switch/bootstrap-switch.min.css",
"~/Content/style-font.css",
"~/Content/PrintJS/print.min.css"
));

            BundleTable.EnableOptimizations = false;







        }
    }
}
