using slnQF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace slnQF.Controllers
{
    public class En_Prue_PrueEspController : Controller
    {
        // GET: En_Prue_PrueEsp
        public ActionResult Index()
        {
            ModelState.Clear();
            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            List<SP_CONSULTA_EN_PRUE_PRUE_ESPResult> PRUEBA = db.SP_CONSULTA_EN_PRUE_PRUE_ESP()
               .OrderBy(x => x.ID_ENSAYO).OrderBy(x => x.ID_PRUEBA).OrderBy(x => x.ID_PRUEBA_ESPECIFICA)
               .ToList<SP_CONSULTA_EN_PRUE_PRUE_ESPResult>();
            return View(PRUEBA);
        }


        public PartialViewResult Create(int ID_ENSAYO)
        {

            DC_CALIDADDataContext db = new DC_CALIDADDataContext();

            
            ViewBag.ID_ENSAYO = ID_ENSAYO;

            ViewBag.PruebaDDL = new SelectList(db.SP_CONSULTA_ENSAYO_PRUEBA().Where(x => x.ID_ENSAYO == ID_ENSAYO && x.SWSI_NO == "SI").OrderBy(x => x.NOMBRE_PRUEBA) , "ID_PRUEBA", "NOMBRE_PRUEBA");

            ViewBag.PruebaEspecificaDDL = new SelectList(db.SP_CONSULTA_PRUEBA_ESPECIFICA().OrderBy(x => x.DESCRIPCION), "ID_PRUEBA_ESPECIFICA", "DESCRIPCION");

 
             
            return PartialView(new slnQF.Models.En_Prue_PrueEspModels());
        }


        public PartialViewResult Listado(int id)
        {
            ModelState.Clear();
            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            List<SP_CONSULTA_EN_PRUE_PRUE_ESPResult> PRUEBA = db.SP_CONSULTA_EN_PRUE_PRUE_ESP()
               .OrderBy(x => x.ID_ENSAYO).OrderBy(x => x.ID_PRUEBA).Where(x => x.ID_ENSAYO == id)
               .ToList<SP_CONSULTA_EN_PRUE_PRUE_ESPResult>();
            return PartialView("~/Views/En_Prue_PrueEsp/Listado.cshtml", PRUEBA);
        }

        [HttpPost]
        public JsonResult Create(slnQF.CAT_ENSAYO_PRUEBA_PESPECIFICA product)
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
                if ((product.ID_ENSAYO <= 0) || (product.ID_PRUEBA <= 0) || (product.ID_PRUEBA_ESPECIFICA <= 0))
                {
                    error = "1";
                    errorStr = "Revise los datos seleccionados.";
                }
                else { 
                    try
                    {
                        db.Connection.Open();
                        db.Transaction = db.Connection.BeginTransaction();
                        db.CAT_ENSAYO_PRUEBA_PESPECIFICAs.InsertOnSubmit(product);
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

        public PartialViewResult Edit(int Id, int Id2,int Id3)
        {
            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            CAT_ENSAYO_PRUEBA_PESPECIFICA product = db.CAT_ENSAYO_PRUEBA_PESPECIFICAs.Where(x => x.ID_ENSAYO == Id && x.ID_PRUEBA == Id2 && x.ID_PRUEBA_ESPECIFICA == Id3).FirstOrDefault();
            slnQF.Models.En_Prue_PrueEspModels prod = new slnQF.Models.En_Prue_PrueEspModels();
            prod.ID_PRUEBA = product.ID_PRUEBA;
            prod.ID_ENSAYO = product.ID_ENSAYO;
            prod.ID_PRUEBA_ESPECIFICA = product.ID_PRUEBA_ESPECIFICA;
            prod.ESTADO = Convert.ToInt32(product.ESTADO);
            
            ViewBag.CategoriesDDL = new SelectList(db.CAT_ESTADOs, "ESTADO_ID", "DESCRIPCION", prod.ESTADO);

 


            return PartialView(prod);
        }

        [HttpPost]
        public JsonResult Edit(slnQF.CAT_ENSAYO_PRUEBA_PESPECIFICA product)
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
                    CAT_ENSAYO_PRUEBA_PESPECIFICA tblProd = db.CAT_ENSAYO_PRUEBA_PESPECIFICAs.Where(x => x.ID_ENSAYO == product.ID_ENSAYO && x.ID_PRUEBA == product.ID_PRUEBA && x.ID_PRUEBA_ESPECIFICA == product.ID_PRUEBA_ESPECIFICA).FirstOrDefault();

             
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