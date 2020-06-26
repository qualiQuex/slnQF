
using Newtonsoft.Json;
using slnQF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace slnQF.Controllers
{
    public class ResultadoEnsayoProductoController : Controller
    {
        // GET: ResultadoEnsayoProducto
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult Listado(int id)
        {
            ModelState.Clear();
            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            List<SP_CONSULTA_RESULTADO_ENSAYO_PRODResult> PRUEBA = db.SP_CONSULTA_RESULTADO_ENSAYO_PROD()
               .OrderBy(x => x.ID_PRODUCTO).Where(x => x.ID_LOTE_INGRESO == id)
               .ToList<SP_CONSULTA_RESULTADO_ENSAYO_PRODResult>();
            return PartialView("~/Views/ResultadoEnsayoProducto/Listado.cshtml", PRUEBA);
        }

        public PartialViewResult Listado_Muestra(int id)
        {
            ModelState.Clear();

            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            
            ViewBag.TecnicoDDL = new SelectList(db.SP_CONSULTA_TECNICO().Where(x => x.ESTADO_ID == 1).OrderBy(x => x.NOMBRE_TECNICO), "ID_TECNICO", "NOMBRE_TECNICO");

            ViewBag.RevisaDDL = new SelectList(db.SP_CONSULTA_TECNICO().Where(x => x.ESTADO_ID == 1).OrderBy(x => x.NOMBRE_TECNICO), "ID_TECNICO", "NOMBRE_TECNICO");

            SP_CONSULTA_LOTE_INGRESOResult REG = db.SP_CONSULTA_LOTE_INGRESO().Where(x => x.ID_LOTE_INGRESO == id).SingleOrDefault();

            ViewBag.NO_INGRESO = REG.NO_INGRESO;
            ViewBag.ID_LOTE_INGRESO = REG.ID_LOTE_INGRESO;

            List<SP_CONSULTA_PRUEBA_RESULTADOResult> PRUEBA = db.SP_CONSULTA_PRUEBA_RESULTADO()
               .OrderBy(x => x.ID_PRODUCTO).Where(x => x.ID_PRODUCTO == REG.ID_PRODUCTO && x.ID_LOTE_INGRESO == REG.ID_LOTE_INGRESO)
               .ToList<SP_CONSULTA_PRUEBA_RESULTADOResult>();
            return PartialView("~/Views/ResultadoEnsayoProducto/Listado_Muestra.cshtml", PRUEBA);
        }

        public PartialViewResult Historial(int id)
        {
            ModelState.Clear();

            DC_CALIDADDataContext db = new DC_CALIDADDataContext();



            List<SP_CONSULTA_RESULTADO_BITACORAResult> REG = db.SP_CONSULTA_RESULTADO_BITACORA().Where(x => x.ID_RESULTADO_ENSAYO_PROD == id).OrderBy(x => x.FECHA_CREACION).ToList<SP_CONSULTA_RESULTADO_BITACORAResult>();
  
              
            return PartialView("~/Views/ResultadoEnsayoProducto/Historial.cshtml", REG);
        }

        public PartialViewResult Create(int ID, int ID2)
        {

            DC_CALIDADDataContext db = new DC_CALIDADDataContext();

            ViewBag.ID_PRODUCTO = ID;
            ViewBag.ID_LOTE_INGRESO = ID2;


            ViewBag.EnsayoDDL = new SelectList(traeComboEnsayos(ID).ToList(), "id","value");

            ViewBag.PruebaDDL = new SelectList(db.SP_CONSULTA_ENSAYO_PRUEBA().OrderBy(x => x.NOMBRE_PRUEBA), "ID_PRUEBA", "NOMBRE_PRUEBA").Distinct();

            ViewBag.PruebaEspecificaDDL = new SelectList(db.SP_CONSULTA_PRUEBA_ESPECIFICA().OrderBy(x => x.DESCRIPCION), "ID_PRUEBA_ESPECIFICA", "DESCRIPCION").Distinct();

            ViewBag.TecnicoDDL = new SelectList(db.SP_CONSULTA_TECNICO().Where(x => x.ESTADO_ID == 1).OrderBy(x => x.NOMBRE_TECNICO), "ID_TECNICO", "NOMBRE_TECNICO");

            ViewBag.RevisaDDL = new SelectList(db.SP_CONSULTA_TECNICO().Where(x => x.ESTADO_ID == 1).OrderBy(x => x.NOMBRE_TECNICO), "ID_TECNICO", "NOMBRE_TECNICO");

            return PartialView(new slnQF.Models.ResultadoEnsayoProductoModels());
        }

        [HttpPost]
        public JsonResult CreaReg(slnQF.RESULTADO_ENSAYO_PRODUCTO product)
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

            if (Convert.ToInt32(idUsuarioLog) > 0)
            {
               
                if (product.ID_PRUEBA_ESPECIFICA == 0)
                    product.ID_PRUEBA_ESPECIFICA = 0;
                product.ID_ESTADO = 1;
                product.FECHA_CREACION = DateTime.Now;
                product.ID_USUARIO = Convert.ToInt32(idUsuarioLog);


                DC_CALIDADDataContext db = new DC_CALIDADDataContext();
                try
                {
                    db.Connection.Open();
                    db.Transaction = db.Connection.BeginTransaction();
                    db.RESULTADO_ENSAYO_PRODUCTOs.InsertOnSubmit(product);
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
                errorStr = "Su sesion ha caducado, ingrese nuevamente";
                error = "1";
            }
            return Json(new { ID = error, MENSAJE = errorStr });
        }

        public JsonResult CreaRegs(string json)
        {
            string error = "2";
            string errorStr = "";
            string idUsuarioLog = "-1";
            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            List<ResultadoEnsayoProductoModels> listResultados = new List<ResultadoEnsayoProductoModels>();
            ResultadoEnsayoProductoModels itemResultado;
            dynamic dynJsonTara = JsonConvert.DeserializeObject(json);
            try {
                foreach (var item in dynJsonTara)
                {
                    if (item != null)
                    {
                        itemResultado = new ResultadoEnsayoProductoModels();
                        itemResultado.ID_ENSAYO = Convert.ToInt32(item.ID_ENSAYO);
                        itemResultado.ID_PRUEBA = Convert.ToInt32(item.ID_PRUEBA);
                        itemResultado.ID_PRUEBA_ESPECIFICA = Convert.ToInt32(item.ID_PRUEBA_ESPECIFICA);
                        itemResultado.ID_LOTE_INGRESO = Convert.ToInt32(item.ID_LOTE_INGRESO);
                        itemResultado.RESULTADO = item.RESULTADO.ToString();
                        itemResultado.ID_RESULTADO_ENSAYO_PROD = Convert.ToInt32(item.ID_RESULTADO_ENSAYO_PROD);
                        //staTaraSeleccionados.Add(itemTaraSel);
                        listResultados.Add(itemResultado);
                    }
                }
            }
            catch (Exception es) {
                errorStr = "Error el lectura de datos de resultados." + es.Message;
                error = "1";
            }

            if (listResultados.Count > 0)
            {
                try
                {
                    idUsuarioLog = Session["LogedUserID"].ToString();
                }
                catch (Exception Ex)
                {
                    errorStr = "Su sesion ha caducado, ingrese nuevamente";
                    error = "1";
                }

                if (Convert.ToInt32(idUsuarioLog) > 0)
                {
                    int v_nuevo = 0;
                    foreach (ResultadoEnsayoProductoModels itemModificado in listResultados) {
                        v_nuevo = 0;
                        RESULTADO_BITACORA log = new RESULTADO_BITACORA();
                        if (itemModificado.ID_RESULTADO_ENSAYO_PROD <= 0)
                        {
                            if (itemModificado.RESULTADO.Length > 0) {
                                
                                db = new DC_CALIDADDataContext();
                                //es nuevo registro
                                RESULTADO_ENSAYO_PRODUCTO itemNuevo= new RESULTADO_ENSAYO_PRODUCTO();
                                itemNuevo.ID_ENSAYO = Convert.ToInt32(itemModificado.ID_ENSAYO);
                                itemNuevo.ID_PRUEBA = Convert.ToInt32(itemModificado.ID_PRUEBA);
                                itemNuevo.ID_PRUEBA_ESPECIFICA = Convert.ToInt32(itemModificado.ID_PRUEBA_ESPECIFICA);
                                itemNuevo.ID_LOTE_INGRESO = Convert.ToInt32(itemModificado.ID_LOTE_INGRESO);
                                itemNuevo.RESULTADO = itemModificado.RESULTADO.ToString();
                                if (itemNuevo.ID_PRUEBA_ESPECIFICA <= 0)
                                    itemNuevo.ID_PRUEBA_ESPECIFICA = 0;
                                itemNuevo.ID_ESTADO = 1;
                                itemNuevo.FECHA_CREACION = DateTime.Now;
                                itemNuevo.ID_USUARIO = Convert.ToInt32(idUsuarioLog);



                                try
                                {
                                    db.Connection.Open();
                                    db.Transaction = db.Connection.BeginTransaction();
                                    db.RESULTADO_ENSAYO_PRODUCTOs.InsertOnSubmit(itemNuevo);
                                    db.SubmitChanges();
                                    db.Transaction.Commit();
                                    v_nuevo = 1;
                                    error = "0";
                                    errorStr = "Informacion guardada con exito";
                                    db.Connection.Close();
                                    db.Dispose();
                                    log.ID_ENSAYO = itemNuevo.ID_ENSAYO;
                                    log.ID_PRUEBA = itemNuevo.ID_PRUEBA;
                                    log.ID_PRUEBA_ESPECIFICA = itemNuevo.ID_PRUEBA_ESPECIFICA;
                                    log.ID_LOTE_INGRESO = itemNuevo.ID_LOTE_INGRESO;
                                    log.RESULTADO = itemNuevo.RESULTADO;
                                    log.ID_PRUEBA_ESPECIFICA = itemNuevo.ID_PRUEBA_ESPECIFICA;
                                    log.FECHA_CREACION = DateTime.Now;
                                    log.ID_USUARIO = Convert.ToInt32(idUsuarioLog);
                                    log.ID_RESULTADO_ENSAYO_PROD = itemNuevo.ID_RESULTADO_ENSAYO_PROD;

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
                            }
                            else {
                                
                            try
                            {
                                db = new DC_CALIDADDataContext();
                                RESULTADO_ENSAYO_PRODUCTO product = db.RESULTADO_ENSAYO_PRODUCTOs.Where(x => x.ID_RESULTADO_ENSAYO_PROD == itemModificado.ID_RESULTADO_ENSAYO_PROD).FirstOrDefault();

                                
                                
                                db.Connection.Open();
                                db.Transaction = db.Connection.BeginTransaction();
                                //ya existe solo se actualiza
                                if (product.RESULTADO != itemModificado.RESULTADO)
                                { 
                                    product.RESULTADO = itemModificado.RESULTADO;
                                
                            
                                    db.SubmitChanges();
                                    db.Transaction.Commit();
                                    v_nuevo = 2;
                                    error = "0";
                                    errorStr = "Informacion actualizada con exito";
                                    log.ID_ENSAYO = product.ID_ENSAYO;
                                    log.ID_PRUEBA = product.ID_PRUEBA;
                                    log.ID_PRUEBA_ESPECIFICA = product.ID_PRUEBA_ESPECIFICA;
                                    log.ID_LOTE_INGRESO = product.ID_LOTE_INGRESO;
                                    log.RESULTADO = product.RESULTADO;
                                    log.ID_PRUEBA_ESPECIFICA = product.ID_PRUEBA_ESPECIFICA;
                                    log.FECHA_CREACION = DateTime.Now;
                                    log.ID_USUARIO = Convert.ToInt32(idUsuarioLog);
                                    log.ID_RESULTADO_ENSAYO_PROD = product.ID_RESULTADO_ENSAYO_PROD;
                                    db.Connection.Close();
                                    db.Dispose();
                                }
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
                        if ((v_nuevo == 1) || (v_nuevo == 2)) { 
                            DC_CALIDADDataContext db2 = new DC_CALIDADDataContext();
                            try
                            {
                                db2.Connection.Open();
                                db2.Transaction = db2.Connection.BeginTransaction();
                                db2.RESULTADO_BITACORA.InsertOnSubmit(log);
                                db2.SubmitChanges();
                                db2.Transaction.Commit();
                                db2.Connection.Close();
                                db2.Dispose();
                            }
                            catch (Exception e)
                            {
                                db2.Transaction.Rollback();
                                db2.Connection.Close();
                                db2.Dispose();

                            }
                        }

                    }
                }
                else
                {
                    errorStr = "Su sesion ha caducado, ingrese nuevamente";
                    error = "1";
                }
            }
            else {
                errorStr = "No hay datos en la tabla de respuestas.";
                error = "1";
            }
            return Json(new { ID = error, MENSAJE = errorStr });
        }

        public List<CatalogoModel.MyDropList> traeComboEnsayos(int idProducto)
        {
            List<CatalogoModel.MyDropList> LISTA = new List<CatalogoModel.MyDropList>();

            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            List<SP_CONSULTA_ESPECIFICACION_PRODUCTOResult> PRUEBA = db.SP_CONSULTA_ESPECIFICACION_PRODUCTO()
               .OrderBy(x => x.NOMBRE_ENSAYO).Where(x => x.ID_PRODUCTO == idProducto).Distinct()
               .ToList<SP_CONSULTA_ESPECIFICACION_PRODUCTOResult>();
            foreach (SP_CONSULTA_ESPECIFICACION_PRODUCTOResult dr in PRUEBA)
            {
                CatalogoModel.MyDropList fila = new CatalogoModel.MyDropList();
                fila.id = dr.ID_ENSAYO.ToString();
                fila.value = dr.NOMBRE_ENSAYO.ToString();
                bool alreadyExists = LISTA.Any(x => x.id == dr.ID_ENSAYO.ToString() && x.value == dr.NOMBRE_ENSAYO.ToString());
                if (alreadyExists == false)
                LISTA.Add(fila);
            }

            return LISTA;
        }


        public List<CatalogoModel.MyDropList> traeComboPruebas(int idProducto, int id_Ensayo)
        {
            List<CatalogoModel.MyDropList> LISTA = new List<CatalogoModel.MyDropList>();

            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            List<SP_CONSULTA_ESPECIFICACION_PRODUCTOResult> PRUEBA = db.SP_CONSULTA_ESPECIFICACION_PRODUCTO()
               .OrderBy(x => x.NOMBRE_PRUEBA).Where(x => x.ID_ENSAYO == id_Ensayo && x.ID_PRODUCTO == idProducto)
               .ToList<SP_CONSULTA_ESPECIFICACION_PRODUCTOResult>();

            foreach (SP_CONSULTA_ESPECIFICACION_PRODUCTOResult dr in PRUEBA)
            {
                CatalogoModel.MyDropList fila = new CatalogoModel.MyDropList();
                fila.id = dr.ID_PRUEBA.ToString();
                fila.value = dr.NOMBRE_PRUEBA.ToString();
                bool alreadyExists = LISTA.Any(x => x.id == dr.ID_PRUEBA.ToString() && x.value == dr.NOMBRE_PRUEBA.ToString());
                if (alreadyExists == false)
                    LISTA.Add(fila);
            }

            return LISTA;
        }

        public List<CatalogoModel.MyDropList> traeComboPruebasEspecificas(int id_produto,int idCombo, int idCombo2)
        {
            List<CatalogoModel.MyDropList> LISTA = new List<CatalogoModel.MyDropList>();

            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            List<SP_CONSULTA_ESPECIFICACION_PRODUCTOResult> PRUEBA = db.SP_CONSULTA_ESPECIFICACION_PRODUCTO()
               .OrderBy(x => x.NOMBRE_PRUEBA).Where(x => x.ID_ENSAYO == idCombo && x.ID_PRODUCTO == id_produto && x.ID_PRUEBA== idCombo2)
               .ToList<SP_CONSULTA_ESPECIFICACION_PRODUCTOResult>();

            foreach (SP_CONSULTA_ESPECIFICACION_PRODUCTOResult dr in PRUEBA)
            { 
                CatalogoModel.MyDropList fila = new CatalogoModel.MyDropList();
                fila.id = dr.ID_PRUEBA_ESPECIFICA.ToString();
                fila.value = dr.NOMBRE_PRUEBA_ESPECIFICA.ToString();
                bool alreadyExists = LISTA.Any(x => x.id == dr.ID_PRUEBA_ESPECIFICA.ToString() && x.value == dr.NOMBRE_PRUEBA_ESPECIFICA.ToString());
                if (alreadyExists == false)
                    LISTA.Add(fila);
            }

            if (LISTA.Count == 0)
            {
                CatalogoModel.MyDropList fila = new CatalogoModel.MyDropList();
                fila.id = "-1";
                fila.value = "SIN PRUEBA ESPECIFICA";

                LISTA.Add(fila);
            }

            return LISTA;
        }

        public JsonResult traeEnsayos(string id_producto)
        {
            List<CatalogoModel.MyDropList> lista = new List<CatalogoModel.MyDropList>();


            lista = traeComboEnsayos(Convert.ToInt32(id_producto));


            return Json(lista, JsonRequestBehavior.AllowGet);
        }


        public JsonResult traePruebas(string id_producto,string id_ensayo)
        {
            List<CatalogoModel.MyDropList> lista = new List<CatalogoModel.MyDropList>();



            lista = traeComboPruebas(Convert.ToInt32(id_producto),Convert.ToInt32(id_ensayo));


            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        public JsonResult traePruebasEspecificas(string id_producto, string id_ensayo, string id_prueba)
        {
            List<CatalogoModel.MyDropList> lista = new List<CatalogoModel.MyDropList>();

            if (id_prueba.ToString().Length <= 0) {
                id_prueba = "-1";
            }

            lista = traeComboPruebasEspecificas(Convert.ToInt32(id_producto), Convert.ToInt32(id_ensayo), Convert.ToInt32(id_prueba));


            return Json(lista, JsonRequestBehavior.AllowGet);
        }
    }
}