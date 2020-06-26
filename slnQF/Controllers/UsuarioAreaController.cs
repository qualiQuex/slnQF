using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace slnQF.Controllers
{
    public class UsuarioAreaController : Controller
    {
        // GET: UsuarioArea
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult CreateAreaUsuario(int id_Usuario)
        {
            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            ViewBag.AreasDLL = new SelectList(db.SP_CONSULTA_AREA_X_USUARIO(id_Usuario), "ID_AREA", "AREA");
            ViewBag.ID_USER = id_Usuario;
            return PartialView(new CAT_USUARIO_AREA_CERT());
        }

        [HttpPost]
        public JsonResult createAreaUsuario(slnQF.CAT_USUARIO_AREA_CERT product)
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
                    db.CAT_USUARIO_AREA_CERTs.InsertOnSubmit(product);
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
            List<SP_CONSULTA_USUARIO_AREA_CERTResult> PRUEBA = db.SP_CONSULTA_USUARIO_AREA_CERT()
               .Where(x => x.ID_USUARIO == id)
               .ToList<SP_CONSULTA_USUARIO_AREA_CERTResult>();
            return PartialView(PRUEBA);
        }

        [HttpPost]
        public JsonResult fnCambioEstado(string id)
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
            int V_USUARIO_AREA_CERT = 0;
            try
            {
                V_USUARIO_AREA_CERT = Convert.ToInt32(id);

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
                    CAT_USUARIO_AREA_CERT tblProd = db.CAT_USUARIO_AREA_CERTs.Where(x => x.ID_USUARIO_AREA_CERT == V_USUARIO_AREA_CERT)
                                           .FirstOrDefault();

                    if (tblProd.ID_ESTADO == 1)
                        tblProd.ID_ESTADO = 0;
                    else if (tblProd.ID_ESTADO == 0)
                        tblProd.ID_ESTADO = 1;


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