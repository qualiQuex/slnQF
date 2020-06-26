using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace slnQF.Controllers
{
    public class Cliente_NombreComController : Controller
    {


        public PartialViewResult Create(int Id)
        {

            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
              
            List<SP_CONSULTA_CLIENTE_NOMCOM_DISPResult> lista = db.SP_CONSULTA_CLIENTE_NOMCOM_DISP(Id)
               .OrderBy(x => x.NOMBRE_CLIENTE)
               .ToList<SP_CONSULTA_CLIENTE_NOMCOM_DISPResult>();

            ViewBag.ClienteDDL = new SelectList(lista, "ID_CLIENTE", "NOMBRE_CLIENTE");

            ViewBag.NOMBRE_COMERCIAL = db.SP_CONSULTA_NOMBRE_COMERCIAL().Where(x => x.ID_NOMBRE_COMERCIAL == Id).FirstOrDefault().DESCRIPCION;

            ViewBag.ID_NOMBRE_COMERCIAL = Id;

           

            return PartialView(new slnQF.Models.Cliente_Nom_Com());
        }
        [HttpPost]
        public JsonResult CreateJS(slnQF.CAT_CLIENTE_NOMBRE_COM product)
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
            if (product.ID_CLIENTE <= 0 || product.ID_NOMBRE_COMERCIAL <= 0) {
                error = "1";
                errorStr = "Revise los datos seleccionados y vuelva a intentarlo";
                return Json(new { ID = error, MENSAJE = errorStr });
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
                    db.CAT_CLIENTE_NOMBRE_COMs.InsertOnSubmit(product);
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

        // GET: Cliente_NombreCom
        public PartialViewResult ListadoCliente(int id)
        {
            ModelState.Clear();
            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            List<SP_CONSULTA_CLIENTE_NOMBRE_COMResult> PRUEBA = db.SP_CONSULTA_CLIENTE_NOMBRE_COM()
               .OrderBy(x => x.NOMBRE_CLIENTE).Where(x => x.ID_NOMBRE_COMERCIAL == id)
               .ToList<SP_CONSULTA_CLIENTE_NOMBRE_COMResult>();
            return PartialView("~/Views/Cliente_NombreCom/ListadoCliente.cshtml", PRUEBA);
        }
    }
}