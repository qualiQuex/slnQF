using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions;

using Microsoft.Reporting.WebForms;
using Newtonsoft.Json;
using SAPbobsCOM;
using slnQF.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using iTextSharp.text.pdf;
using iTextSharp.text;

namespace slnQF.Controllers
{
    public class ProduccionController : Controller
    {
        public ActionResult Produccion(string estado)
        {
            try
            {
                ProduccionModel model = new ProduccionModel();
                if ((System.Web.HttpContext.Current.Session["LogedUserID"] != null))
                {
                    
                    int v_idUSUARIO = Convert.ToInt32(Session["LogedUserID"].ToString());
                    string p_tmpTipo = "";
                    if (estado == null)
                    {
                        estado = "Lib";
                    }
                    if (System.Web.HttpContext.Current.Session["tipo"] != null)
                    {
                        p_tmpTipo = System.Web.HttpContext.Current.Session["tipo"].ToString();
                    }

                    System.Web.HttpContext.Current.Session["tipo"] = estado;

                    //se llenan los comobobox que no cambian 
                    List<MyDrop> listaVacia = new List<MyDrop>();
                    ViewBag.cmbWareHouse = new SelectList(traeAlmacen(), "value", "value");
                    ViewBag.cmbSeries = new SelectList(listaVacia, "id", "value");


                    ViewBag.cmbDispensadores = new SelectList(traeListaDispensadores(), "id", "value");
                    ViewBag.cmbVerificadores = new SelectList(traeListaVerificadores(), "id", "value");



                    List<clsProduccionModel> items = traeOrdenesSap(v_idUSUARIO, estado);
                    if (items != null)
                    {
                        model.TotalCount = items.Count();
                        model.Items = items;
                    }

                    ViewBag.ordenesproduccion = model.Items;
                    List<clsItemsProduction> items2 = new List<clsItemsProduction>();

                    model.ItemsOrdenProduccion = items2;

                    return View(model);
                }
                else
                {
                    return RedirectToAction("Login2", "Usuario");
                }
            }
            catch {
                return RedirectToAction("Login2", "Usuario");
            }
        }
 
        public List<clsProduccionModel> traeOrdenesSap(int iduser, string estado)
        {
            string p_estado = "";
            if (estado == null)
            {
                estado = "Lib";
            }
            if (estado == "Can")
            {
                p_estado = "C";
                //p_top = " TOP 500 ";
            }
            else if (estado == "Cer")
            {
                p_estado = "L";
                //p_top = " TOP 15000 ";
            }
            else if (estado == "Pla")
            {
                p_estado = "P";
                //p_top = " TOP 100 ";
            }
            else
            {
                p_estado = "R";
            }
            List<clsProduccionModel> lista = new List<clsProduccionModel>();
            try
            {
                clsDB cDB = new clsDB();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds = cDB.CONSULTA_ORDENES_SAP(iduser, p_estado);
                dt = ds.Tables[0];


                foreach (DataRow dr in dt.Rows) //dr["UNIDADESFABRICAR"].ToString()
                {

                    clsProduccionModel fila = new clsProduccionModel(dr["DOCENTRY"].ToString(), dr["TYPE"].ToString(), dr["ESTADO"].ToString(), dr["NOPRODUCTO"].ToString(), dr["CANTIDADPLANIFICADA"].ToString(),
                    dr["ALMACEN"].ToString(), dr["SERIE"].ToString(), dr["DOCNUM"].ToString(), dr["POSTDATE"].ToString(), dr["DUEDATE"].ToString(), dr["USUARIO"].ToString(), dr["PEDIDO_CLIENTE"].ToString(),
                    dr["CLIENTE"].ToString(), dr["NORMAREPARTO"].ToString(), dr["PROJECT"].ToString(), dr["COMENTARIOS"].ToString(), dr["TIPO_FABRICACION"].ToString(), dr["FORMULAMAESTRA"].ToString(),
                    dr["FORMA_FARMACEU"].ToString(), dr["UNIDADESFABRICAR"].ToString(), dr["LOTE"].ToString(), dr["VENCE"].ToString(), dr["SUPERVISOR"].ToString(), dr["ITEMCODE"].ToString() ,
                    dr["ITEMNAME"].ToString(), dr["PRECIO"].ToString(), dr["ALMACEN_DESC"].ToString(), dr["ORIGEN"].ToString(),"", dr["OBSERVACIONES"].ToString(), "", "", dr["CANTIDAD"].ToString(), dr["UOM"].ToString());

                    lista.Add(fila);
                }
            }
            catch (Exception e)
            {
                System.Console.Write(e.Message);
            }
            return lista;
        }

        public List<accesos_user> traeAccesosUser()
        {
        List<accesos_user> listaAccesos = new List<accesos_user>();

            try
            {
                clsDB cDB = new clsDB();
                string idUsuarioLog = Session["LogedUserID"].ToString();
                int idUser = Convert.ToInt32(idUsuarioLog);
                if (idUser > 0) { 
                    DataSet ds = new DataSet();
                    DataTable dt = new DataTable();
                    ds = cDB.CONSULTA_ACCESO_X_USUARIOS(idUser);
                    dt = ds.Tables[0];


                    foreach (DataRow dr in dt.Rows)
                    {
                        accesos_user fila = new accesos_user();
                        fila.IDPERMISO = dr["ID_PERM"].ToString();
                        fila.IDACCESO = dr["ID_ACC"].ToString();

                        listaAccesos.Add(fila);
                    }
                }

            }
            catch (Exception e)
            {
                System.Console.Write(e.Message);
            }
            return listaAccesos;
        }
        public string traeSupervisoresUser(string User){
        
        List<string> tmp = new List<string>();
        string idUsuarioLog = User;

        tmp = traeSupervisaUser(idUsuarioLog);
        string useSup = "";
        int v_cuenta_comas = 0;
        foreach (string c in tmp)
        {
            v_cuenta_comas++;
            useSup = useSup+"'" + c+"'";
            if (v_cuenta_comas < tmp.Count())
                useSup = useSup + ",";

                    
        }
            return useSup;
        }
        public string traeAlmacenUser(string User){
        
                List<UsuarioModel.cls_bodega_asocia> tmp = new List<UsuarioModel.cls_bodega_asocia>();
                string idUsuarioLog = User;

                tmp = traeBodegaAsocia(idUsuarioLog);
                string almacen = "";
                int v_cuenta_comas = 0;
                foreach (UsuarioModel.cls_bodega_asocia c in tmp)
                {
                    v_cuenta_comas++;
                    almacen = almacen+"'" + c.nombre_bodega+"'";
                    if (v_cuenta_comas < tmp.Count())
                        almacen = almacen + ",";

                    
                }
            return almacen;
        }

        public List<CatalogoModel.MyDropList> traeAlmacen()
        {
            List<CatalogoModel.MyDropList> LISTA = new List<CatalogoModel.MyDropList>();
            clsDB cDB = new clsDB();

            try
            {
                string idUsuarioLog = Session["LogedUserID"].ToString();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds = cDB.CONSULTA_BODEGA_X_USUARIOS(Convert.ToInt32(idUsuarioLog));
                dt = ds.Tables[0];


                foreach (DataRow dr in dt.Rows)
                {
                    CatalogoModel.MyDropList fila = new CatalogoModel.MyDropList();
                    fila.id = dr["ID"].ToString();
                    fila.value = dr["NOMBRE"].ToString();

                    LISTA.Add(fila);
                }
            }
            catch (Exception e)
            {
                System.Console.Write(e.Message);
            }
            return LISTA;
        }
        public JsonResult traeNumDocto(string value)
        {
             clsDB cDB = new clsDB();
            int vNexNumber = 0;
            if (value == null)
            {
                value = "-1";
            }
            try
            {
                vNexNumber= cDB.NUMERO_SIG_SERIE(Convert.ToInt32(value)); 
            }
            catch
            {
                vNexNumber = 0;
            }
            return Json(vNexNumber, JsonRequestBehavior.AllowGet);
        }
        public JsonResult AgregaReciboSap(string docentry, string cantidad, string docnum, string serie, string almacen, string comentarios, string journal, string referencia2, string fecha, string jsonclsLoteSeleccionado, string verificador,string configEtiqueta)
        {
            string v_ok = "NOK";
            DateTime fecha_contabiliza;
            try
            {
                fecha_contabiliza = DateTime.Parse(fecha);
            }
            catch
            {
                return Json(new { ID = "-1", MENSAJE = "Error en formato de fecha de contabilizacion", P_OK = v_ok });
            }

            string v_respuesta = "";
            int id_codigo = 0;

            v_respuesta = creaReciboProduccion(Convert.ToInt32(docentry), Convert.ToDouble(cantidad), Convert.ToInt32(docnum), Convert.ToInt32(serie), almacen, comentarios, journal, referencia2, fecha_contabiliza, jsonclsLoteSeleccionado, verificador, configEtiqueta, out v_ok);
             
            try
            {
                id_codigo = Convert.ToInt32(v_respuesta);

            }
            catch
            {
                id_codigo = -1;
            }

            

            return Json(new { ID = id_codigo.ToString(), MENSAJE = v_respuesta.ToString(), P_OK = v_ok });
        }

        public List<clsItemsProduction> listaOrdenesDocentry(string docentry){
            List<clsItemsProduction> items2 = new List<clsItemsProduction>();
            if ((System.Web.HttpContext.Current.Session["LogedUserID"] != null)){ 
            int v_iduser = Convert.ToInt32(System.Web.HttpContext.Current.Session["LogedUserID"]);
            int v_docentry = Convert.ToInt32(docentry);

            clsDB cDB = new clsDB();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds = cDB.CONSULTA_LISTAITEMSORDEN_SAP(v_iduser, v_docentry);
            dt = ds.Tables[0];


            foreach (DataRow dr in dt.Rows) //dr["UNIDADESFABRICAR"].ToString()
            {

                clsItemsProduction fila = new clsItemsProduction(
                    dr["DOCNUM"].ToString()
                    , dr["ITEMCODE"].ToString()
                    , dr["ITEMNAME"].ToString()
                    , ""
                    , dr["PLANNEDQTY"].ToString()
                    , ""
                    , dr["WAREHOUSE"].ToString()
                    , dr["LINENUM"].ToString() 
                    , dr["LOTE"].ToString() 
                    , ""
                    , ""
                    , dr["WHSNAME"].ToString() 
                    , dr["BASEQTY"].ToString() 
                    , dr["PLANNEDQTY2"].ToString() 
                    , ""
                    , dr["invntryUom"].ToString() 
                    , dr["ISCOMMITED"].ToString() 
                    , dr["DISP"].ToString()
                    , dr["SWLOTE"].ToString()
                    , dr["COMPROMETIDO"].ToString()
                    , dr["STOCK"].ToString()
                    , dr["SOLICITADO"].ToString());
                   
                    items2.Add(fila);
            }
            }
            return items2;
        }
        public JsonResult SeleccionItemsOrdenProd(string docentry)
        {
            List<clsItemsProduction> items2 = new  List<clsItemsProduction>();
            if ((System.Web.HttpContext.Current.Session["LogedUserID"] != null)){
              items2= listaOrdenesDocentry(docentry);
            }
            //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "fn_datePicker();", true);


            return Json(new { ListaItems = items2 });
        }

