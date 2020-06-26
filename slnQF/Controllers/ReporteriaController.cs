using CrystalDecisions.CrystalReports.Engine;
using Newtonsoft.Json;
using slnQF.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace slnQF.Controllers
{
    public class ReporteriaController : Controller
    {
        // GET: Reporteria
        public ActionResult Index()
        {
            ModelState.Clear();
            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            List<SP_CONSULTA_REPORTEResult> listreporte = db.SP_CONSULTA_REPORTE()
               .OrderBy(x => x.DESCRIPCION)
               .ToList<SP_CONSULTA_REPORTEResult>();
            return View(listreporte); 
        }

        public ActionResult Reportes()
        {
            ModelState.Clear();
            string idUsuarioLog = "-1";
            int idUsuarioINT =-1;
            try
            {
                idUsuarioLog = Session["LogedUserID"].ToString();
                idUsuarioINT = Convert.ToInt32(idUsuarioLog);
            }
            catch (Exception Ex)
            {
                idUsuarioINT = 0;
            }
            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            ViewBag.ReporteDDL = new SelectList(db.SP_CONSULTA_REPORTE_USUARIO().Where(x => x.ID_USUARIO == idUsuarioINT ).OrderBy(x => x.DESCRIPCION), "ID_REPORTE", "DESCRIPCION");

            return View(new slnQF.TREPORTE());
        }

        public JsonResult Create2(slnQF.TREPORTE product)
        {
            string error = "";
            string errorStr = "";
            string idUsuarioLog = "-1";
            try
            {
                idUsuarioLog = Session["LogedUserID"].ToString();

            }
            catch (Exception Ex)
            {
                error = "1";
                errorStr = "Su sesion ha caducado, ingrese nuevamente";
            }
            if (Convert.ToInt32(idUsuarioLog) > 0)
            {
                clsOperaciones ope = new clsOperaciones();

                if (ope.verificaConexionDB(product.BDSERVER, product.BDNAME, product.BDUSER, product.BDPASS, out errorStr) == 1)
                {


                    DC_CALIDADDataContext db = new DC_CALIDADDataContext();
                    product.FECHA_CREACION = DateTime.Now;
                    product.ESTADO = 1;
                    product.ID_PERMISO = 0;
                    product.ID_RESPONSABLE = Convert.ToInt32(idUsuarioLog);
                    CAT_PERMISO perm = new CAT_PERMISO();

                    try
                    {
                        db = new DC_CALIDADDataContext();
                        db.Connection.Open();
                        db.Transaction = db.Connection.BeginTransaction();
                       
                        perm.DESCRIPCION = product.DESCRIPCION;
                        perm.FECHA_CREACION = DateTime.Now;
                        perm.ESTADO = 1;
                        perm.ID_RESPONSABLE = Convert.ToInt32(idUsuarioLog);
                        db.CAT_PERMISOs.InsertOnSubmit(perm);
                        db.SubmitChanges();
                        db.Transaction.Commit();
                        error = "0";
                        errorStr = "Informacion guardada con exito";
                        db.Connection.Close();
                        db.Dispose();
                        try
                        {
                            TACCESO_PERMISO ACCperm = new TACCESO_PERMISO();
                            db = new DC_CALIDADDataContext();
                            db.Connection.Open();
                            db.Transaction = db.Connection.BeginTransaction();

                            ACCperm.IDPERMISO = perm.IDPERMISO;
                            perm.FECHA_CREACION = DateTime.Now;
                            ACCperm.IDACCESO = 2;
                            perm.ID_RESPONSABLE = Convert.ToInt32(idUsuarioLog);
                            db.TACCESO_PERMISOs.InsertOnSubmit(ACCperm);
                            db.SubmitChanges();
                            db.Transaction.Commit();
                            error = "0";
                            errorStr = "Informacion guardada con exito";
                            db.Connection.Close();
                            db.Dispose();
                        }
                        catch
                        {
                            error = "0";
                            errorStr = "Se grabo el reporte pero hubo problema con la creacion del permiso";
                            db.Connection.Close();
                            db.Dispose();
                        }
                    }
                    catch
                    {
                        error = "0";
                        errorStr = "Se grabo el reporte pero hubo problema con la creacion del permiso";
                        db.Connection.Close();
                        db.Dispose();
                    }
                    try
                    {
                        product.ID_PERMISO = perm.IDPERMISO;
                        db = new DC_CALIDADDataContext();
                        db.Connection.Open();
                        db.Transaction = db.Connection.BeginTransaction();
                        db.TREPORTEs.InsertOnSubmit(product);
                        db.SubmitChanges();
                        db.Transaction.Commit();
                        error = "0";
                        errorStr = "Informacion guardada con exito";
                        db.Connection.Close();
                        db.Dispose();
                    }
                    catch (Exception e)
                    {
                        db.Transaction.Rollback();
                        error = "1";
                        errorStr = "Error en el guardado de informacion, " + e.Message;
                        db.Connection.Close();
                        db.Dispose();
                    }
                    

                }
                else {
                    error = "1";
                }

            }
            else
            {
                error = "1";
                errorStr = "Su sesion ha caducado, ingrese nuevamente";
            }
            return Json(new { ID = error, MENSAJE = errorStr });
        }

        
        public JsonResult updateJS(slnQF.TREPORTE product)
        {
            string error = "";
            string errorStr = "";
            string idUsuarioLog = "-1";
            try
            {
                idUsuarioLog = Session["LogedUserID"].ToString();

            }
            catch (Exception Ex)
            {
                error = "1";
                errorStr = "Su sesion ha caducado, ingrese nuevamente";
            }
            if (Convert.ToInt32(idUsuarioLog) > 0)
            {
                clsOperaciones ope = new clsOperaciones();

                if (ope.verificaConexionDB(product.BDSERVER, product.BDNAME, product.BDUSER, product.BDPASS, out errorStr) == 1)
                {

                    
                    DC_CALIDADDataContext db = new DC_CALIDADDataContext();
                    
                    try
                    {
                        db.Connection.Open();
                        db.Transaction = db.Connection.BeginTransaction();
                        TREPORTE tmp = db.TREPORTEs.Where(x => x.ID_REPORTE == product.ID_REPORTE).FirstOrDefault();

                        tmp.ESTADO = product.ESTADO;
                        tmp.BDSERVER = product.BDSERVER;
                        tmp.BDNAME = product.BDNAME;
                        tmp.BDUSER = product.BDUSER;
                        tmp.BDPASS = product.BDPASS;
                        tmp.BDSP = product.BDSP;
                        tmp.DESCRIPCION = product.DESCRIPCION;
                        db.SubmitChanges();
                        db.Transaction.Commit();
                        error = "0";
                        errorStr = "Informacion guardada con exito";
                        db.Connection.Close();
                        db.Dispose();
                    }
                    catch (Exception e)
                    {
                        db.Transaction.Rollback();
                        error = "1";
                        errorStr = "Error en el guardado de informacion, " + e.Message;
                        db.Connection.Close();
                        db.Dispose();
                    }
                }
                else
                {
                    error = "1";
                }

            }
            else
            {
                error = "1";
                errorStr = "Su sesion ha caducado, ingrese nuevamente";
            }
            return Json(new { ID = error, MENSAJE = errorStr });
        }

        public JsonResult creaParamJS(slnQF.TREPORTE_PARAMETRO product)
        {
            string error = "";
            string errorStr = "";
            string idUsuarioLog = "-1";
            try
            {
                idUsuarioLog = Session["LogedUserID"].ToString();

            }
            catch (Exception Ex)
            {
                error = "1";
                errorStr = "Su sesion ha caducado, ingrese nuevamente";
            }
            if (Convert.ToInt32(idUsuarioLog) > 0)
            {
              

                    DC_CALIDADDataContext db = new DC_CALIDADDataContext();
                    product.FECHA_CREACION = DateTime.Now;
                    product.ESTADO = 1; 
                    product.ID_RESPONSABLE = Convert.ToInt32(idUsuarioLog);
                    try
                    {
                        db.Connection.Open();
                        db.Transaction = db.Connection.BeginTransaction();
                        db.TREPORTE_PARAMETROs.InsertOnSubmit(product);
                        db.SubmitChanges();
                        db.Transaction.Commit();
                        error = "0";
                        errorStr = "Informacion guardada con exito";
                    db.Connection.Close();
                    db.Dispose();
                }
                    catch (Exception e)
                    {
                        db.Transaction.Rollback();
                        error = "1";
                        errorStr = "Error en el guardado de informacion, " + e.Message;
                    db.Connection.Close();
                    db.Dispose();
                }
               

            }
            else
            {
                error = "1";
                errorStr = "Su sesion ha caducado, ingrese nuevamente";
            }
            return Json(new { ID = error, MENSAJE = errorStr });
        }

        public PartialViewResult Create()
        {

            return PartialView(new slnQF.TREPORTE());
        }

        public PartialViewResult CreateParametro(string ID)
        {
            TREPORTE_PARAMETRO TMP = new slnQF.TREPORTE_PARAMETRO();
            TMP.ID_REPORTE = Convert.ToInt32(ID);
            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            

            ViewBag.CategoriesDDL = new SelectList(db.CAT_TIPO_DATOs, "ID_TIPO_DATO", "DESCRIPCION");
            return PartialView(TMP);
        }

        public PartialViewResult loadImage(string id)
        {
            
            TREPORTE tmp = new TREPORTE();
            tmp.ID_REPORTE = Convert.ToInt32(id);
            return PartialView(tmp);
        }

        public PartialViewResult Update(string id)
        {
            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            TREPORTE product = db.TREPORTEs.Where(x => x.ID_REPORTE == Convert.ToInt32(id)).FirstOrDefault();

            ViewBag.CategoriesDDL = new SelectList(db.CAT_ESTADOs, "ESTADO_ID", "DESCRIPCION", product.ESTADO);

            return PartialView(product);
        }

        public PartialViewResult ListadoParametro(int id)
        {
            ModelState.Clear();
            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            List<SP_CONSULTA_REPORTE_PARAMETROResult> PRUEBA = db.SP_CONSULTA_REPORTE_PARAMETRO()
               .OrderBy(x => x.TIPODATO).Where(x => x.ID_REPORTE == id)
               .ToList<SP_CONSULTA_REPORTE_PARAMETROResult>();
            return PartialView(PRUEBA);
        }

        public PartialViewResult ListadoParametroReporte(int id)
        {
            ModelState.Clear();
            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            List<SP_CONSULTA_REPORTE_PARAMETROResult> PRUEBA = db.SP_CONSULTA_REPORTE_PARAMETRO()
               .OrderBy(x => x.TIPODATO).Where(x => x.ID_REPORTE == id && x.ESTADO ==1)
               .ToList<SP_CONSULTA_REPORTE_PARAMETROResult>();
            return PartialView(PRUEBA);
        }
        [HttpPost]
        public ActionResult Upload()
        {
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Reportes/appReport"), fileName);
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                    file.SaveAs(path);
                }
            }

            //return RedirectToAction("Index");
            ViewBag.Message = "Please select file";
            return View();
        }

        [HttpPost]
        public JsonResult UploadFile2(string id)
        {
            string error = "";
            string errorStr = "";
            string idUsuarioLog = "-1";
            try
            {
                idUsuarioLog = Session["LogedUserID"].ToString();

            }
            catch (Exception Ex)
            {
                errorStr = "Su sesion ha caducado, ingrese nuevamente";
                return Json(new { ID = 0, MENSAJE = errorStr });
            }
            int resultado = 0;
            
            string _imgname = string.Empty;
            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var file = System.Web.HttpContext.Current.Request.Files["UploadedImage"];
                
                if (file.ContentLength > 0)
                {
                    MemoryStream ms = new MemoryStream();
                    file.InputStream.CopyTo(ms);
                    byte[] data = ms.ToArray();
                    TREPORTE_BY product = new TREPORTE_BY();
                    DC_CALIDADDataContext db = new DC_CALIDADDataContext();
                    product.FECHA_CREACION = DateTime.Now;
                    product.ESTADO = 1; 
                    product.ID_RESPONSABLE = Convert.ToInt32(idUsuarioLog);
                    product.BIN_REP = data;
                    product.ID_REPORTE = Convert.ToInt32(id);
                     try
                    {
                        int x = fnCambiaEstadoRepBy(Convert.ToInt32(id));
                        db.Connection.Open();
                        db.Transaction = db.Connection.BeginTransaction();
                        db.TREPORTE_Bies.InsertOnSubmit(product);
                        db.SubmitChanges();
                        db.Transaction.Commit();
                        resultado = 1;
                        errorStr = "Informacion guardada con exito";
                        /*
                        var path = Path.Combine(Server.MapPath("~/Reportes/appReport"), "Report.rpt");
                        try
                        {
                            FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite);
                            fileStream.Write(data, 0, data.Length);
                            fileStream.Close();
                             
                        }
                        catch (Exception ex)
                        {
                             
                        }
                        */
                        //int archivoDesc = SaveBytesToFile(data, path);
                        db.Connection.Close();
                        db.Dispose();
                    }
                    catch (Exception e)
                    {
                        db.Transaction.Rollback();
                        resultado = 0;
                        errorStr = "Error en el guardado de informacion, " + e.Message;
                        db.Connection.Close();
                        db.Dispose();
                    }
                    /*
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Reportes/appReport"), fileName);
                    if (System.IO.File.Exists(path))
                        return Json(new { ID = 0, MENSAJE = "Archivo con el mismo nombre ya ingresado." });
                    file.SaveAs(path);
                    resultado = 1;
                    */

                }
                else {
                    errorStr = "No hay archivo seleccionado.";
                    resultado = 0;
                }
            }
            return Json(new { ID = resultado, MENSAJE = errorStr }) ;
        }

        [HttpPost]
        public JsonResult GenReport(string ParametrosJson,string tipo )
        {
            string error = "";
            string errorStr = "";
            string idUsuarioLog = "-1";
            try
            {
                idUsuarioLog = Session["LogedUserID"].ToString();

            }
            catch (Exception Ex)
            {
                errorStr = "Su sesion ha caducado, ingrese nuevamente";
                return Json(new { ID = 0, MENSAJE = errorStr });
            }
          
            List<ReporteParamValModels> dynJson = JsonConvert.DeserializeObject<List<ReporteParamValModels>>(ParametrosJson);
            int id_Reporte = 0;
            foreach (ReporteParamValModels tmp in dynJson) {
                id_Reporte = tmp.ID_REPORTE;
            }
            //OBTIENE DATOS DEL REPORTE

            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            SP_CONSULTA_REPORTE_DATAResult DATA = db.SP_CONSULTA_REPORTE_DATA().Where(x => x.ID_REPORTE == id_Reporte).FirstOrDefault();
            if (DATA != null) { 

                //VALIDA PARAMETROS CON TIPO
                string v_str = "";
                int v_int = 0;
                DateTime v_date;
                

                try {
                    string sqlCon = "Data Source=" + DATA.BDSERVER + ";Initial Catalog=" + DATA.BDNAME + ";User ID=" + DATA.BDUSER + ";Password=" + DATA.BDPASS + "";
                    SqlConnection sqlConnection1 = new SqlConnection(sqlCon);
                    SqlCommand cmd = new SqlCommand();


                    cmd.CommandText = DATA.BDSP;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = sqlConnection1;
                    
                    foreach (ReporteParamValModels tmp in dynJson)
                    {
                        if (tmp.ID_TIPO_DATO == 1)
                        {
                            try
                            {
                                v_str = tmp.VALOR.ToString();
                                cmd.Parameters.Add(new SqlParameter(tmp.NOMBRE_PARAMETRO, v_str));
                            }
                            catch
                            {
                                return Json(new { ID = 1, MENSAJE = "Revisar el parametro, " + tmp.NOMBRE_PARAMETRO });
                            }
                        }
                        else if (tmp.ID_TIPO_DATO == 2)
                        {
                            try
                            {
                                v_int = Convert.ToInt32(tmp.VALOR);
                                cmd.Parameters.Add(new SqlParameter(tmp.NOMBRE_PARAMETRO, v_int));
                            }
                            catch
                            {
                                return Json(new { ID = 1, MENSAJE = "Revisar el parametro, " + tmp.NOMBRE_PARAMETRO });
                            }
                        }
                        else if (tmp.ID_TIPO_DATO == 3)
                        {
                            try
                            {
                                v_date = DateTime.Parse(tmp.VALOR);
                                cmd.Parameters.Add(new SqlParameter(tmp.NOMBRE_PARAMETRO, v_date));
                            }
                            catch
                            {
                                return Json(new { ID = 1, MENSAJE = "Revisar el parametro, " + tmp.NOMBRE_PARAMETRO });
                            }
                        }
                    }
                    sqlConnection1.Open();


                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    adp.Fill(ds);
                    // Data is accessible through the DataReader object here.

                    sqlConnection1.Close();


                    var path = Path.Combine(Server.MapPath("~/Reportes/appReport"), "Report" + idUsuarioLog + id_Reporte.ToString() + ".rpt");
                    try
                    {
                        FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite);
                        fileStream.Write(DATA.BIN_REP.ToArray(), 0, DATA.BIN_REP.Length);
                        fileStream.Close();

                    }
                    catch (Exception ex)
                    {
                        return Json(new { ID = 1, MENSAJE = ex.Message });
                    }

                    if (ds.Tables[0].Rows.Count > 0)
                    {

                        ReportDocument rd = new ReportDocument();
                        rd.Load(path);

                        rd.SetDataSource(ds.Tables[0]);

                        Response.Buffer = false;
                        Response.ClearContent();
                        Response.ClearHeaders();
                        Stream stream;
                        String file="";
                        if (tipo == "pdf")
                        {
                            stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                            stream.Seek(0, SeekOrigin.Begin);
                            file = Convert.ToBase64String(ReadFully(stream));
                        }
                        else {
                            stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.Excel);
                            stream.Seek(0, SeekOrigin.Begin);
                            file = Convert.ToBase64String(ReadFully(stream));
                        }

                        
                        return Json(new { ID = 0, MENSAJE = file });
                    }
                    else
                    {
                        return Json(new { ID = 1, MENSAJE = "No se genero PDF"});

                    }

                }
                catch (Exception ex)  {
                    return Json(new { ID = 1, MENSAJE = "Error en ejecucion de SP:" + ex.Message });
                }
            }

            return Json(new { ID = 0, MENSAJE = "" });
        }


        [HttpPost]
        public JsonResult fnCambioEstadoRepParam(string id)
        {
            string error = "";
            string errorStr = "";
            string idUsuarioLog = "-1";
            try
            {
                idUsuarioLog = Session["LogedUserID"].ToString();

            }
            catch (Exception Ex)
            {
                errorStr = "Su sesion ha caducado, ingrese nuevamente";
                error = "1";
            }
            int v_RepParam_id = 0;
            try
            {
                v_RepParam_id = Convert.ToInt32(id);

            }
            catch (Exception Ex)
            {
                errorStr = "Su sesion ha caducado, ingrese nuevamente";
                error = "1";
                return Json(new { ID = error, MENSAJE = errorStr });
            }

            if (Convert.ToInt32(idUsuarioLog) > 0)
            {

                DC_CALIDADDataContext db = new DC_CALIDADDataContext();
                try
                {
                    db.Connection.Open();
                    db.Transaction = db.Connection.BeginTransaction();
                    TREPORTE_PARAMETRO tblProd = db.TREPORTE_PARAMETROs.Where(x => x.ID_REPORTE_PARAMETRO == v_RepParam_id)
                                           .FirstOrDefault();

                    if (tblProd.ESTADO == 1)
                        tblProd.ESTADO = 0;
                    else if (tblProd.ESTADO == 0)
                        tblProd.ESTADO = 1;


                    db.SubmitChanges();
                    db.Transaction.Commit();
                    error = "0";
                    errorStr = "Informacion guardada con exito";
                    db.Connection.Close();
                    db.Dispose();
                }
                catch (Exception e)
                {
                    errorStr = "Error en el guardado de informacion, " + e.Message;
                    db.Transaction.Rollback();
                    error = "1";
                    db.Connection.Close();
                    db.Dispose();
                }
            }
            else
            {
                errorStr = "Su sesion ha caducado, ingrese nuevamente";
                error = "1";
            }
            return Json(new { ID = error, MENSAJE = errorStr });

        }
        public static byte[] ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }
        protected int fnCambiaEstadoRepBy(int id_reporte) {
            int respuesta = 0;
            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            TREPORTE_BY product = new TREPORTE_BY();
            try
            {
                db.Connection.Open();
                db.Transaction = db.Connection.BeginTransaction();
                TREPORTE_BY tblProd = db.TREPORTE_Bies.Where(x => x.ID_REPORTE == id_reporte && x.ESTADO == 1)
                                       .FirstOrDefault();
                if (tblProd.ID_REPORTE_BY > 0) { 
                    tblProd.ESTADO = 0;

                    db.SubmitChanges();
                    db.Transaction.Commit();
                }
                respuesta = 1;
                db.Connection.Close();
                db.Dispose();
            }
            catch (Exception e)
            {
                
                db.Transaction.Rollback();
                respuesta = 0;
                db.Connection.Close();
                db.Dispose();
            }
            return respuesta;
        }
    }
}