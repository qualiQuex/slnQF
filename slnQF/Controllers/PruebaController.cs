using slnQF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace slnQF.Controllers
{
    public class PruebaController : Controller
    {
        // GET: Prueba
        public ActionResult Index()
        {
            ModelState.Clear();
            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            List<SP_CONSULTA_PRUEBAResult> PRUEBA = db.SP_CONSULTA_PRUEBA()
               .OrderBy(x => x.ID_PRUEBA)
               .ToList<SP_CONSULTA_PRUEBAResult>();
            return View(PRUEBA);
        }


        public PartialViewResult Create()
        {
            var GenreLst = new List<MyDrop>();

            MyDrop LISTOPT_NO = new MyDrop();
            LISTOPT_NO.id = 0;
            LISTOPT_NO.value = "NO";

            MyDrop LISTOPT_SI = new MyDrop();
            LISTOPT_SI.id = 1;
            LISTOPT_SI.value = "SI";

            GenreLst.Add(LISTOPT_NO);
            GenreLst.Add(LISTOPT_SI);

            ViewBag.SWSI_NO = new SelectList(GenreLst, "id", "value");


            return PartialView(new slnQF.Models.PruebaModels());
        }
        [HttpPost]
        public JsonResult Create(slnQF.CAT_PRUEBA product)
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
                    db.CAT_PRUEBAs.InsertOnSubmit(product);
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
            CAT_PRUEBA product = db.CAT_PRUEBAs.Where(x => x.ID_PRUEBA == Id).FirstOrDefault();
            slnQF.Models.PruebaModels prod = new slnQF.Models.PruebaModels();
            prod.NOMBRE = product.NOMBRE;
            prod.DESCRIPCION = product.DESCRIPCION;
            prod.ESTADO = Convert.ToInt32(product.ESTADO);
            prod.ID_PRUEBA = Convert.ToInt32(product.ID_PRUEBA);
            prod.SWSI_NO = Convert.ToInt32(product.SWSI_NO);
            ViewBag.CategoriesDDL = new SelectList(db.CAT_ESTADOs, "ESTADO_ID", "DESCRIPCION", prod.ESTADO);
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
        public JsonResult Edit(slnQF.CAT_PRUEBA product)
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
                    CAT_PRUEBA tblProd = db.CAT_PRUEBAs.Where(x => x.ID_PRUEBA == product.ID_PRUEBA)
                                           .FirstOrDefault();

                    tblProd.DESCRIPCION = product.DESCRIPCION;
                    tblProd.NOMBRE = product.NOMBRE;
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