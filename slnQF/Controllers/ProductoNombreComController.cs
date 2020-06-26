using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace slnQF.Controllers
{
    public class ProductoNombreComController : Controller
    {
        // GET: EnsayoPrueba
        public ActionResult Index()
        {
            ModelState.Clear();
            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            List<SP_CONSULTA_PRODUCTO_NOMBRE_COMResult> PRUEBA = db.SP_CONSULTA_PRODUCTO_NOMBRE_COM()
               .OrderBy(x => x.ID_PRODUCTO).OrderBy(x => x.ID_NOMBRE_COMERCIAL)
               .ToList<SP_CONSULTA_PRODUCTO_NOMBRE_COMResult>();
            return View(PRUEBA);
        }

        public PartialViewResult Listado(int id)
        {
            ModelState.Clear();
            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            List<SP_CONSULTA_PRODUCTO_NOMBRE_COMResult> PRUEBA = db.SP_CONSULTA_PRODUCTO_NOMBRE_COM()
               .OrderBy(x => x.ID_PRODUCTO).OrderBy(x => x.ID_NOMBRE_COMERCIAL).Where(x=>x.ID_PRODUCTO == id)
               .ToList<SP_CONSULTA_PRODUCTO_NOMBRE_COMResult>();
            return PartialView("~/Views/ProductoNombreCom/Listado.cshtml", PRUEBA);
        }
        public PartialViewResult Create(int Id)
        {

            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            ViewBag.ID_PRODUCTO = Id;
            //ViewBag.ProductoDDL = new SelectList(db.TPRODUCTOs, "ID_PRODUCTO", "NOMBRE");

            ViewBag.NombreComercialDDL = new SelectList(db.SP_CONSULTA_NOMBRE_COMERCIAL().OrderBy(x => x.DESCRIPCION), "ID_NOMBRE_COMERCIAL", "DESCRIPCION");

 

            return PartialView(new slnQF.Models.ProductoNombreCom());
        }

        [HttpPost]
        public JsonResult Create(slnQF.CAT_PRODUCTO_NOMBRE_COM product)
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
                    db.CAT_PRODUCTO_NOMBRE_COMs.InsertOnSubmit(product);
                    db.SubmitChanges();
                    db.Transaction.Commit();
                    error = "0";
                    db.Connection.Close();
                    db.Dispose();
                }
                catch (Exception e)
                {
                    db.Transaction.Rollback();
                    error = "1";
                    db.Connection.Close();
                    db.Dispose();
                }
            }
            else
            {
                error = "1";
            }
            return Json(new { ID = error, MENSAJE = errorStr });
        }

        public PartialViewResult Edit(int Id, int Id2)
        {
            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            CAT_PRODUCTO_NOMBRE_COM product = db.CAT_PRODUCTO_NOMBRE_COMs.Where(x => x.ID_PRODUCTO == Id && x.ID_NOMBRE_COMERCIAL == Id2).FirstOrDefault();
            slnQF.Models.ProductoNombreCom prod = new slnQF.Models.ProductoNombreCom();
            prod.ID_PRODUCTO = product.ID_PRODUCTO;
            prod.ID_NOMBRE_COMERCIAL = product.ID_NOMBRE_COMERCIAL;
            prod.ESTADO = Convert.ToInt32(product.ESTADO);
            
            ViewBag.CategoriesDDL = new SelectList(db.CAT_ESTADOs, "ESTADO_ID", "DESCRIPCION", prod.ESTADO);


            ViewBag.ProductoDDL = new SelectList(db.TPRODUCTOs, "ID_PRODUCTO", "NOMBRE", product.ID_PRODUCTO);

            ViewBag.NombreComercialDDL = new SelectList(db.SP_CONSULTA_NOMBRE_COMERCIAL().OrderBy(x => x.DESCRIPCION), "ID_NOMBRE_COMERCIAL", "DESCRIPCION", product.ID_NOMBRE_COMERCIAL);



            return PartialView(prod);
        }

        [HttpPost]
        public JsonResult Edit(slnQF.CAT_PRODUCTO_NOMBRE_COM product)
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
                    CAT_PRODUCTO_NOMBRE_COM tblProd = db.CAT_PRODUCTO_NOMBRE_COMs.Where(x => x.ID_PRODUCTO == product.ID_PRODUCTO && x.ID_NOMBRE_COMERCIAL == product.ID_NOMBRE_COMERCIAL).FirstOrDefault();

                    tblProd.ID_PRODUCTO= product.ID_PRODUCTO;
                    tblProd.ID_NOMBRE_COMERCIAL = product.ID_NOMBRE_COMERCIAL;
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