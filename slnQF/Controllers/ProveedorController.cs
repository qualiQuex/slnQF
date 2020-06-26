using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace slnQF.Controllers
{
    public class ProveedorController : Controller
    {
        // GET: Proveedor
        public ActionResult Index()
        {
            ModelState.Clear();
            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            List<SP_CONSULTA_PROVEEDORResult> PRUEBA = db.SP_CONSULTA_PROVEEDOR()
               .OrderBy(x => x.CARDNAME)
               .ToList<SP_CONSULTA_PROVEEDORResult>();
            return View(PRUEBA);
        }
        [HttpPost]
        public JsonResult fnCambioEstadoRepParam(string id)
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
            int v_RepParam_id = 0;
            try
            {
                v_RepParam_id = Convert.ToInt32(id);

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
                    CAT_PROVEEDOR tblProd = db.CAT_PROVEEDORs.Where(x => x.ID_PROVEEDOR == v_RepParam_id)
                                           .FirstOrDefault();

                    if (tblProd.SW_MUESTRACERT == 1)
                        tblProd.SW_MUESTRACERT = 0;
                    else if (tblProd.SW_MUESTRACERT == 0)
                        tblProd.SW_MUESTRACERT = 1;


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
        public JsonResult fnActualizaData()
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
                    string v_respuesta="";
                    string v_m1 = "";
                    string v_m2 = "";
                    var q = db.SP_MIGRA_PROVEEDOR(Convert.ToInt32(idUsuarioLog), ref v_respuesta,ref  v_m1,ref v_m2);
                    

                      
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