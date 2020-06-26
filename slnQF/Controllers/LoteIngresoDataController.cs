using slnQF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;
using System.Linq.Dynamic;


namespace slnQF.Controllers
{
    public class LoteIngresoDataController : Controller
    {
        // GET: LoteIngresoData
        public ActionResult Index()
        {
            ModelState.Clear();


            return View();
        }
        public PartialViewResult Create(string p_idLoteIngreso) {
            ModelState.Clear();

            DC_CALIDADDataContext db = new DC_CALIDADDataContext();

            LOTE_INGRESO_DATA LOTE_DATA = new LOTE_INGRESO_DATA();

            LOTE_DATA = db.LOTE_INGRESO_DATAs.Where(x => x.ID_LOTE_INGRESO == Convert.ToInt32(p_idLoteIngreso)).FirstOrDefault();

            if (LOTE_DATA == null) {
                LOTE_DATA = new LOTE_INGRESO_DATA();
                LOTE_DATA.ID_LOTE_INGRESO = Convert.ToInt32(p_idLoteIngreso);
                LOTE_DATA.RESULT_FISQUI = 3;
                LOTE_DATA.RESULT_MICROBIO = 3;
                LOTE_DATA.RESULT_FINAL = 3;

            }

            


            ViewBag.ESTADO_CERTIFICADO = new SelectList(db.CAT_ESTADO_CERTIFICADOs.OrderBy(x => x.ESTADO_ID), "ESTADO_ID", "DESCRIPCION");
          

            List<MyDrop> tec = new List<MyDrop>();
            List<SP_CONSULTA_PROVEEDORResult> persona = db.SP_CONSULTA_PROVEEDOR()
           .OrderBy(x => x.CARDNAME)
           .Where(x => x.SW_MUESTRACERT == 1)
           .ToList<SP_CONSULTA_PROVEEDORResult>();
            MyDrop _temp = new MyDrop();
            _temp.id = 0;
            _temp.value = "--";

            tec.Add(_temp);
            foreach (SP_CONSULTA_PROVEEDORResult item in persona)
            {
                MyDrop temp = new MyDrop();
                temp.id = (int)item.ID_PROVEEDOR;
                temp.value = item.CARDNAME;
                tec.Add(temp);
            }

            ViewBag.PROVEEDOR = new SelectList(tec, "id", "value");


            return PartialView(LOTE_DATA);
        }
        public JsonResult createDictamen(slnQF.LOTE_INGRESO_DATA product)
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
                
               
                string respuesta = "0";
                string msj1 = "";
                string msj2 = "";
                DC_CALIDADDataContext db = new DC_CALIDADDataContext();

