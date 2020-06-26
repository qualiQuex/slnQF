using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace slnQF.Controllers
{
    public class ItemEmbalajeController : Controller
    {
        // GET: ItemEmbalaje
        public ActionResult Index()
        {
            ModelState.Clear();
            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            List<SP_CONSULTA_ITEM_EMBALAJEResult> products = db.SP_CONSULTA_ITEM_EMBALAJE() 
               .ToList<SP_CONSULTA_ITEM_EMBALAJEResult>();
            return View(products);
        }
        public PartialViewResult Create(string id)
        {
            string idUsuarioLog = "-1";
            int intIdUsuarioLog=0;
            try
            {
                idUsuarioLog = Session["LogedUserID"].ToString();
                intIdUsuarioLog = Convert.ToInt32(idUsuarioLog);
            }
            catch (Exception Ex)
            {
                intIdUsuarioLog=0;
            }
            ViewBag.ITEMCODE = id;
            TITEM_EMBALAJE ARTICULO = new TITEM_EMBALAJE();
            ARTICULO.ITEMCODE = id;
            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            SP_CONSULTA_SAP_ARTICULO_DATA_USERResult dat = db.SP_CONSULTA_SAP_ARTICULO_DATA_USER("T", intIdUsuarioLog).Where(x => x.ItemCode == id).FirstOrDefault();

            ViewBag.ItemEmbalajeDDL = new SelectList(db.SP_CONSULTA_SAP_ARTICULO_DATA_EMB(intIdUsuarioLog).OrderBy(x => x.ItemName), "ItemCode", "ItemName");


            ViewBag.ITEMNAME = "[" + id + "]" + dat.ItemName;
            return PartialView(ARTICULO);
        }
        public PartialViewResult Listado(string id)
        {
            ModelState.Clear();
            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            List<TITEM_EMBALAJE> PRUEBA = db.TITEM_EMBALAJEs.Where(x => x.ITEMCODE == id).ToList<TITEM_EMBALAJE>();

            return PartialView("~/Views/ItemEmbalaje/Listado.cshtml", PRUEBA);
        }
        public PartialViewResult ListadoListadoItemEmbalaje(string id, string cantidad)
        {
            ModelState.Clear();
            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            List<SP_CONSULTA_SAP_ARTICULO_EMBALAJE_DATAResult> PRUEBA = db.SP_CONSULTA_SAP_ARTICULO_EMBALAJE_DATA(id).ToList<SP_CONSULTA_SAP_ARTICULO_EMBALAJE_DATAResult>();
            if (PRUEBA.Count > 0) {
                PRUEBA[0].DATO_INGRESADO = cantidad;
            }
            return PartialView("~/Views/ItemEmbalaje/ListadoItemEmbalaje.cshtml", PRUEBA);
        }

        [HttpPost]
        public JsonResult Create(slnQF.TITEM_EMBALAJE product)
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
                    db.TITEM_EMBALAJEs.InsertOnSubmit(product);
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


        public PartialViewResult Edit(int Id)
        {

            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            TITEM_EMBALAJE product = db.TITEM_EMBALAJEs.Where(x => x.ID_ITEM_EMBALAJE == Id).FirstOrDefault();
            
            ViewBag.CategoriesDDL = new SelectList(db.CAT_ESTADOs, "ESTADO_ID", "DESCRIPCION", product.ESTADO);

            return PartialView(product);
        }
            [HttpPost]
            public JsonResult Edit(slnQF.TITEM_EMBALAJE product)
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
                    try
                    {
                        db.Connection.Open();
                        db.Transaction = db.Connection.BeginTransaction();
                        TITEM_EMBALAJE tblProd = db.TITEM_EMBALAJEs.Where(x => x.ID_ITEM_EMBALAJE == product.ID_ITEM_EMBALAJE)
                                               .FirstOrDefault();

                        tblProd.DESCRIPCION = product.DESCRIPCION;
                        tblProd.CODIGO = product.CODIGO;
                        tblProd.CAPACIDAD = product.CAPACIDAD;
                        tblProd.ESTADO = product.ESTADO;

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
                else{
                    error = "1";
                    errorStr = "Su sesion ha caducado, ingrese nuevamente";
                }
                return Json(new { ID = error, MENSAJE = errorStr });

            }
        }
}