using slnQF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace slnQF.Controllers
{
    public class EnsayoPruebaController : Controller
    {
        // GET: EnsayoPrueba
        public ActionResult Index()
        {
            ModelState.Clear();
            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            List<SP_CONSULTA_ENSAYO_PRUEBAResult> PRUEBA = db.SP_CONSULTA_ENSAYO_PRUEBA()
               .OrderBy(x => x.ID_ENSAYO).OrderBy(x=>x.ID_PRUEBA)
               .ToList<SP_CONSULTA_ENSAYO_PRUEBAResult>();
            return View(PRUEBA);
        }

 
        public PartialViewResult Create(int Id)
        {

            DC_CALIDADDataContext db = new DC_CALIDADDataContext();

            ViewBag.PruebaDDL = new SelectList(db.SP_CONSULTA_PRUEBA().OrderBy(x => x.NOMBRE), "ID_PRUEBA", "NOMBRE");

            ViewBag.ID_ENSAYO = Id;
            
            ViewBag.SWSI_NO_B = new SelectList(db.CAT_SWSI_NOs, "SWSI_NO", "DESCRIPCION");

            return PartialView(new slnQF.Models.EnsayoPruebaModels());
        }

        public PartialViewResult Listado(int id)
        {
            ModelState.Clear();
            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            List<SP_CONSULTA_ENSAYO_PRUEBAResult> PRUEBA = db.SP_CONSULTA_ENSAYO_PRUEBA()
               .OrderBy(x => x.ID_ENSAYO).OrderBy(x => x.ID_PRUEBA).Where(x => x.ID_ENSAYO == id)
               .ToList<SP_CONSULTA_ENSAYO_PRUEBAResult>();
            return PartialView("~/Views/EnsayoPrueba/Listado.cshtml", PRUEBA);
        }

        [HttpPost]
        public JsonResult Create(slnQF.CAT_ENSAYO_PRUEBA product)
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
                    db.CAT_ENSAYO_PRUEBAs.InsertOnSubmit(product);
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

        public PartialViewResult Edit(int Id,int Id2)
        {
            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            CAT_ENSAYO_PRUEBA product = db.CAT_ENSAYO_PRUEBAs.Where(x => x.ID_ENSAYO == Id && x.ID_PRUEBA == Id2).FirstOrDefault();
            slnQF.Models.EnsayoPruebaModels prod = new slnQF.Models.EnsayoPruebaModels();
            prod.ID_PRUEBA = product.ID_PRUEBA;
            prod.ID_ENSAYO = product.ID_ENSAYO;
            prod.ESTADO = Convert.ToInt32(product.ESTADO);
            prod.SWSI_NO = Convert.ToInt32(product.SWSI_NO);
            ViewBag.CategoriesDDL = new SelectList(db.CAT_ESTADOs, "ESTADO_ID", "DESCRIPCION", prod.ESTADO);

            ViewBag.PruebaDDL = new SelectList(db.CAT_PRUEBAs, "ID_PRUEBA", "NOMBRE", prod.ID_PRUEBA);

            ViewBag.EnsayoDDL = new SelectList(db.CAT_ENSAYOs, "ID_ENSAYO", "DESCRIPCION", prod.ID_ENSAYO);


            var GenreLst = new List<MyDrop>();

            MyDrop LISTOPT_NO = new MyDrop();
            LISTOPT_NO.id = 0;
            LISTOPT_NO.value = "NO";

            MyDrop LISTOPT_SI = new MyDrop();
            LISTOPT_SI.id = 1;
            LISTOPT_SI.value = "SI";

            GenreLst.Add(LISTOPT_NO);
            GenreLst.Add(LISTOPT_SI);

            ViewBag.SWSI_NO_B = new SelectList(db.CAT_SWSI_NOs, "SWSI_NO", "DESCRIPCION", prod.SWSI_NO);


            return PartialView(prod);
        }

        [HttpPost]
        public JsonResult Edit(slnQF.CAT_ENSAYO_PRUEBA product)
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
                    CAT_ENSAYO_PRUEBA tblProd = db.CAT_ENSAYO_PRUEBAs.Where(x => x.ID_ENSAYO == product.ID_ENSAYO && x.ID_PRUEBA == product.ID_PRUEBA).FirstOrDefault();

                    tblProd.ID_ENSAYO = product.ID_ENSAYO;
                    tblProd.ID_PRUEBA= product.ID_PRUEBA;
                    tblProd.ESTADO = product.ESTADO;
                    tblProd.SWSI_NO = product.SWSI_NO;
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