        public JsonResult traeListaItemsREciboDocnum(string Docnum)
        {
            List<clsItemsProduction> items2 = new List<clsItemsProduction>();
            if ((System.Web.HttpContext.Current.Session["LogedUserID"] != null)){
       
              int v_iduser=Convert.ToInt32(Session["LogedUserID"].ToString());
              items2 = traeListaItemsRecibo(v_iduser,Docnum); 
                 
            }
            return Json(new { ListaItems = items2 });
        }

        public JsonResult traeListaItemsEmisionDocnum(string Docnum)
        {
            
            List<clsItemsProduction> items2 = new List<clsItemsProduction>();
            
            if ((System.Web.HttpContext.Current.Session["LogedUserID"] != null)){
                
                try
                {
                    int v_iduser=Convert.ToInt32(Session["LogedUserID"].ToString());
                    items2 = traeListaItemsEmision(v_iduser,Docnum);
                }catch(Exception e){
            
                }
            }
            

            return Json(new { ListaItems = items2 });
        
        }

        public List<clsItemsProduction> traeListaItemsEmision(int iduser, string docnum)
        {
           
            List<clsItemsProduction> lista = new List<clsItemsProduction>();
            try
            {
                clsDB cDB = new clsDB();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds = cDB.CONSULTA_SAP_EMISION_LISTA_DOCNUM(docnum,iduser);
                dt = ds.Tables[0];


                foreach (DataRow dr in dt.Rows) //dr["UNIDADESFABRICAR"].ToString()
                {
                    clsItemsProduction fila = new clsItemsProduction(dr["DOCNUM"].ToString(),dr["ITEMCODE"].ToString(), dr["ItemName"].ToString(), ""
                        , dr["Quantity"].ToString(), "",dr["warehouse"].ToString(), dr["LINENUM"].ToString(), dr["U_LOTE"].ToString(), "", dr["U_VENCE"].ToString(),
                        "", "", "", "",  dr["invntryUom"].ToString(),dr["ISCOMMITED"].ToString(), dr["DISP"].ToString(), dr["MANBTCHNUM"].ToString(),"","","");
                   
                    lista.Add(fila);
                }
            }
            catch (Exception e)
            {
                System.Console.Write(e.Message);
            }
            return lista;
        }

        public List<clsItemsProduction> traeListaItemsRecibo(int iduser, string docnum)
        {
           
            List<clsItemsProduction> lista = new List<clsItemsProduction>();
            try
            {
                clsDB cDB = new clsDB();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds = cDB.CONSULTA_SAP_RECIBO_LISTA_DOCNUM(docnum,iduser);
                dt = ds.Tables[0];

              
                foreach (DataRow dr in dt.Rows) //dr["UNIDADESFABRICAR"].ToString()
                {
                    clsItemsProduction fila = new clsItemsProduction(dr["DOCNUM"].ToString(),dr["ITEMCODE"].ToString(), dr["ItemName"].ToString(), ""
                        , dr["Quantity"].ToString(), "",dr["warehouse"].ToString(), dr["LINENUM"].ToString(), dr["U_LOTE"].ToString(), "", dr["U_VENCE"].ToString(),
                        "", "", "", "",  dr["invntryUom"].ToString(),dr["ISCOMMITED"].ToString(), dr["DISP"].ToString(), dr["MANBTCHNUM"].ToString(),"","","");
                   
                    lista.Add(fila);
                }
            }
            catch (Exception e)
            {
                System.Console.Write(e.Message);
            }
            return lista;
        }

        public string creaReciboProduccion(int V_DOCENTRY, double V_CANTIDAD, int v_docnum, int v_serie, string v_almacen, string comentarios, string journal, string referencia2, DateTime fecha, string jsonclsLoteSeleccionado, string verificador,string configEtiqueta, out string resp)
        {
            string v_itemcode = "";
            string vsuarioLog = "";
            string idUsuarioLog = "";
            int v_band = 0;

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

                    v_band = 0;
                }

            }
            catch
            {

                v_band = 0;

            }
            string respuesta_str = "";

