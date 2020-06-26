using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace slnQF.Controllers
{
    public class EtapaFabricacionController : Controller
    {
        public ActionResult Index()
        {
            ModelState.Clear();
            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            List<SP_CONSULTA_ETAPA_FABRICACIONResult> products = db.SP_CONSULTA_ETAPA_FABRICACION()
               .OrderBy(x => x.ID_ETAPA_FABRICACION)
               .ToList<SP_CONSULTA_ETAPA_FABRICACIONResult>();
            return View(products);
        }

        public PartialViewResult Create()
        {
            return PartialView(new slnQF.Models.EtiquetaFabricacionModels());
        }

        [HttpPost]
        public JsonResult Create(slnQF.CAT_ETAPA_FABRICACION product)
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
                    db.CAT_ETAPA_FABRICACIONs.InsertOnSubmit(product);
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
        public PartialViewResult Edit(int CertId)
        {

            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            CAT_ETAPA_FABRICACION product = db.CAT_ETAPA_FABRICACIONs.Where(x => x.ID_ETAPA_FABRICACION == CertId).FirstOrDefault();
            slnQF.Models.EtiquetaFabricacionModels prod = new slnQF.Models.EtiquetaFabricacionModels();
            prod.DESCRIPCION = product.DESCRIPCION;
            prod.ESTADO = Convert.ToInt32(product.ESTADO);
            prod.ID_ETAPA_FABRICACION = Convert.ToInt32(product.ID_ETAPA_FABRICACION);
            ViewBag.CategoriesDDL = new SelectList(db.CAT_ESTADOs, "ESTADO_ID", "DESCRIPCION", prod.ESTADO);


            return PartialView(prod);
        }

        [HttpPost]
        public JsonResult Edit(slnQF.CAT_ETAPA_FABRICACION product)
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
                    CAT_ETAPA_FABRICACION tblProd = db.CAT_ETAPA_FABRICACIONs.Where(x => x.ID_ETAPA_FABRICACION == product.ID_ETAPA_FABRICACION)
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