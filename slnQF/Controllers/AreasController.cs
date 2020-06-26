using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace slnQF.Controllers
{
    public class AreasController : Controller
    {
        // GET: Areas
        public ActionResult Index()
        {
            ModelState.Clear();
            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            List<SP_CONSULTA_AREAResult> areas = db.SP_CONSULTA_AREA()
               .OrderBy(x => x.ID_AREA)
               .ToList<SP_CONSULTA_AREAResult>();
            return View(areas);

        }

        public PartialViewResult Create()
        {


            return PartialView(new CAT_AREA());
        }

        public PartialViewResult EditArea(int Id)
        {
            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            CAT_AREA product = db.CAT_AREAs.Where(x => x.ID_AREA == Id).FirstOrDefault();
            CAT_AREA prod = new CAT_AREA();
            
            prod.DESCRIPCION = product.DESCRIPCION;
            prod.ID_ESTADO = Convert.ToInt32(product.ID_ESTADO);
            prod.ID_AREA = Convert.ToInt32(product.ID_AREA);
            ViewBag.CategoriesDDL = new SelectList(db.CAT_ESTADOs, "ESTADO_ID", "DESCRIPCION", prod.ID_ESTADO);


            return PartialView(prod);
        }
        [HttpPost]
        public JsonResult Create(slnQF.CAT_AREA product)
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
                product.ID_ESTADO = 1;
                product.ID_USUARIO = Convert.ToInt32(idUsuarioLog);
                try
                {
                    db.Connection.Open();
                    db.Transaction = db.Connection.BeginTransaction();
                    db.CAT_AREAs.InsertOnSubmit(product);
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

        [HttpPost]
        public JsonResult EditArea(slnQF.CAT_AREA product)
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
                    CAT_AREA tblProd = db.CAT_AREAs.Where(x => x.ID_AREA == product.ID_AREA)
                                           .FirstOrDefault();

                    tblProd.DESCRIPCION = product.DESCRIPCION;
                   
                    tblProd.ID_ESTADO = product.ID_ESTADO;

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