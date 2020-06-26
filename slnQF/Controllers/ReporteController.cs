using Microsoft.Reporting.WebForms;
using slnQF.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace slnQF.Controllers
{
    public class ReporteController : Controller
    {
        clsDbReporte cDB = new clsDbReporte();
        // GET: Reporte
        [AllowAnonymous]
        public ActionResult ReportViewer()
        {

            SearchParameterModel um = new SearchParameterModel();

            return View(um);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ViewResult ReportViewer(SearchParameterModel um)
        {
            return View(um);
        }

        public IList<clsFilas> traeLista()
        {
            IList<clsFilas> Lista = new List<clsFilas>();
            Lista.Add(new clsFilas("CENTRAL", "1 AÑO", "85", "TIPO DE PAGO"));
            Lista.Add(new clsFilas("CENTRAL", "2 AÑOS", "34", "TIPO DE PAGO"));
            Lista.Add(new clsFilas("CENTRAL", "3 AÑOS", "10", "TIPO DE PAGO"));
            Lista.Add(new clsFilas("CENTRAL", "4 AÑOS", "11", "TIPO DE PAGO"));
            Lista.Add(new clsFilas("CENTRAL", "5 AÑOS", "8", "TIPO DE PAGO"));

            Lista.Add(new clsFilas("SAN RAFAEL", "1 AÑO", "5", "TIPO DE PAGO"));
            Lista.Add(new clsFilas("SAN RAFAEL", "2 AÑOS", "7", "TIPO DE PAGO"));
            Lista.Add(new clsFilas("SAN RAFAEL", "3 AÑOS", "16", "TIPO DE PAGO"));
            Lista.Add(new clsFilas("SAN RAFAEL", "4 AÑOS", "4", "TIPO DE PAGO"));
            Lista.Add(new clsFilas("SAN RAFAEL", "5 AÑOS", "1", "TIPO DE PAGO"));

            Lista.Add(new clsFilas("CENTRAL", "A", "37", "TIPO"));
            Lista.Add(new clsFilas("CENTRAL", "B", "51", "TIPO"));
            Lista.Add(new clsFilas("CENTRAL", "C", "50", "TIPO"));
            Lista.Add(new clsFilas("CENTRAL", "M", "33", "TIPO"));
            Lista.Add(new clsFilas("CENTRAL", "E", "5", "TIPO"));

            Lista.Add(new clsFilas("SAN RAFAEL", "A", "10", "TIPO"));
            Lista.Add(new clsFilas("SAN RAFAEL", "B", "15", "TIPO"));
            Lista.Add(new clsFilas("SAN RAFAEL", "C", "12", "TIPO"));
            Lista.Add(new clsFilas("SAN RAFAEL", "M", "9", "TIPO"));
            Lista.Add(new clsFilas("SAN RAFAEL", "E", "0", "TIPO"));

            Lista.Add(new clsFilas("CENTRAL", "PRIMERA", "24", "TRAMITE"));
            Lista.Add(new clsFilas("CENTRAL", "NUEVA", "2", "TRAMITE"));
            Lista.Add(new clsFilas("CENTRAL", "RENOVACION", "122", "TRAMITE"));
            Lista.Add(new clsFilas("CENTRAL", "REPOSICION", "24", "TRAMITE"));
            Lista.Add(new clsFilas("CENTRAL", "TRANSFERENCIA", "4", "TRAMITE"));

            Lista.Add(new clsFilas("SAN RAFAEL", "PRIMERA", "2", "TRAMITE"));
            Lista.Add(new clsFilas("SAN RAFAEL", "NUEVA", "0", "TRAMITE"));
            Lista.Add(new clsFilas("SAN RAFAEL", "RENOVACION", "31", "TRAMITE"));
            Lista.Add(new clsFilas("SAN RAFAEL", "REPOSICION", "13", "TRAMITE"));
            Lista.Add(new clsFilas("SAN RAFAEL", "TRANSFERENCIA", "0", "TRAMITE"));


            return Lista;
        }

        public IList<ClsEtiquetaPesado> traeEtiquetas(string p_docnum) {
            IList<ClsEtiquetaPesado> ListaEtiquetas = new List<ClsEtiquetaPesado>();
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds = cDB.TRAE_ETIQUETAS(p_docnum);
                dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    //dr["ID_USUARIO"].ToString() 
                    //row[0].ToString()) 
                    ClsEtiquetaPesado fila = new ClsEtiquetaPesado(dr["U_NombrePO"].ToString(), dr["U_LOTE"].ToString(), dr["DocNum"].ToString(), dr["ItemCode"].ToString(), dr["bulto"].ToString(), dr["bultofin"].ToString(), dr["ItemName"].ToString(), dr["ExpDate"].ToString(), dr["U_LOTE"].ToString(), dr["Notes"].ToString(), dr["POTENCIA"].ToString(), dr["Quantity"].ToString(), dr["TARA"].ToString(), DateTime.Now.ToString("dd/MM/yyyy"), dr["PesoNeto"].ToString(), dr["Dispensado"].ToString(), dr["Verificado"].ToString(), dr["UOM"].ToString(), dr["BaseRef"].ToString());

                    ListaEtiquetas.Add(fila);
                }
            }
            catch (Exception e)
            {
                System.Console.Write(e.Message);
            }
            return ListaEtiquetas;
        }

        public FileContentResult GenerateAndDisplayReport(string docnum, string format)
        {
            
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath("~/Reportes/rptAnexoA.rdlc");
            IList<ClsPrueba> ListatPAGO = new List<ClsPrueba>();
            ListatPAGO.Add(new ClsPrueba("1", "1"));
            ListatPAGO.Add(new ClsPrueba("1", "2"));
            ListatPAGO.Add(new ClsPrueba("1", "3"));
            ListatPAGO.Add(new ClsPrueba("1", "4"));
            ListatPAGO.Add(new ClsPrueba("1", "5"));

   
            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "DataSet1";

            reportDataSource.Value = ListatPAGO;

            localReport.DataSources.Add(reportDataSource);

            string renderFormat = "";
            if (format == "PDF")
            {
                renderFormat = "pdf";
            }
            else
            {
                renderFormat = "Image";
            }
            string reportType = "Image";
            string mimeType;
            string encoding;
            string fileNameExtension;
            //The DeviceInfo settings should be changed based on the reportType            
            //http://msdn2.microsoft.com/en-us/library/ms155397.aspx            
            string deviceInfo = "<DeviceInfo>" +
                "  <OutputFormat>pdf</OutputFormat>" +
                "  <PageWidth>10cm</PageWidth>" +
                "  <PageHeight>10cm</PageHeight>" +
                "  <MarginTop>0in</MarginTop>" +
                "  <MarginLeft>0in</MarginLeft>" +
                "  <MarginRight>0in</MarginRight>" +
                "  <MarginBottom>0in</MarginBottom>" +
                "</DeviceInfo>";
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;
            //Render the report      
           
             renderedBytes = localReport.Render(renderFormat, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            
            //Response.AddHeader("content-disposition", "attachment; filename=NorthWindCustomers." + fileNameExtension); 
            if (format == null)
            {
                return File(renderedBytes, "image/jpeg");
            }
            else if (format == "PDF")
            {
                return File(renderedBytes, "pdf");
            }
            else
            {
                return File(renderedBytes, "image/jpeg");
            }
            
            
        }

        public ActionResult DownloadReport(string docnum, string format)
        {
            traeEtiquetas(docnum);
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath("~/Reportes/rptAnexoA.rdlc");
            IList<ClsEtiquetaPesado> list = new List<ClsEtiquetaPesado>();
            list = traeEtiquetas(docnum);
             
            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "DataSet1";

            reportDataSource.Value = list;
            
            localReport.DataSources.Add(reportDataSource);




            string filenameToSave = "test.xlsx";
            string renderFormat = "";
            if (format == "XLS")
            {
                renderFormat = (filenameToSave.EndsWith(".xlsx") ? "EXCELOPENXML" : "Excel");
            }
            else if (format == "PDF")
            {
                renderFormat = "pdf";
            }
            else
            {
                renderFormat = "Image";
            }

            string mimeType;
            string encoding;
            string fileNameExtension;
            //The DeviceInfo settings should be changed based on the reportType            
            //http://msdn2.microsoft.com/en-us/library/ms155397.aspx            
            string deviceinfo = "<DeviceInfo>" +
                           "  <OutputFormat>jpeg</OutputFormat>" +
                           "  <PageWidth>10cm</PageWidth>" +
                           "  <PageHeight>10cm</PageHeight>" +
                           "  <MarginTop>0in</MarginTop>" +
                           "  <MarginLeft>0in</MarginLeft>" +
                           "  <MarginRight>0in</MarginRight>" +
                           "  <MarginBottom>0in</MarginBottom>" +
                           "</DeviceInfo>";
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;
            //Render the report            
            renderedBytes = localReport.Render(renderFormat, deviceinfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            //Response.AddHeader("content-disposition", "attachment; filename=NorthWindCustomers." + fileNameExtension); 
            if (format == null)
            {
                return File(renderedBytes, "image/jpeg");
            }
            else if (format == "PDF")
            {
                return File(renderedBytes, mimeType);
            }
            else if (format == "XLS")
            {
                return File(renderedBytes, mimeType);
            }
            else
            {
                return File(renderedBytes, "image/jpeg");
            }
        }

    }
}