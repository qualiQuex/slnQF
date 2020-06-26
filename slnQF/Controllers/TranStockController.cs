using Newtonsoft.Json;
using SAPbobsCOM;
using slnQF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace slnQF.Controllers
{
    public class TranStockController : Controller
    {
        // GET: TranStock
        public ActionResult Index()
        {
            List<MyDrop> listaVacia = new List<MyDrop>(); 
            ViewBag.cmbSeries = new SelectList(listaVacia, "id", "value");
            return View();
        }
        public ActionResult ListaArticuloStockT(string IdItemCode)
        {
             
            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            List<SP_CONSULTA_SAP_ARTICULO_ITEMResult> loteData = db.SP_CONSULTA_SAP_ARTICULO_ITEM(IdItemCode)
               .ToList<SP_CONSULTA_SAP_ARTICULO_ITEMResult>();
            
            return PartialView(loteData);
        }
        
        public ActionResult ListaSociosNegocios(string pCriterio)
        {

            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            List<SP_CONSULTA_SAP_SOCIOS_NEGOCIOSResult> sociodData = db.SP_CONSULTA_SAP_SOCIOS_NEGOCIOS(pCriterio)
               .ToList<SP_CONSULTA_SAP_SOCIOS_NEGOCIOSResult>();

            return PartialView(sociodData);
        }
        public JsonResult creaTransferenciStock(string jsonStringItems,string jsonStringLotes, string pserie, string palmacen, string pnumero)
        {
            List<itemTransferenciaStock> dynJsonItem = JsonConvert.DeserializeObject<List<itemTransferenciaStock>>(jsonStringItems);
            List<reciboclsLoteSeleccionado> dynJsonLote = JsonConvert.DeserializeObject<List<reciboclsLoteSeleccionado>>(jsonStringLotes);

            string vsuarioLog = "";
            string idUsuarioLog = "";
            int v_band = 0;
            string respuesta_str = "";
            string respOk = "OK";
            int vIdFlagErr = 0;
            try
            {

                idUsuarioLog = Session["LogedUserID"].ToString();

                if (idUsuarioLog.Length > 0)
                {

                    v_band = 1;
                    vsuarioLog = Session["LogedUser"].ToString();
                }
                else
                {
                    respOk = "NOK";
                    v_band = 0;
                }

            }
            catch
            {
                respOk = "NOK";
                v_band = 0;
                vIdFlagErr = -1;
            }
            if (v_band > 0)
            {
                try { 
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
                    //conectar 
                    lRetCode = oCompany.Connect();
                    oCompany.StartTransaction();
                    if (lRetCode != 0)
                    {
                        oCompany.GetLastError(out lErrCode, out sErrMsg);

                        oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                        oCompany.Disconnect();
                        oCompany = null;
                        GC.Collect();

                        return Json(new { ID = "-3", MENSAJE = "Error en conexion a SAP, " + sErrMsg, P_OK = "NOK" });
                    }
                    else
                    {
                        int respuesta = -1;

                        //Documents v_StockEntry = oCompany.GetBusinessObject(BoObjectTypes.oStockTransfer);
                   
                        SAPbobsCOM.StockTransfer v_StockEntry = (StockTransfer)oCompany.GetBusinessObject(BoObjectTypes.oStockTransfer);
                        int vLinea = 0;
                        v_StockEntry.Series = Convert.ToInt32(pserie);
                        v_StockEntry.DocDate = DateTime.Now;
                        
                        v_StockEntry.FromWarehouse = palmacen;
                        v_StockEntry.PriceList = -1;
                        
                        v_StockEntry.JournalMemo = "Traslados -";
                        v_StockEntry.UserFields.Fields.Item("U_SysUser").Value = vsuarioLog;
                        foreach (itemTransferenciaStock item in dynJsonItem)
                        {
                            v_StockEntry.Lines.BaseLine = vLinea;
                            v_StockEntry.Lines.ItemCode = item.ITEMCODE;
                            v_StockEntry.Lines.Quantity = Convert.ToDouble(item.CANTIDAD);
                            v_StockEntry.Lines.WarehouseCode=item.ALMACEN_DESTINO; 
                                try {
                                int cuentaLote = 0;
                                    foreach (reciboclsLoteSeleccionado itemLote in dynJsonLote.Where(x => x.ITEMCODE == item.ITEMCODE))
                                    {
                                        v_StockEntry.Lines.BatchNumbers.BatchNumber = itemLote.BATCHNUM;
                                        v_StockEntry.Lines.BatchNumbers.Quantity = Convert.ToDouble(itemLote.TTOTAL);
                                        v_StockEntry.Lines.BatchNumbers.BaseLineNumber = cuentaLote;
                                        v_StockEntry.Lines.BatchNumbers.ExpiryDate = DateTime.Parse(itemLote.FECHA_EXP); 
                                        v_StockEntry.Lines.BatchNumbers.InternalSerialNumber = itemLote.LOTNUMBER;
                                        v_StockEntry.Lines.BatchNumbers.Notes = itemLote.LOTNOTES;
                                       // v_StockEntry.Lines.BatchNumbers.BaseLineNumber


                                    v_StockEntry.Lines.BatchNumbers.Add();
                                        cuentaLote++;
                                    }
                                }
                                catch (Exception Ex)
                                {
                                    oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                                    oCompany.Disconnect();
                                    oCompany = null;
                                    GC.Collect();

                                    return Json(new { ID = "-4", MENSAJE = "Error en Lotes, " + Ex.Message, P_OK = "NOK" });
                                }


                                try
                                {

                                vLinea++;
                                v_StockEntry.Lines.Add();
                                }
                                catch (Exception Ex)
                                {
                                    return Json(new { ID = "-4", MENSAJE = "Error en Lotes, " + Ex.Message, P_OK = "NOK" });
                                }

                        }
                        if (v_StockEntry.Add() != 0)
                        {
                            respuesta = 0;
                            respOk = "NOK";
                            vIdFlagErr = -5;
                            respuesta_str = "ERROR AGREGAR DOCUMENTO" + oCompany.GetLastErrorDescription();
                            //oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);    
                            oCompany.Disconnect();
                            oCompany = null;
                            GC.Collect();
                             
                        }
                        if (respuesta != 0)
                        {
                            oCompany.EndTransaction(BoWfTransOpt.wf_Commit);
                            string docEntryDraft;
                            oCompany.GetNewObjectCode(out docEntryDraft);

                            respOk = "OK";
                            respuesta_str = "Documento creado exitosamente, " + docEntryDraft;
                            vIdFlagErr = 0;
                            oCompany.Disconnect();
                            oCompany = null;
                           

                        }
                    }
                }
                catch(Exception ex) {
                    respOk = "NOK";
                    respuesta_str = "Error: " + ex.Message;
                    vIdFlagErr = -2;
                }

            }
            else
            {
                respOk = "NOK";
                respuesta_str = "Error en el usuario";
            }
            GC.Collect();

            return Json(new { ID = vIdFlagErr.ToString(), MENSAJE = respuesta_str.ToString(), P_OK = respOk });
        }
        }
}