using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace slnQF.Controllers
{
    public class ProductoController : Controller
    {

        // GET: Forma_Farmaceutica
        public ActionResult Index()
        {
            ModelState.Clear();
            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            List<SP_CONSULTA_PRODUCTOResult> ffroma = db.SP_CONSULTA_PRODUCTO()
               .OrderBy(x => x.ID_PRODUCTO)
               .ToList<SP_CONSULTA_PRODUCTOResult>();
            return View(ffroma);
        }

        public PartialViewResult Create()
        {
            
            DC_CALIDADDataContext db = new DC_CALIDADDataContext();

            ViewBag.FFARMACEUTICADDL = new SelectList(db.CAT_FORMA_FARMACEUTICAs.OrderBy(x=>x.NOMBRE), "ID_FORMA_FARMACEUTICA", "NOMBRE");
            //ViewBag.CONCENTRACIONDDL = new SelectList(db.CAT_CONCENTRACIONs, "ID_CONCENTRACION", "NOMBRE");
            ViewBag.UOMDDL = new SelectList(db.CAT_UOM, "ID_UOM", "DESCRIPCION");


            return PartialView(new slnQF.Models.ProductoModels());
        }
   
     

        [HttpPost]
        public JsonResult Create(slnQF.TPRODUCTO product)
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
                DC_CALIDADDataContext db = new DC_CALIDADDataContext();
                product.FECHA_CREACION = DateTime.Now;
                product.ESTADO = 1;
                product.ID_USUARIO = Convert.ToInt32(idUsuarioLog);
                try
                {
                    db.Connection.Open();
                    db.Transaction = db.Connection.BeginTransaction();
                    db.TPRODUCTOs.InsertOnSubmit(product);
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
        [HttpGet]
        public PartialViewResult Edit(int Id)
        {
            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            TPRODUCTO product = db.TPRODUCTOs.Where(x => x.ID_PRODUCTO == Id).FirstOrDefault();
            slnQF.Models.ProductoModels prod = new slnQF.Models.ProductoModels();
            prod.NOMBRE = product.NOMBRE;
            prod.DESCRIPCION = product.DESCRIPCION; 
            
            prod.ESTADO = Convert.ToInt32(product.ESTADO);
            prod.ID_PRODUCTO = Convert.ToInt32(product.ID_PRODUCTO);
            prod.ID_FORMA_FARMACEUTICA = Convert.ToInt32(product.ID_FORMA_FARMACEUTICA);
            prod.CONCENTRACION = product.CONCENTRACION;
            prod.ID_UOM = Convert.ToInt32(product.ID_UOM);
            prod.CANTIDAD_MUESTRAS = Convert.ToInt32(product.CANTIDAD_MUESTRAS);
            prod.REFERENCIA = product.REFERENCIA;
            prod.CONDICION_ALMACENA = product.CONDICION_ALMACENA;
            ViewBag.CategoriesDDL = new SelectList(db.CAT_ESTADOs, "ESTADO_ID", "DESCRIPCION", prod.ESTADO);
            ViewBag.FFARMACEUTICADDL = new SelectList(db.CAT_FORMA_FARMACEUTICAs, "ID_FORMA_FARMACEUTICA", "NOMBRE", prod.ID_FORMA_FARMACEUTICA);
            //ViewBag.CONCENTRACIONDDL = new SelectList(db.CAT_CONCENTRACIONs, "ID_CONCENTRACION", "NOMBRE", prod.ID_CONCENTRACION);
            ViewBag.UOMDDL = new SelectList(db.CAT_UOM, "ID_UOM", "DESCRIPCION", prod.ID_UOM);


            return PartialView(prod);
        }

        [HttpPost]
        public JsonResult Edit(slnQF.TPRODUCTO product)
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
                    TPRODUCTO tblProd = db.TPRODUCTOs.Where(x => x.ID_PRODUCTO == product.ID_PRODUCTO)
                                           .FirstOrDefault();

                    tblProd.DESCRIPCION = product.DESCRIPCION;
                    tblProd.NOMBRE = product.NOMBRE;
                    tblProd.CANTIDAD_MUESTRAS = product.CANTIDAD_MUESTRAS;
                    tblProd.ID_FORMA_FARMACEUTICA = product.ID_FORMA_FARMACEUTICA;
                    tblProd.CONCENTRACION = product.CONCENTRACION;
                    tblProd.ID_UOM = product.ID_UOM;
                    tblProd.ESTADO = product.ESTADO;
                    tblProd.REFERENCIA = product.REFERENCIA;
                    if (product.CONDICION_ALMACENA == null) 
                    { 
                    tblProd.CONDICION_ALMACENA = "";
                    }else{

                        tblProd.CONDICION_ALMACENA = product.CONDICION_ALMACENA;
                    }

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
                error = "1";
                errorStr = "Su sesion ha caducado, ingrese nuevamente";
            }
            return Json(new { ID = error, MENSAJE = errorStr });

        }

    }
}