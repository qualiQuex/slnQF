using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace slnQF.Controllers
{
    public class ProductoPresentNomComController : Controller
    {
        // GET: ProductoPresentNomCom
        public ActionResult Index()
        {
            ModelState.Clear();
            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            List<SP_CONSULTA_PRO_PRE_NOMResult> PRUEBA = db.SP_CONSULTA_PRO_PRE_NOM()
               .OrderBy(x => x.ID_PRODUCTO).OrderBy(x => x.ID_PRESENTACION).OrderBy(x => x.ID_NOMBRE_COMERCIAL)
               .ToList<SP_CONSULTA_PRO_PRE_NOMResult>();
            return View(PRUEBA);
        }

        public PartialViewResult Listado(int id)
        {
            ModelState.Clear();
            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            List<SP_CONSULTA_PRO_PRE_NOMResult> PRUEBA = db.SP_CONSULTA_PRO_PRE_NOM()
               .OrderBy(x => x.ID_PRODUCTO).OrderBy(x => x.ID_PRESENTACION).OrderBy(x => x.ID_NOMBRE_COMERCIAL).Where(x => x.ID_PRODUCTO == id)
               .ToList<SP_CONSULTA_PRO_PRE_NOMResult>();
            return PartialView("~/Views/ProductoPresentNomCom/Listado.cshtml", PRUEBA);
        }

        public PartialViewResult Create(int ID)
        {

            DC_CALIDADDataContext db = new DC_CALIDADDataContext();


            ViewBag.ID_PRODUCTO = ID;

            ViewBag.PresentacionlDDL = new SelectList(db.SP_CONSULTA_PRODUCTO_PRESENTACION().Where(x => x.ID_PRODUCTO ==ID), "ID_PRESENTACION", "NOMBRE_PRESENTACION");

            ViewBag.NombreComercialDDL = new SelectList(db.SP_CONSULTA_PRODUCTO_NOMBRE_COM().Where(x => x.ID_PRODUCTO == ID), "ID_NOMBRE_COMERCIAL", "NOMBRE_COMERCIAL");

            return PartialView(new slnQF.Models.ProductoPresentNomCom());
        }

        [HttpPost]
        public JsonResult Create(slnQF.CAT_PRODUCTO_PRES_NOMCOM product)
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
                
                product.FECHA_CREACION = DateTime.Now;
                product.ESTADO = 1;
                product.ID_USUARIO = Convert.ToInt32(idUsuarioLog);
                if ((product.ID_PRODUCTO <= 0) || (product.ID_PRESENTACION <= 0) || (product.ID_NOMBRE_COMERCIAL <= 0))
                {
                    error = "1";
                    errorStr = "Revise nuevamente los datos seleccionados.";
                }
                else
                {
                    DC_CALIDADDataContext db = new DC_CALIDADDataContext();
                    try
                    {
                        db.Connection.Open();
                        db.Transaction = db.Connection.BeginTransaction();
                        db.CAT_PRODUCTO_PRES_NOMCOMs.InsertOnSubmit(product);
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

        public PartialViewResult Edit(int Id, int Id2, int Id3)
        {
            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            CAT_PRODUCTO_PRES_NOMCOM product = db.CAT_PRODUCTO_PRES_NOMCOMs.Where(x => x.ID_PRODUCTO == Id && x.ID_PRESENTACION == Id2 && x.ID_NOMBRE_COMERCIAL == Id3).FirstOrDefault();
            slnQF.Models.ProductoPresentNomCom prod = new slnQF.Models.ProductoPresentNomCom();
            prod.ID_NOMBRE_COMERCIAL = product.ID_NOMBRE_COMERCIAL;
            prod.ID_PRESENTACION = product.ID_PRESENTACION;
            prod.ID_PRODUCTO = product.ID_PRODUCTO;
            prod.ESTADO = Convert.ToInt32(product.ESTADO);
            prod.CODIGO_SAP = product.CODIGO_SAP;
            ViewBag.CategoriesDDL = new SelectList(db.CAT_ESTADOs, "ESTADO_ID", "DESCRIPCION", prod.ESTADO);




            return PartialView(prod);
        }

        [HttpPost]
        public JsonResult Edit(slnQF.CAT_PRODUCTO_PRES_NOMCOM product)
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
                    CAT_PRODUCTO_PRES_NOMCOM tblProd = db.CAT_PRODUCTO_PRES_NOMCOMs.Where(x => x.ID_PRODUCTO == product.ID_PRODUCTO && x.ID_PRESENTACION == product.ID_PRESENTACION && x.ID_NOMBRE_COMERCIAL == product.ID_NOMBRE_COMERCIAL).FirstOrDefault();

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
    }
}