            if (v_band > 0)
            {
                try
                {
                    List<clsLoteSeleccionadoRecibo> listaLoteSeleccionados = new List<clsLoteSeleccionadoRecibo>();
                    clsLoteSeleccionadoRecibo itemLoteSel;
                    List<reciboclsLoteSeleccionado> dynJson = JsonConvert.DeserializeObject<List<reciboclsLoteSeleccionado>>(jsonclsLoteSeleccionado);
                   
                    foreach (reciboclsLoteSeleccionado item in dynJson)
                    {
                        if (item != null)
                        {
                            itemLoteSel = new clsLoteSeleccionadoRecibo(item.ITEMCODE.ToString(), "", item.BATCHNUM.ToString(), item.QUANTITY.ToString(), item.TTOTAL.ToString(), item.FECHA_EXP.ToString(), item.NUEVO.ToString(), item.MNFSERIAL.ToString(),item.LOTNUMBER.ToString(),item.LOTNOTES.ToString());
                            listaLoteSeleccionados.Add(itemLoteSel);
                            v_itemcode = item.ITEMCODE.ToString();
                        }
                    }

                    List<clsItemEtiquetaCantidad> listaEtiquetasItem = new List<clsItemEtiquetaCantidad>();
                    clsItemEtiquetaCantidad EtiquetaItem;
                    
                    List<clsItemEtiquetaCantidad> dynJson2 = JsonConvert.DeserializeObject<List<clsItemEtiquetaCantidad>>(configEtiqueta);
                    foreach (clsItemEtiquetaCantidad item in dynJson2)
                    {
                        if (item != null)
                        {
                            EtiquetaItem = new clsItemEtiquetaCantidad(item.IDITEMEMBALAJE.ToString(), item.LIMITE.ToString(), item.CANTIDAD.ToString(), item.NOETIQUETAS.ToString());
                            listaEtiquetasItem.Add(EtiquetaItem);
                        }
                    }
                    
                    resp = "NOK";
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
                    oCompany.StartTransaction();

                    // validar que la conexion sea correcta
                    if (lRetCode != 0)
                    {
                        oCompany.GetLastError(out lErrCode, out sErrMsg);
                        Console.WriteLine(lErrCode + sErrMsg);
                        resp = "NOK";
                        oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);    
                        oCompany.Disconnect();
                        oCompany  = null;
                        GC.Collect();

                        return "No hay conexion con el SDK";
                    }
                    else
                    {
                        Console.WriteLine("Conectado");

                    
                    int respuesta = -1;

                    Documents v_StockEntry = null;

                    v_StockEntry = oCompany.GetBusinessObject(BoObjectTypes.oInventoryGenEntry);

                    v_StockEntry.HandWritten = BoYesNoEnum.tNO;

                    v_StockEntry.DocNum = v_docnum;
                    v_StockEntry.Series = v_serie;
                    v_StockEntry.DocDate = fecha;

                    v_StockEntry.Reference2 = referencia2;
                    v_StockEntry.Lines.Quantity = V_CANTIDAD;
                    v_StockEntry.Lines.BaseEntry = V_DOCENTRY;
                    v_StockEntry.Lines.WarehouseCode = v_almacen;
                    v_StockEntry.UserFields.Fields.Item("U_SysUser").Value = vsuarioLog;
                    if (verificador.Length > 0)
                    {
                        v_StockEntry.UserFields.Fields.Item("U_VERIFICADOR").Value = verificador;
                    }
                    try{
                    int contador = 0; 
                    foreach (clsLoteSeleccionadoRecibo c in listaLoteSeleccionados)
                    {
                        if ((c.TOTAL.Length > 0))
                        {
                            v_StockEntry.Lines.BatchNumbers.BatchNumber = c.BATCHNUM;
                            v_StockEntry.Lines.BatchNumbers.Quantity = Convert.ToDouble(c.TOTAL);
                            v_StockEntry.Lines.BatchNumbers.ExpiryDate = DateTime.Parse(c.FECHA_EXP);
                            v_StockEntry.Lines.BatchNumbers.ManufacturerSerialNumber = c.MNFSERIAL;
                            v_StockEntry.Lines.BatchNumbers.InternalSerialNumber = c.LOTNUMBER;
                            v_StockEntry.Lines.BatchNumbers.Notes = c.LOTNOTE;
                            v_StockEntry.Lines.BatchNumbers.BaseLineNumber = 0;
                            v_StockEntry.Lines.BatchNumbers.Add();
                            contador++;
                        }
                    }
                    }catch(Exception Ex){
                        oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);    
                        oCompany.Disconnect();
                        oCompany  = null;
                        GC.Collect();

                        return "Error en lotes " + Ex.Message;
                    }
                    v_StockEntry.JournalMemo = journal;

                    v_StockEntry.Comments = comentarios;

                    v_StockEntry.Lines.Add();

                    if (v_StockEntry.Add() != 0)
                    {
                        respuesta = 0;
                        respuesta_str = "ERROR AGREGAR LINEA" + oCompany.GetLastErrorDescription();
                        //oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);    
                        oCompany.Disconnect();
                        oCompany  = null;
                        GC.Collect();

                        return respuesta_str;
                    }

                    if (respuesta != 0)
                    {
                        string docEntryDraft;
                        oCompany.GetNewObjectCode(out docEntryDraft);

                       
                        resp = "OK";

                            int v_DOCENTRY = Convert.ToInt32(docEntryDraft);


                            oCompany.EndTransaction(BoWfTransOpt.wf_Commit);
                            oCompany.Disconnect();
                            oCompany = null;
                            int r_Docnum=0;
                            DC_CALIDADDataContext db2 = new DC_CALIDADDataContext();
                            
                                r_Docnum = (int)db2.F_TRAE_RECIBO_DOCNUM(v_DOCENTRY);
                            db2 = null;
                            respuesta_str = r_Docnum.ToString();
                            foreach (clsItemEtiquetaCantidad ins in listaEtiquetasItem) {
                                DC_CALIDADDataContext db = new DC_CALIDADDataContext();
                                TITEM_EMBALAJE_ORDEN nw = new TITEM_EMBALAJE_ORDEN();
                            nw.ID_ITEM_EMBALAJE = Convert.ToInt32(ins.IDITEMEMBALAJE.ToString());
                            nw.ID_ORDEN_RECIBO = r_Docnum.ToString();
                            nw.LIMITE = Convert.ToInt32(ins.LIMITE.ToString());
                            nw.CANTIDAD = Convert.ToDecimal(ins.CANTIDAD.ToString());
                            nw.ETIQUETAS = Convert.ToInt32(ins.NOETIQUETAS.ToString());
                            nw.ID_RESPONSABLE = Convert.ToInt32(idUsuarioLog.ToString());
                            nw.ESTADO = 1;
                            nw.FECHA_CREACION = DateTime.Today;
                                try
                                {
                                    db.Connection.Open();
                                    db.Transaction = db.Connection.BeginTransaction();
                                    db.TITEM_EMBALAJE_ORDENs.InsertOnSubmit(nw);
                                    db.SubmitChanges();
                                    db.Transaction.Commit();
                                }
                                catch (Exception e)
                                {
                                    db.Transaction.Rollback();
                                    
                                }
                            }

                        GC.Collect();

                    }
                    }
                }
                catch (Exception ex)
                {
                    resp = "NOK";
                    return ex.Message.ToString();
                }
            }
            else
            {
                respuesta_str = "Error en el usuario";
                resp = "NOK";
            }
            return respuesta_str;
        }

        public List<clsItemData> traeItemWhsData(string itemCode,string whscode)
        {
            List<clsItemData> tmpLista = new List<clsItemData>();
            try
            {
                clsDB cDB = new clsDB();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds = cDB.CONSULTA_SAP_ITEM_DATA(itemCode,whscode);
                dt = ds.Tables[0];


                foreach (DataRow dr in dt.Rows) //dr["UNIDADESFABRICAR"].ToString()
                {

                    clsItemData fila = new clsItemData(dr["WHSCODE"].ToString(), dr["ONHAND"].ToString(), dr["ISCOMMITED"].ToString(),dr["ONORDER"].ToString());

                    tmpLista.Add(fila);
                }
            }
            catch (Exception e)
            {
                System.Console.Write(e.Message);
            }
            return tmpLista;
        }

        public JsonResult traeJSItemWhsData(string vitemcode, string vwhscode)
        {
            List<clsItemData> tmpLista = new List<clsItemData>();
            string v_info = "0";
            try
            {

                tmpLista = traeItemWhsData(vitemcode, vwhscode);
                
            }
            catch (Exception er)
            {
                Console.WriteLine("Error en trae informacion de item" + er.Message);
            }

            if (tmpLista.Count > 0) {
                v_info = "1";
            }
            return Json(new { ID = "1", ListaItems = tmpLista, DATA = v_info });
        }
        
        public JsonResult traeLotesItem(string docentry)
        {
            List<clsItemLoteTotal> tmpLista = new List<clsItemLoteTotal>();

                try
                {
                    if (Convert.ToInt32(docentry) > 0)
                    { 
                        int v_itemcode= Convert.ToInt32(docentry);
                        tmpLista = traeLoteItem( v_itemcode);
                    }
                }
                catch (Exception er)
                {
                    Console.WriteLine("Error en docentry- ProduccionController " + er.Message);
                }
 
            return Json(new { ID = "1", ListaItems = tmpLista });
        }
        public  List<clsItemLoteTotal> traeLoteItem(int itemCode){
            List<clsItemLoteTotal> tmpLista = new List<clsItemLoteTotal>();
            try
            {
                clsDB cDB = new clsDB();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds = cDB.CONSULTA_SAP_LOTE_EMISION(itemCode);
                dt = ds.Tables[0];


                foreach (DataRow dr in dt.Rows) //dr["UNIDADESFABRICAR"].ToString()
                {

                    clsItemLoteTotal fila = new clsItemLoteTotal(dr["ITEMCODE"].ToString(),dr["WHSTOTAL"].ToString(),dr["DISTNUMBER"].ToString(),
                        dr["QUANTITY"].ToString(),dr["WHSCODE"].ToString(),dr["EXPDATE"].ToString(),dr["MNFSERIAL"].ToString(),dr["LOTNUMBER"].ToString()
                        ,dr["NOTES"].ToString());

                    tmpLista.Add(fila);
                }
            }
            catch (Exception e)
            {
                System.Console.Write(e.Message);
            }
            return tmpLista;
        }
        public  List<clsItemLoteTotal> traeLoteDevolucion(int docentry, int baseentry, string whscode){
            List<clsItemLoteTotal> tmpLista = new List<clsItemLoteTotal>();
            try
            {
                clsDB cDB = new clsDB();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds = cDB.CONSULTA_SAP_LOTE_DEVOLUCION(docentry, baseentry,whscode);
                dt = ds.Tables[0];


                foreach (DataRow dr in dt.Rows) //dr["UNIDADESFABRICAR"].ToString()
                {

                    clsItemLoteTotal fila = new clsItemLoteTotal(dr["ITEMCODE"].ToString(),dr["WHSTOTAL"].ToString(),dr["DISTNUMBER"].ToString(),
                        dr["QUANTITY"].ToString(),dr["WHSCODE"].ToString(),dr["EXPDATE"].ToString(),dr["MNFSERIAL"].ToString(),dr["LOTNUMBER"].ToString()
                        ,dr["NOTES"].ToString());

                    tmpLista.Add(fila);
                }
            }
            catch (Exception e)
            {
                System.Console.Write(e.Message);
            }
            return tmpLista;
        }
        public JsonResult traeLotesItemDevolucion(string docentry, string baseentry, string whscode)
        {
            List<clsItemLoteTotal> tmpLista = new List<clsItemLoteTotal>();
 
                try
                {
                    if (Convert.ToInt32(docentry) > 0)
                    {
                        
                        int v_docentry=Convert.ToInt32(docentry);
                        int v_baseentry=Convert.ToInt32(baseentry);
                        tmpLista = traeLoteDevolucion(v_docentry, v_baseentry, whscode);
                    }
                }
                catch (Exception er)
                {
                    Console.WriteLine("Error en docentry- ProduccionController " + er.Message);
                }
 

            return Json(new { ID = "1", ListaItems = tmpLista });
        }

        public JsonResult traeWareHouse()
        {
            List<CatalogoModel.MyDropList> lista = new List<CatalogoModel.MyDropList>();

            lista = traeAlmacen();


            return Json(lista, JsonRequestBehavior.AllowGet);
        }
        public JsonResult traeSerie(string value)
        {

            int vObjCodeDoc = 0;
            if (value == "ORDEN")
            {
                //59 recibo de fabricacion
                vObjCodeDoc = 59;
            }
            if (value == "EMISION")
            {
                //60 emision de fabricacion
                vObjCodeDoc = 60;
            }

            clsDB db = new clsDB();
            string serie = "";
            try
            {


                string idUsuarioLog = Session["LogedUserID"].ToString();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds = db.CONSULTA_SERIES_USER(Convert.ToInt32(idUsuarioLog));
                dt = ds.Tables[0];


                foreach (DataRow dr in dt.Rows)
                {
                    if (value == "ORDEN")
                    {
                        serie = dr["SERIE_RECIBO"].ToString();
                    }
                    else if (value == "EMISION")
                    {
                        serie = dr["SERIE_EMISION"].ToString();
                    }
                }

            }
            catch (Exception e)
            {
                System.Console.Write(e.Message);
            }

            List<MyDropList> lista = new List<MyDropList>();
 
            Console.WriteLine("Conectado");

            List<MyDropList> listaTMP = new List<MyDropList>();
               
            lista = traeListaSerie(vObjCodeDoc);

            foreach (MyDropList tmp in lista)
            {
                if (tmp.value == serie)
                {

                    listaTMP.Add(tmp);
                }
            }

            lista = listaTMP;
 
            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        public JsonResult traeSeriesUser()
        {
            slnQF.Models.UsuarioModel.clsUsuarioSerie series = new slnQF.Models.UsuarioModel.clsUsuarioSerie();

            clsDB db = new clsDB();
            string serieRecibo = "";
            string serieEmision = "";
            try
            {
                string idUsuarioLog = Session["LogedUserID"].ToString();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds = db.CONSULTA_SERIES_USER(Convert.ToInt32(idUsuarioLog));
                dt = ds.Tables[0];


                foreach (DataRow dr in dt.Rows)
                {

                    serieRecibo = dr["SERIE_RECIBO"].ToString();

                    serieEmision = dr["SERIE_EMISION"].ToString();
                    series.serieEmision = serieEmision;
                    series.serieRecibo = serieRecibo;
                }

            }
            catch (Exception e)
            {
                System.Console.Write(e.Message);
            }

            return Json(series, JsonRequestBehavior.AllowGet);
        }

        public JsonResult traeDispensadores()
        {
            List<MyDropList> lista = new List<MyDropList>();
 
                lista = traeListaDispensadores();
 


            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        public JsonResult traeVerificadores()
        {
            List<MyDropList> lista = new List<MyDropList>();

 
            lista = traeListaVerificadores();
 

            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        public JsonResult traeListadoPorOrden(string docto, string numDoc)
        {


            List<MyDropList> lista = new List<MyDropList>();
  
            if (docto == "ORDEN")
            {
                    
                lista = traeListaRecibosXDocnum(numDoc);
            }
            else if (docto == "EMISION")
            {
                lista = traeListaEmisionesXDocnum(numDoc);
            }
           
           


            return Json(lista, JsonRequestBehavior.AllowGet);
        }
        public JsonResult actualizaOrden(string vDocEntry,string vComentario)
        {

            string resp = "NOK";
             string vsuarioLog = "";
            string idUsuarioLog = "";
            int v_band = 0;
            string respuesta_str = "";
            
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
                    v_band = 0;
                    return Json(new { ID = "-1", MENSAJE = "Reinicie la sesion del usuario.", P_OK = resp });

                }

            }
            catch
            {
                v_band = 0;
                return Json(new { ID = "-1", MENSAJE = "Reinicie la sesion del usuario.", P_OK = resp });
            }
            
            if (v_band > 0)
            {
               

                try
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
                        return Json(new { ID = "-1", MENSAJE = "Error en conexion con el SDK.", P_OK = resp });
                    }
                    else
                    {

                        oCompany.StartTransaction();
                        int respuesta = -1;

                        ProductionOrders v_StockEntry = null;

                        v_StockEntry = oCompany.GetBusinessObject(BoObjectTypes.oProductionOrders);
                        v_StockEntry.GetByKey(Convert.ToInt32(vDocEntry));
                        //v_StockEntry.Remarks = vComentario;
                        v_StockEntry.UserFields.Fields.Item("U_Observaciones").Value = vComentario;
                        
                        if (v_StockEntry.Update() != 0)
                        {
                            respuesta = 0;
                            respuesta_str = "ERROR AGREGAR LINEA" + oCompany.GetLastErrorDescription();
                            
                            //oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);    
                            oCompany.Disconnect();
                            oCompany = null;
                            GC.Collect();

                        }
                        if (respuesta != 0)
                        {
                            string docEntryDraft;
                            oCompany.GetNewObjectCode(out docEntryDraft);
                            respuesta_str = docEntryDraft;
                            resp = "OK";
                         
                            oCompany.EndTransaction(BoWfTransOpt.wf_Commit);
                            oCompany.Disconnect();
                            oCompany = null;
                            GC.Collect();
                            
                        }
                        else
                        {
                            resp = "NOK";
                            respuesta_str = "Problema con grabar el documento.";
                            oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                            oCompany.Disconnect();
                            oCompany = null;
                            GC.Collect();

                        }

                    }
                }
                catch (Exception ex)
                {
                    respuesta_str = "Excepcion de SDK " + ex.ToString();
                    resp = "NOK";
                }
            }
            else
            {
                if (v_band == -3)
                {
                    respuesta_str = "Configure renglon";
                    resp = "NOK";
                }
                else if (v_band == -4)
                {
                    respuesta_str = "Tara o peso neto no es numerico";
                    resp = "NOK";
                }
                else
                {
                    respuesta_str = "Error en el usuario";
                    resp = "NOK";
                }
            }

            return Json(new { ID = "1", MENSAJE = respuesta_str, P_OK = resp });
        }

        public JsonResult cierraOrden(string vDocEntry)
        {

            string resp = "NOK";
            string vsuarioLog = "";
            string idUsuarioLog = "";
            int v_band = 0;
            string respuesta_str = "";

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
                    v_band = 0;
                    return Json(new { ID = "-1", MENSAJE = "Reinicie la sesion del usuario.", P_OK = resp });

                }

            }
            catch
            {
                v_band = 0;
                return Json(new { ID = "-1", MENSAJE = "Reinicie la sesion del usuario.", P_OK = resp });
            }

            if (v_band > 0)
            {


                try
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
                        return Json(new { ID = "-1", MENSAJE = "Error en conexion con el SDK.", P_OK = resp });
                    }
                    else
                    {

                        oCompany.StartTransaction();
                        int respuesta = -1;

                        ProductionOrders v_StockEntry = null;

                        v_StockEntry = oCompany.GetBusinessObject(BoObjectTypes.oProductionOrders);
                        v_StockEntry.GetByKey(Convert.ToInt32(vDocEntry));
                        //v_StockEntry.Remarks = vComentario;
                        v_StockEntry.ProductionOrderStatus = BoProductionOrderStatusEnum.boposClosed;

                        if (v_StockEntry.Update() != 0)
                        {
                            respuesta = 0;
                            respuesta_str = "ERROR AGREGAR LINEA" + oCompany.GetLastErrorDescription();

                            //oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);    
                            oCompany.Disconnect();
                            oCompany = null;
                            GC.Collect();

                        }
                        if (respuesta != 0)
                        {
                            string docEntryDraft;
                            oCompany.GetNewObjectCode(out docEntryDraft);
                            respuesta_str = docEntryDraft;
                            resp = "OK";

                            oCompany.EndTransaction(BoWfTransOpt.wf_Commit);
                            oCompany.Disconnect();
                            oCompany = null;
                            GC.Collect();

                        }
                        else
                        {
                            resp = "NOK";
                            respuesta_str = "Problema con grabar el documento.";
                            oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                            oCompany.Disconnect();
                            oCompany = null;
                            GC.Collect();

                        }

                    }
                }
                catch (Exception ex)
                {
                    respuesta_str = "Excepcion de SDK " + ex.ToString();
                    resp = "NOK";
                }
            }
            else
            {
                if (v_band == -3)
                {
                    respuesta_str = "Configure renglon";
                    resp = "NOK";
                }
                else if (v_band == -4)
                {
                    respuesta_str = "Tara o peso neto no es numerico";
                    resp = "NOK";
                }
                else
                {
                    respuesta_str = "Error en el usuario";
                    resp = "NOK";
                }
            }

            return Json(new { ID = "1", MENSAJE = respuesta_str, P_OK = resp });
        }

        public JsonResult creaEmision(string linenums, string no_orden, string docentry, string jsonclsLoteSeleccionado, string jsonclsDatosMod, string docnum, string serie, string referencia, string fecha, string journal, string comentarios, string dispensador, string verificador, string vjsonstringTara)
        {

            string resp = "NOK";
            DateTime fecha_contabiliza;
            try
            {
                fecha_contabiliza = DateTime.Parse(fecha);
            }
            catch
            {
                return Json(new { ID = "-1", MENSAJE = "Error en formato de fecha de contabilizacion", P_OK = resp });
            }
            string vsuarioLog = "";
            string idUsuarioLog = "";
            int v_band = 0;

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
                    v_band = 0;
                    return Json(new { ID = "-1", MENSAJE = "Reinicie la sesion del usuario.", P_OK = resp });
                    
                }

            }
            catch
            {
                v_band = 0;
                return Json(new { ID = "-1", MENSAJE = "Reinicie la sesion del usuario.", P_OK = resp });
            }
            string respuesta_str = "";
            List<clsTarasRenglon> listaTaraSeleccionados = new List<clsTarasRenglon>();
            try{
            
                clsTarasRenglon itemTaraSel;
                List<emisionclsTara> dynJsonTara = JsonConvert.DeserializeObject <List<emisionclsTara>>(vjsonstringTara);
                foreach (emisionclsTara item in dynJsonTara)
                {
                    if (item != null)
                    {
                        itemTaraSel = new clsTarasRenglon(item.LINENUM.ToString(),item.TARA.ToString(),item.PESONETO.ToString(),"",idUsuarioLog);
                        try{
                            decimal num = Convert.ToDecimal(item.TARA.ToString());
                            decimal num2 = Convert.ToDecimal(item.PESONETO.ToString());
                        }catch{
                            v_band= -4;
                        }
                        listaTaraSeleccionados.Add(itemTaraSel);
                    }
                }
            }catch(Exception er){
                v_band =-3;
                return Json(new { ID = "-3", MENSAJE = "Error en la tara de los items." + er , P_OK = resp });
            }
            if (v_band > 0)
            {
                List<clsLoteSeleccionado> listaLoteSeleccionados = new List<clsLoteSeleccionado>();
                clsLoteSeleccionado itemLoteSel;
                List<emisionclsLoteSeleccionado> dynJson = JsonConvert.DeserializeObject<List<emisionclsLoteSeleccionado>>(jsonclsLoteSeleccionado);
                foreach (emisionclsLoteSeleccionado item in dynJson)
                {
                    if ((item != null) && ( item.TTOTAL.ToString()!="0"))
                    {
                        itemLoteSel = new clsLoteSeleccionado(item.ITEMCODE.ToString(), "", item.BATCHNUM.ToString(), item.QUANTITY.ToString(), item.TTOTAL.ToString(), item.FECHA_EXP.ToString(), item.NUEVO.ToString(),"");
                        listaLoteSeleccionados.Add(itemLoteSel);
                    }
                }


                List<clsItemDatosModProd> listaItemsMod = new List<clsItemDatosModProd>();
                clsItemDatosModProd itemItemsMod;
               
                
                List<emisionclsDatosModProd> dynJson2 = JsonConvert.DeserializeObject<List<emisionclsDatosModProd>>(jsonclsDatosMod);
                foreach (emisionclsDatosModProd item in dynJson2)
                {
                    itemItemsMod = new clsItemDatosModProd(item.linenum.ToString(), item.QUANTITY.ToString(), item.WHSHOUSE.ToString(), item.RENGLON.ToString(), item.TARA.ToString());
                    listaItemsMod.Add(itemItemsMod);
                }

                string[] linumesList = linenums.Split(',');

                List<clsItemsProduction> items2 = listaOrdenesDocentry(docentry);
                //List<clsItemsProduction> items2 = new List<clsItemsProduction>();

                foreach (clsItemsProduction item in items2)
                {
                    
                    foreach (clsItemDatosModProd itemTmp in listaItemsMod.Where(x => x.linenum == item.linenum))
                    {
                        if (item.linenum == itemTmp.linenum)
                        {
                            item.quantity = itemTmp.QUANTITY;
                            item.warehouse = itemTmp.WHSHOUSE;

                            if (itemTmp.RENGLON == "0")
                            {
                                item.renglon = itemTmp.RENGLON;
                                string v_taraDat = itemTmp.TARA;
                                if (v_taraDat.Length<=0){
                                    v_taraDat= "0";
                                }
                                item.TARA = v_taraDat;
                            }else{
                                int v_cuentaRenglon = 0;
                                foreach (clsTarasRenglon item3 in listaTaraSeleccionados)
                                {
                                    if (item3.LINENUM == itemTmp.linenum)
                                    {
                                        v_cuentaRenglon = v_cuentaRenglon +1;
                                    }
                                }
                                item.renglon = v_cuentaRenglon.ToString();
                                item.TARA = itemTmp.TARA;
                            }
                           
                        }

                    }

                } 
                
                try
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
                        return Json(new { ID = "-1", MENSAJE = "Error en conexion con el SDK.", P_OK = resp });
                    }
                    else
                    {
                        
                        oCompany.StartTransaction();
                        int respuesta = -1;

                        Documents v_StockEntry = null;

                        v_StockEntry = oCompany.GetBusinessObject(BoObjectTypes.oInventoryGenExit);

                        v_StockEntry.HandWritten = BoYesNoEnum.tNO;

                        v_StockEntry.DocDate = fecha_contabiliza;

                        v_StockEntry.Series = Convert.ToInt32(serie);

                        v_StockEntry.DocNum = Convert.ToInt32(docnum);

                        v_StockEntry.UserFields.Fields.Item("U_SysUser").Value = vsuarioLog;
                        if (dispensador.Length > 0)
                        {
                            v_StockEntry.UserFields.Fields.Item("U_DISPENSADOR").Value = dispensador;
                        }
                        if (verificador.Length > 0)
                        {
                            v_StockEntry.UserFields.Fields.Item("U_VERIFICADOR").Value = verificador;
                        }
                    
                        v_StockEntry.Reference2 = referencia;

                        int agregar = 0;
                        int Row = 0;
                        List<linenumPAR> lineasID= new List<linenumPAR>();
                        List<string> lineasAGG= new List<string>();
                        int contador = 0;
                        foreach (clsItemsProduction b in items2)
                        {
                            agregar = 0;
                            foreach (string line in linumesList)
                            {
                                if (b.linenum == line)
                                {
                                    agregar = 1;
                                    lineasAGG.Add(line);

                                }
                            }
                            if (agregar == 1)
                            {
                                linenumPAR line = new linenumPAR(Convert.ToInt32(b.linenum),contador);
                                lineasID.Add(line);
                                contador ++;
                                lineasID.Add(line);
                                v_StockEntry.Lines.SetCurrentLine(Row);
                                v_StockEntry.Lines.BaseEntry = Convert.ToInt32(docentry);
                                v_StockEntry.Lines.BaseType = 202;
                                v_StockEntry.Lines.BaseLine = Convert.ToInt32(b.linenum);
                                //v_StockEntry.Lines.ItemCode = b.itemCode;
                                //v_StockEntry.Lines.ItemDescription = b.itemName;
                                v_StockEntry.Lines.Quantity = Convert.ToDouble(b.quantity);
                                v_StockEntry.Lines.WarehouseCode = b.warehouse;

                                v_StockEntry.Lines.UserFields.Fields.Item("U_LOTE").Value = b.lote;
                                v_StockEntry.Lines.UserFields.Fields.Item("U_Renglon").Value = b.renglon;
                                v_StockEntry.Lines.UserFields.Fields.Item("U_Tara").Value = b.TARA;
                                //listaLoteSeleccionados

                                foreach (clsLoteSeleccionado c in listaLoteSeleccionados)
                                {
                                    if ((c.ITEMCODE == b.itemCode) && (c.TOTAL.Length > 0))
                                    {
                                        v_StockEntry.Lines.BatchNumbers.BatchNumber = c.BATCHNUM;
                                        v_StockEntry.Lines.BatchNumbers.Quantity = Convert.ToDouble(c.TOTAL);
                                        v_StockEntry.Lines.BatchNumbers.BaseLineNumber = Row;
                                        v_StockEntry.Lines.BatchNumbers.Add();
                                    }
                                }
                                v_StockEntry.Lines.Add();
                                Row++;
                            }

                        }
                    
                        v_StockEntry.JournalMemo = journal;
                        v_StockEntry.Comments = comentarios;
                    
                        if (v_StockEntry.Add() != 0)
                        {
                            respuesta = 0;
                            respuesta_str = "ERROR AGREGAR LINEA: " + oCompany.GetLastErrorDescription();
                            
                            //oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);    
                            oCompany.Disconnect();
                            oCompany  = null;
                            GC.Collect();

                        }
                        if (respuesta != 0)
                        {
                            string docEntryDraft;
                            oCompany.GetNewObjectCode(out docEntryDraft);
                            respuesta_str = docEntryDraft;
                            
                            clsDbReporte cDB  = new clsDbReporte();
                            string p_msg1;
                            int v_cuentarenglon  = 0;
                            int v_linenumant =0;
                            oCompany.EndTransaction(BoWfTransOpt.wf_Commit);
                            oCompany.Disconnect();
                            oCompany = null;
                            GC.Collect();
                            try{
                            foreach (clsTarasRenglon item3 in listaTaraSeleccionados)
                                    { 
                                    int v_cuenta_liag =0;
                                    foreach(string lineaagrega in lineasAGG){
                                        if (lineaagrega == item3.LINENUM){
                                            v_cuenta_liag++;
                                        }
                                    }
                                        if (v_cuenta_liag>0){
                                         if(v_cuentarenglon >0){
                                                if(v_linenumant!=Convert.ToInt32(item3.LINENUM))
                                                    v_cuentarenglon = 0;
                                             }
                                         int resp_pro = 0;
                                         v_cuentarenglon = v_cuentarenglon +1;
                                            int lineaA=-01;
                                            foreach(linenumPAR line in lineasID) {
                                                if(line.linenum1==Convert.ToInt32(item3.LINENUM)){
                                                    lineaA = line.linenum2;
                                                }
                                            }

                                            resp_pro = cDB.INGRESA_DATOS(lineaA,v_cuentarenglon,
                                                Convert.ToDecimal(item3.TARA), 
                                                 Convert.ToDecimal(item3.PESO_NETO),
                                                 Convert.ToInt32(docEntryDraft), 
                                                 Convert.ToInt32(idUsuarioLog),out p_msg1);
                                            v_linenumant = Convert.ToInt32(item3.LINENUM);
                                            }
                                    }
                            resp = "OK";
                            

                            }catch(Exception e){
                                resp = "NOK";
                                respuesta_str ="Problema con grabar el documento, en ingresar tara." + e.Message;
                                oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);    
                                oCompany.Disconnect();
                                oCompany  = null;
                                GC.Collect();

                            }
                            
                        }
                        
                    }
                }
                catch (Exception ex)
                {
                    respuesta_str = "Excepcion de SDK " + ex.ToString();
                    resp = "NOK";
                }
            }
            else
            {
                if (v_band==-3){
                    respuesta_str = "Configure renglon";
                    resp = "NOK";
                }else if(v_band==-4){
                    respuesta_str = "Tara o peso neto no es numerico";
                    resp = "NOK";
                }else{
                respuesta_str = "Error en el usuario";
                resp = "NOK";
                    }
            }

            return Json(new { ID = "1", MENSAJE = respuesta_str, P_OK = resp });
        }

        public JsonResult creaDevuelve(string linenums, string no_orden, string docentry, string jsonclsLoteSeleccionado, string jsonclsDatosMod, string docnum, string serie, string referencia, string comentarios, string journal, string fecha,string verificador)
        {

            string vsuarioLog = "";
            string idUsuarioLog = "";
            int v_band = 0;

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
                    return Json(new { ID = "-3", MENSAJE = "Reinicie la sesion del usuario.", P_OK = "" });
                    v_band = 0;
                }

            }
            catch
            {
                return Json(new { ID = "-3", MENSAJE = "Reinicie la sesion del usuario.", P_OK = "" });
                v_band = 0;

            }
            string respuesta_str = "";
            string resp = "NOK";
            if (v_band > 0)
            {

                DateTime fecha_contabiliza;
                try
                {
                    fecha_contabiliza = DateTime.Parse(fecha);
                }
                catch
                {
                    return Json(new { ID = "-1", MENSAJE = "Error en formato de fecha de contabilizacion", P_OK = resp });
                }


                List<clsLoteSeleccionado> listaLoteSeleccionados = new List<clsLoteSeleccionado>();
                clsLoteSeleccionado itemLoteSel;
                List<emisionclsLoteSeleccionado> dynJson = JsonConvert.DeserializeObject<List<emisionclsLoteSeleccionado>>(jsonclsLoteSeleccionado);
                foreach (emisionclsLoteSeleccionado item in dynJson)
                {
                    if (item != null)
                    {
                        itemLoteSel = new clsLoteSeleccionado(item.ITEMCODE.ToString(), "", item.BATCHNUM.ToString(), item.QUANTITY.ToString(), item.TTOTAL.ToString(), item.FECHA_EXP.ToString(), item.NUEVO.ToString(),"");
                        listaLoteSeleccionados.Add(itemLoteSel);
                    }
                }

                List<clsItemDatosModProd> listaItemsMod = new List<clsItemDatosModProd>();
                clsItemDatosModProd itemItemsMod; 
                List<devolucionclsDatosModProd> dynJson2 = JsonConvert.DeserializeObject<List<devolucionclsDatosModProd>>(jsonclsDatosMod);
                foreach (devolucionclsDatosModProd item in dynJson2)
                {
                    itemItemsMod = new clsItemDatosModProd(item.linenum.ToString(), item.QUANTITY.ToString(), item.WHSHOUSE.ToString(), "", "");
                    listaItemsMod.Add(itemItemsMod);

                }

                string[] linumesList = linenums.Split(',');
                List<clsItemsProduction> items2 = listaOrdenesDocentry(docentry);
                foreach (clsItemsProduction item in items2)
                {

                    foreach (clsItemDatosModProd item2 in listaItemsMod)
                    {
                        if (item.linenum == item2.linenum)
                        {
                            item.quantity = item2.QUANTITY;
                            item.warehouse = item2.WHSHOUSE;
                        }

                    }

                }

                try
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
                        
                        oCompany.Disconnect();
                        oCompany  = null;
                        GC.Collect();

                        //Console.ReadKey();
                    }
                    else
                    {
                     try{
                        oCompany.StartTransaction();
                        int respuesta = -1;

                        Documents v_StockEntry = null;

                        v_StockEntry = oCompany.GetBusinessObject(BoObjectTypes.oInventoryGenEntry);

                        v_StockEntry.HandWritten = BoYesNoEnum.tNO;

                        v_StockEntry.DocDate = fecha_contabiliza;

                        v_StockEntry.Series = Convert.ToInt32(serie);

                        v_StockEntry.DocNum = Convert.ToInt32(docnum);

                        v_StockEntry.Reference2 = referencia;

                        v_StockEntry.UserFields.Fields.Item("U_SysUser").Value = vsuarioLog;
                        if (verificador.Length > 0)
                        {
                            v_StockEntry.UserFields.Fields.Item("U_VERIFICADOR").Value = verificador;
                        }
                    
                        int agregar = 0;
                        int Row = 0;
                        foreach (clsItemsProduction b in items2)
                        {
                            agregar = 0;
                            foreach (string line in linumesList)
                            {
                                if (b.linenum == line)
                                {
                                    agregar = 1;
                                }
                            }
                            if (agregar == 1)
                            {


                                v_StockEntry.Lines.SetCurrentLine(Row);
                                v_StockEntry.Lines.BaseEntry = Convert.ToInt32(docentry);

                                v_StockEntry.Lines.BaseLine = Convert.ToInt32(b.linenum);
                                //v_StockEntry.Lines.ItemCode = b.itemCode;
                                //v_StockEntry.Lines.ItemDescription = b.itemName;
                                v_StockEntry.Lines.Quantity = Convert.ToDouble(b.quantity);
                                v_StockEntry.Lines.WarehouseCode = b.warehouse;



                                //listaLoteSeleccionados

                                foreach (clsLoteSeleccionado c in listaLoteSeleccionados)
                                {
                                    if ((c.ITEMCODE == b.itemCode) && (c.TOTAL.Length > 0))
                                    {
                                        v_StockEntry.Lines.BatchNumbers.BatchNumber = c.BATCHNUM;
                                        v_StockEntry.Lines.BatchNumbers.Quantity = Convert.ToDouble(c.TOTAL);
                                   
                                        v_StockEntry.Lines.BatchNumbers.BaseLineNumber = Row;
                                        v_StockEntry.Lines.BatchNumbers.Add();
                                    }
                                }


                                v_StockEntry.Lines.Add();
                                Row++;
                            }

                        }
                        v_StockEntry.Comments = comentarios;
                        v_StockEntry.JournalMemo = journal;
                        
                        if (v_StockEntry.Add() != 0)
                        {
                            respuesta = 0;
                            respuesta_str = "ERROR AGREGAR LINEA" + oCompany.GetLastErrorDescription();
                            
                            //oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);    
                            oCompany.Disconnect();
                            oCompany  = null;
                            GC.Collect();

                        }

                        if (respuesta != 0)
                        {
                            string docEntryDraft;
                            oCompany.GetNewObjectCode(out docEntryDraft);
                            respuesta_str = docEntryDraft;
                            resp = "OK";
                            
                            oCompany.EndTransaction(BoWfTransOpt.wf_Commit);    
                            oCompany.Disconnect();
                            oCompany  = null;
                            GC.Collect();

                        }
                        
                     }catch(Exception exp){
                        oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);    
                        oCompany.Disconnect();
                        oCompany  = null;
                        GC.Collect();

                        resp = "NOK";
                        respuesta_str = "ERROR AGREGAR DOCUMENTO: " + exp.Message;
                     }
                    } 
                }
                catch (Exception ex)
                {
                    respuesta_str = "Excepcion de SDK " + ex.ToString();
                }
            }
            else
            {
                respuesta_str = "Error en el usuario";
                resp = "NOK";
            }

            return Json(new { ID = "1", MENSAJE = respuesta_str, P_OK = resp });
        }

        
          public List<UsuarioModel.cls_bodega_asocia> traeBodegaAsocia(string p_bodega)
        {
            List<UsuarioModel.cls_bodega_asocia> listaUsers = new List<UsuarioModel.cls_bodega_asocia>();
              
            clsDB cDB2 = new clsDB();
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds = cDB2.CONSULTA_BODEGA_USUARIOS(Convert.ToInt32(p_bodega));
                dt = ds.Tables[0];


                foreach (DataRow dr in dt.Rows)
                {
                    UsuarioModel.cls_bodega_asocia fila = new UsuarioModel.cls_bodega_asocia(dr["ID_BODEGA"].ToString(), dr["NOMBRE"].ToString(), dr["SW_ASOCIA"].ToString());

                    if (  dr["SW_ASOCIA"].ToString() =="1")
                    listaUsers.Add(fila);
                }

            }
            catch (Exception e)
            {
                System.Console.Write(e.Message);
            }
            return listaUsers;
        }
        
        public List<string> traeSupervisaUser(string p_user)
        {
            
            clsDB cDB2 = new clsDB();
            List<string> listaUsers = new List<string>();

            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds = cDB2.CONSULTA_SUPERVISA_USER(Convert.ToInt32(p_user));
                dt = ds.Tables[0];


                foreach (DataRow dr in dt.Rows)
                {
                    string fila = dr[0].ToString();

                   
                    listaUsers.Add(fila);
                }

            }
            catch (Exception e)
            {
                System.Console.Write(e.Message);
            }
            return listaUsers;
        }
        
        public IList<ClsEtiquetaPesado> traeEtiquetas(string p_docnum)
        {
            clsDbReporte cDB = new clsDbReporte();
            IList<ClsEtiquetaPesado> ListaEtiquetas = new List<ClsEtiquetaPesado>();
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds = cDB.TRAE_ETIQUETAS(p_docnum);
                dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    decimal vpesoneto =0;
                    decimal vtara =0;
                    decimal total = 0;
                    //dr["ID_USUARIO"].ToString() 
                    //row[0].ToString()) 
                    try{


                        vpesoneto = Convert.ToDecimal(dr["PesoNeto"].ToString());
                        }
                    catch{
                        vpesoneto =0;   
                        }

                    try{
                        vtara =Convert.ToDecimal(dr["TARA"].ToString());
                            }catch{
                                vtara= 0;
                            }
                    total = vpesoneto+ vtara;
                    ClsEtiquetaPesado fila = new ClsEtiquetaPesado(dr["U_NombrePO"].ToString(), dr["U_LOTE"].ToString(), dr["DocNum"].ToString(), dr["ItemCode"].ToString(), dr["bulto"].ToString(), dr["bultofin"].ToString(), dr["ItemName"].ToString(), dr["ExpDate"].ToString(), dr["DistNumber"].ToString(), dr["Notes"].ToString(), dr["POTENCIA"].ToString(), dr["PesoNeto"].ToString(), dr["TARA"].ToString(), DateTime.Now.ToString("dd/MM/yyyy"),total.ToString() , dr["Dispensado"].ToString(), dr["Verificado"].ToString(), dr["UOM"].ToString(), dr["BaseRef"].ToString());

                    ListaEtiquetas.Add(fila);
                }
            }
            catch (Exception e)
            {
                System.Console.Write(e.Message);
            }
            return ListaEtiquetas;
        }

        public IList<ClsAnexoB> traeEtiquetasB(string p_docnum)
        {
            clsDbReporte cDB = new clsDbReporte();
            IList<ClsAnexoB> ListaEtiquetas = new List<ClsAnexoB>();
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds = cDB.TRAE_ETIQUETASB(p_docnum);
                dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    //dr["ID_USUARIO"].ToString() 
                    //row[0].ToString()) 
                    string nombre_User = trae_NombreUser(dr["OPERADOR"].ToString());
                    ClsAnexoB fila = new ClsAnexoB(dr["ORDEN_PRODUCCION"].ToString(), dr["NOMBRE_PRODUCTO"].ToString(), dr["LOTE"].ToString(), dr["ORDEN_EMISION"].ToString(), dr["ITEMCODE"].ToString(), dr["invntryUom"].ToString(), dr["LOTE_ITEM"].ToString(), dr["Quantity"].ToString(), dr["ItemName"].ToString(), dr["LINENUM"].ToString(), nombre_User,Convert.ToDateTime(dr["DOCTIME"].ToString()),dr["VERIFICADOR"].ToString(),dr["COMENTARIOS"].ToString(),dr["REF2"].ToString(),dr["U_VENCE"].ToString(),dr["BULTO"].ToString(),dr["BULTO_DE"].ToString(),dr["U_U_FABRICAR"].ToString());

                    ListaEtiquetas.Add(fila);
                }
            }
            catch (Exception e)
            {
                System.Console.Write(e.Message);
            }
            return ListaEtiquetas;
        }

        public IList<ClsAnexoB> traeEtiquetasD(string p_docnum)
        {
            clsDbReporte cDB = new clsDbReporte();
            IList<ClsAnexoB> ListaEtiquetas = new List<ClsAnexoB>();
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds = cDB.TRAE_ETIQUETASD(p_docnum);
                dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    //dr["ID_USUARIO"].ToString() 
                    //row[0].ToString()) 
                    string nombre_User = trae_NombreUser(dr["OPERADOR"].ToString());
                    ClsAnexoB fila = new ClsAnexoB(dr["ORDEN_PRODUCCION"].ToString(), dr["NOMBRE_PRODUCTO"].ToString(), dr["LOTE"].ToString(), dr["ORDEN_EMISION"].ToString(), dr["ITEMCODE"].ToString(), dr["invntryUom"].ToString(), dr["LOTE_ITEM"].ToString(), dr["Quantity"].ToString(), dr["ItemName"].ToString(), dr["LINENUM"].ToString(), nombre_User,Convert.ToDateTime(dr["DOCTIME"].ToString()),dr["VERIFICADOR"].ToString(),dr["COMENTARIOS"].ToString(),dr["REF2"].ToString(),dr["U_VENCE"].ToString(),dr["BULTO"].ToString(),dr["BULTO_DE"].ToString(),dr["U_U_FABRICAR"].ToString());

                    ListaEtiquetas.Add(fila);
                }
            }
            catch (Exception e)
            {
                System.Console.Write(e.Message);
            }
            return ListaEtiquetas;
        }

        public IList<ClsAnexoB> traeEtiquetasE(string p_docnum)
        {
            clsDbReporte cDB = new clsDbReporte();
            IList<ClsAnexoB> ListaEtiquetas = new List<ClsAnexoB>();
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds = cDB.TRAE_ETIQUETASE(p_docnum);
                dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    //dr["ID_USUARIO"].ToString() 
                    //row[0].ToString()) 
                    string nombre_User = trae_NombreUser(dr["OPERADOR"].ToString());
                    ClsAnexoB fila = new ClsAnexoB(dr["ORDEN_PRODUCCION"].ToString(), dr["NOMBRE_PRODUCTO"].ToString(), dr["LOTE"].ToString(), dr["ORDEN_EMISION"].ToString(), dr["ITEMCODE"].ToString(), dr["invntryUom"].ToString(), dr["LOTE_ITEM"].ToString(), dr["Quantity"].ToString(), dr["ItemName"].ToString(), dr["LINENUM"].ToString(), nombre_User,Convert.ToDateTime(dr["DOCTIME"].ToString()),dr["VERIFICADOR"].ToString(),dr["COMENTARIOS"].ToString(),dr["REF2"].ToString(),dr["U_VENCE"].ToString(),dr["BULTO"].ToString(),dr["BULTO_DE"].ToString(),dr["U_U_FABRICAR"].ToString());

                    ListaEtiquetas.Add(fila);
                }
            }
            catch (Exception e)
            {
                System.Console.Write(e.Message);
            }
            return ListaEtiquetas;
        }
        public FileContentResult GenerateAndDisplayReport(string docnum, string format)
        {

            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath("~/Reportes/rptAnexoA.rdlc");
            IList<ClsPrueba> ListatPAGO = new List<ClsPrueba>();
            ListatPAGO.Add(new ClsPrueba("1", "1"));
            ListatPAGO.Add(new ClsPrueba("1", "2"));
            ListatPAGO.Add(new ClsPrueba("1", "3"));
            ListatPAGO.Add(new ClsPrueba("1", "4"));
            ListatPAGO.Add(new ClsPrueba("1", "5"));


            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "DataSet1";

            reportDataSource.Value = ListatPAGO;

            localReport.DataSources.Add(reportDataSource);

            string renderFormat = "";
            if (format == "PDF")
            {
                renderFormat = "pdf";
            }
            else
            {
                renderFormat = "Image";
            }
            string reportType = "Image";
            string mimeType;
            string encoding;
            string fileNameExtension;
            //The DeviceInfo settings should be changed based on the reportType            
            //http://msdn2.microsoft.com/en-us/library/ms155397.aspx            
            string deviceInfo = "<DeviceInfo>" +
                "  <OutputFormat>pdf</OutputFormat>" +
                "  <PageWidth>10cm</PageWidth>" +
                "  <PageHeight>10cm</PageHeight>" +
                "  <MarginTop>0in</MarginTop>" +
                "  <MarginLeft>0in</MarginLeft>" +
                "  <MarginRight>0in</MarginRight>" +
                "  <MarginBottom>0in</MarginBottom>" +
                "</DeviceInfo>";
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;
            //Render the report      

            renderedBytes = localReport.Render(renderFormat, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

            byte[] archivos;

            List<byte[]> list = new List<byte[]> ();

            list.Add(renderedBytes);
            list.Add(renderedBytes);
            archivos = ConcatenatePdfs(list);
            //Response.AddHeader("content-disposition", "attachment; filename=NorthWindCustomers." + fileNameExtension); 
            if (format == null)
            {
                return File(renderedBytes, "image/jpeg");
            }
            else if (format == "PDF")
            {
                return File(archivos, "pdf");
            }
            else
            {
                return File(renderedBytes, "image/jpeg");
            }


        }
        public FileResult DownloadResumen(string docnum)
        {

            try
            {
                DC_CALIDADDataContext db2 = new DC_CALIDADDataContext();
                List<RPT_RESUMEN_EMBALAJEResult> list = db2.RPT_RESUMEN_EMBALAJE(Convert.ToInt32(docnum))
                   .ToList<RPT_RESUMEN_EMBALAJEResult>();

                
                ReportDocument rd = new ReportDocument();
                
                    rd.Load(Path.Combine(Server.MapPath("~/Reportes"), "reporteResumen.rpt"));
                 

                rd.SetParameterValue("@DOCNUMSAP", docnum);
                rd.SetDataSource(list);
                
                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();


                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "reporteResumen"+docnum+".pdf");
            }
            catch (Exception ex)
            {
                DC_CALIDADDataContext db3 = new DC_CALIDADDataContext();
                db3.SP_INSERT_LOG_ERROR(ex.Message, "", DateTime.Now, "docnum" + Json(docnum), "", "", "", "", "", "", "", "", "SLNQF DOWNLOADRESUMEN");
                return null;
            }





        }
        public string DownloadResumen2(string docnum)
        {

            try
            {
                DC_CALIDADDataContext db2 = new DC_CALIDADDataContext();
                List<RPT_RESUMEN_EMBALAJEResult> list = db2.RPT_RESUMEN_EMBALAJE(Convert.ToInt32(docnum))
                   .ToList<RPT_RESUMEN_EMBALAJEResult>();


                ReportDocument rd = new ReportDocument();

                rd.Load(Path.Combine(Server.MapPath("~/Reportes"), "reporteResumen.rpt"));
                
                rd.SetParameterValue("@DOCNUMSAP", docnum);
                rd.SetDataSource(list);

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();


                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                

                String file = Convert.ToBase64String(ReadFully(stream));
                return file;
            }
            catch (Exception ex)
            {
                DC_CALIDADDataContext db3 = new DC_CALIDADDataContext();
                db3.SP_INSERT_LOG_ERROR(ex.Message,"",DateTime.Now,"DOCNUM" + docnum.ToString(), "", "", "", "", "", "", "", "", "");
                return null;
            }





        }

        public FileResult DownloadEtiqueta(string docnum)
        {
             
            try {
                DC_CALIDADDataContext db2 = new DC_CALIDADDataContext();
                List<RPT_ETIQUETA_EMBALAJEResult> list = db2.RPT_ETIQUETA_EMBALAJE(Convert.ToInt32(docnum))
                   .ToList<RPT_ETIQUETA_EMBALAJEResult>();
                 
                ReportDocument rd = new ReportDocument();
                if (list[0].ID_TIPO_ETIQUETA == 1)
                    rd.Load(Path.Combine(Server.MapPath("~/Reportes"), "reporteEmbalaje4x6Carta.rpt"));
                else
                    rd.Load(Path.Combine(Server.MapPath("~/Reportes"), "reporteEmbalajeSemi.rpt"));

                rd.SetParameterValue("@DOCNUMSAP", docnum);
                rd.SetDataSource(list);

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();


                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "reporteEtiqueta" + docnum + ".pdf");
            }
            catch (Exception ex) {
                DC_CALIDADDataContext db3 = new DC_CALIDADDataContext();

                db3.SP_INSERT_LOG_ERROR(ex.Message, "", DateTime.Now, "docnum" + Json(docnum), "", "", "", "", "", "", "", "", "SLNQF DOWNLOADETIQEUTA");
                return null;
            }

           



        }

        public string DownloadEtiqueta2(string docnum)
        {

            try
            {
                DC_CALIDADDataContext db2 = new DC_CALIDADDataContext();
                List<RPT_ETIQUETA_EMBALAJEResult> list = db2.RPT_ETIQUETA_EMBALAJE(Convert.ToInt32(docnum))
                   .ToList<RPT_ETIQUETA_EMBALAJEResult>();
                int tipoEtiqueta;
                try
                {
                    tipoEtiqueta = list[0].ID_TIPO_ETIQUETA;
                }
                catch {
                    tipoEtiqueta = 0;
                }

                ReportDocument rd = new ReportDocument();
                if (tipoEtiqueta == 1)
                    rd.Load(Path.Combine(Server.MapPath("~/Reportes"), "reporteEmbalaje4x6.rpt"));
                else
                    rd.Load(Path.Combine(Server.MapPath("~/Reportes"), "reporteEmbalajeSemi.rpt"));

                rd.SetParameterValue("@DOCNUMSAP", docnum);
                rd.SetDataSource(list);

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();


                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);

                String file = Convert.ToBase64String(ReadFully(stream));
                return file;
            }
            catch (Exception ex)
            {
                DC_CALIDADDataContext db3 = new DC_CALIDADDataContext();
                db3.SP_INSERT_LOG_ERROR(ex.Message, "", DateTime.Now, "docnum" + Json(docnum), "", "", "", "", "", "", "", "", "SLNQF DOWNLOADETIQEUTA2");

                return null;
            }
        }

        public string DownloadEtiqueta2Carta(string docnum)
        {

            try
            {
                DC_CALIDADDataContext db2 = new DC_CALIDADDataContext();
                List<RPT_ETIQUETA_EMBALAJEResult> list = db2.RPT_ETIQUETA_EMBALAJE(Convert.ToInt32(docnum))
                   .ToList<RPT_ETIQUETA_EMBALAJEResult>();
                int tipoEtiqueta;
                try
                {
                    tipoEtiqueta = list[0].ID_TIPO_ETIQUETA;
                }
                catch
                {
                    tipoEtiqueta = 0;
                }

                ReportDocument rd = new ReportDocument();
                if (tipoEtiqueta == 1)
                    rd.Load(Path.Combine(Server.MapPath("~/Reportes"), "reporteEmbalaje4x6Carta.rpt"));
                else
                    rd.Load(Path.Combine(Server.MapPath("~/Reportes"), "reporteEmbalajeSemiCarta.rpt"));

                rd.SetParameterValue("@DOCNUMSAP", docnum);
                rd.SetDataSource(list);

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();


                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                String file = Convert.ToBase64String(ReadFully(stream));
                return file;
            }
            catch (Exception ex)
            {
                DC_CALIDADDataContext db3 = new DC_CALIDADDataContext();
                db3.SP_INSERT_LOG_ERROR(ex.Message, "", DateTime.Now, "docnum" + Json(docnum), "", "", "", "", "", "", "", "", "SLNQF DOWNLOADETIQEUTA2carta");
                return null;
            }





        }

        public FileResult DownloadReport(string docnum, string format)
        {
             
            IList<ClsEtiquetaPesado> list = new List<ClsEtiquetaPesado>();
            list = traeEtiquetas(docnum);
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reportes"), "reporteAnexoA.rpt"));
           
            rd.SetDataSource(list);
            rd.SetParameterValue("titulo", "ETIQUETA DE DISPENSADO");
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);


            return File(stream, "application/pdf", "reporteAnexoA"+ docnum+".pdf");



        }

        public string DownloadReport2(string docnum, string format)
        {

            IList<ClsEtiquetaPesado> list = new List<ClsEtiquetaPesado>();
            list = traeEtiquetas(docnum);
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reportes"), "reporteAnexoA.rpt"));

            rd.SetDataSource(list);
            rd.SetParameterValue("titulo", "ETIQUETA DE DISPENSADO");
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            
            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            MemoryStream ms1 = new MemoryStream();
            PdfReader reader = new PdfReader(stream);
            int pagesCount = reader.NumberOfPages;
             
            for (int n = 1; n <= reader.NumberOfPages; n++)
            {
                PdfDictionary page = reader.GetPageN(n);
                PdfNumber rotate = page.GetAsNumber(PdfName.ROTATE);
                int rotation = rotate == null ? 90 : (rotate.IntValue + 90) % 360;
                page.Put(PdfName.ROTATE, new PdfNumber(rotation));
            }
            PdfStamper stamper = new PdfStamper(reader, ms1);
            stamper.Close();
            reader.Close();

            byte[] bytes = ms1.ToArray();
            String file = Convert.ToBase64String(bytes);
            return file;


        }

        public string DownloadReport2Carta(string docnum, string format)
        {

            IList<ClsEtiquetaPesado> list = new List<ClsEtiquetaPesado>();
            list = traeEtiquetas(docnum);
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reportes"), "reporteAnexoACarta.rpt"));

            rd.SetDataSource(list);
            rd.SetParameterValue("titulo", "ETIQUETA DE DISPENSADO");
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);

            String file = Convert.ToBase64String(ReadFully(stream));
            return file;


        }
        public static byte[] ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }
       
        public FileResult DownloadReportAnexoB(string docnum, string format)
        {

            string vsuarioLog = Session["LogedUser"].ToString();
            string nombre_User = trae_NombreUser(vsuarioLog);

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reportes"), "reporteAnexoB.rpt"));
            IList<ClsAnexoB> list = new List<ClsAnexoB>();
            list = traeEtiquetasB(docnum);
            rd.SetDataSource(list);
            rd.SetParameterValue("titulo", "Boleta de emisión de fabricación");
            rd.SetParameterValue("titulo2", "Emisión de fabricación");
             
            rd.SetParameterValue("operador", nombre_User);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
             

            ReportDocument rdA = new ReportDocument();
            rdA.Load(Path.Combine(Server.MapPath("~/Reportes"), "reporteAnexoA.rpt"));
            IList<ClsEtiquetaPesado> listA = new List<ClsEtiquetaPesado>();
            listA = traeEtiquetas(docnum);
            rdA.SetDataSource(listA);
            rdA.SetParameterValue("titulo", "ETIQUETA DE DISPENSADO");
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            Stream streamA = rdA.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            streamA.Seek(0, SeekOrigin.Begin);

            Stream s;
            byte[] A;

            using (BinaryReader br = new BinaryReader(streamA))
            {
                A = br.ReadBytes((int)streamA.Length);
            }
            byte[] B;

            using (BinaryReader br = new BinaryReader(stream))
            {
               B = br.ReadBytes((int)stream.Length);
            }

            byte[] archivos;
            List<byte[]> list1 = new List<byte[]>();

            
            list1.Add(B);
            list1.Add(A);
            archivos = ConcatenatePdfs(list1);

         
                return File(archivos, "application/pdf", Server.UrlEncode("reporteAnexoB" + docnum + ".PDF"));
            
        }


        public String DownloadReportAnexoB2(string docnum, string format,string AnexoACarta)
        {

            string vsuarioLog = Session["LogedUser"].ToString();
            string nombre_User = trae_NombreUser(vsuarioLog);

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reportes"), "reporteAnexoB.rpt"));
            IList<ClsAnexoB> list = new List<ClsAnexoB>();
            list = traeEtiquetasB(docnum);
            rd.SetDataSource(list);
            rd.SetParameterValue("titulo", "Boleta de emisión de fabricación");
            rd.SetParameterValue("titulo2", "Emisión de fabricación");

            rd.SetParameterValue("operador", nombre_User);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            string v_ReporteA = "";
            if (AnexoACarta == "1")
            {
                v_ReporteA = "reporteAnexoACarta.rpt";
            }
            else {
                v_ReporteA = "reporteAnexoA.rpt";
            }

            ReportDocument rdA = new ReportDocument();
            rdA.Load(Path.Combine(Server.MapPath("~/Reportes"), v_ReporteA));
            IList<ClsEtiquetaPesado> listA = new List<ClsEtiquetaPesado>();
            listA = traeEtiquetas(docnum);
            rdA.SetDataSource(listA);
            rdA.SetParameterValue("titulo", "ETIQUETA DE DISPENSADO");
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            Stream streamA = rdA.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            streamA.Seek(0, SeekOrigin.Begin);


            Stream s;
            byte[] A;
            if (AnexoACarta == "1")
            {
                using (BinaryReader br = new BinaryReader(streamA))
                {
                    A = br.ReadBytes((int)streamA.Length);
                }
            }
            else
            {
                MemoryStream ms1 = new MemoryStream();
                PdfReader reader = new PdfReader(streamA);
                int pagesCount = reader.NumberOfPages;

                for (int n = 1; n <= reader.NumberOfPages; n++)
                {
                    PdfDictionary page = reader.GetPageN(n);
                    PdfNumber rotate = page.GetAsNumber(PdfName.ROTATE);
                    int rotation = rotate == null ? 90 : (rotate.IntValue + 90) % 360;
                    page.Put(PdfName.ROTATE, new PdfNumber(rotation));
                }
                PdfStamper stamper = new PdfStamper(reader, ms1);
                stamper.Close();
                reader.Close();

                A = ms1.ToArray();
            }

            byte[] B;

            using (BinaryReader br = new BinaryReader(stream))
            {
                B = br.ReadBytes((int)stream.Length);
            }

            byte[] archivos;
            List<byte[]> list1 = new List<byte[]>();


            list1.Add(B);
            list1.Add(A);
            archivos = ConcatenatePdfs(list1);
            
            String file = Convert.ToBase64String(archivos);
            return file;

        }

        public FileResult DownloadReportAnexoC(string docnum, string format)
        {
            string vsuarioLog = Session["LogedUser"].ToString();
            string nombre_User = trae_NombreUser(vsuarioLog);

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reportes"), "reporteAnexoB.rpt"));
            IList<ClsAnexoB> list = new List<ClsAnexoB>();
            list = traeEtiquetasB(docnum);
            rd.SetDataSource(list);
            rd.SetParameterValue("titulo", "Boleta de emisión de fabricación");
            rd.SetParameterValue("titulo2", "Emisión de fabricación");
            
            rd.SetParameterValue("operador", nombre_User);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "reporteAnexoC" + docnum + ".pdf");
        }

        public string DownloadReportAnexoC2(string docnum, string format)
        {
            string vsuarioLog = Session["LogedUser"].ToString();
            string nombre_User = trae_NombreUser(vsuarioLog);

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reportes"), "reporteAnexoB.rpt"));
            IList<ClsAnexoB> list = new List<ClsAnexoB>();
            list = traeEtiquetasB(docnum);
            rd.SetDataSource(list);
            rd.SetParameterValue("titulo", "Boleta de emisión de fabricación");
            rd.SetParameterValue("titulo2", "Emisión de fabricación");

            rd.SetParameterValue("operador", nombre_User);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            
         String file = Convert.ToBase64String(ReadFully(stream));
            return file;
            
        }

        public FileResult DownloadReportAnexoD(string docnum, string format)
        {
            string vsuarioLog = Session["LogedUser"].ToString();
            string nombre_User = trae_NombreUser(vsuarioLog);
            IList<ClsAnexoB> list = new List<ClsAnexoB>();
            list = traeEtiquetasD(docnum);
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reportes"), "reporteAnexoD.rpt"));
            
            rd.SetDataSource(list);
            rd.SetParameterValue("titulo", "Boleta de recibo de producción ");
            rd.SetParameterValue("titulo2", "Recibo de producción"); 
            rd.SetParameterValue("operador", nombre_User);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "reporteAnexoD" + docnum + ".pdf");
        }

        public string DownloadReportAnexoD2(string docnum, string format)
        {
            string vsuarioLog = Session["LogedUser"].ToString();
            string nombre_User = trae_NombreUser(vsuarioLog);
            IList<ClsAnexoB> list = new List<ClsAnexoB>();
            list = traeEtiquetasD(docnum);
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reportes"), "reporteAnexoD.rpt"));

            rd.SetDataSource(list);
            rd.SetParameterValue("titulo", "Boleta de recibo de producción ");
            rd.SetParameterValue("titulo2", "Recibo de producción");
            rd.SetParameterValue("operador", nombre_User);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);

            String file = Convert.ToBase64String(ReadFully(stream));
            return file;
        }
        public string trae_NombreUser(string usuario) {
            
            clsDB cDB2 = new clsDB();
            string nombre_User = "";
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds = cDB2.CONSULTA_NOMBRE_USUARIO(usuario);
                dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {


                    nombre_User = dr["NOMBRE_COM"].ToString();

                }

            }
            catch (Exception e)
            {
                nombre_User = usuario;
            }
            return nombre_User;
        }
        public FileResult DownloadReportAnexoE(string docnum, string format)
        {

            string vsuarioLog = Session["LogedUser"].ToString();
            string nombre_User = trae_NombreUser(vsuarioLog);
              
            
            IList<ClsAnexoB> list = new List<ClsAnexoB>();
            list = traeEtiquetasE(docnum);
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reportes"), "reporteAnexoB.rpt"));
            rd.SetDataSource(list);
            rd.SetParameterValue("titulo", "Boleta de devolución de producción");
            rd.SetParameterValue("titulo2", "Devolución de producción"); 
            rd.SetParameterValue("operador", nombre_User);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "reporteAnexoE_"+ docnum + ".pdf");
        }

        public string DownloadReportAnexoE2(string docnum, string format)
        {

            string vsuarioLog = Session["LogedUser"].ToString();
            string nombre_User = trae_NombreUser(vsuarioLog);


            IList<ClsAnexoB> list = new List<ClsAnexoB>();
            list = traeEtiquetasE(docnum);
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reportes"), "reporteAnexoB.rpt"));
            rd.SetDataSource(list);
            rd.SetParameterValue("titulo", "Boleta de devolución de producción");
            rd.SetParameterValue("titulo2", "Devolución de producción");
            rd.SetParameterValue("operador", nombre_User);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            String file = Convert.ToBase64String(ReadFully(stream));
            return file;
        }


        public static byte[] ConcatenatePdfs(IEnumerable<byte[]> documents)
        {
            using (var ms = new MemoryStream())
            {
                var outputDocument = new Document();
                var writer = new PdfCopy(outputDocument, ms);
                outputDocument.Open();

                foreach (var doc in documents)
                {
                    var reader = new PdfReader(doc);
                    for (var i = 1; i <= reader.NumberOfPages; i++)
                    {
                        
                        writer.AddPage(writer.GetImportedPage(reader, i)); 
                    }
                   
                    writer.FreeReader(reader);
                    reader.Close();
                }

                writer.Close();
                outputDocument.Close();
                var allPagesContent = ms.GetBuffer();
                ms.Flush();

                return allPagesContent;
            }
        }

        public List<MyDropList> traeListaVerificadores()
        {
            var GenreLst = new List<MyDropList>();
            List<clsProduccionModel> lista = new List<clsProduccionModel>();
            try
            {
                clsDB cDB = new clsDB();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds = cDB.CONSULTA_SAP_VERIFICADORES();
                dt = ds.Tables[0];


                foreach (DataRow dr in dt.Rows) //dr["UNIDADESFABRICAR"].ToString()
                {

                    MyDropList fila = new MyDropList();
                    fila.id = dr["CODE"].ToString();
                    fila.value = dr["NAME"].ToString();
                    
                    GenreLst.Add(fila);
                }
            }
            catch (Exception e)
            {
                System.Console.Write(e.Message);
            }

         
            return GenreLst;
        }
        public List<MyDropList> traeListaDispensadores()
        {
            var GenreLst = new List<MyDropList>();
            List<clsProduccionModel> lista = new List<clsProduccionModel>();
            try
            {
                clsDB cDB = new clsDB();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds = cDB.CONSULTA_SAP_DISPENSADORES();
                dt = ds.Tables[0];


                foreach (DataRow dr in dt.Rows) //dr["UNIDADESFABRICAR"].ToString()
                {

                    MyDropList fila = new MyDropList();
                    fila.id = dr["CODE"].ToString();
                    fila.value = dr["NAME"].ToString();
                    
                    GenreLst.Add(fila);
                }
            }
            catch (Exception e)
            {
                System.Console.Write(e.Message);
            }

         
            return GenreLst;
        }

        public List<MyDropList> traeListaSerie(int objec)
        {
            var GenreLst = new List<MyDropList>();
            List<clsProduccionModel> lista = new List<clsProduccionModel>();
            try
            {
                clsDB cDB = new clsDB();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds = cDB.CONSULTA_SAP_SERIE(objec.ToString());
                dt = ds.Tables[0];


                foreach (DataRow dr in dt.Rows) //dr["UNIDADESFABRICAR"].ToString()
                {

                    MyDropList fila = new MyDropList();
                    fila.id = dr["SERIES"].ToString();
                    fila.value = dr["SERIESNAME"].ToString();
                    
                    GenreLst.Add(fila);
                }
            }
            catch (Exception e)
            {
                System.Console.Write(e.Message);
            }

         
            return GenreLst;
        }

        public List<MyDropList> traeListaEmisionesXDocnum(string docnum)
        {
            var GenreLst = new List<MyDropList>();
            List<clsProduccionModel> lista = new List<clsProduccionModel>();
            try
            {
                clsDB cDB = new clsDB();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds = cDB.CONSULTA_SAP_EMISION_DOCNUM(docnum.ToString());
                dt = ds.Tables[0];
                int contador = 0;

                foreach (DataRow dr in dt.Rows) //dr["UNIDADESFABRICAR"].ToString()
                {
                    contador = contador + 1;
                    MyDropList fila = new MyDropList();
                    fila.id = contador.ToString();
                    fila.value = dr["DocNum"].ToString();
                    
                    GenreLst.Add(fila);
                }
            }
            catch (Exception e)
            {
                System.Console.Write(e.Message);
            }

         
            return GenreLst;
        }
         public List<MyDropList> traeListaRecibosXDocnum(string docnum)
        {
            var GenreLst = new List<MyDropList>();
            List<clsProduccionModel> lista = new List<clsProduccionModel>();
            try
            {
                clsDB cDB = new clsDB();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds = cDB.CONSULTA_SAP_RECIBO_DOCNUM(docnum.ToString());
                dt = ds.Tables[0];
                int contador = 0;

                foreach (DataRow dr in dt.Rows) //dr["UNIDADESFABRICAR"].ToString()
                {
                    contador = contador + 1;
                    MyDropList fila = new MyDropList();
                    fila.id = contador.ToString();
                    fila.value = dr["DocNum"].ToString();
                    
                    GenreLst.Add(fila);
                }
            }
            catch (Exception e)
            {
                System.Console.Write(e.Message);
            }

         
            return GenreLst;
        }
       
    }
}
