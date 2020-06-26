using slnQF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace slnQF.Controllers
{
    public class LoteIngresoController : Controller
    {
        // GET: LoteIngreso
        public ActionResult Index()
        {
            ModelState.Clear();
          
            return View();
        }

        [HttpPost]
        public ActionResult LoadDataLote()
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

                List<SP_CONSULTA_LOTE_INGRESO_PAGResult> v = dc.SP_CONSULTA_LOTE_INGRESO_PAG(Convert.ToInt32(length), page, searchValue, sortColumn, sortColumnDir).ToList<SP_CONSULTA_LOTE_INGRESO_PAGResult>();



                if (v.Count() > 0)
                    totalRecords = Convert.ToInt32(v[0].TOTAL);

                int recordF = v.Count();
                return Json(new { draw = draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = v });
            }
        }

        public List<CatalogoModel.MyDropList> traeComboStatus()
        {
            List<CatalogoModel.MyDropList> LISTA = new List<CatalogoModel.MyDropList>();

            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            List<CAT_STATUS> PRUEBA = db.CAT_STATUS.ToList<CAT_STATUS>();

            CatalogoModel.MyDropList fila2 = new CatalogoModel.MyDropList();
            fila2.id = "-1";
            fila2.value = "NO DEFINIDO";

            LISTA.Add(fila2);

            foreach (CAT_STATUS dr in PRUEBA)
            {
                CatalogoModel.MyDropList fila = new CatalogoModel.MyDropList();
                fila.id = dr.ID_STATUS.ToString();
                fila.value = dr.DESCRIPCION.ToString();

                LISTA.Add(fila);
            }
            return LISTA;
        }
        public PartialViewResult Crear()
        {

            DC_CALIDADDataContext db = new DC_CALIDADDataContext();

            
            ViewBag.ProductoDDL = new SelectList(db.SP_CONSULTA_PRODUCTO().Where(x => x.ESTADO_ID == 1).OrderBy(x => x.NOMBRE), "ID_PRODUCTO", "NOMBRE");

            ViewBag.CertificadoDDL = new SelectList(db.SP_CONSULTA_CERTIFICADO().Where(x => x.ESTADO_ID == 1).OrderBy(x => x.DESCRIPCION), "ID_CERTIFICADO", "DESCRIPCION");

            ViewBag.FabricanteDDL = new SelectList(db.CAT_FABRICANTEs.Where(x => x.ESTADO == 1).OrderBy(x => x.NOMBRE), "ID_FABRICANTE", "NOMBRE");

            ViewBag.PaisFabricanteDDL = new SelectList(db.CAT_PAIS_FABRICANTEs.Where(x => x.ESTADO == 1).OrderBy(x => x.NOMBRE), "ID_PAIS_FABRICANTE", "NOMBRE");

            List <MyDrop> tec = new List<MyDrop>();
            
            List<SP_CONSULTA_PERSONAResult> persona = db.SP_CONSULTA_PERSONA()
              .OrderBy(x => x.NOMBRE)
              .Where(x => x.ESTADO_ID == 1)
              .ToList<SP_CONSULTA_PERSONAResult>();
            MyDrop _temp = new MyDrop();
                _temp.id = 0;
                _temp.value = "--";

            tec.Add(_temp);
            foreach (SP_CONSULTA_PERSONAResult item in persona) {
                MyDrop temp = new MyDrop();
                temp.id = item.ID_PERSONA;
                temp.value = item.NOMBRE;
                tec.Add(temp);
            }

            ViewBag.TecnicoDDL = new SelectList(tec, "id", "value");

            ViewBag.RevisaDDL = new SelectList(tec, "id", "value");

            ViewBag.AseguraDDL = new SelectList(tec, "id", "value");

            ViewBag.StatusDDL = new SelectList(traeComboStatus(), "id", "value");


            slnQF.Models.LoteIngresoModels r = new slnQF.Models.LoteIngresoModels();
            r.FECHA_ANALISIS = DateTime.Now;
            r.FECHA_INI_ANALISIS = DateTime.Now;
            r.FECHA_FIN_ANALISIS = DateTime.Now.AddDays(1);
            

            return PartialView(r);
        }
        public PartialViewResult ListadoTecnico(int id)
        {
            ModelState.Clear();
            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            List<SP_CONSULTA_TECNICO_CERTIFICADOResult> TECNICO = db.SP_CONSULTA_TECNICO_CERTIFICADO()
               .OrderBy(x => x.ID_LOTE_INGRESO_TECNICO).Where(x => x.ID_LOTE_INGRESO == id)
               .ToList<SP_CONSULTA_TECNICO_CERTIFICADOResult>();
            return PartialView(TECNICO);
        }
        [HttpPost]
        public JsonResult fnCambioEstadoTecnicoCert(string id)
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
            int v_ingreso_tecnico= 0;
            try
            {
                v_ingreso_tecnico = Convert.ToInt32(id);

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
                    TLOTEINGRESO_TECNICO tblProd = db.TLOTEINGRESO_TECNICOs.Where(x => x.ID_LOTE_INGRESO_TECNICO == v_ingreso_tecnico)
                                           .FirstOrDefault();

                    if (tblProd.ID_ESTADO == 1)
                        tblProd.ID_ESTADO = 0;
                    else if (tblProd.ID_ESTADO == 0)
                        tblProd.ID_ESTADO = 1;


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
        public PartialViewResult CrearLote_Tecnico(string id)
        {

            DC_CALIDADDataContext db = new DC_CALIDADDataContext();

            List<MyDrop> tec = new List<MyDrop>();

            List<SP_CONSULTA_PERSONA_LOTE_EXPResult> persona = db.SP_CONSULTA_PERSONA_LOTE_EXP(Convert.ToInt32(id))
              .OrderBy(x => x.NOMBRE)
              .ToList<SP_CONSULTA_PERSONA_LOTE_EXPResult>();
            
            foreach (SP_CONSULTA_PERSONA_LOTE_EXPResult item in persona)
            {
                MyDrop temp = new MyDrop();
                temp.id = item.ID_PERSONA;
                temp.value = item.NOMBRE;
                tec.Add(temp);
            }

            ViewBag.TecnicoDDL = new SelectList(tec, "id", "value");

            TLOTEINGRESO_TECNICO tmp = new TLOTEINGRESO_TECNICO();
            tmp.ID_LOTE_INGRESO = Convert.ToInt32(id);

            return PartialView(tmp);
        }

        [HttpPost]
        public JsonResult Crear(slnQF.LOTE_INGRESO product)
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


                if (product.ID_STATUS == -1)
                    product.ID_STATUS = null;
                product.ID_USUARIO = Convert.ToInt32(idUsuarioLog);
                try {
                    if (product.FECHA_ANALISIS.Value.Year != 2001)
                    {
                        product.FECHA_ANALISIS = product.FECHA_ANALISIS.Value.AddDays(1);
                    }
                    else {
                        product.FECHA_ANALISIS = Convert.ToDateTime("01/01/2001");
                    }
                }
                catch {
                product.FECHA_ANALISIS = null;
                }
                try
                {
                
                    if (product.FECHA_FABRICACION.Value.Year != 2001)
                    {
                        product.FECHA_FABRICACION = product.FECHA_FABRICACION.Value.AddDays(1);
                    }
                    else
                    {
                        product.FECHA_FABRICACION = Convert.ToDateTime("01/01/2001");
                    }
                }
                catch
                {
                product.FECHA_FABRICACION = null;
                }
                try
                { 
                    if (product.FECHA_FIN_ANALISIS.Value.Year != 2001)
                    {
                        product.FECHA_FIN_ANALISIS = product.FECHA_FIN_ANALISIS.Value.AddDays(1);
                    }
                    else
                    {
                        product.FECHA_FIN_ANALISIS = Convert.ToDateTime("01/01/2001");
                    }
                }
                catch
                {
                product.FECHA_FIN_ANALISIS = null;
                }
                try
                {
                 
                    if (product.FECHA_INI_ANALISIS.Value.Year != 2001)
                    {
                        product.FECHA_INI_ANALISIS = product.FECHA_INI_ANALISIS.Value.AddDays(1);
                    }
                    else
                    {
                        product.FECHA_INI_ANALISIS = Convert.ToDateTime("01/01/2001");
                    }
                }
                catch
                {
                product.FECHA_INI_ANALISIS = null;
                }
                try
                {

                    if (product.FECHA_VENCIMIENTO.Value.Year != 2001)
                    {
                        product.FECHA_VENCIMIENTO = product.FECHA_VENCIMIENTO.Value.AddDays(1);
                    }
                    else
                    {
                        product.FECHA_VENCIMIENTO = Convert.ToDateTime("01/01/2001");
                    }

                 
                }
                catch
                {
                product.FECHA_VENCIMIENTO = null;
                }

                try
                {
                    

                    if (product.FECHA_INGRESO.Value.Year != 2001)
                    {
                        product.FECHA_INGRESO = product.FECHA_INGRESO.Value.AddDays(1);
                    }
                    else
                    {
                        product.FECHA_INGRESO = Convert.ToDateTime("01/01/2001");
                    }


                }
                catch
                {
                    product.FECHA_INGRESO = null;
                }

                try
                {

                    if (product.FECHA_REANALISIS.Value.Year != 2001)
                    {
                        product.FECHA_REANALISIS = product.FECHA_REANALISIS.Value.AddDays(1);
                    }
                    else
                    {
                        product.FECHA_REANALISIS = Convert.ToDateTime("01/01/2001");
                    }
                }
                catch
                {
                    product.FECHA_REANALISIS = null;
                }

                product.FECHA_CREACION = DateTime.Now;
                string respuesta = "0";
                string msj1 = "";
                string msj2 = "";
                try
                {
                    db.Connection.Open();

                    db.Transaction = db.Connection.BeginTransaction();
                    //db.LOTE_INGRESOs.InsertOnSubmit(product);
 
                    db.SP_INSERT_LOTE_INGRESO(product.ID_CERTIFICADO, product.ID_PRODUCTO, product.FECHA_ANALISIS, product.FECHA_FABRICACION
                        , product.FECHA_VENCIMIENTO, product.FECHA_INI_ANALISIS, product.FECHA_FIN_ANALISIS, product.TOMO, product.PAGINA,
                        product.NO_LOTE, product.ID_FABRICANTE, product.ID_PAIS_FABRICANTE, product.OBSERVACIONES, product.ID_TECNICO_ANALISTA
                        , product.ID_REVISADO_POR,product.ID_ASEGURAMIENTO, product.ID_STATUS, product.ASEGURAMIENTO, product.ID_USUARIO, product.POTENCIA_PROVEEDOR,
                        product.FECHA_INGRESO, product.FECHA_REANALISIS, product.CANTIDAD_ENVASES, ref respuesta, ref msj1, ref msj2);
                    db.SubmitChanges();
                    db.Transaction.Commit();
                    error = respuesta;
                    if(error == "0")
                    errorStr = "Informacion guardada con exito";
                    else
                        errorStr = "Error en el guardado de informacion, " + msj1 + " / " + msj2;
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
   
        [HttpPost]
        public JsonResult createLoteTecnico(slnQF.TLOTEINGRESO_TECNICO product)
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
                product.ID_USUARIO = Convert.ToInt32(idUsuarioLog);
                product.ID_ESTADO = 1;
                product.FECHA_CREACION = DateTime.Now;
                string respuesta = "0";
                string msj1 = "";
                string msj2 = "";
                try
                {
                    db.Connection.Open();

                    db.Transaction = db.Connection.BeginTransaction();
                    db.TLOTEINGRESO_TECNICOs.InsertOnSubmit(product);

                   
                    db.SubmitChanges();
                    db.Transaction.Commit();
                    error = respuesta;
                    if (error == "0")
                        errorStr = "Informacion guardada con exito";
                    else
                        errorStr = "Error en el guardado de informacion, " + msj1 + " / " + msj2;
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
        public PartialViewResult Edit(int Id)
        {
            
            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
 
            LOTE_INGRESO product = db.LOTE_INGRESOs.Where(x => x.ID_LOTE_INGRESO == Id ).FirstOrDefault();
            slnQF.Models.LoteIngresoModels prod = new slnQF.Models.LoteIngresoModels();
            prod.ID_LOTE_INGRESO = product.ID_LOTE_INGRESO;
            prod.ID_PRODUCTO = product.ID_PRODUCTO;
            prod.ID_CERTIFICADO = product.ID_CERTIFICADO;
            prod.NO_INGRESO = product.NO_INGRESO;
            prod.TOMO = product.TOMO;
            prod.PAGINA = product.PAGINA;
            prod.NO_LOTE = product.NO_LOTE;
            prod.ID_FABRICANTE = product.ID_FABRICANTE;
            prod.ID_PAIS_FABRICANTE = product.ID_PAIS_FABRICANTE;
            prod.OBSERVACIONES = product.OBSERVACIONES;
            
            try { 
                prod.ID_ASEGURAMIENTO = (int)product.ID_ASEGURAMIENTO;
            }catch{
                prod.ID_ASEGURAMIENTO = 0;
            }
            try { 
            prod.CANTIDAD_ENVASES = (int)product.CANTIDAD_ENVASES;
            }catch{
                prod.CANTIDAD_ENVASES = 0;
            }
            prod.POTENCIA_PROVEEDOR = product.POTENCIA_PROVEEDOR;
            prod.ID_TECNICO_ANALISTA = product.ID_TECNICO_ANALISTA;
            prod.ID_REVISADO_POR = product.ID_REVISADO_POR;
            prod.ASEGURAMIENTO = product.ASEGURAMIENTO;
            
            try
            {
                prod.FECHA_ANALISIS = Convert.ToDateTime(product.FECHA_ANALISIS);
            }
            catch
            {
                prod.FECHA_ANALISIS = DateTime.Now;
            }
            try
            {
                prod.FECHA_FABRICACION = Convert.ToDateTime(product.FECHA_FABRICACION);
            }
            catch
            {
                prod.FECHA_FABRICACION = DateTime.Now;
            }
            try
            {
                prod.FECHA_VENCIMIENTO = Convert.ToDateTime(product.FECHA_VENCIMIENTO);
            }
            catch
            {
                prod.FECHA_VENCIMIENTO = DateTime.Now;
            }
            try
            {
                prod.FECHA_INI_ANALISIS = Convert.ToDateTime(product.FECHA_INI_ANALISIS);
            }
            catch
            {
                prod.FECHA_INI_ANALISIS = DateTime.Now;
            }
            try
            {
                prod.FECHA_FIN_ANALISIS = Convert.ToDateTime(product.FECHA_FIN_ANALISIS);
            }
            catch
            {
                prod.FECHA_FIN_ANALISIS = DateTime.Now;
            }
            try
            {
                prod.FECHA_INGRESO = Convert.ToDateTime(product.FECHA_INGRESO);
            }
            catch
            {
                prod.FECHA_INGRESO = Convert.ToDateTime("01/01/1991") ;
            }
            try
            {
                prod.FECHA_REANALISIS = Convert.ToDateTime(product.FECHA_REANALISIS);
            }
            catch
            {
                prod.FECHA_REANALISIS = Convert.ToDateTime("01/01/1991");
            }
            try
            {
                prod.ID_STATUS = Convert.ToInt32(product.ID_STATUS);
            }
            catch {
                prod.ID_STATUS = -1;
            }

            ViewBag.CertificadoDDL = new SelectList(db.SP_CONSULTA_CERTIFICADO().Where(x => x.ESTADO_ID == 1).OrderBy(x => x.DESCRIPCION), "ID_CERTIFICADO", "DESCRIPCION",prod.ID_CERTIFICADO);

            ViewBag.FabricanteDDL = new SelectList(db.CAT_FABRICANTEs.Where(x => x.ESTADO == 1).OrderBy(x => x.NOMBRE), "ID_FABRICANTE", "NOMBRE", prod.ID_FABRICANTE);

            ViewBag.PaisFabricanteDDL = new SelectList(db.CAT_PAIS_FABRICANTEs.Where(x => x.ESTADO == 1).OrderBy(x => x.NOMBRE), "ID_PAIS_FABRICANTE", "NOMBRE", prod.ID_PAIS_FABRICANTE);

            ViewBag.ProductoDDL = new SelectList(db.SP_CONSULTA_PRODUCTO().Where(x => x.ESTADO_ID == 1).OrderBy(x => x.NOMBRE), "ID_PRODUCTO", "NOMBRE",prod.ID_PRODUCTO);



            List<MyDrop> tec = new List<MyDrop>();

            List<SP_CONSULTA_PERSONAResult> persona = db.SP_CONSULTA_PERSONA()
              .OrderBy(x => x.NOMBRE)
              .Where(x => x.ESTADO_ID == 1)
              .ToList<SP_CONSULTA_PERSONAResult>();
            MyDrop _temp = new MyDrop();
            _temp.id = 0;
            _temp.value = "--";
            tec.Add(_temp);
            foreach (SP_CONSULTA_PERSONAResult item in persona)
            {
                MyDrop temp = new MyDrop();
                temp.id = item.ID_PERSONA;
                temp.value = item.NOMBRE;
                tec.Add(temp);
            }

            ViewBag.TecnicoDDL = new SelectList(tec, "id", "value", prod.ID_TECNICO_ANALISTA);

            ViewBag.RevisaDDL = new SelectList(tec, "id", "value", prod.ID_REVISADO_POR);

            ViewBag.AseguraDDL = new SelectList(tec, "id", "value", prod.ID_ASEGURAMIENTO);

            /*
            ViewBag.TecnicoDDL = new SelectList(db.SP_CONSULTA_PERSONA().Where(x => x.ESTADO_ID == 1).OrderBy(x => x.NOMBRE), "ID_PERSONA", "NOMBRE", prod.ID_TECNICO_ANALISTA);

            ViewBag.RevisaDDL = new SelectList(db.SP_CONSULTA_PERSONA().Where(x => x.ESTADO_ID == 1).OrderBy(x => x.NOMBRE), "ID_PERSONA", "NOMBRE", prod.ID_REVISADO_POR);
            */

            ViewBag.StatusDDL = new SelectList(traeComboStatus(), "id", "value",prod.ID_STATUS);
           
            return PartialView(prod);
        }

        [HttpPost]
        public JsonResult Edit(slnQF.LOTE_INGRESO product)
        {

            string errorStr = "";
            string error = "";
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
                try
                {
                    db.Connection.Open();
                    db.Transaction = db.Connection.BeginTransaction();
                    LOTE_INGRESO tblProd = db.LOTE_INGRESOs.Where(x => x.ID_LOTE_INGRESO == product.ID_LOTE_INGRESO ).FirstOrDefault();
                    tblProd.ID_STATUS = product.ID_STATUS;
                    if (tblProd.ID_STATUS == -1)
                        tblProd.ID_STATUS = null;
                    tblProd.ID_CERTIFICADO = product.ID_CERTIFICADO;
                    tblProd.NO_INGRESO = product.NO_INGRESO;
                    tblProd.TOMO = product.TOMO;
                    tblProd.PAGINA = product.PAGINA;
                    tblProd.NO_LOTE = product.NO_LOTE;
                    tblProd.ID_FABRICANTE = product.ID_FABRICANTE;
                    tblProd.ID_PAIS_FABRICANTE = product.ID_PAIS_FABRICANTE;
                    tblProd.OBSERVACIONES = product.OBSERVACIONES;
                    tblProd.ID_PRODUCTO= product.ID_PRODUCTO;
                    tblProd.ID_USUARIO = Convert.ToInt32(idUsuarioLog);

                    tblProd.CANTIDAD_ENVASES = product.CANTIDAD_ENVASES;

                    tblProd.POTENCIA_PROVEEDOR = product.POTENCIA_PROVEEDOR;
                    tblProd.ID_TECNICO_ANALISTA = product.ID_TECNICO_ANALISTA;
                    tblProd.ID_REVISADO_POR = product.ID_REVISADO_POR;
                    tblProd.ID_ASEGURAMIENTO= product.ID_ASEGURAMIENTO;
                    tblProd.ASEGURAMIENTO = product.ASEGURAMIENTO;

                    tblProd.FECHA_ANALISIS = Convert.ToDateTime(product.FECHA_ANALISIS).AddDays(1);
                    tblProd.FECHA_FABRICACION = Convert.ToDateTime(product.FECHA_FABRICACION).AddDays(1);
                    tblProd.FECHA_VENCIMIENTO = Convert.ToDateTime(product.FECHA_VENCIMIENTO).AddDays(1);
                    tblProd.FECHA_INI_ANALISIS = Convert.ToDateTime(product.FECHA_INI_ANALISIS).AddDays(1);
                    tblProd.FECHA_FIN_ANALISIS = Convert.ToDateTime(product.FECHA_FIN_ANALISIS).AddDays(1);

                    tblProd.FECHA_INGRESO = Convert.ToDateTime(product.FECHA_INGRESO).AddDays(1);
                    tblProd.FECHA_REANALISIS = Convert.ToDateTime(product.FECHA_REANALISIS).AddDays(1);

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
        public static byte[] ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }
        public String DownloadReport2(string id)
        {
            string nombre = "";
            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            int numeroID = Convert.ToInt32(id);
            IList<SP_REPORTE_CERTIFICADO_2Result> list = db.SP_REPORTE_CERTIFICADO_2(numeroID).ToList<SP_REPORTE_CERTIFICADO_2Result>();
            List<SP_REPORTE_CERTIFICADOModels> LISTA = new List<SP_REPORTE_CERTIFICADOModels>();

            foreach (SP_REPORTE_CERTIFICADO_2Result item in list)
            {
                SP_REPORTE_CERTIFICADOModels itemN = new SP_REPORTE_CERTIFICADOModels();
                itemN.ID_LOTE_INGRESO = item.ID_LOTE_INGRESO;
                itemN.CANTIDAD_MUESTRAS = item.CANTIDAD_MUESTRAS;
                itemN.CANTIDAD_ENVASES = item.CANTIDAD_ENVASES;
                itemN.CONDICION_ALMACENA = item.CONDICION_ALMACENA;
                itemN.PROVEEDOR = item.PROVEEDOR;
                itemN.TAMANO_LOTE = item.TAMANO_LOTE;
               
                itemN.CONCENTRACION = item.CONCENTRACION;
                itemN.ENSAYO_NOMBRE = item.ENSAYO_NOMBRE;
                itemN.ESPECIFICACION = item.ESPECIFICACION;
                itemN.TECNICOS = item.TECNICOS + ',';
                itemN.UOMPROD = item.UOMPROD;
                itemN.FABRICANTE_NOMBRE = item.FABRICANTE_NOMBRE;
                itemN.ID_CERTIFICADO = item.ID_CERTIFICADO;
                itemN.CERTIFICADO = item.CERTIFICADO;
                itemN.PRESENTACION = item.PRESENTACION;
                itemN.NOMBRE_COMERCIAL = item.NOMBRE_COMERCIAL;
                itemN.FECHA_FIN_ANALISIS = item.FECHA_FIN_ANALISIS;
                try
                {
                    itemN.FECHA_ANALISIS = (DateTime)item.FECHA_ANALISIS;
                }
                catch
                {
                    itemN.FECHA_ANALISIS = Convert.ToDateTime("01/01/1990");
                }
                try
                {
                    itemN.FECHA_FABRICACION = (DateTime)item.FECHA_FABRICACION;
                }
                catch
                {
                    itemN.FECHA_FABRICACION = Convert.ToDateTime("01/01/1990");
                }
                try
                {
                    itemN.FECHA_VENCIMIENTO = (DateTime)item.FECHA_VENCIMIENTO;
                }
                catch
                {
                    itemN.FECHA_VENCIMIENTO = Convert.ToDateTime("01/01/1990");
                }
                itemN.FORMA_FARMACEUTICA_NOMBRE = item.FORMA_FARMACEUTICA_NOMBRE;
                itemN.NO_INGRESO = item.NO_INGRESO;
                itemN.NO_LOTE = item.NO_LOTE;
                itemN.OBSERVACIONES = item.OBSERVACIONES;
                itemN.PAGINA = item.PAGINA;
                itemN.PAIS_NOMBRE = item.PAIS_NOMBRE;
                itemN.PRODUCTO_NOMBRE = item.PRODUCTO_NOMBRE;
                itemN.PRUEBA_ESPECIFICA_NOMBRE = item.PRUEBA_ESPECIFICA_NOMBRE;
                itemN.PRUEBA_NOMBRE = item.PRUEBA_NOMBRE;
                itemN.REFERENCIA = item.REFERENCIA;
                itemN.RESULTADO = item.RESULTADO;
                itemN.TOMO = item.TOMO;
                itemN.TECNICO_ANALISTA = item.TECNICO_ANALISTA;
                itemN.REVISADO_POR = item.REVISADO_POR;
                itemN.ASEGURAMIENTO_POR = item.ASEGURAMIENTO_POR;
                itemN.APROBADO = item.APROBADO;
                itemN.REPROBADO = item.REPROBADO;
                itemN.UOM = item.UOM;
                itemN.PUESTO_ANALISTA = item.PUESTO_ANALISTA;
                itemN.PUESTO_ASEGURAMIENTO = item.PUESTO_ASEGURAMIENTO;
                itemN.PUESTO_REVISADO_POR = item.PUESTO_REVISADO_POR;
                itemN.CERTIFICADO = item.CERTIFICADO;
                itemN.ID_CERTIFICADO = item.ID_CERTIFICADO;
                itemN.CLIENTE = item.CLIENTE;
                
                LISTA.Add(itemN);
                nombre = "Certificado_" + itemN.NO_INGRESO;
            }
            var groupedPresenList = LISTA
                        .GroupBy(p => new { p.TAMANO_LOTE ,p.CLIENTE, p.PRESENTACION })
                        
                        .ToList();

            foreach (var group in groupedPresenList)
            {
                
                    var v_clientes2 = group.Key;
                 
            }

            var groupedCustomerList = LISTA
                        .GroupBy(u => u.CLIENTE)
                        .ToList();
            string v_clientes="";
            foreach (var group in groupedCustomerList)
            {                
                    if (v_clientes != "")
                    {
                        v_clientes = v_clientes +", "+ group.Key;
                    }
                    else {
                        v_clientes = group.Key;
                    }
            } 

            if (LISTA.Count() > 0)
            {

                ReportDocument rd = new ReportDocument();
                rd.Load(Path.Combine(Server.MapPath("~/Reportes"), "Reporte_Certificado_1.rpt"));

                rd.SetDataSource(LISTA);

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
                SP_REPORTE_CERTIFICADOModels itemN = new SP_REPORTE_CERTIFICADOModels();

                itemN.ID_LOTE_INGRESO = 0;

                itemN.NO_INGRESO = "";

                itemN.FECHA_ANALISIS = DateTime.Today;

                itemN.TOMO = "";

                itemN.PAGINA = "";

                itemN.NO_LOTE = "";

                itemN.FECHA_FABRICACION = DateTime.Today;

                itemN.FECHA_VENCIMIENTO = DateTime.Today;

                itemN.OBSERVACIONES = "";

                itemN.PRODUCTO_NOMBRE = "";

                itemN.CANTIDAD_MUESTRAS = 0;

                itemN.CANTIDAD_ENVASES = 0;
                itemN.CONDICION_ALMACENA = "";
                itemN.PROVEEDOR = "";

                itemN.REFERENCIA = "";

                itemN.FORMA_FARMACEUTICA_NOMBRE = "";

                itemN.CONCENTRACION = "";

                itemN.FABRICANTE_NOMBRE = "";

                itemN.PAIS_NOMBRE = "";

                itemN.TECNICO_ANALISTA = "";

                itemN.REVISADO_POR = "";

                itemN.ASEGURAMIENTO_POR = "";

                itemN.PUESTO_ANALISTA = "";

                itemN.PUESTO_REVISADO_POR = "";

                itemN.PUESTO_ASEGURAMIENTO = "";

                itemN.APROBADO = "";

                itemN.REPROBADO = "";

                itemN.UOM = "";

                itemN.UOMPROD = "";

                itemN.ENSAYO_NOMBRE = "";

                itemN.PRUEBA_NOMBRE = "";

                itemN.PRUEBA_ESPECIFICA_NOMBRE = "";

                itemN.ESPECIFICACION = "";

                itemN.RESULTADO = "";

                itemN.ID_CERTIFICADO = 0;

                itemN.CERTIFICADO = "";
                itemN.PRESENTACION = "";
                itemN.TECNICOS = "";
                itemN.CLIENTE = "";
                LISTA.Add(itemN);
                rd.SetDataSource(LISTA);
                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();


                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                String file = Convert.ToBase64String(ReadFully(stream));
                return file;

            }





        }
        public FileResult DownloadReport(string id)
        {
            string nombre = "";
            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            int numeroID = Convert.ToInt32(id);
            IList<SP_REPORTE_CERTIFICADO_2Result> list = db.SP_REPORTE_CERTIFICADO_2(numeroID).ToList<SP_REPORTE_CERTIFICADO_2Result>();
            List<SP_REPORTE_CERTIFICADOModels> LISTA = new List<SP_REPORTE_CERTIFICADOModels>();

            foreach (SP_REPORTE_CERTIFICADO_2Result item in list)
            {
                SP_REPORTE_CERTIFICADOModels itemN = new SP_REPORTE_CERTIFICADOModels();
                itemN.ID_LOTE_INGRESO = item.ID_LOTE_INGRESO;
                itemN.CANTIDAD_MUESTRAS = item.CANTIDAD_MUESTRAS;
                itemN.CANTIDAD_ENVASES = item.CANTIDAD_ENVASES;
                itemN.CONDICION_ALMACENA = item.CONDICION_ALMACENA;
                itemN.PROVEEDOR = item.PROVEEDOR;
                itemN.TAMANO_LOTE = item.TAMANO_LOTE;
                itemN.CONCENTRACION = item.CONCENTRACION;
                itemN.ENSAYO_NOMBRE = item.ENSAYO_NOMBRE;
                itemN.ESPECIFICACION = item.ESPECIFICACION;
                itemN.FABRICANTE_NOMBRE = item.FABRICANTE_NOMBRE;
                itemN.ID_CERTIFICADO = item.ID_CERTIFICADO;
                itemN.CERTIFICADO = item.CERTIFICADO;
                itemN.PRESENTACION = item.PRESENTACION;
                itemN.NOMBRE_COMERCIAL = item.NOMBRE_COMERCIAL;
                itemN.FECHA_FIN_ANALISIS = item.FECHA_FIN_ANALISIS;
                try
                {
                    itemN.FECHA_ANALISIS = (DateTime)item.FECHA_ANALISIS;
                }
                catch
                {
                    itemN.FECHA_ANALISIS = Convert.ToDateTime("01/01/1990");
                }
                try
                {
                    itemN.FECHA_FABRICACION = (DateTime)item.FECHA_FABRICACION;
                }
                catch
                {
                    itemN.FECHA_FABRICACION = Convert.ToDateTime("01/01/1990");
                }
                try
                {
                    itemN.FECHA_VENCIMIENTO = (DateTime)item.FECHA_VENCIMIENTO;
                }
                catch
                {
                    itemN.FECHA_VENCIMIENTO = Convert.ToDateTime("01/01/1990");
                }
                itemN.FORMA_FARMACEUTICA_NOMBRE = item.FORMA_FARMACEUTICA_NOMBRE;
                itemN.NO_INGRESO = item.NO_INGRESO;
                itemN.NO_LOTE = item.NO_LOTE;
                itemN.OBSERVACIONES = item.OBSERVACIONES;
                itemN.PAGINA = item.PAGINA;
                itemN.PAIS_NOMBRE = item.PAIS_NOMBRE;
                itemN.PRODUCTO_NOMBRE = item.PRODUCTO_NOMBRE;
                itemN.PRUEBA_ESPECIFICA_NOMBRE = item.PRUEBA_ESPECIFICA_NOMBRE;
                itemN.PRUEBA_NOMBRE = item.PRUEBA_NOMBRE;
                itemN.REFERENCIA = item.REFERENCIA;
                itemN.RESULTADO = item.RESULTADO;
                itemN.TOMO = item.TOMO;
                itemN.TECNICO_ANALISTA = item.TECNICO_ANALISTA;
                itemN.TECNICOS = item.TECNICOS + ',';
                itemN.REVISADO_POR = item.REVISADO_POR;
                itemN.ASEGURAMIENTO_POR = item.ASEGURAMIENTO_POR;
                itemN.APROBADO = item.APROBADO;
                itemN.REPROBADO = item.REPROBADO;
                itemN.UOM = item.UOM;
                itemN.PUESTO_ANALISTA = item.PUESTO_ANALISTA;
                itemN.PUESTO_ASEGURAMIENTO = item.PUESTO_ASEGURAMIENTO;
                itemN.PUESTO_REVISADO_POR = item.PUESTO_REVISADO_POR;
                itemN.CERTIFICADO = item.CERTIFICADO;
                itemN.ID_CERTIFICADO = item.ID_CERTIFICADO;
                itemN.CLIENTE = item.CLIENTE;
                itemN.UOMPROD = item.UOMPROD;
                LISTA.Add(itemN);
                nombre = "Certificado_" + itemN.NO_INGRESO;
            }
            if (LISTA.Count() > 0)
            {

                ReportDocument rd = new ReportDocument();
                rd.Load(Path.Combine(Server.MapPath("~/Reportes"), "Reporte_Certificado_1.rpt"));
                 
                rd.SetDataSource(LISTA);
                
                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();
               
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", nombre + ".pdf");

            }
            else {
                ReportDocument rd = new ReportDocument();
                rd.Load(Path.Combine(Server.MapPath("~/Reportes"), "Reporte_Certificado_1.rpt"));
                SP_REPORTE_CERTIFICADOModels itemN = new SP_REPORTE_CERTIFICADOModels();

                itemN.ID_LOTE_INGRESO = 0;

                itemN.NO_INGRESO = "";

                itemN.FECHA_ANALISIS = DateTime.Today;

                itemN.TOMO = "";

                itemN.PAGINA = "";

                itemN.NO_LOTE = "";

                itemN.FECHA_FABRICACION = DateTime.Today;

                itemN.FECHA_VENCIMIENTO = DateTime.Today;

                itemN.OBSERVACIONES = "";

                itemN.PRODUCTO_NOMBRE = "";

                itemN.CANTIDAD_MUESTRAS = 0;

                itemN.CANTIDAD_ENVASES = 0;
                itemN.CONDICION_ALMACENA = "";
                itemN.PROVEEDOR = "";

                itemN.REFERENCIA = "";

                itemN.FORMA_FARMACEUTICA_NOMBRE = "";

                itemN.CONCENTRACION = "";

                itemN.FABRICANTE_NOMBRE = "";

                itemN.PAIS_NOMBRE = "";

                itemN.TECNICO_ANALISTA = "";

                itemN.TECNICOS = "";

                itemN.REVISADO_POR = "";

                itemN.ASEGURAMIENTO_POR = "";

                itemN.PUESTO_ANALISTA = "";

                itemN.PUESTO_REVISADO_POR = "";

                itemN.PUESTO_ASEGURAMIENTO = "";

                itemN.APROBADO = "";

                itemN.REPROBADO = "";

                itemN.UOM = "";

                itemN.UOMPROD = "";

                itemN.ENSAYO_NOMBRE = "";

                itemN.PRUEBA_NOMBRE = "";

                itemN.PRUEBA_ESPECIFICA_NOMBRE = "";

                itemN.ESPECIFICACION = "";

                itemN.RESULTADO = "";

                itemN.ID_CERTIFICADO = 0;

                itemN.CERTIFICADO = "";
                itemN.PRESENTACION = "";

                itemN.CLIENTE = "";
                LISTA.Add(itemN);
                rd.SetDataSource(LISTA);
                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();


                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", nombre + ".pdf");

            }

            



        }


    }
}