using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace slnQF.Controllers
{
    public class ProductoPresentacionController : Controller
    {
        // GET: EnsayoPrueba
        public ActionResult Index()
        {
            ModelState.Clear();
            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            List<SP_CONSULTA_PRODUCTO_PRESENTACIONResult> PRUEBA = db.SP_CONSULTA_PRODUCTO_PRESENTACION()
               .OrderBy(x => x.ID_PRODUCTO).OrderBy(x => x.ID_PRESENTACION)
               .ToList<SP_CONSULTA_PRODUCTO_PRESENTACIONResult>();
            return View(PRUEBA);
        }

        public PartialViewResult Listado(int id)
        {
            ModelState.Clear();
            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            List<SP_CONSULTA_PRODUCTO_PRESENTACIONResult> PRUEBA = db.SP_CONSULTA_PRODUCTO_PRESENTACION()
               .OrderBy(x => x.ID_PRODUCTO).OrderBy(x => x.ID_PRESENTACION).Where(x => x.ID_PRODUCTO == id)
               .ToList<SP_CONSULTA_PRODUCTO_PRESENTACIONResult>();
            return PartialView("~/Views/ProductoPresentacion/Listado.cshtml", PRUEBA);
        }
        public PartialViewResult Create(int Id)
        {

            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            ViewBag.ID_PRODUCTO = Id;

            ViewBag.PresentacionDDL = new SelectList(db.SP_CONSULTA_PRESENTACION().OrderBy(x => x.NOMBRE), "ID_PRESENTACION", "NOMBRE");



            return PartialView(new slnQF.Models.ProductoPresentacionModels());
        }

        [HttpPost]
        public JsonResult Create(slnQF.CAT_PRODUCTO_PRESENTACION product)
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
                product.FECHA_CREACION = DateTime.Now;
                product.ESTADO = 1;
                product.ID_USUARIO = Convert.ToInt32(idUsuarioLog);
                try
                {
                    db.Connection.Open();
                    db.Transaction = db.Connection.BeginTransaction();
                    db.CAT_PRODUCTO_PRESENTACIONs.InsertOnSubmit(product);
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

        public PartialViewResult Edit(int Id, int Id2)
        {
            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            CAT_PRODUCTO_PRESENTACION product = db.CAT_PRODUCTO_PRESENTACIONs.Where(x => x.ID_PRODUCTO == Id && x.ID_PRESENTACION == Id2).FirstOrDefault();
            slnQF.Models.ProductoPresentacionModels prod = new slnQF.Models.ProductoPresentacionModels();
            prod.ID_PRODUCTO = product.ID_PRODUCTO;
            prod.ID_PRESENTACION = product.ID_PRESENTACION;
            prod.ESTADO = Convert.ToInt32(product.ESTADO);

            ViewBag.CategoriesDDL = new SelectList(db.CAT_ESTADOs, "ESTADO_ID", "DESCRIPCION", prod.ESTADO);


            ViewBag.ProductoDDL = new SelectList(db.TPRODUCTOs, "ID_PRODUCTO", "NOMBRE", product.ID_PRODUCTO);

            ViewBag.PresentacionDDL = new SelectList(db.SP_CONSULTA_PRESENTACION().OrderBy(x => x.NOMBRE), "ID_PRESENTACION", "NOMBRE", product.ID_PRESENTACION);



            return PartialView(prod);
        }

        [HttpPost]
        public JsonResult Edit(slnQF.CAT_PRODUCTO_PRESENTACION product)
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
                    CAT_PRODUCTO_PRESENTACION tblProd = db.CAT_PRODUCTO_PRESENTACIONs.Where(x => x.ID_PRODUCTO == product.ID_PRODUCTO && x.ID_PRESENTACION == product.ID_PRESENTACION).FirstOrDefault();

                    tblProd.ID_PRODUCTO = product.ID_PRODUCTO;
                    tblProd.ID_PRESENTACION = product.ID_PRESENTACION;
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
            else
            {
                errorStr = "Su sesion ha caducado, ingrese nuevamente";
                error = "1";
            }
            return Json(new { ID = error, MENSAJE = errorStr });

        }
    }
}