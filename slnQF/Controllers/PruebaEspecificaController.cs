using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace slnQF.Controllers
{
    public class PruebaEspecificaController : Controller
    {
        // GET: Certificado
        public ActionResult Index()
        {
            ModelState.Clear();
            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            List<SP_CONSULTA_PRUEBA_ESPECIFICAResult> products = db.SP_CONSULTA_PRUEBA_ESPECIFICA()
               .OrderBy(x => x.ID_PRUEBA_ESPECIFICA)
               .ToList<SP_CONSULTA_PRUEBA_ESPECIFICAResult>();
            return View(products);
        }
        public PartialViewResult Create()
        {
            return PartialView(new slnQF.Models.PruebaEspecificaModels());
        }
        [HttpPost]
        public JsonResult Create(slnQF.CAT_PRUEBA_ESPECIFICA product)
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
                    db.CAT_PRUEBA_ESPECIFICAs.InsertOnSubmit(product);
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
        [HttpGet]
        public PartialViewResult Edit(int Id)
        {
            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            CAT_PRUEBA_ESPECIFICA product = db.CAT_PRUEBA_ESPECIFICAs.Where(x => x.ID_PRUEBA_ESPECIFICA == Id).FirstOrDefault();
            slnQF.Models.PruebaEspecificaModels prod = new slnQF.Models.PruebaEspecificaModels();
            prod.DESCRIPCION = product.DESCRIPCION;
            prod.ESTADO = Convert.ToInt32(product.ESTADO);
            prod.ID_PRUEBA_ESPECIFICA = Convert.ToInt32(product.ID_PRUEBA_ESPECIFICA);
            ViewBag.CategoriesDDL = new SelectList(db.CAT_ESTADOs, "ESTADO_ID", "DESCRIPCION", prod.ESTADO);


            return PartialView(prod);
        }

        [HttpPost]
        public JsonResult Edit(slnQF.CAT_PRUEBA_ESPECIFICA product)
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
                    CAT_PRUEBA_ESPECIFICA tblProd = db.CAT_PRUEBA_ESPECIFICAs.Where(x => x.ID_PRUEBA_ESPECIFICA == product.ID_PRUEBA_ESPECIFICA)
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