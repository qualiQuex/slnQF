using slnQF.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace slnQF.Controllers
{
    public class EspecificacionProductoController : Controller
    {
        // GET: EspecificacionProducto
        public ActionResult Index()
        {
            {
                ModelState.Clear();
                DC_CALIDADDataContext db = new DC_CALIDADDataContext();
                List<SP_CONSULTA_ESPECIFICACION_PRODUCTOResult> products = db.SP_CONSULTA_ESPECIFICACION_PRODUCTO()
                   .OrderBy(x => x.ID_ESPECIFICACION_PRODUCTO)
                   .ToList<SP_CONSULTA_ESPECIFICACION_PRODUCTOResult>();
                return View(products);
            }
        }


        public PartialViewResult Create()
        {

            DC_CALIDADDataContext db = new DC_CALIDADDataContext();

            ViewBag.ProductoDDL = new SelectList(db.TPRODUCTOs.OrderBy(x=>x.NOMBRE), "ID_PRODUCTO", "NOMBRE");
            /*
            ViewBag.FormaFarmaceuticaDDL = new SelectList(db.CAT_FORMA_FARMACEUTICAs, "ID_FORMA_FARMACEUTICA", "NOMBRE");

            ViewBag.ConcentracionDDL = new SelectList(db.CAT_CONCENTRACIONs, "ID_CONCENTRACION", "NOMBRE");
            */
            ViewBag.EnsayoDDL = new SelectList(db.CAT_ENSAYOs.Where(x => x.ESTADO == 1).OrderBy(x => x.DESCRIPCION), "ID_ENSAYO", "DESCRIPCION");

            ViewBag.PruebaDDL = new SelectList(db.CAT_PRUEBAs.Where(x => x.ESTADO == 1).OrderBy(x => x.NOMBRE), "ID_PRUEBA", "NOMBRE");

            ViewBag.PruebaEspecificaDDL = new SelectList(db.CAT_PRUEBA_ESPECIFICAs.Where(x => x.ESTADO == 1).OrderBy(x => x.DESCRIPCION), "ID_PRUEBA_ESPECIFICA", "DESCRIPCION");



            return PartialView(new slnQF.Models.EspecificacionProductoModels());
        }

        [HttpPost]
        public JsonResult Create(slnQF.ESPECIFICACION_PRODUCTO product)
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
                DC_CALIDADDataContext db2 = new DC_CALIDADDataContext();
               

                if (product.ID_PRUEBA_ESPECIFICA==-1)
                product.ID_PRUEBA_ESPECIFICA = null;
                product.FECHA_CREACION = DateTime.Now;
                product.ESTADO = 1;
                product.ID_USUARIO= Convert.ToInt32(idUsuarioLog);
                db2.Connection.Open();
                List<ESPECIFICACION_PRODUCTO> espe = db2.ESPECIFICACION_PRODUCTOs
                    .Where(x => x.ID_PRUEBA == product.ID_PRUEBA && x.ID_ENSAYO == product.ID_ENSAYO && x.ID_PRODUCTO == product.ID_PRODUCTO)
                    .ToList<ESPECIFICACION_PRODUCTO>();
                db2.Connection.Close();
                int v_cuenta_tmp = 0;
                foreach (ESPECIFICACION_PRODUCTO tmp in espe) {
                    if (tmp.ID_PRUEBA_ESPECIFICA == product.ID_PRUEBA_ESPECIFICA)
                        v_cuenta_tmp = 1;
                }
                if (v_cuenta_tmp > 0) {
                    return Json(new { ID = "3", MENSAJE = "Ya existe una especificacion con las opciones seleccionadas, verificar." });
                }
                if ((product.ID_ENSAYO <= 0) || (product.ID_PRUEBA <= 0) )
                {
                    error = "1";
                }
                else
                {
                    try
                    {
                        db.Connection.Open();
                        db.Transaction = db.Connection.BeginTransaction();
                        db.ESPECIFICACION_PRODUCTOs.InsertOnSubmit(product);
                        db.SubmitChanges();
                        db.Transaction.Commit();
                        error = "0";
                        errorStr = "Informacion guardada con exito";
                        db.Connection.Close();
                        db.Dispose();
                        db2.Connection.Close();
                        db2.Dispose();
                    }
                    catch (Exception e)
                    {
                        db.Transaction.Rollback();
                        error = "1";
                        errorStr = "Error en el guardado de informacion, " + e.Message;
                        db.Connection.Close();
                        db.Dispose();
                        db2.Connection.Close();
                        db2.Dispose();
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
            ESPECIFICACION_PRODUCTO product = db.ESPECIFICACION_PRODUCTOs.Where(x => x.ID_ESPECIFICACION_PRODUCTO == Id ).FirstOrDefault();
            slnQF.Models.EspecificacionProductoModels prod = new slnQF.Models.EspecificacionProductoModels();
            prod.ID_ESPECIFICACION_PRODUCTO = product.ID_ESPECIFICACION_PRODUCTO;
            prod.ID_PRODUCTO = product.ID_PRODUCTO;
            prod.ID_ENSAYO = product.ID_ENSAYO;
            prod.ID_PRUEBA_ESPECIFICA = Convert.ToInt32(product.ID_PRUEBA_ESPECIFICA);
            prod.ID_PRUEBA = product.ID_PRUEBA;
            prod.ESPECIFICACION = product.ESPECIFICACION;
            prod.ESTADO = Convert.ToInt32(product.ESTADO);
            prod.TIPO_RESPUESTA = product.TIPO_RESPUESTA;

            ViewBag.ID_PRODUCTO = prod.ID_PRODUCTO;

            ViewBag.EnsayoDDL = new SelectList(db.CAT_ENSAYOs.OrderBy(x => x.DESCRIPCION), "ID_ENSAYO", "DESCRIPCION", prod.ID_ENSAYO);

            ViewBag.PruebaDDL = new SelectList(db.CAT_PRUEBAs.OrderBy(x => x.NOMBRE), "ID_PRUEBA", "NOMBRE", prod.ID_PRUEBA);

            ViewBag.PruebaEspecificaDDL = new SelectList(db.CAT_PRUEBA_ESPECIFICAs.OrderBy(x => x.DESCRIPCION), "ID_PRUEBA_ESPECIFICA", "DESCRIPCION",prod.ID_PRUEBA_ESPECIFICA);

            ViewBag.CategoriesDDL = new SelectList(db.CAT_ESTADOs, "ESTADO_ID", "DESCRIPCION", prod.ESTADO);

            return PartialView(prod);
        }

        [HttpPost]
        public JsonResult Edit(slnQF.ESPECIFICACION_PRODUCTO product)
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
                    ESPECIFICACION_PRODUCTO tblProd = db.ESPECIFICACION_PRODUCTOs.Where(x => x.ID_ESPECIFICACION_PRODUCTO == product.ID_ESPECIFICACION_PRODUCTO).FirstOrDefault();

                   
                    tblProd.ID_PRODUCTO = product.ID_PRODUCTO;
                    tblProd.ID_ENSAYO = product.ID_ENSAYO;
                    tblProd.ID_PRUEBA = product.ID_PRUEBA;
                    tblProd.ID_PRUEBA_ESPECIFICA = product.ID_PRUEBA_ESPECIFICA;
                    if (product.ID_PRUEBA_ESPECIFICA == -1)
                        tblProd.ID_PRUEBA_ESPECIFICA = null;
                    tblProd.ESPECIFICACION = product.ESPECIFICACION;
                    tblProd.TIPO_RESPUESTA = product.TIPO_RESPUESTA;
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
        public List<CatalogoModel.MyDropList> traeComboPruebas(int idCombo)
        {
            List<CatalogoModel.MyDropList> LISTA = new List<CatalogoModel.MyDropList>();
           
            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            List<SP_CONSULTA_ENSAYO_PRUEBAResult> PRUEBA = db.SP_CONSULTA_ENSAYO_PRUEBA()
               .OrderBy(x => x.NOMBRE_PRUEBA).Where(x => x.ID_ENSAYO == idCombo && x.ESTADO == "ACTIVO")
               .ToList<SP_CONSULTA_ENSAYO_PRUEBAResult>();
 
            foreach (SP_CONSULTA_ENSAYO_PRUEBAResult dr in PRUEBA)
            {
                CatalogoModel.MyDropList fila = new CatalogoModel.MyDropList();
                fila.id = dr.ID_PRUEBA.ToString();
                fila.value = dr.NOMBRE_PRUEBA.ToString();

                LISTA.Add(fila);
            }
  
            return LISTA;
        }

        public List<CatalogoModel.MyDropList> traeComboPruebasEspecificas(int idCombo,int idCombo2)
        {
            List<CatalogoModel.MyDropList> LISTA = new List<CatalogoModel.MyDropList>();

            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            List<SP_CONSULTA_EN_PRUE_PRUE_ESPResult> PRUEBA = db.SP_CONSULTA_EN_PRUE_PRUE_ESP()
               .OrderBy(x => x.PRUEBA_ESPECIFICA_DESC).Where(x => x.ID_ENSAYO == idCombo && x.ID_PRUEBA == idCombo2 && x.ESTADO == "ACTIVO")
               .ToList<SP_CONSULTA_EN_PRUE_PRUE_ESPResult>();

            foreach (SP_CONSULTA_EN_PRUE_PRUE_ESPResult dr in PRUEBA)
            {
                CatalogoModel.MyDropList fila = new CatalogoModel.MyDropList();
                fila.id = dr.ID_PRUEBA_ESPECIFICA.ToString();
                fila.value = dr.PRUEBA_ESPECIFICA_DESC.ToString();

                LISTA.Add(fila);
            }

            if (LISTA.Count == 0) {
                CatalogoModel.MyDropList fila = new CatalogoModel.MyDropList();
                fila.id = "-1";
                fila.value = "SIN PRUEBA ESPECIFICA";

                LISTA.Add(fila);
            }

            return LISTA;
        }
        

        public JsonResult traePruebas(string id_ensayo)
        {
            List<CatalogoModel.MyDropList> lista = new List<CatalogoModel.MyDropList>();


          
            lista = traeComboPruebas(Convert.ToInt32(id_ensayo));


            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        public JsonResult traePruebasEspecificas(string id_ensayo, string id_prueba)
        {
            List<CatalogoModel.MyDropList> lista = new List<CatalogoModel.MyDropList>();



            lista = traeComboPruebasEspecificas(Convert.ToInt32(id_ensayo), Convert.ToInt32(id_prueba));


            return Json(lista, JsonRequestBehavior.AllowGet);
        }

    }
}