using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace slnQF.Controllers
{
    public class NombreComercialController : Controller
    {
        // GET: Ensayo
        public ActionResult Index()
        {
            ModelState.Clear();
            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            List<SP_CONSULTA_NOMBRE_COMERCIALResult> products = db.SP_CONSULTA_NOMBRE_COMERCIAL()
               .OrderBy(x => x.ID_NOMBRE_COMERCIAL)
               .ToList<SP_CONSULTA_NOMBRE_COMERCIALResult>();
            return View(products);
        }

        public PartialViewResult Create()
        {
            return PartialView(new slnQF.Models.NombreComercialModels());
        }
        [HttpPost]
        public JsonResult Create(slnQF.CAT_NOMBRE_COMERCIAL product)
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
                    db.CAT_NOMBRE_COMERCIALs.InsertOnSubmit(product);
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
            CAT_NOMBRE_COMERCIAL product = db.CAT_NOMBRE_COMERCIALs.Where(x => x.ID_NOMBRE_COMERCIAL == Id).FirstOrDefault();
            slnQF.Models.NombreComercialModels prod = new slnQF.Models.NombreComercialModels();
            prod.DESCRIPCION = product.DESCRIPCION;
            prod.ESTADO = Convert.ToInt32(product.ESTADO);
            prod.ID_NOMBRE_COMERCIAL = Convert.ToInt32(product.ID_NOMBRE_COMERCIAL);
            ViewBag.CategoriesDDL = new SelectList(db.CAT_ESTADOs, "ESTADO_ID", "DESCRIPCION", prod.ESTADO);


            return PartialView(prod);
        }

        [HttpPost]
        public JsonResult Edit(slnQF.CAT_NOMBRE_COMERCIAL product)
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
                    CAT_NOMBRE_COMERCIAL tblProd = db.CAT_NOMBRE_COMERCIALs.Where(x => x.ID_NOMBRE_COMERCIAL == product.ID_NOMBRE_COMERCIAL)
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