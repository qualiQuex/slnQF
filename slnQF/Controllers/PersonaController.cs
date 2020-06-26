using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace slnQF.Controllers
{
    public class PersonaController : Controller
    {
        // GET: Persona
        public ActionResult Index()
        {
            ModelState.Clear();
            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            List<SP_CONSULTA_PERSONAResult> PERSONA = db.SP_CONSULTA_PERSONA()
               .OrderBy(x => x.NOMBRE).ToList<SP_CONSULTA_PERSONAResult>();
            return View(PERSONA);
        }

        public PartialViewResult Create()
        {

            DC_CALIDADDataContext db = new DC_CALIDADDataContext();

            ViewBag.UsuariosDDL = new SelectList(db.SP_CONSULTA_USER_PER().Where(x => x.ESTADO== 1).OrderBy(x => x.NOMBRE), "ID_USUARIO", "NOMBRE");

            ViewBag.PuestosDDL = new SelectList(db.SP_CONSULTA_PUESTO().Where(x => x.ID_ESTADO == 1).OrderBy(x => x.DESCRIPCION), "ID_PUESTO", "DESCRIPCION");



            return PartialView(new slnQF.Models.PersonaModels());
        }

        public JsonResult Creates(slnQF.TPERSONA product)
        {
            string error = "";
            string idUsuarioLog = "-1";
            string errorStr = "";
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
                if ((product.ID_PUESTO <= 0) || (product.ID_USER <= 0) )
                {
                    error = "1";
                    errorStr = "Revise los datos seleccionados.";
                }
                else
                {
                    try
                    {
                        db.Connection.Open();
                        db.Transaction = db.Connection.BeginTransaction();
                        db.TPERSONAs.InsertOnSubmit(product);
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
            TPERSONA product = db.TPERSONAs.Where(x => x.ID_PERSONA == Id).FirstOrDefault();
            slnQF.Models.PersonaModels prod = new slnQF.Models.PersonaModels();
            prod.ID_PERSONA = product.ID_PERSONA;
            prod.ID_PUESTO = product.ID_PUESTO;
            prod.ID_USER = product.ID_USER;
            prod.ID_ESTADO = Convert.ToInt32(product.ESTADO);

            ViewBag.PuestosDDL = new SelectList(db.SP_CONSULTA_PUESTO().Where(x => x.ID_ESTADO == 1).OrderBy(x => x.DESCRIPCION), "ID_PUESTO", "DESCRIPCION",prod.ID_PUESTO);

            ViewBag.CategoriesDDL = new SelectList(db.CAT_ESTADOs, "ESTADO_ID", "DESCRIPCION", prod.ID_ESTADO);

            ViewBag.NOMBRE = db.SP_CONSULTA_USUARIO().Where(x=>x.ID_USUARIO == prod.ID_USER).FirstOrDefault().NOMBRE;

            return PartialView(prod);
        }

        [HttpPost]
        public JsonResult EditJ(slnQF.TPERSONA product)
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
                    TPERSONA tblProd = db.TPERSONAs.Where(x => x.ID_PERSONA == product.ID_PERSONA).FirstOrDefault();


                    tblProd.ESTADO = product.ESTADO;
                    tblProd.ID_PUESTO = product.ID_PUESTO;

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