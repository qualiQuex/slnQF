using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace slnQF.Controllers
{
    public class Area_ProductoController : Controller
    {
        // GET: Area_Producto
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult CreateAreaProducto(int id_Producto)
        {
            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            ViewBag.AreasDLL = new SelectList(db.SP_CONSULTA_AREA_X_PRODUCTO(id_Producto), "ID_AREA", "AREA");
            ViewBag.ID_PRODUCTO = id_Producto;
            return PartialView(new CAT_AREA_PRODUCTO());
        }

        public PartialViewResult CreateProductoArea(int id_Area)
        {
            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            ViewBag.AreasDLL = new SelectList(db.SP_CONSULTA_PRODUCTO_X_AREA(id_Area), "ID_PRODUCTO", "NOMBRE");
            ViewBag.ID_AREA = id_Area;
            SP_CONSULTA_PRODUCTO_X_AREAResult x = new SP_CONSULTA_PRODUCTO_X_AREAResult();

            return PartialView(new CAT_AREA_PRODUCTO());
        }

        [HttpPost]
        public JsonResult createAreaProducto(slnQF.CAT_AREA_PRODUCTO product)
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
                    db.CAT_AREA_PRODUCTOs.InsertOnSubmit(product);
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
            List<SP_CONSULTA_AREA_PRODUCTOResult> PRUEBA = db.SP_CONSULTA_AREA_PRODUCTO()
               .Where(x => x.ID_PRODUCTO == id)
               .ToList<SP_CONSULTA_AREA_PRODUCTOResult>();
            return PartialView(PRUEBA);
        }

        public PartialViewResult ListadoProductos(int id)
        {
            ModelState.Clear();
            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            List<SP_CONSULTA_AREA_PRODUCTOResult> PRUEBA = db.SP_CONSULTA_AREA_PRODUCTO()
               .Where(x => x.ID_AREA == id)
               .ToList<SP_CONSULTA_AREA_PRODUCTOResult>();
            
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
            int V_producto_area = 0;
            try
            {
                V_producto_area = Convert.ToInt32(id);

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
                    CAT_AREA_PRODUCTO tblProd = db.CAT_AREA_PRODUCTOs.Where(x => x.ID_AREA_PRODUCTO == V_producto_area)
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