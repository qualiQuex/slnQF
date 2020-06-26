using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace slnQF.Controllers
{
    public class UomController : Controller
    {
        public ActionResult Index()
        {
            ModelState.Clear();
            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            List<SP_CONSULTA_UOMResult> products = db.SP_CONSULTA_UOM()
               .OrderBy(x => x.ID_UOM)
               .ToList<SP_CONSULTA_UOMResult>();
            return View(products);
        }
        [HttpGet]
        public PartialViewResult Create()
        {
            return PartialView(new slnQF.Models.UomModels());
        }

        [HttpPost]
        public JsonResult Create(slnQF.CAT_UOM product)
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
                product.ID_USUARIO = Convert.ToInt32(idUsuarioLog);
                try
                {
                    db.Connection.Open();
                    db.Transaction = db.Connection.BeginTransaction();
                    db.CAT_UOM.InsertOnSubmit(product);
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
            return Json(new { ID = error, MENSAJE = errorStr});
        }

        [HttpGet]
        public PartialViewResult Edit(int UomId)
        {
            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            CAT_UOM product = db.CAT_UOM.Where(x => x.ID_UOM == UomId).FirstOrDefault();
            slnQF.Models.UomModels prod = new slnQF.Models.UomModels();
            prod.DESCRIPCION = product.DESCRIPCION;
            prod.ESTADO = Convert.ToInt32(product.ESTADO);
            prod.ID_UOM = UomId;
            ViewBag.CategoriesDDL = new SelectList(db.CAT_ESTADOs, "ESTADO_ID", "DESCRIPCION", prod.ESTADO);

            
            return PartialView(prod);
        }

        [HttpPost]
        public JsonResult Edit(slnQF.CAT_UOM product)
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
                    CAT_UOM tblProd = db.CAT_UOM.Where(x => x.ID_UOM == product.ID_UOM)
                                          .FirstOrDefault();

                    tblProd.DESCRIPCION = product.DESCRIPCION;
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
                error = "1";
                errorStr = "Su sesion ha caducado, ingrese nuevamente";
            }
            return Json(new { ID = error, MENSAJE = errorStr });
        }
    }

   

}
