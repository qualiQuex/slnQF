using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace slnQF.Controllers
{
    public class ArticuloController : Controller
    {
        // GET: Articulo
        public ActionResult Index()
        {
            string idUsuarioLog = "-1";
            int intIdUsuarioLog = 0;
            try
            {
                idUsuarioLog = Session["LogedUserID"].ToString();
                intIdUsuarioLog = Convert.ToInt32(idUsuarioLog);
            }
            catch (Exception Ex)
            {
                intIdUsuarioLog = 0;
            }
            ModelState.Clear();
            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            List<SP_CONSULTA_SAP_ARTICULO_DATA_USERResult> aritculos = db.SP_CONSULTA_SAP_ARTICULO_DATA_USER("07%",intIdUsuarioLog)
               .ToList<SP_CONSULTA_SAP_ARTICULO_DATA_USERResult>();
 

            return View(aritculos);
        }

        // GET: Articulo
        public ActionResult Index2(string tipo) {
            string idUsuarioLog = "-1";
            int intIdUsuarioLog = 0;
            try
            {
                idUsuarioLog = Session["LogedUserID"].ToString();
                intIdUsuarioLog = Convert.ToInt32(idUsuarioLog);
            }
            catch (Exception Ex)
            {
                intIdUsuarioLog = 0;
            }
            tipo = tipo + "%";
            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            List<SP_CONSULTA_SAP_ARTICULO_DATA_USERResult> aritculos = db.SP_CONSULTA_SAP_ARTICULO_DATA_USER(tipo,intIdUsuarioLog)
               .ToList<SP_CONSULTA_SAP_ARTICULO_DATA_USERResult>();

            return PartialView("_Grid", aritculos);
        }

        public ActionResult ListaMaesLote(string IdItemCode)
        {
            string idUsuarioLog = "-1";
            int intIdUsuarioLog = 0;
            try
            {
                idUsuarioLog = Session["LogedUserID"].ToString();
                intIdUsuarioLog = Convert.ToInt32(idUsuarioLog);
            }
            catch (Exception Ex)
            {
                intIdUsuarioLog = 0;
            }
            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            List<SP_CONSULTA_SAP_ITEM_OPELOTEResult> loteData = db.SP_CONSULTA_SAP_ITEM_OPELOTE(IdItemCode, intIdUsuarioLog)
               .ToList<SP_CONSULTA_SAP_ITEM_OPELOTEResult>();
          
            return PartialView(loteData);
        }
        public ActionResult ListaContaLote(string IdItemCode)
        {
            string idUsuarioLog = "-1";
            int intIdUsuarioLog = 0;
            try
            {
                idUsuarioLog = Session["LogedUserID"].ToString();
                intIdUsuarioLog = Convert.ToInt32(idUsuarioLog);
            }
            catch (Exception Ex)
            {
                intIdUsuarioLog = 0;
            }
            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            List<SP_CONSULTA_SAP_ITEM_CONTAResult> loteContaData = db.SP_CONSULTA_SAP_ITEM_CONTA(IdItemCode, intIdUsuarioLog)
               .ToList<SP_CONSULTA_SAP_ITEM_CONTAResult>();

            return PartialView(loteContaData);
        }
        

        public ActionResult ListaDetLote(string IdItemCode,string batchnum, string whscode)
        {
            string idUsuarioLog = "-1";
            int intIdUsuarioLog = 0;
            try
            {
                idUsuarioLog = Session["LogedUserID"].ToString();
                intIdUsuarioLog = Convert.ToInt32(idUsuarioLog);
            }
            catch (Exception Ex)
            {
                intIdUsuarioLog = 0;
            }
            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            List<SP_CONSULTA_SAP_ITEM_OPELOTE_DETAResult> loteDataDeta = db.SP_CONSULTA_SAP_ITEM_OPELOTE_DETA(IdItemCode, intIdUsuarioLog, batchnum, whscode)
               .ToList<SP_CONSULTA_SAP_ITEM_OPELOTE_DETAResult>();

            return PartialView(loteDataDeta);
        }



        [HttpGet]
        public PartialViewResult Edit(string Id, int ID_ITEM,char pQUERYGROUP64)
        {

            string idUsuarioLog = "-1";
            int intIdUsuarioLog = 0;
            try
            {
                idUsuarioLog = Session["LogedUserID"].ToString();
                intIdUsuarioLog = Convert.ToInt32(idUsuarioLog);
            }
            catch (Exception Ex)
            {
                intIdUsuarioLog = 0;
            }

            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            SP_CONSULTA_SAP_ARTICULO_DATA_USERResult dat = db.SP_CONSULTA_SAP_ARTICULO_DATA_USER("T",intIdUsuarioLog).Where(x => x.ItemCode == Id).FirstOrDefault();
            if (ID_ITEM == -1)
            {
                TITEM product = new TITEM();
                product.DOSIS = "DOSIS: La que el médico señale.";
                product.HECHO_POR = "Producto Centroamericano hecho en Guatemala, por Laboratorio Qualipharm, S.A.";
                product.CONDICION_ALMACENAMIENTO = "Almacenar a una temperatura entre 15° y 30° en lugar seco, protegido de la luz.";
                product.INDICACION_VENTA = "Venta bajo receta medica.";
                product.INDICACION_ESPECIAL = "Mantengase fuera del alcance de los niños.";
                product.ITEMCODE = Id;
                product.QUERYGROUP64 = pQUERYGROUP64;
                product.U_NORE_GUATE = dat.U_NORE_GUATE;
                product.U_NORE_CR = dat.U_NORE_CR;
                product.U_NORE_ELSAL = dat.U_NORE_ELSAL;
                product.U_NORE_HOND = dat.U_NORE_HOND;
                product.U_NORE_NICA = dat.U_NORE_NICA;
                product.U_NORE_PANA = dat.U_NORE_PANA;


                product.U_NORE_GUATE_BOOL = (bool)dat.U_NORE_GUATE_BOOL;
                product.U_NORE_CR_BOOL = (bool)dat.U_NORE_CR_BOOL;
                product.U_NORE_ELSAL_BOOL = (bool)dat.U_NORE_ELSAL_BOOL;
                product.U_NORE_HOND_BOOL = (bool)dat.U_NORE_HOND_BOOL;
                product.U_NORE_NICA_BOOL = (bool)dat.U_NORE_NICA_BOOL;
                product.U_NORE_PANA_BOOL = (bool)dat.U_NORE_PANA_BOOL;
                product.DATOS_BULTO = dat.DATOS_BULTO;
                //ViewBag.TipoEtiqueta = new SelectList(db.CAT_TIPO_ETIQUETAs, "ESTADO_ID", "DESCRIPCION", prod.ESTADO);
                ViewBag.TipoEtiqueta = new SelectList(db.CAT_TIPO_ETIQUETAs.OrderBy(x => x.DESCRIPCION), "ID_TIPO_ETIQUETA", "DESCRIPCION");
                ViewBag.FFarmaceutica = new SelectList(db.CAT_FORMA_FARMACEUTICAs.OrderBy(x => x.NOMBRE), "ID_FORMA_FARMACEUTICA", "NOMBRE");
                ViewBag.EFabricacion = new SelectList(db.CAT_ETAPA_FABRICACIONs.OrderBy(x => x.DESCRIPCION), "ID_ETAPA_FABRICACION", "DESCRIPCION");
                ViewBag.ITEMCODE = Id;
                ViewBag.ITEMNAME = "["+Id+ "]" + dat.ItemName;
                 
                 
                product.ITEMNAME = dat.ItemName;
                return PartialView(product);
            }
            else {
               
               
                ViewBag.ITEMCODE = Id;
                TITEM product = db.TITEMs.Where(x => x.ID_ITEM == ID_ITEM).FirstOrDefault();
                ViewBag.FFarmaceutica = new SelectList(db.CAT_FORMA_FARMACEUTICAs.OrderBy(x => x.NOMBRE), "ID_FORMA_FARMACEUTICA", "NOMBRE", product.ID_FORMA_FARMACEUTICA);
                ViewBag.EFabricacion = new SelectList(db.CAT_ETAPA_FABRICACIONs.OrderBy(x => x.DESCRIPCION), "ID_ETAPA_FABRICACION", "DESCRIPCION", product.ID_ETAPA_FABRICACION);
                ViewBag.TipoEtiqueta = new SelectList(db.CAT_TIPO_ETIQUETAs.OrderBy(x => x.DESCRIPCION), "ID_TIPO_ETIQUETA", "DESCRIPCION", product.TIPO_ETIQUETA);
                ViewBag.ITEMNAME = "[" + Id + "]" + dat.ItemName;
                product.ITEMNAME = dat.ItemName;
                return PartialView(product);
            }
                       
        }

        public JsonResult Edit(slnQF.TITEM product)
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
                if (product.ID_ITEM == -1)
                {

                    product.FECHA_CREACION = DateTime.Now;
                    product.ID_RESPONSABLE = Convert.ToInt32(idUsuarioLog);

                    try
                    {
                        db.Connection.Open();
                        db.Transaction = db.Connection.BeginTransaction();
                        db.TITEMs.InsertOnSubmit(product);
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
                else {
                    try
                    {
                        db.Connection.Open();
                        db.Transaction = db.Connection.BeginTransaction();
                        TITEM tblProd = db.TITEMs.Where(x => x.ID_ITEM == product.ID_ITEM)
                                               .FirstOrDefault();

                        tblProd.ID_ETAPA_FABRICACION = product.ID_ETAPA_FABRICACION;
                        tblProd.ID_FORMA_FARMACEUTICA = product.ID_FORMA_FARMACEUTICA;
                        tblProd.TIPO_ETIQUETA = product.TIPO_ETIQUETA;
                        tblProd.CONTENIDO = product.CONTENIDO;
                        tblProd.VIA_ADMIN = product.VIA_ADMIN;
                        tblProd.DOSIS = product.DOSIS;
                        tblProd.ADVERTENCIA = product.ADVERTENCIA;
                        tblProd.HECHO_POR = product.HECHO_POR;
                        tblProd.CONDICION_ALMACENAMIENTO = product.CONDICION_ALMACENAMIENTO;
                        tblProd.EMPAQUE = product.EMPAQUE;
                        tblProd.PRESENTACION = product.PRESENTACION;
                        tblProd.INDICACION_ESPECIAL = product.INDICACION_ESPECIAL;
                        tblProd.INDICACION_VENTA = product.INDICACION_VENTA;
                        tblProd.CLIENTE = product.CLIENTE;
                        tblProd.U_NORE_GUATE = product.U_NORE_GUATE;
                        tblProd.U_NORE_ELSAL = product.U_NORE_ELSAL;
                        tblProd.U_NORE_HOND = product.U_NORE_HOND;
                        tblProd.U_NORE_NICA = product.U_NORE_NICA;
                        tblProd.U_NORE_CR = product.U_NORE_CR;
                        tblProd.U_NORE_PANA = product.U_NORE_PANA;

                        tblProd.U_NORE_GUATE_BOOL = product.U_NORE_GUATE_BOOL;
                        tblProd.U_NORE_ELSAL_BOOL = product.U_NORE_ELSAL_BOOL;
                        tblProd.U_NORE_HOND_BOOL = product.U_NORE_HOND_BOOL;
                        tblProd.U_NORE_NICA_BOOL = product.U_NORE_NICA_BOOL;
                        tblProd.U_NORE_CR_BOOL = product.U_NORE_CR_BOOL;
                        tblProd.U_NORE_PANA_BOOL = product.U_NORE_PANA_BOOL;

                        tblProd.DATOS_BULTO = product.DATOS_BULTO;


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
    }

    
}