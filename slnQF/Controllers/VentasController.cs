using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using slnQF.Models;
using System.Web.Helpers;
using SAPbobsCOM;
using System.Data;

namespace slnQF.Controllers
{
    public class VentasController : Controller
    {
        ItemModel model = new ItemModel();
        clsSAP objSap = new clsSAP();
        // GET: Ventas

        public ActionResult VentasEntrega(string tipo)
        {
            /*
            clsDB bdSP = new clsDB();
            
            DataSet ds;
            ds = bdSP.DATOS_GENERALES_USUARIO("CQUEX");
            */
            if (objSap.conectar() != 0)
            {
 
                Console.WriteLine("Error");
                //Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Conectado");

            }
            
            System.Web.HttpContext.Current.Session["tipo"] = tipo;
            
            if ((tipo == "sale") || (tipo=="sale-return"))
            {
                ViewBag.Clientes = new SelectList(objSap.traeClientes(objSap.oCompany), "id", "value");
            }
            if ((tipo == "purchase") || (tipo == "purchase-return"))
            {
                ViewBag.Clientes = new SelectList(objSap.traeVendor(objSap.oCompany), "id", "value");
            } 
            ViewBag.CmbMoneda = new SelectList(objSap.traeMonedas(objSap.oCompany), "id", "value");
            ViewBag.CmbSerie = new SelectList(objSap.traeMonedas(objSap.oCompany), "id", "value");
            ViewBag.CmbArticulos = new SelectList(objSap.traeArticulos(objSap.oCompany), "id", "value");
            ViewBag.CmbCodsArticulos = new SelectList(objSap.traeCodArticulos(objSap.oCompany), "id", "value");
            ViewBag.CmbImpuestos = new SelectList(objSap.traeImpuestosOrden(objSap.oCompany), "id", "value");

             
            if (System.Web.HttpContext.Current.Session["Items"] == null)
            {
                List<clsItem> items = clsItem.GetSampleclsItem();

                if (items != null)
                {
                    model.TotalCount = items.Count();
                    model.Items = items;
                }
                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "fn_datePicker();", true);

                System.Web.HttpContext.Current.Session["Items"] = model.Items;
            }
            else {
                model.Items = (List<clsItem>)System.Web.HttpContext.Current.Session["Items"];
            }
            objSap.desconectar();
            return View(  model);
         
        }

        
        public JsonResult quitarItemLista(string value )
        {
            /*List<clsItem> items = new List<clsItem>();
            var x = System.Web.HttpContext.Current.Session["Items"] ;
            items = (List<clsItem>) x;
            model.Items = items;*/
            var modelList = (List<clsItem>)System.Web.HttpContext.Current.Session["Items"];
            var item = modelList.Find(x => x.Id == value);
            modelList.Remove(item);
            System.Web.HttpContext.Current.Session["Items"] = modelList;
            model.Items = modelList;


            return Json(new { ID = value });
        }

        public JsonResult AgregarItemLista(string Id, string Nombre, string Precio, string Impuesto,string Cantidad,string Lote)
        {
              
            var modelList = (List<clsItem>)System.Web.HttpContext.Current.Session["Items"];
            if (modelList == null)
                modelList = new List<clsItem>();
            clsItem item  = new clsItem(Id, Nombre, Precio, Impuesto,Cantidad,Lote) ;
            
            modelList.Add(item);
            System.Web.HttpContext.Current.Session["Items"] = modelList;
            model.Items = modelList;

             
            return Json(new { ID = Id });
        }

        public JsonResult AgregarOrden(string IdCliente)
        {
             
            Company oCompany;
            //Capturar errores
            long lRetCode;
            int lErrCode;
            string sErrMsg;

            oCompany = new Company();
            oCompany.Server = System.Configuration.ConfigurationManager.AppSettings.Get("Server");
            oCompany.DbServerType = BoDataServerTypes.dst_MSSQL2008;
            oCompany.CompanyDB = System.Configuration.ConfigurationManager.AppSettings.Get("CompanyDB");
            oCompany.UserName = System.Configuration.ConfigurationManager.AppSettings.Get("UserName");
            oCompany.Password = System.Configuration.ConfigurationManager.AppSettings.Get("Password");
            //oCompany.language = ln_English;


            //conectar 
            lRetCode = oCompany.Connect();


            // validar que la conexion sea correcta
            if (lRetCode != 0)
            {
                oCompany.GetLastError(out lErrCode, out sErrMsg);
                Console.WriteLine(lErrCode + sErrMsg);
                //Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Conectado");

            }
            
            string tipo = System.Web.HttpContext.Current.Session["tipo"].ToString() ;
            List<clsItem> modelList = (List<clsItem>)System.Web.HttpContext.Current.Session["Items"];
            int respuesta = 0;
            SAPbobsCOM.Documents v_StockEntry = null;
            if (tipo == "sale")
            {
                v_StockEntry = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDeliveryNotes);
            }

