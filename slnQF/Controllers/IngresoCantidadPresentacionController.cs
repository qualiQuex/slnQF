using slnQF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace slnQF.Controllers
{
    public class IngresoCantidadPresentacionController : Controller
    {
        // GET: IngresoCantidadPresentacion
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult Create(int IDLOTEINGRESO)
        {

            DC_CALIDADDataContext db = new DC_CALIDADDataContext();

            LOTE_INGRESO product = db.LOTE_INGRESOs.Where(x => x.ID_LOTE_INGRESO == IDLOTEINGRESO).FirstOrDefault();

            TPRODUCTO prod = db.TPRODUCTOs.Where(x => x.ID_PRODUCTO == product.ID_PRODUCTO).FirstOrDefault();
            
            ViewBag.ID_LOTE_INGRESO = IDLOTEINGRESO;

            ViewBag.TITULO ="Ingreso:"+ product.NO_INGRESO   + " Producto:"+ prod.NOMBRE;
            List<MyDrop> listaPresnta = new List<MyDrop>();
            try
            {
                List<SP_CONSULTA_PRO_PRE_NOMResult> listaPresentacion = new List<SP_CONSULTA_PRO_PRE_NOMResult>();

                listaPresentacion = db.SP_CONSULTA_PRO_PRE_NOM().Where(x => x.ID_PRODUCTO == product.ID_PRODUCTO).ToList<SP_CONSULTA_PRO_PRE_NOMResult>();


                foreach (SP_CONSULTA_PRO_PRE_NOMResult item in listaPresentacion)
                {
                    MyDrop it = new MyDrop();
                    it.id = item.ID_PRESENTACION;
                    it.value = item.NOMBRE_PRESENTACION;
                    bool alreadyExists = listaPresnta.Any(x => x.id == it.id && x.value == it.value);
                    if (alreadyExists == false)
                    listaPresnta.Add(it);
                }
                listaPresnta = listaPresnta.Distinct().ToList();
            }
            catch {

            }
            ViewBag.PresentacionlDDL = new SelectList(listaPresnta, "id", "value").Distinct(); 

            ViewBag.ID_PRODUCTO = prod.ID_PRODUCTO;

            int presentacion = 0;

            try {
                presentacion = db.SP_CONSULTA_PRO_PRE_NOM().Where(x => x.ID_PRODUCTO == product.ID_PRODUCTO).FirstOrDefault().ID_PRESENTACION;
            } catch {
                presentacion = 0;
            }

            ViewBag.NombreComercial = new SelectList(db.SP_CONSULTA_PRO_PRE_NOM().Where(x => x.ID_PRODUCTO == product.ID_PRODUCTO && x.ID_PRESENTACION == presentacion), "ID_NOMBRE_COMERCIAL", "NOMBRE_COMERCIAL");

            SP_CONSULTA_PRO_PRE_NOMResult NOMBRE_COMERCIAL = db.SP_CONSULTA_PRO_PRE_NOM().Where(x => x.ID_PRODUCTO == product.ID_PRODUCTO).FirstOrDefault();

            try {
                if (NOMBRE_COMERCIAL != null)
                {
                    ViewBag.Cliente = new SelectList(db.SP_CONSULTA_CLIENTE_NOMBRE_COM().Where(x => x.ID_NOMBRE_COMERCIAL == NOMBRE_COMERCIAL.ID_NOMBRE_COMERCIAL), "ID_CLIENTE", "NOMBRE_CLIENTE");
                }
                else {
                    List<MyDropList> lista = new List<MyDropList>();
                    ViewBag.Cliente = new SelectList(lista, "id", "value");
                }
                
            }
            catch{  
                
            }

            int sel_Id_uom = 0;
            try
            {
                sel_Id_uom = prod.ID_UOM;
            }
            catch {
                sel_Id_uom = 0;
            }
            ViewBag.UomDDL = new SelectList(db.CAT_UOM.Where(x => x.ESTADO == 1), "ID_UOM", "DESCRIPCION", sel_Id_uom);

            slnQF.Models.IngresoCantidadPresentacionModels model = new slnQF.Models.IngresoCantidadPresentacionModels();
            model.LOTE = product.NO_LOTE; 
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult Crear(slnQF.INGRESO_CANTIDAD_PRESENTACION product)
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
                
            }

            if (Convert.ToInt32(idUsuarioLog) > 0)
            {
                if (product.ID_CLIENTE == null)
                {
                    product.ID_CLIENTE = 0;

                }
                    if ((product.ID_PRESENTACION == 0) || (product.ID_NOMBRE_COMERCIAL == 0) || (product.ID_UOM == 0) || (product.ID_CLIENTE == 0)) {
                    error = "1";
                    errorStr = "Revise los datos seleccionados. ";
                    return Json(new { ID = error, MENSAJE = errorStr });
                }
                DC_CALIDADDataContext db = new DC_CALIDADDataContext();
                product.ID_USUARIO = Convert.ToInt32(idUsuarioLog);
                product.FECHA_CREACION = DateTime.Now;

                try
                {
                    db.Connection.Open();
                    db.Transaction = db.Connection.BeginTransaction();
                    db.INGRESO_CANTIDAD_PRESENTACIONs.InsertOnSubmit(product);
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

        public List<CatalogoModel.MyDropList> traeComboNombreComercial(int idProducto, int idPresentacion)
        {
            List<CatalogoModel.MyDropList> LISTA = new List<CatalogoModel.MyDropList>();

            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            List<SP_CONSULTA_PRO_PRE_NOMResult> PRUEBA = db.SP_CONSULTA_PRO_PRE_NOM()
               .OrderBy(x => x.NOMBRE_COMERCIAL).Where(x => x.ID_PRESENTACION == idPresentacion && x.ID_PRODUCTO == idProducto )
               .ToList<SP_CONSULTA_PRO_PRE_NOMResult>();

            foreach (SP_CONSULTA_PRO_PRE_NOMResult dr in PRUEBA)
            {
                CatalogoModel.MyDropList fila = new CatalogoModel.MyDropList();
                fila.id = dr.ID_NOMBRE_COMERCIAL.ToString();
                fila.value = dr.NOMBRE_COMERCIAL.ToString();

                LISTA.Add(fila);
            }

            return LISTA;
        }

        public List<CatalogoModel.MyDropList> traeComboCliente(int idNombreComercial)
        {
            List<CatalogoModel.MyDropList> LISTA = new List<CatalogoModel.MyDropList>();

            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            List<SP_CONSULTA_CLIENTE_NOMBRE_COMResult> PRUEBA = db.SP_CONSULTA_CLIENTE_NOMBRE_COM()
               .OrderBy(x => x.NOMBRE_CLIENTE).Where(x => x.ID_NOMBRE_COMERCIAL == idNombreComercial)
               .ToList<SP_CONSULTA_CLIENTE_NOMBRE_COMResult>();

            foreach (SP_CONSULTA_CLIENTE_NOMBRE_COMResult dr in PRUEBA)
            {
                CatalogoModel.MyDropList fila = new CatalogoModel.MyDropList();
                fila.id = dr.ID_CLIENTE.ToString();
                fila.value = dr.NOMBRE_CLIENTE.ToString();

                LISTA.Add(fila);
            }

            return LISTA;
        }

        public JsonResult traeNombreComercial(string id_producto, string id_presentacion)
        {
            List<CatalogoModel.MyDropList> lista = new List<CatalogoModel.MyDropList>();



            lista = traeComboNombreComercial(Convert.ToInt32(id_producto), Convert.ToInt32(id_presentacion));


            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        public JsonResult traeClienteJson(string id_nombre_comercial)
        {
            List<CatalogoModel.MyDropList> lista = new List<CatalogoModel.MyDropList>();



            lista = traeComboCliente(Convert.ToInt32(id_nombre_comercial));


            return Json(lista, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult fnCambioEstadoCanInPre(string id)
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
            int v_ingreso_cant_id = 0; 
            try
            {
                v_ingreso_cant_id = Convert.ToInt32(id);

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
                    INGRESO_CANTIDAD_PRESENTACION tblProd = db.INGRESO_CANTIDAD_PRESENTACIONs.Where(x => x.ID_INGRESO_CANTIDAD_PRESENTACION == v_ingreso_cant_id)
                                           .FirstOrDefault();

                    if(tblProd.ESTADO == 1)
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

        public PartialViewResult Listado(int id)
        {
            ModelState.Clear();
            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            List<SP_CONSULTA_INGRESO_CANTIDAD_PRESENTACIONResult> PRUEBA = db.SP_CONSULTA_INGRESO_CANTIDAD_PRESENTACION()
               .Where(x => x.ID_LOTE_INGRESO== id)
               .ToList<SP_CONSULTA_INGRESO_CANTIDAD_PRESENTACIONResult>();
            return PartialView("~/Views/IngresoCantidadPresentacion/Listado.cshtml", PRUEBA);
        }
    }
}