using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace slnQF.Controllers
{
    public class ClienteController : Controller
    {
        // GET: Cliente
        public ActionResult Index()
        {
            ModelState.Clear();
            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            List<SP_CONSULTA_CLIENTEResult> products = db.SP_CONSULTA_CLIENTE()
               .OrderBy(x => x.NOMBRE)
               .ToList<SP_CONSULTA_CLIENTEResult>();
            return View(products);
        }
        public PartialViewResult Create()
        {
            return PartialView(new slnQF.Models.ClienteModels());
        }

        [HttpPost]
        public JsonResult Create(slnQF.CAT_CLIENTE product)
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
                    db.CAT_CLIENTEs.InsertOnSubmit(product);
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
            CAT_CLIENTE product = db.CAT_CLIENTEs.Where(x => x.ID_CLIENTE == Id).FirstOrDefault();
            slnQF.Models.ClienteModels prod = new slnQF.Models.ClienteModels();
            prod.NOMBRE = product.NOMBRE;
            prod.CODIGO_SAP = product.CODIGO_SAP;
            prod.ESTADO = Convert.ToInt32(product.ESTADO);
            prod.ID_CLIENTE =  Convert.ToInt32(product.ID_CLIENTE);
            ViewBag.CategoriesDDL = new SelectList(db.CAT_ESTADOs, "ESTADO_ID", "DESCRIPCION", prod.ESTADO);

            return PartialView(prod);
        }

        [HttpPost]
        public JsonResult Edit(slnQF.CAT_CLIENTE product)
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
                try
                {
                    db.Connection.Open();
                    db.Transaction = db.Connection.BeginTransaction();
                    CAT_CLIENTE tblProd = db.CAT_CLIENTEs.Where(x => x.ID_CLIENTE == product.ID_CLIENTE)
                                           .FirstOrDefault();

                    tblProd.NOMBRE = product.NOMBRE;
                    tblProd.CODIGO_SAP = product.CODIGO_SAP;
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
    }
}