                //si es nuevo
                if ((product.ID_LOTE_DATA == null)|| (product.ID_LOTE_DATA == 0))
                {
                    try
                    {
                        product.ID_USUARIO = Convert.ToInt32(idUsuarioLog);
                        product.ID_ESTADO = 1;
                        product.FECHA_CREACION = DateTime.Now;
                        try
                        {

                            product.FECHA_FISQUI = product.FECHA_FISQUI;

                        }
                        catch
                        {
                            product.FECHA_FISQUI = null;
                        }
                        try
                        {

                            product.FECHA_MICROBIO = product.FECHA_MICROBIO;

                        }
                        catch
                        {
                            product.FECHA_MICROBIO = null;
                        }
                        db.Connection.Open();
                        db.Transaction = db.Connection.BeginTransaction();
                        db.LOTE_INGRESO_DATAs.InsertOnSubmit(product);
                        db.SubmitChanges();
                        db.Transaction.Commit();
                        error = respuesta;
                        if (error == "0")
                            errorStr = "Informacion guardada con exito";
                        else
                            errorStr = "Error en el guardado de informacion, " + msj1 + " / " + msj2;

                        LOG_LOTE_INGRESO_DATA(product);
                        db.Connection.Close();
                        db.Dispose();
                    }
                    catch (Exception e)
                    {
                        db.Transaction.Rollback();
                        error = "1";
                        errorStr = "Error en el guardado de informacion, " + e.Message;
                    }
                }
                else {
                    try {
                        db.Connection.Open();
                        db.Transaction = db.Connection.BeginTransaction();
                        LOTE_INGRESO_DATA LOTE_DATA = db.LOTE_INGRESO_DATAs.Where(x => x.ID_LOTE_INGRESO == Convert.ToInt32(product.ID_LOTE_INGRESO)).FirstOrDefault();
                        LOTE_DATA.ID_PROVEEDOR = product.ID_PROVEEDOR;
                        LOTE_DATA.RESULT_FINAL = product.RESULT_FINAL;
                        LOTE_DATA.RESULT_FISQUI = product.RESULT_FISQUI;
                        LOTE_DATA.RESULT_MICROBIO = product.RESULT_MICROBIO;
                        try
                        {

                            LOTE_DATA.FECHA_FISQUI = product.FECHA_FISQUI;

                        }
                        catch
                        {
                            LOTE_DATA.FECHA_FISQUI = null;
                        }

                        try
                        {

                            LOTE_DATA.FECHA_MICROBIO = product.FECHA_MICROBIO;

                        }
                        catch
                        {
                            LOTE_DATA.FECHA_MICROBIO = null;
                        }
                        db.SubmitChanges();
                        db.Transaction.Commit();
                        LOG_LOTE_INGRESO_DATA(product);
                        error = respuesta;
                        if (error == "0")
                            errorStr = "Informacion guardada con exito";
                        else
                            errorStr = "Error en el guardado de informacion, " + msj1 + " / " + msj2;
                        db.Connection.Close();
                        db.Dispose();
                    }
                    catch (Exception e) {
                        db.Transaction.Rollback();
                        error = "1";
                        errorStr = "Error en el guardado de informacion, " + e.Message;
                        db.Connection.Close();
                        db.Dispose();
                    }



                }
            }
            else
            {
                error = "1";
                errorStr = "Su sesion ha caducado, ingrese nuevamente";
            }
            return Json(new { ID = error, MENSAJE = errorStr });
        }

        private void LOG_LOTE_INGRESO_DATA(LOTE_INGRESO_DATA data) {
            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            try
            {
               
                db.Connection.Open();
                db.Transaction = db.Connection.BeginTransaction();
                //db.LOG_LOTE_INGRESO_DATAs.InsertOnSubmit();
                LOG_LOTE_INGRESO_DATA datIns = new LOG_LOTE_INGRESO_DATA();
                datIns.FECHA_CREACION = DateTime.Now;
                datIns.FECHA_FISQUI = data.FECHA_FISQUI;
                datIns.FECHA_MICROBIO = data.FECHA_MICROBIO;
                datIns.ID_ESTADO = data.ID_ESTADO;
                datIns.ID_LOTE_DATA = data.ID_LOTE_DATA;
                datIns.ID_LOTE_INGRESO = data.ID_LOTE_INGRESO;
                datIns.ID_PROVEEDOR = data.ID_PROVEEDOR;
                datIns.ID_USUARIO = data.ID_USUARIO;
                db.LOG_LOTE_INGRESO_DATAs.InsertOnSubmit(datIns);
                db.SubmitChanges();
                db.Transaction.Commit();
                db.Connection.Close();
                db.Dispose();
            }
            catch (Exception e)
            {
                db.Transaction.Rollback();
                db.SP_INSERT_LOG_ERROR(e.Message, "", DateTime.Now, "LOTE_INGRESO_DATA" + Json(data), "", "", "", "", "", "", "", "", "SLNQF LOG_LOTE_INGRESO_DATA");
              
                db.Connection.Close();
                db.Dispose();
            }
        }


        [HttpPost]
        public ActionResult LoadDataTerm()
        {
            var draw = "0";
            var start = "0";
            var length = "10";
            var sortColumn = "";
            var sortColumnDir = "";
            string idUsuarioLog = "-1";
            try
            {
                idUsuarioLog = Session["LogedUserID"].ToString();

            }
            catch (Exception Ex)
            {
                ViewBag.idUsuarioLog = 0;
            }

            draw = Request.Form.GetValues("draw").FirstOrDefault();
            start = Request.Form.GetValues("start").FirstOrDefault();
            length = Request.Form.GetValues("length").FirstOrDefault();
            sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();


            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int page = start != null ? (Convert.ToInt32(start)/ pageSize)+1 : 1;
            if (page == 0)
                page = 1;

            int totalRecords = 0;

            using (DC_CALIDADDataContext dc = new DC_CALIDADDataContext())
            {

                List<SP_CONSULTA_LIB_PROD_TERMResult> v = dc.SP_CONSULTA_LIB_PROD_TERM(Convert.ToInt32(idUsuarioLog), Convert.ToInt32(length), page, searchValue, sortColumn, sortColumnDir).ToList<SP_CONSULTA_LIB_PROD_TERMResult>();
 

              
                if (v.Count() > 0)
                    totalRecords = Convert.ToInt32(v[0].TOTAL);

                int recordF = v.Count();
                return Json(new { draw = draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = v });
            }
        }

        [HttpPost]
        public ActionResult LoadDataMateria()
        {
            var draw = "0";
            var start = "0";
            var length = "10";
            var sortColumn = "";
            var sortColumnDir = "";
            string idUsuarioLog = "-1";
            try
            {
                idUsuarioLog = Session["LogedUserID"].ToString();

            }
            catch (Exception Ex)
            {
                ViewBag.idUsuarioLog = 0;
            }

            draw = Request.Form.GetValues("draw").FirstOrDefault();
            start = Request.Form.GetValues("start").FirstOrDefault();
            length = Request.Form.GetValues("length").FirstOrDefault();
            sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();


            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int page = start != null ? (Convert.ToInt32(start) / pageSize) + 1 : 1;
            if (page == 0)
                page = 1;

            int totalRecords = 0;

            using (DC_CALIDADDataContext dc = new DC_CALIDADDataContext())
            {

                List<SP_CONSULTA_LIB_MAT_PRIMResult> v = dc.SP_CONSULTA_LIB_MAT_PRIM(Convert.ToInt32(idUsuarioLog), Convert.ToInt32(length), page, searchValue, sortColumn, sortColumnDir).ToList<SP_CONSULTA_LIB_MAT_PRIMResult>();

                if (v.Count() > 0)
                    totalRecords = Convert.ToInt32(v[0].TOTAL);

                int recordF = v.Count();
                return Json(new { draw = draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = v });
            }
        }


        public PartialViewResult ListadoMatPri()
        {
            ViewBag.idUsuarioLog = 1;
            string idUsuarioLog = "-1";
            try
            {
                idUsuarioLog = Session["LogedUserID"].ToString();

            }
            catch (Exception Ex)
            {
                ViewBag.idUsuarioLog = 0;
            }

            ModelState.Clear();
            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            List<SP_CONSULTA_LIB_MAT_PRIMResult> PRUEBA = db.SP_CONSULTA_LIB_MAT_PRIM(Convert.ToInt32(idUsuarioLog), 10, 1, "","","")
                .OrderByDescending(x => x.ID_LOTE_INGRESO)
               .ToList<SP_CONSULTA_LIB_MAT_PRIMResult>();
            return PartialView(PRUEBA);
        }

        public PartialViewResult ListadoProdTerm()
        {
            ViewBag.idUsuarioLog = 1;
            string idUsuarioLog = "-1";
            try
            {
                idUsuarioLog = Session["LogedUserID"].ToString();

            }
            catch (Exception Ex)
            {
                ViewBag.idUsuarioLog = 0;
            }

            ModelState.Clear();
            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            List<SP_CONSULTA_LIB_PROD_TERMResult> PRUEBA = db.SP_CONSULTA_LIB_PROD_TERM(Convert.ToInt32(idUsuarioLog),-1,-1,"", "", "")
               .ToList<SP_CONSULTA_LIB_PROD_TERMResult>();
            return PartialView(PRUEBA);
        }


    }
}