            if (tipo == "sale-return")
            {
                v_StockEntry = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oReturns);
            }
            if (tipo == "purchase")
            {
                v_StockEntry = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseDeliveryNotes);
            }

            if (tipo == "purchase-return")
            {
                v_StockEntry = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseReturns);
            }

            v_StockEntry.CardCode = IdCliente;
            v_StockEntry.HandWritten = SAPbobsCOM.BoYesNoEnum.tNO;

            v_StockEntry.DocDate = DateTime.Today;
            v_StockEntry.DocDueDate = DateTime.Today;
            foreach (clsItem val in modelList) {
                
                 
                v_StockEntry.Lines.ItemCode = val.Id;
                //v_StockEntry.Lines.WarehouseCode = "01";
                v_StockEntry.Lines.Quantity = Convert.ToInt32(val.Cantidad);
                v_StockEntry.Lines.Price = Convert.ToInt32(val.Precio);
                v_StockEntry.Lines.TaxCode = val.Impuesto;

                
                v_StockEntry.Lines.BatchNumbers.SetCurrentLine(0);
                v_StockEntry.Lines.BatchNumbers.BatchNumber = val.Lote ;
                v_StockEntry.Lines.BatchNumbers.Quantity = Convert.ToInt32(val.Cantidad); ;
                v_StockEntry.Lines.BatchNumbers.BaseLineNumber = 0;

                v_StockEntry.Lines.BatchNumbers.AddmisionDate = DateTime.Today;

                v_StockEntry.Lines.BatchNumbers.ExpiryDate = DateTime.Today.AddMonths(6);

                v_StockEntry.Lines.BatchNumbers.Add();
                
                 /*
                v_StockEntry.Lines.SerialNumbers.SetCurrentLine(0);

                v_StockEntry.Lines.SerialNumbers.ManufacturerSerialNumber = "1";

                v_StockEntry.Lines.SerialNumbers.InternalSerialNumber = "1";

                v_StockEntry.Lines.SerialNumbers.SystemSerialNumber = 1;

                v_StockEntry.Lines.SerialNumbers.Add();
                */
                v_StockEntry.Lines.Add();
                
            }

            if (v_StockEntry.Add() != 0)
                {
                    respuesta = 0;
                    string err = "ERROR AGREGAR LOTE: " + oCompany.GetLastErrorDescription();
                }
                respuesta = 1;
                oCompany.Disconnect();
            
            return Json(respuesta.ToString(), JsonRequestBehavior.AllowGet);

        }
        public JsonResult traeNumDocto(string value)
        {
            
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        public JsonResult traePrecioArticulo(modelPrecio precio) {
            int vprecio = 0;
           clsSAP objSap = new clsSAP();
            if (objSap.conectar() != 0)
            {

                Console.WriteLine("Error");
                //Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Conectado");
                vprecio = objSap.traePrecioArticulo(precio.codCliente, precio.codArticulo, DateTime.Today, objSap.oCompany);

            }
            objSap.desconectar();
            return Json(vprecio, JsonRequestBehavior.AllowGet);
        }

        public JsonResult traeLotesArt(string itemCode)
        {
            List<MyDropList> lista = new List<MyDropList>();
             
            clsSAP objSap = new clsSAP();
            if (objSap.conectar() != 0)
            {

                Console.WriteLine("Error");
                //Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Conectado");
                lista = objSap.traeLotesItem(itemCode, objSap.oCompany);

            }
            objSap.desconectar();


            return Json(lista, JsonRequestBehavior.AllowGet);
        }
        public JsonResult traeCliente(string value)
        {

            modelFormaVenta MASTER = new modelFormaVenta();
            clsSAP objSap = new clsSAP();
            if (objSap.conectar() != 0)
            {

                Console.WriteLine("Error");
                //Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Conectado");
                MASTER = objSap.traeContacto(value, objSap.oCompany);

            }
            objSap.desconectar();
             
             
            return Json(MASTER, JsonRequestBehavior.AllowGet);
        }

    }
}