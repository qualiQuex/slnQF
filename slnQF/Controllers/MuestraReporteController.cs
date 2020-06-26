using CrystalDecisions.CrystalReports.Engine;
using slnQF.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace slnQF.Controllers
{
    public class MuestraReporteController : Controller
    {
        // GET: MuestraReporte
        public ActionResult Index()
        {
            
            return View("~/Views/MuestraReporte/Reporte.cshtml");
        }
        
       public FileResult DownloadReport()
        {


            string nombre = "";
            DATACONTEXT_APPLYDataContext db = new DATACONTEXT_APPLYDataContext(); 
            IList<RPT_FacturacionMensualResult> list = db.RPT_FacturacionMensual().ToList<RPT_FacturacionMensualResult>();
            SP_REPORTE_FACTURAMENSUAL LISTA = new SP_REPORTE_FACTURAMENSUAL();
            List<SP_REPORTE_FACTURAMENSUAL> lista2 = new List<SP_REPORTE_FACTURAMENSUAL>();
            foreach (RPT_FacturacionMensualResult tmp in list) {

                LISTA.CardCode = tmp.CardCode;
                LISTA.CardName = tmp.CardName;
               
                try
                {
                    LISTA.DiscPrcnt = (decimal)tmp.DiscPrcnt;

                }
                catch
                {
                    LISTA.DiscPrcnt = 0;
                }
                try
                {
                    LISTA.DocDate = (DateTime)tmp.DocDate;
                     
                }
                catch
                {
                    LISTA.DocDate = Convert.ToDateTime("01/01/1990");
                }
                LISTA.DocNum = tmp.DocNum;
                LISTA.GroupName = tmp.GroupName; 
                try
                {
                    LISTA.GTotal = (decimal)tmp.GTotal;

                }
                catch
                {
                    LISTA.GTotal = 0;
                }
                
                try
                {
                    LISTA.GTotalSC = (decimal)tmp.GTotalSC;

                }
                catch
                {
                    LISTA.GTotalSC = 0;
                }
                LISTA.ItemCode = tmp.ItemCode;
                LISTA.ItemName = tmp.ItemName; 

                try
                {
                    LISTA.LineTotal = (decimal)tmp.LineTotal;

                }
                catch
                {
                    LISTA.LineTotal = 0;
                }
                LISTA.Name = tmp.Name;
                

                try
                {
                    LISTA.PriceAfVAT = (decimal)tmp.PriceAfVAT;

                }
                catch
                {
                    LISTA.PriceAfVAT = 0;
                }
                
                try
                {
                    LISTA.Quantity = (decimal)tmp.Quantity;

                }
                catch
                {
                    LISTA.Quantity = 0;
                }
                LISTA.SlpName = tmp.SlpName;
                lista2.Add(LISTA);
            }
            if (lista2.Count() > 0)
            {

                ReportDocument rd = new ReportDocument();
                rd.Load(Path.Combine(Server.MapPath("~/Reportes"), "ReporteFacturacionPrivado.rpt"));

                rd.SetDataSource(lista2);

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "REPORTE_FACTURA_MENSUAL" + ".pdf");

            }
            else
            {
                ReportDocument rd = new ReportDocument();
                rd.Load(Path.Combine(Server.MapPath("~/Reportes"), "Reporte_Certificado_1.rpt"));
                RPT_FacturacionMensualResult itemN = new RPT_FacturacionMensualResult();
              

                itemN.CardCode = "";
                itemN.CardName = "";
                itemN.DiscPrcnt = 0;
                itemN.DocDate = DateTime.Today;
                itemN.DocNum = 0;
                itemN.GroupName = "";
                itemN.GTotal = 0;
                itemN.GTotalSC = 0;
                itemN.ItemCode = "";
                itemN.ItemName = "";
                itemN.LineTotal = 0;
                itemN.Name = "";
                itemN.PriceAfVAT = 0;
                itemN.Quantity = 0;
                itemN.SlpName = "";
                


              
                rd.SetDataSource(itemN);
                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();


                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", nombre + ".pdf");

            }

        }

        public String DownloadReport23()
        {


            string nombre = "";
            DATACONTEXT_APPLYDataContext db = new DATACONTEXT_APPLYDataContext();
            IList<RPT_FacturacionMensualResult> list = db.RPT_FacturacionMensual().ToList<RPT_FacturacionMensualResult>();
            SP_REPORTE_FACTURAMENSUAL LISTA = new SP_REPORTE_FACTURAMENSUAL();
            List<SP_REPORTE_FACTURAMENSUAL> lista2 = new List<SP_REPORTE_FACTURAMENSUAL>();
            foreach (RPT_FacturacionMensualResult tmp in list)
            {

                LISTA.CardCode = tmp.CardCode;
                LISTA.CardName = tmp.CardName;

                try
                {
                    LISTA.DiscPrcnt = (decimal)tmp.DiscPrcnt;

                }
                catch
                {
                    LISTA.DiscPrcnt = (decimal)0;
                }
                try
                {
                    LISTA.DocDate = (DateTime)tmp.DocDate;

                }
                catch
                {
                    LISTA.DocDate = Convert.ToDateTime("01/01/1990");
                }
                LISTA.DocNum = tmp.DocNum;
                LISTA.GroupName = tmp.GroupName;
                try
                {
                    LISTA.GTotal = (decimal)tmp.GTotal;

                }
                catch
                {
                    LISTA.GTotal = (decimal)0;
                }

                try
                {
                    LISTA.GTotalSC = (decimal)tmp.GTotalSC;

                }
                catch
                {
                    LISTA.GTotalSC = (decimal)0;
                }
                LISTA.ItemCode = tmp.ItemCode;
                LISTA.ItemName = tmp.ItemName;

                try
                {
                    LISTA.LineTotal = (decimal)tmp.LineTotal;

                }
                catch
                {
                    LISTA.LineTotal = (decimal)0;
                }
                LISTA.Name = tmp.Name;


                try
                {
                    LISTA.PriceAfVAT = (decimal)tmp.PriceAfVAT;

                }
                catch
                {
                    LISTA.PriceAfVAT = (decimal)0;
                }

                try
                {
                    LISTA.Quantity = (decimal)tmp.Quantity;

                }
                catch
                {
                    LISTA.Quantity = (decimal)0;
                }
                LISTA.SlpName = tmp.SlpName;


                lista2.Add(LISTA);
            }
            if (lista2.Count() > 0)
            {
                 
                ReportDocument rd = new ReportDocument();
                rd.Load(Path.Combine(Server.MapPath("~/Reportes"), "ReporteFacturacionPrivado.rpt"));
                DataSet DS = ListConverter.ToDataSet<SP_REPORTE_FACTURAMENSUAL>(lista2);
                rd.SetDataSource(lista2);

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                String file = Convert.ToBase64String(ReadFully(stream));
                return file;
            }
            else
            {
                ReportDocument rd = new ReportDocument();
                rd.Load(Path.Combine(Server.MapPath("~/Reportes"), "Reporte_Certificado_1.rpt"));
                RPT_FacturacionMensualResult itemN = new RPT_FacturacionMensualResult();


                itemN.CardCode = "";
                itemN.CardName = "";
                itemN.DiscPrcnt = 0;
                itemN.DocDate = DateTime.Today;
                itemN.DocNum = 0;
                itemN.GroupName = "";
                itemN.GTotal = 0;
                itemN.GTotalSC = 0;
                itemN.ItemCode = "";
                itemN.ItemName = "";
                itemN.LineTotal = 0;
                itemN.Name = "";
                itemN.PriceAfVAT = 0;
                itemN.Quantity = 0;
                itemN.SlpName = "";




                rd.SetDataSource(itemN);
                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();


                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                String file = Convert.ToBase64String(ReadFully(stream));
                return file;

            }

        }

        public String DownloadReport2()
        {

         
            SqlConnection sqlConnection1 = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ApplyConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            

            cmd.CommandText = "RPT_FacturacionMensual";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = sqlConnection1;

            sqlConnection1.Open();

 
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            // Data is accessible through the DataReader object here.
            
            sqlConnection1.Close();
            
            if (ds.Tables[0].Rows.Count > 0)
            {

                ReportDocument rd = new ReportDocument();
                rd.Load(Path.Combine(Server.MapPath("~/Reportes"), "ReporteFacturacionPrivado.rpt"));
                
                rd.SetDataSource(ds.Tables[0]);

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                String file = Convert.ToBase64String(ReadFully(stream));
                return file;
            }
            else
            {
                ReportDocument rd = new ReportDocument();
                rd.Load(Path.Combine(Server.MapPath("~/Reportes"), "Reporte_Certificado_1.rpt"));
                RPT_FacturacionMensualResult itemN = new RPT_FacturacionMensualResult();


                itemN.CardCode = "";
                itemN.CardName = "";
                itemN.DiscPrcnt = 0;
                itemN.DocDate = DateTime.Today;
                itemN.DocNum = 0;
                itemN.GroupName = "";
                itemN.GTotal = 0;
                itemN.GTotalSC = 0;
                itemN.ItemCode = "";
                itemN.ItemName = "";
                itemN.LineTotal = 0;
                itemN.Name = "";
                itemN.PriceAfVAT = 0;
                itemN.Quantity = 0;
                itemN.SlpName = "";




                rd.SetDataSource(itemN);
                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();


                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                String file = Convert.ToBase64String(ReadFully(stream));
                return file;

            }

        }

        public FileResult DownloadReportExcel()
        {


            SqlConnection sqlConnection1 = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ApplyConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();


            cmd.CommandText = "RPT_FacturacionMensual";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = sqlConnection1;

            sqlConnection1.Open();


            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            // Data is accessible through the DataReader object here.

            sqlConnection1.Close();

            if (ds.Tables[0].Rows.Count > 0)
            {

                ReportDocument rd = new ReportDocument();
                rd.Load(Path.Combine(Server.MapPath("~/Reportes"), "ReporteFacturacionPrivado.rpt"));

                rd.SetDataSource(ds.Tables[0]);

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.Excel);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "REPORTE_FACTURA_MENSUAL" + ".xls");
            }
            else
            {
                ReportDocument rd = new ReportDocument();
                rd.Load(Path.Combine(Server.MapPath("~/Reportes"), "Reporte_Certificado_1.rpt"));
                RPT_FacturacionMensualResult itemN = new RPT_FacturacionMensualResult();


                itemN.CardCode = "";
                itemN.CardName = "";
                itemN.DiscPrcnt = 0;
                itemN.DocDate = DateTime.Today;
                itemN.DocNum = 0;
                itemN.GroupName = "";
                itemN.GTotal = 0;
                itemN.GTotalSC = 0;
                itemN.ItemCode = "";
                itemN.ItemName = "";
                itemN.LineTotal = 0;
                itemN.Name = "";
                itemN.PriceAfVAT = 0;
                itemN.Quantity = 0;
                itemN.SlpName = "";




                rd.SetDataSource(itemN);
                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();


                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.Excel);
                stream.Seek(0, SeekOrigin.Begin);

                return File(stream, "application/pdf", "REPORTE_FACTURA_MENSUAL" + ".xls");

            }

        }

        public static byte[] ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }
        public static void AddProperty(ExpandoObject expando, string propertyName, object propertyValue)
        {
            // ExpandoObject supports IDictionary so we can extend it like this
            var expandoDict = expando as IDictionary<string, object>;
            if (expandoDict.ContainsKey(propertyName))
                expandoDict[propertyName] = propertyValue;
            else
                expandoDict.Add(propertyName, propertyValue);
        }
    }
}