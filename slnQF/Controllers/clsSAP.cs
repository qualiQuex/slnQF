using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using SAPbobsCOM;
namespace slnQF.Models
{
    public class MyDropList
    {
        public string id { get; set; }
        public string value { get; set; }
    }
    public class VentasForma
    {
        public string referencia { get; set; }
    }
    public class MyContacto
    {
        public string id { get; set; }
        public string value { get; set; }
    }
    public class clsSAP
    {
        public SAPbobsCOM.Documents oOrder; // Order object
        public SAPbobsCOM.Documents oInvoice; // Invoice Object
        public SAPbobsCOM.Recordset oRecordSet; // A recordset object
        public SAPbobsCOM.Company oCompany; // The company object
        private static string Server = System.Configuration.ConfigurationManager.AppSettings.Get("Server");
        private static string CompanyDB = System.Configuration.ConfigurationManager.AppSettings.Get("CompanyDB");
        private static string UserName = System.Configuration.ConfigurationManager.AppSettings.Get("UserName");
        private static string Password = System.Configuration.ConfigurationManager.AppSettings.Get("Password");

        public void conectar2()
        {
            //Capturar errores
            long lRetCode;
            int lErrCode;
            string sErrMsg;

            oCompany = new Company();
            oCompany.Server = Server;
            oCompany.DbServerType = BoDataServerTypes.dst_MSSQL2008;
            oCompany.CompanyDB = CompanyDB;
            oCompany.UserName = UserName;
            oCompany.Password = Password;

            lRetCode = oCompany.Connect();
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

        }

        public long conectar()
        {
            //Capturar errores
            long lRetCode;
            int lErrCode;
            string sErrMsg;

            oCompany = new Company();
            oCompany.Server = Server;
            oCompany.DbServerType = BoDataServerTypes.dst_MSSQL2008;
            oCompany.CompanyDB = CompanyDB;
            oCompany.UserName = UserName;
            oCompany.Password = Password;

            lRetCode = oCompany.Connect();
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
            return lRetCode;
        }
        public void desconectar()
        {
            oCompany.Disconnect();
            //System.Runtime.InteropServices.Marshal.ReleaseComObject(oCompany);
            GC.Collect();
            oCompany = null;

        }
        public List<MyDropList> traeClientes(Company tCom)
        {
            var GenreLst = new List<MyDropList>();

            Company tmpCompany;

            tmpCompany = tCom;

            // validar que la conexion sea correcta


            SAPbobsCOM.SBObob oObj;
            SAPbobsCOM.Recordset rs;

            oObj = (SAPbobsCOM.SBObob)tmpCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoBridge);
            rs = (SAPbobsCOM.Recordset)tmpCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

            // Set the Customer Name and Customer Code Combo Boxes
            rs = oObj.GetBPList(SAPbobsCOM.BoCardTypes.cCustomer);
            rs.MoveFirst();
            ListBox cmbCustomer = new ListBox();
            ListItem cmbLista = new ListItem();
            while (!(rs.EoF))
            {
                MyDropList a = new MyDropList();
                a.id = rs.Fields.Item(0).Value;
                a.value = rs.Fields.Item(1).Value;
                GenreLst.Add(a);
                //cmbCustomer.Items.Add(a); 
                rs.MoveNext();
            }
            return GenreLst;
        }

        public List<MyDropList> traeSerie2(Company tCom, int objtCode)
        {
            var GenreLst = new List<MyDropList>();
            Company tmpCompany;

            tmpCompany = tCom;

            SAPbobsCOM.Recordset rs;
            String sSQL = "";
            sSQL =
            "SELECT SERIES, SERIESNAME FROM NNM1 WHERE OBJECTCODE = " + objtCode.ToString();
            rs = (SAPbobsCOM.Recordset)tmpCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            rs.DoQuery(sSQL);
            // Set the Customer Name and Customer Code Combo Boxes 
            rs.MoveFirst();

            while (!(rs.EoF))
            {
                MyDropList a = new MyDropList();
                a.id = rs.Fields.Item(0).Value.ToString();
                a.value = rs.Fields.Item(1).Value;
                GenreLst.Add(a);
                //cmbCustomer.Items.Add(a); 
                rs.MoveNext();
            }
            return GenreLst;
        }

        public List<MyDropList> traeListaEmisionPorOrden(Company tCom, string numOrden)
        {
            var GenreLst = new List<MyDropList>();
            Company tmpCompany;

            tmpCompany = tCom;

            SAPbobsCOM.Recordset rs;
            String sSQL = "";
            sSQL =
            "SELECT DISTINCT T0.[DocNum] FROM OIGE T0  INNER JOIN IGE1 T1 ON T0.DocEntry = T1.DocEntry WHERE T1.[BaseEntry]  = (SELECT R.DOCENTRY FROM OWOR R WHERE R.DOCNUM = '" + numOrden + "')";
            rs = (SAPbobsCOM.Recordset)tmpCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            rs.DoQuery(sSQL);
            // Set the Customer Name and Customer Code Combo Boxes 
            rs.MoveFirst();
            int contador = 0;
            while (!(rs.EoF))
            {
                contador = contador + 1;
                MyDropList a = new MyDropList();
                a.id = contador.ToString();
                a.value = rs.Fields.Item(0).Value.ToString();
                GenreLst.Add(a);
                //cmbCustomer.Items.Add(a); 
                rs.MoveNext();
            }
            return GenreLst;
        }
        public List<MyDropList> traeListaRecibosPorOrden(Company tCom, string numOrden)
        {
            var GenreLst = new List<MyDropList>();
            Company tmpCompany;

            tmpCompany = tCom;

            SAPbobsCOM.Recordset rs;
            String sSQL = "";
            sSQL =
            "SELECT DISTINCT T0.[DocNum] FROM OIGN T0  INNER JOIN IGN1 T1 ON T0.DocEntry = T1.DocEntry WHERE T1.[BaseEntry]  = (SELECT R.DOCENTRY FROM OWOR R WHERE R.DOCNUM = '" + numOrden + "')";
            rs = (SAPbobsCOM.Recordset)tmpCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            rs.DoQuery(sSQL);
            // Set the Customer Name and Customer Code Combo Boxes 
            rs.MoveFirst();
            int contador = 0;
            while (!(rs.EoF))
            {
                contador = contador + 1;
                MyDropList a = new MyDropList();
                a.id = contador.ToString();
                a.value = rs.Fields.Item(0).Value.ToString();
                GenreLst.Add(a);
                //cmbCustomer.Items.Add(a); 
                rs.MoveNext();
            }
            return GenreLst;
        }
        public List<MyDropList> traeWarehouse(Company tCom)
        {
            var GenreLst = new List<MyDropList>();
            Company tmpCompany;

            tmpCompany = tCom;

            SAPbobsCOM.Recordset rs;
            String sSQL = "";
            sSQL =
            "SELECT WHSCODE, WHSNAME FROM OWHS ";
            rs = (SAPbobsCOM.Recordset)tmpCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            rs.DoQuery(sSQL);
            // Set the Customer Name and Customer Code Combo Boxes 
            rs.MoveFirst();

            while (!(rs.EoF))
            {
                MyDropList a = new MyDropList();
                a.id = rs.Fields.Item(0).Value.ToString();
                a.value = rs.Fields.Item(1).Value;
                GenreLst.Add(a);
                //cmbCustomer.Items.Add(a); 
                rs.MoveNext();
            }
            return GenreLst;
        }

        public int traeDoctoSeries(Company tCom, int objSerie)
        {
            int vNextNumber = 0;
            Company tmpCompany;

            tmpCompany = tCom;

            SAPbobsCOM.Recordset rs;
            String sSQL = "";
            sSQL = "SELECT NEXTNUMBER FROM NNM1 WHERE SERIES = " + objSerie.ToString();

            rs = (SAPbobsCOM.Recordset)tmpCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            rs.DoQuery(sSQL);
            // Set the Customer Name and Customer Code Combo Boxes 
            rs.MoveFirst();

            while (!(rs.EoF))
            {
                vNextNumber = rs.Fields.Item(0).Value;

                rs.MoveNext();
            }
            return vNextNumber;
        }
        public List<MyDropList> traeMonedas(Company tCom, int objType)
        {
            var GenreLst = new List<MyDropList>();
            Company tmpCompany;

            tmpCompany = tCom;

            SAPbobsCOM.Recordset rs;
            String sSQL = "";
            sSQL = "SELECT CurrCode,CurrName FROM OCRN";

            rs = (SAPbobsCOM.Recordset)tmpCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            rs.DoQuery(sSQL);
            // Set the Customer Name and Customer Code Combo Boxes 
            rs.MoveFirst();

            while (!(rs.EoF))
            {
                MyDropList a = new MyDropList();
                a.id = rs.Fields.Item(0).Value;
                a.value = rs.Fields.Item(1).Value;
                GenreLst.Add(a);
                //cmbCustomer.Items.Add(a); 
                rs.MoveNext();
            }
            return GenreLst;
        }


        public List<MyDropList> traeVendor(Company tCom)
        {
            var GenreLst = new List<MyDropList>();
            Company tmpCompany;

            tmpCompany = tCom;

            SAPbobsCOM.SBObob oObj;
            SAPbobsCOM.Recordset rs;

            oObj = (SAPbobsCOM.SBObob)tmpCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoBridge);
            rs = (SAPbobsCOM.Recordset)tmpCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

            // Set the Customer Name and Customer Code Combo Boxes
            rs = oObj.GetBPList(SAPbobsCOM.BoCardTypes.cSupplier);
            rs.MoveFirst();
            ListBox cmbCustomer = new ListBox();
            ListItem cmbLista = new ListItem();
            while (!(rs.EoF))
            {
                MyDropList a = new MyDropList();
                a.id = rs.Fields.Item(0).Value;
                a.value = rs.Fields.Item(1).Value;
                GenreLst.Add(a);
                //cmbCustomer.Items.Add(a); 
                rs.MoveNext();
            }
            return GenreLst;
        }
        public List<MyDropList> traeMonedas(Company tCom)
        {
            var GenreLst = new List<MyDropList>();
            Company tmpCompany;

            tmpCompany = tCom;

            SAPbobsCOM.Recordset rs;
            String sSQL = "";
            sSQL = "SELECT CurrCode,CurrName FROM OCRN";

            rs = (SAPbobsCOM.Recordset)tmpCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            rs.DoQuery(sSQL);
            // Set the Customer Name and Customer Code Combo Boxes 
            rs.MoveFirst();

            while (!(rs.EoF))
            {
                MyDropList a = new MyDropList();
                a.id = rs.Fields.Item(0).Value;
                a.value = rs.Fields.Item(1).Value;
                GenreLst.Add(a);
                //cmbCustomer.Items.Add(a); 
                rs.MoveNext();
            }
            return GenreLst;
        }

        public List<MyDropList> traeArticulos(Company tCom)
        {
            var GenreLst = new List<MyDropList>();
            Company tmpCompany;

            tmpCompany = tCom;

            SAPbobsCOM.Recordset rs;
            String sSQL = "";
            sSQL = "SELECT ItemCode, ItemName FROM OITM";

            rs = (SAPbobsCOM.Recordset)tmpCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            rs.DoQuery(sSQL);
            // Set the Customer Name and Customer Code Combo Boxes 
            rs.MoveFirst();

            while (!(rs.EoF))
            {
                MyDropList a = new MyDropList();
                a.id = rs.Fields.Item(0).Value;
                a.value = rs.Fields.Item(1).Value;
                GenreLst.Add(a);
                //cmbCustomer.Items.Add(a); 
                rs.MoveNext();
            }
            return GenreLst;
        }

        public List<MyDropList> traeCodArticulos(Company tCom)
        {
            var GenreLst = new List<MyDropList>();
            Company tmpCompany;

            tmpCompany = tCom;

            SAPbobsCOM.Recordset rs;
            String sSQL = "";
            sSQL = "SELECT ItemCode, ItemCode FROM OITM";

            rs = (SAPbobsCOM.Recordset)tmpCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            rs.DoQuery(sSQL);
            // Set the Customer Name and Customer Code Combo Boxes 
            rs.MoveFirst();

            while (!(rs.EoF))
            {
                MyDropList a = new MyDropList();
                a.id = rs.Fields.Item(0).Value;
                a.value = rs.Fields.Item(1).Value;
                GenreLst.Add(a);
                //cmbCustomer.Items.Add(a); 
                rs.MoveNext();
            }
            return GenreLst;
        }

        public List<MyDropList> traeImpuestosOrden(Company tCom)
        {
            var GenreLst = new List<MyDropList>();
            Company tmpCompany;

            tmpCompany = tCom;


            SAPbobsCOM.Recordset rs;
            String sSQL = "";
            sSQL = "SELECT Code, Code FROM OSTC";

            rs = (SAPbobsCOM.Recordset)tmpCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            rs.DoQuery(sSQL);
            // Set the Customer Name and Customer Code Combo Boxes 
            rs.MoveFirst();

            while (!(rs.EoF))
            {
                MyDropList a = new MyDropList();
                a.id = rs.Fields.Item(0).Value;
                a.value = rs.Fields.Item(1).Value;
                GenreLst.Add(a);
                //cmbCustomer.Items.Add(a); 
                rs.MoveNext();
            }
            return GenreLst;
        }

        public List<MyDropList> traeDispensadores2(Company tCom)
        {
            var GenreLst = new List<MyDropList>();
            Company tmpCompany;

            tmpCompany = tCom;


            SAPbobsCOM.Recordset rs;
            String sSQL = "";
            sSQL = "Select CODE,NAME from \"@DISPENSADORES\" ORDER BY CODE";
            rs = (SAPbobsCOM.Recordset)tmpCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            rs.DoQuery(sSQL);
            // Set the Customer Name and Customer Code Combo Boxes 
            rs.MoveFirst();

            while (!(rs.EoF))
            {
                MyDropList a = new MyDropList();
                a.id = rs.Fields.Item(0).Value;
                a.value = rs.Fields.Item(1).Value;
                GenreLst.Add(a);
                //cmbCustomer.Items.Add(a); 
                rs.MoveNext();
            }
            return GenreLst;
        }

        public List<MyDropList> traeVerificadores2(Company tCom)
        {
            var GenreLst = new List<MyDropList>();
            Company tmpCompany;

            tmpCompany = tCom;


            SAPbobsCOM.Recordset rs;
            String sSQL = "";
            sSQL = "Select CODE,NAME from \"@VERIFICADORES\" ORDER BY CODE";
            rs = (SAPbobsCOM.Recordset)tmpCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            rs.DoQuery(sSQL);
            // Set the Customer Name and Customer Code Combo Boxes 
            rs.MoveFirst();

            while (!(rs.EoF))
            {
                MyDropList a = new MyDropList();
                a.id = rs.Fields.Item(0).Value;
                a.value = rs.Fields.Item(1).Value;
                GenreLst.Add(a);
                //cmbCustomer.Items.Add(a); 
                rs.MoveNext();
            }
            return GenreLst;
        }
        public List<MyDropList> traeLotesItem(string itemCode, Company tCom)
        {
            //"SELECT DistNumber,ExpDate FROM OBTN WHERE STATUS =0 and itemCode = '04100101' order by expdate desc"
            var GenreLst = new List<MyDropList>();
            Company tmpCompany;

            tmpCompany = tCom;

            SAPbobsCOM.Recordset rs;
            String sSQL = "";
            sSQL = "SELECT DistNumber,DistNumber FROM OBTN WHERE STATUS =0 and itemCode = '" + itemCode + "' order by expdate desc";

            rs = (SAPbobsCOM.Recordset)tmpCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            rs.DoQuery(sSQL);
            // Set the Customer Name and Customer Code Combo Boxes 
            rs.MoveFirst();

            while (!(rs.EoF))
            {
                MyDropList a = new MyDropList();
                a.id = rs.Fields.Item(0).Value;
                a.value = rs.Fields.Item(1).Value;
                GenreLst.Add(a);
                //cmbCustomer.Items.Add(a); 
                rs.MoveNext();
            }
            return GenreLst;
        }
        public int traePrecioArticulo(string codCliente, string codArticulo, DateTime fechaPost, Company tCom)
        {
            int precio = 0;
            Company tmpCompany;

            tmpCompany = tCom;


            SAPbobsCOM.SBObob oObj;
            SAPbobsCOM.Recordset rs;

            oObj = (SAPbobsCOM.SBObob)tmpCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoBridge);
            rs = (SAPbobsCOM.Recordset)tmpCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);


            rs = oObj.GetItemPrice(codCliente, codArticulo, 1F, DateTime.Today);
            precio = 1 * System.Convert.ToInt32(rs.Fields.Item(0).Value);
            // NewRow[5] = 1 * System.Convert.ToInt32(OrderApp.oRecordSet.Fields.Item(0).Value);


            return 1 * precio;
        }
        public modelFormaVenta traeContacto(string value, Company tCom)
        {

            Company tmpCompany;

            tmpCompany = tCom;
            modelFormaVenta MASTER = new modelFormaVenta();
            modelContacto c;
            MASTER.contactos = new List<modelContacto>();
            String sSQL = "";
            sSQL = "SELECT CntctCode, Name, Tel1 FROM OCPR WHERE CardCode = \'" + value + "\'";

            SAPbobsCOM.Recordset rs;
            rs = (SAPbobsCOM.Recordset)tmpCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            rs.DoQuery(sSQL);
            // Set the Customer Name and Customer Code Combo Boxes 
            rs.MoveFirst();

            while (!(rs.EoF))
            {
                c = new modelContacto();

                c.id_Contacto = rs.Fields.Item(0).Value.ToString();
                c.nombre_Contacto = rs.Fields.Item(1).Value;
                MASTER.contactos.Add(c);
                rs.MoveNext();
            }
            return MASTER;
        }
        public List<clsProduccionModel> traeOrdenesProduccion(Company tCom, string estado, string warehouseUser, string v_users)
        {
            //"SELECT DistNumber,ExpDate FROM OBTN WHERE STATUS =0 and itemCode = '04100101' order by expdate desc"
            var clsProduccionModel = new List<clsProduccionModel>();
            Company tmpCompany;

            tmpCompany = tCom;

            SAPbobsCOM.Recordset rs;
            string p_estado = "";
            string p_top = "";
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
            String sSQL = "SELECT" + p_top + " T0.DOCENTRY,(CASE WHEN T0.Type = 'S' THEN 'Estandar' WHEN T0.Type = 'P' THEN 'Especial' WHEN T0.Type = 'd' THEN 'Desmontar' END) Type,(CASE WHEN T0.Status = 'R' THEN 'Liberado' WHEN T0.Status = 'P' THEN 'Planif.' WHEN T0.Status = 'L' THEN 'Cerrado' WHEN T0.Status = 'C' THEN 'Cancelado' END) Estado, T0.ItemCode as NoProducto,case when (T0.PlannedQty - T0.CmpltQty) < 0 then 0 else(T0.PlannedQty - T0.CmpltQty) end as CantidadPlanificada, T0.Warehouse as Almacen, T2.SeriesName as Serie, T0.Docnum,CONVERT(NVARCHAR, isnull(T0.u_fechai1,T0.PostDate),103) PostDate,CONVERT(NVARCHAR, T0.DueDate,103) DueDate, T3.U_NAME as Usuario, T0.OriginType as Origen, T0.CardCode as Cliente, T4.OcrName as NormaReparto, T0.Project, T0.Comments as Comentarios, T5.Descr as TIPO_FABRICACION, T0.U_FM as FormulaMaestra, T0.U_U_FABRICAR as UnidadesFabricar,T0.U_LOTE as Lote,CONVERT(NVARCHAR, T0.U_VENCE,103) as Vence, ISNULL(T7.Descr, '--') as Supervisor, T0.ITEMCODE, T1.ITEMNAME, 10 precio, T6.WhsName as ALMACEN_DESC , T0.ORIGINNUM as PEDIDO_CLIENTE, T0.U_F_FARMA as FORMA_FARMACEU,T0.PLANNEDQTY CANTIDAD, T0.UOM ,ISNULL(T0.U_OBSERVACIONES,'') OBSERVACIONES FROM OWOR T0 left join OITM T1 on T1.ITEMCODE = T0.ITEMCODE left join NNM1 T2 on T0.Series = T2.Series  left join OUSR T3 on T0.UserSign = T3.USERID left join OOCR T4 on T0.OcrCode = T4.OcrCode left join UFD1 T5 on(T0.U_T_FABRICACION = T5.FldValue  and T5.FieldID = 60 and T5.TableID = 'OWOR') left join OWHS T6 on T0.Warehouse = T6.WhsCode  left join UFD1 T7 on (T0.U_SUPERVISOR = T7.FldValue  and T7.TableID = 'OWOR' and t7.FieldID = 52) ";

             //string SQLWHERE = "WHERE status = '" + p_estado + "'  and T0.[ItemCode] NOT LIKE '03%%' "+v_users+" and ( ";
             string SQLWHERE = "WHERE status = '" + p_estado + "' " + v_users + " and ( ";
            string subsqlwhere = "( T0.DOCENTRY IN ( SELECT  T1.DOCENTRY  FROM WOR1 T1 WHERE   T1.DOCENTRY = T0.DOCENTRY AND T1.ISSUETYPE = 'M'   " + warehouseUser.Replace("T0", "T1") + " AND (case when(T1.PlannedQty - T1.IssuedQty) < 0 then (T1.PlannedQty - T1.IssuedQty)  else(T1.PlannedQty - T1.IssuedQty) end) >0 ) "+
                ") OR (" + warehouseUser.Replace("AND", " ") + " " + ") ) AND T0.POSTDATE > DATEADD(YEAR,-2,GETDATE()) ORDER BY T0.Docnum ASC";

            string querySQL = sSQL + SQLWHERE + subsqlwhere;
            rs = (SAPbobsCOM.Recordset)tmpCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            rs.DoQuery(querySQL);
            // Set the Customer Name and Customer Code Combo Boxes 
            rs.MoveFirst();

            while (!(rs.EoF))
            {
                clsProduccionModel a = new clsProduccionModel(rs.Fields.Item(0).Value.ToString(), rs.Fields.Item(1).Value.ToString(), rs.Fields.Item(2).Value.ToString(), rs.Fields.Item(3).Value.ToString(), rs.Fields.Item(4).Value.ToString(), rs.Fields.Item(5).Value.ToString(), rs.Fields.Item(6).Value.ToString(), rs.Fields.Item(7).Value.ToString(), rs.Fields.Item(8).Value.ToString(), rs.Fields.Item(9).Value.ToString(), rs.Fields.Item(10).Value.ToString(), rs.Fields.Item(26).Value.ToString(), rs.Fields.Item(12).Value.ToString(), rs.Fields.Item(13).Value.ToString(), rs.Fields.Item(14).Value.ToString(), rs.Fields.Item(15).Value.ToString(), rs.Fields.Item(16).Value.ToString(), rs.Fields.Item(17).Value.ToString(), rs.Fields.Item(27).Value.ToString(), rs.Fields.Item(18).Value.ToString(), rs.Fields.Item(19).Value.ToString(), rs.Fields.Item(20).Value.ToString(), rs.Fields.Item(21).Value.ToString(), rs.Fields.Item(22).Value.ToString(), rs.Fields.Item(23).Value.ToString(), rs.Fields.Item(24).Value.ToString(), rs.Fields.Item(25).Value.ToString(), rs.Fields.Item(11).Value.ToString(), "", rs.Fields.Item(30).Value.ToString(), "", "", rs.Fields.Item(28).Value.ToString(), rs.Fields.Item(29).Value.ToString());

                clsProduccionModel.Add(a);
                //cmbCustomer.Items.Add(a); 
                rs.MoveNext();
            }
            tmpCompany.Disconnect();
            return clsProduccionModel;
        }
        public List<clsItemLoteTotal> traeListaLotesItem2(Company tCom, string itemCode)
        {
            /*
             * ITEMCODE
             * WHSTOTAL
             * BATCHNUM
             * QUANTITY
            */
            List<clsItemLoteTotal> cls_itemsLotes = new List<clsItemLoteTotal>();
            Company tmpCompany;

            tmpCompany = tCom;

            SAPbobsCOM.Recordset rs;
            String sSQL = "";
            //sSQL = "Select * from(SELECT T0.[ItemCode], v0.[Quantity][WhsTotal], T0.[BatchNum], SUM(CASE T0.[Direction] when 0 then 1 else -1 end * T0.[Quantity])[Quantity] FROM IBT1 T0 INNER JOIN OWHS T1 ON T0.WhsCode = T1.WhsCode  INNER JOIN(SELECT T0.[ItemCode], T1.[WhsName], SUM(CASE T0.[Direction] when 0 then 1 else -1 end * T0.[Quantity])[Quantity] FROM IBT1 T0 INNER JOIN OWHS T1 ON T0.WhsCode = T1.WhsCode GROUP BY T1.[WhsName], T0.[ItemCode]) V0 ON T0.[ItemCode] = V0.[ItemCode] and t1.[WhsName] = v0.[WhsName] WHERE T0.[ItemCode] = '" + itemCode + "' GROUP BY T0.[BatchNum], T1.[WhsName], v0.[Quantity], T0.[ItemCode]) a where Quantity > 0";
            //sSQL = "Select a.*,CONVERT(NVARCHAR,  b.expdate,103) expate  from(SELECT T0.[ItemCode], v0.[Quantity][WhsTotal], T0.[BatchNum], SUM(CASE T0.[Direction] when 0 then 1 else -1 end * T0.[Quantity])[Quantity], T1.WhsCode [WhsCode] FROM IBT1 T0 INNER JOIN OWHS T1 ON T0.WhsCode = T1.WhsCode  INNER JOIN(SELECT T0.[ItemCode], T1.[WhsName], SUM(CASE T0.[Direction] when 0 then 1 else -1 end * T0.[Quantity])[Quantity] FROM IBT1 T0 INNER JOIN OWHS T1 ON T0.WhsCode = T1.WhsCode GROUP BY T1.[WhsName], T0.[ItemCode]) V0 ON T0.[ItemCode] = V0.[ItemCode] and t1.[WhsName] = v0.[WhsName] WHERE T0.[ItemCode] = '" + itemCode + "' GROUP BY T0.[BatchNum], T1.WhsCode, v0.[Quantity], T0.[ItemCode]) a , OBTN B where a.Quantity > 0 and B.ItemCode = a.ItemCode and b.distnumber= batchnum order by b.indate desc";

            sSQL = "SELECT a.ItemCode,0 'WhsTotal',a.BatchNum,a.Quantity, a.WhsCode,CONVERT(NVARCHAR, a.expdate, 103) expdate, isnull(b.mnfserial,'--')mnfserial,isnull(b.LOTNUMBER,'--')LOTNUMBER,isnull(b.notes,'--')notes FROM OIBT a, OBTN b WHERE a.ITEMCODE ='" + itemCode + "'  AND a.Quantity>0  AND b.itemcode = a.itemcode AND b.distnumber = a.batchnum order by  a.indate desc ";
            // sSQL = "SELECT       T0.[ItemCode],0 WhsTotal,       T0.[BatchNum],       SUM(CASE T0.[Direction]                    when 0 then 1                    else -1             end * T0.[Quantity])[Quantity],       T1.WhsName FROM IBT1 T0 INNER JOIN OWHS T1 ON T0.WhsCode = T1.WhsCode WHERE T0.[ItemCode] = '" + itemCode + "'GROUP BY T0.[BatchNum], T1.WhsName, T0.[ItemCode]having SUM(CASE T0.[Direction]                    when 0 then 1                    else -1             end * T0.[Quantity]) > 0";
            rs = (SAPbobsCOM.Recordset)tmpCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

            rs.DoQuery(sSQL);
            // Set the Customer Name and Customer Code Combo Boxes 
            rs.MoveFirst();
            int contador = 1;
            while (!(rs.EoF))
            {
                clsItemLoteTotal a = new clsItemLoteTotal(rs.Fields.Item(0).Value.ToString(), rs.Fields.Item(1).Value.ToString(), rs.Fields.Item(2).Value.ToString(), rs.Fields.Item(3).Value.ToString(), rs.Fields.Item(4).Value.ToString(), rs.Fields.Item(5).Value.ToString(), rs.Fields.Item(6).Value.ToString(), rs.Fields.Item(7).Value.ToString(), rs.Fields.Item(8).Value.ToString());
                contador = contador + 1;

                cls_itemsLotes.Add(a);
                //cmbCustomer.Items.Add(a); 
                rs.MoveNext();
            }
            tmpCompany.Disconnect();
            return cls_itemsLotes;
        }

        public List<clsItemLoteTotal> traeListaLotesItemDevolucion2(Company tCom, string itemCode, string baseEntry, string WhsCode)
        {
            /*
             * ITEMCODE
             * WHSTOTAL
             * BATCHNUM
             * QUANTITY
            */
            List<clsItemLoteTotal> cls_itemsLotes = new List<clsItemLoteTotal>();
            Company tmpCompany;

            tmpCompany = tCom;

            SAPbobsCOM.Recordset rs;
            String sSQL = "";
            //sSQL = "Select * from(SELECT T0.[ItemCode], v0.[Quantity][WhsTotal], T0.[BatchNum], SUM(CASE T0.[Direction] when 0 then 1 else -1 end * T0.[Quantity])[Quantity] FROM IBT1 T0 INNER JOIN OWHS T1 ON T0.WhsCode = T1.WhsCode  INNER JOIN(SELECT T0.[ItemCode], T1.[WhsName], SUM(CASE T0.[Direction] when 0 then 1 else -1 end * T0.[Quantity])[Quantity] FROM IBT1 T0 INNER JOIN OWHS T1 ON T0.WhsCode = T1.WhsCode GROUP BY T1.[WhsName], T0.[ItemCode]) V0 ON T0.[ItemCode] = V0.[ItemCode] and t1.[WhsName] = v0.[WhsName] WHERE T0.[ItemCode] = '" + itemCode + "' GROUP BY T0.[BatchNum], T1.[WhsName], v0.[Quantity], T0.[ItemCode]) a where Quantity > 0";
            //sSQL = "Select a.*,CONVERT(NVARCHAR,  b.expdate,103) expate  from(SELECT T0.[ItemCode], v0.[Quantity][WhsTotal], T0.[BatchNum], SUM(CASE T0.[Direction] when 0 then 1 else -1 end * T0.[Quantity])[Quantity], T1.WhsCode [WhsCode] FROM IBT1 T0 INNER JOIN OWHS T1 ON T0.WhsCode = T1.WhsCode  INNER JOIN(SELECT T0.[ItemCode], T1.[WhsName], SUM(CASE T0.[Direction] when 0 then 1 else -1 end * T0.[Quantity])[Quantity] FROM IBT1 T0 INNER JOIN OWHS T1 ON T0.WhsCode = T1.WhsCode GROUP BY T1.[WhsName], T0.[ItemCode]) V0 ON T0.[ItemCode] = V0.[ItemCode] and t1.[WhsName] = v0.[WhsName] WHERE T0.[ItemCode] = '" + itemCode + "' GROUP BY T0.[BatchNum], T1.WhsCode, v0.[Quantity], T0.[ItemCode]) a , OBTN B where a.Quantity > 0 and B.ItemCode = a.ItemCode and b.distnumber= batchnum order by b.indate desc";

            //sSQL = "SELECT ITEMCODE,WHSTOTAL,DISTNUMBER, (QUANTITY)*-1 QUANTITY, WHSCODE,CONVERT(NVARCHAR, EXPDATE,103) EXPDATE,MNFSERIAL,LOTNUMBER,NOTES FROM( SELECT T3.ITEMCODE,0 'WhsTotal',T3.DISTNUMBER,SUM(T1.QUANTITY) QUANTITY, t4.whscode,T4.ExpDate, isnull(T3.mnfserial,'--') mnfserial,isnull(T3.LOTNUMBER,'--')LOTNUMBER,isnull( CAST(T3.NOTES as NVARCHAR(max)),'--')notes FROM OITL T0 INNER JOIN ITL1 T1 ON T0.LogEntry = T1.LogEntry INNER JOIN OBTQ T2 ON T2.MdAbsEntry = T1.MdAbsEntry AND T2.Quantity!=0 INNER JOIN OBTN T3 ON T3.AbsEntry = T1.MdAbsEntry INNER JOIN OIBT T4 ON T4.ITEMCODE = T3.ITEMCODE AND T4.Quantity!=0 AND T4.BatchNum = T3.DISTNUMBER AND T4.ITEMCODE= '" + itemCode + "' AND T0.BaseType NOT IN (-1,20,22) and T0.BaseEntry = '" + baseEntry + "' GROUP BY T3.ITEMCODE, T3.DISTNUMBER, t4.whscode,T4.ExpDate,ISNULL(T3.MNFSERIAL,'--'),ISNULL(T3.LOTNUMBER,'--'),isnull(CAST(T3.NOTES as NVARCHAR(max)),'--') )A where A.QUANTITY <0 and A.WhsCode = '" + WhsCode + "'";
            sSQL = "SELECT ITEMCODE,WHSTOTAL,DISTNUMBER, (QUANTITY)*-1 QUANTITY, WHSCODE,CONVERT(NVARCHAR, EXPDATE,103) EXPDATE,MNFSERIAL,LOTNUMBER,NOTES FROM( SELECT T3.ITEMCODE,0 'WhsTotal',T3.DISTNUMBER,SUM(T1.QUANTITY) QUANTITY, t4.whscode,T4.ExpDate, isnull(T3.mnfserial,'--') mnfserial,isnull(T3.LOTNUMBER,'--')LOTNUMBER,isnull( CAST(T3.NOTES as NVARCHAR(max)),'--')notes FROM OITL T0 INNER JOIN ITL1 T1 ON T0.LogEntry = T1.LogEntry INNER JOIN OBTQ T2 ON T2.MdAbsEntry = T1.MdAbsEntry INNER JOIN OBTN T3 ON T3.AbsEntry = T1.MdAbsEntry INNER JOIN OIBT T4 ON T4.ITEMCODE = T3.ITEMCODE AND T4.BatchNum = T3.DISTNUMBER AND T4.ITEMCODE= '" + itemCode + "' AND T0.BaseType NOT IN (-1,20,22) and T0.BaseEntry = '" + baseEntry + "' GROUP BY T3.ITEMCODE, T3.DISTNUMBER, t4.whscode,T4.ExpDate,ISNULL(T3.MNFSERIAL,'--'),ISNULL(T3.LOTNUMBER,'--'),isnull(CAST(T3.NOTES as NVARCHAR(max)),'--') )A where A.QUANTITY <0 and A.WhsCode = '" + WhsCode + "'";
            // sSQL = "SELECT       T0.[ItemCode],0 WhsTotal,       T0.[BatchNum],       SUM(CASE T0.[Direction]                    when 0 then 1                    else -1             end * T0.[Quantity])[Quantity],       T1.WhsName FROM IBT1 T0 INNER JOIN OWHS T1 ON T0.WhsCode = T1.WhsCode WHERE T0.[ItemCode] = '" + itemCode + "'GROUP BY T0.[BatchNum], T1.WhsName, T0.[ItemCode]having SUM(CASE T0.[Direction]                    when 0 then 1                    else -1             end * T0.[Quantity]) > 0";
            rs = (SAPbobsCOM.Recordset)tmpCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

            rs.DoQuery(sSQL);
            // Set the Customer Name and Customer Code Combo Boxes 
            rs.MoveFirst();
            int contador = 1;
            while (!(rs.EoF))
            {
                clsItemLoteTotal a = new clsItemLoteTotal(rs.Fields.Item(0).Value.ToString(), rs.Fields.Item(1).Value.ToString(), rs.Fields.Item(2).Value.ToString(), rs.Fields.Item(3).Value.ToString(), rs.Fields.Item(4).Value.ToString(), rs.Fields.Item(5).Value.ToString(), rs.Fields.Item(6).Value.ToString(), rs.Fields.Item(7).Value.ToString(), rs.Fields.Item(8).Value.ToString());
                contador = contador + 1;

                cls_itemsLotes.Add(a);
                //cmbCustomer.Items.Add(a); 
                rs.MoveNext();
            }
            tmpCompany.Disconnect();
            return cls_itemsLotes;
        }
         
        public List<clsItemsProduction> traeListaItemsREcibo(Company tCom, string Docnum,string almacen )
        {

            string v_almacen = "";
            if (almacen.Length <= 2)
            {
                v_almacen = " AND 0=1 ";
            }
            else
            {
                v_almacen = " AND T1.WHSCODE IN (" + almacen + ") ";
            }

            List<clsItemsProduction> cls_ItemsProduction = new List<clsItemsProduction>();
            try
            {
                //"SELECT DistNumber,ExpDate FROM OBTN WHERE STATUS =0 and itemCode = '04100101' order by expdate desc"

                Company tmpCompany;

                tmpCompany = tCom;

                SAPbobsCOM.Recordset rs;
                String sSQL = "";
                sSQL = "SELECT T0.DOCNUM, T1.ITEMCODE,T2.[ItemName], T1.[Quantity],T1.WHSCODE warehouse, ISNULL(T1.LINENUM,0)LINENUM,ISNULL(T1.U_LOTE,'') U_LOTE, ISNULL(T1.U_VENCE,'') U_VENCE,T2.invntryUom, T2.ISCOMMITED,(ISNULL(T2.ONORDER,0)+ISNULL(T2.ONHAND,0)- ISNULL(T2.ISCOMMITED,0))DISP, T2.MANBTCHNUM  FROM OIGN T0  INNER JOIN IGN1 T1 ON T0.DocEntry = T1.DocEntry INNER JOIN OITM T2 ON T1.ItemCode = T2.ItemCode WHERE T0.DOCNUM = '" + Docnum + "' " + v_almacen;
                rs = (SAPbobsCOM.Recordset)tmpCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                rs.DoQuery(sSQL);
                // Set the Customer Name and Customer Code Combo Boxes 
                rs.MoveFirst();
                int contador = 1;
                while (!(rs.EoF))
                {
                    clsItemsProduction a = new clsItemsProduction(rs.Fields.Item(0).Value.ToString(), rs.Fields.Item(1).Value.ToString(), rs.Fields.Item(2).Value.ToString(), "", rs.Fields.Item(3).Value.ToString(), "", rs.Fields.Item(4).Value.ToString(), rs.Fields.Item(5).Value.ToString(), rs.Fields.Item(6).Value.ToString(), "", rs.Fields.Item(7).Value.ToString(), "", "", "", "", rs.Fields.Item(8).Value.ToString(), rs.Fields.Item(9).Value.ToString(), rs.Fields.Item(10).Value.ToString(), rs.Fields.Item(11).Value.ToString(),"","","");
                    contador = contador + 1;

                    cls_ItemsProduction.Add(a);
                    //cmbCustomer.Items.Add(a); 
                    rs.MoveNext();
                }
                tmpCompany.Disconnect();
            }
            catch (Exception e)
            {
                string error = e.Message;
            }
            return cls_ItemsProduction;
        }

        public List<clsItemsProduction> traeListaItemsEmision(Company tCom, string Docnum, string almacen)
        {
            string v_almacen = "";
            if (almacen.Length <= 2)
            {
                v_almacen = " AND 0=1 ";
            }
            else
            {
                v_almacen = " AND T1.WHSCODE IN (" + almacen + ") ";
            }
            List<clsItemsProduction> cls_ItemsProduction = new List<clsItemsProduction>();
            try
            {
                //"SELECT DistNumber,ExpDate FROM OBTN WHERE STATUS =0 and itemCode = '04100101' order by expdate desc"

                Company tmpCompany;

                tmpCompany = tCom;

                SAPbobsCOM.Recordset rs;
                String sSQL = "";

                sSQL = "SELECT T0.DOCNUM, T1.ITEMCODE,T2.[ItemName], T1.[Quantity],T1.WHSCODE warehouse, ISNULL(T1.LINENUM,0)LINENUM,T1.U_LOTE, T1.U_VENCE,T2.invntryUom ,T2.ISCOMMITED,(ISNULL(T2.ONORDER,0)+ISNULL(T2.ONHAND,0)- ISNULL(T2.ISCOMMITED,0)) DISP ,T2.MANBTCHNUM     FROM OIGE T0  INNER JOIN IGE1 T1 ON T0.DocEntry = T1.DocEntry INNER JOIN OITM T2 ON T1.ItemCode = T2.ItemCode WHERE T0.DOCNUM ='" + Docnum + "' " + v_almacen;
                rs = (SAPbobsCOM.Recordset)tmpCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                rs.DoQuery(sSQL);
                // Set the Customer Name and Customer Code Combo Boxes 
                rs.MoveFirst();
                int contador = 1;
                while (!(rs.EoF))
                {
                    clsItemsProduction a = new clsItemsProduction(rs.Fields.Item(0).Value.ToString(), rs.Fields.Item(1).Value.ToString(), rs.Fields.Item(2).Value.ToString(), "", rs.Fields.Item(3).Value.ToString(), "", rs.Fields.Item(4).Value.ToString(), rs.Fields.Item(5).Value.ToString(), rs.Fields.Item(6).Value.ToString(), "", rs.Fields.Item(7).Value.ToString(), "", "", "", "", rs.Fields.Item(8).Value.ToString(), rs.Fields.Item(9).Value.ToString(), rs.Fields.Item(10).Value.ToString(), rs.Fields.Item(11).Value.ToString(),"","","");
                    contador = contador + 1;

                    cls_ItemsProduction.Add(a);
                    //cmbCustomer.Items.Add(a); 
                    rs.MoveNext();
                }
                tmpCompany.Disconnect();
            }
            catch (Exception e)
            {
                string error = e.Message;
            }
            return cls_ItemsProduction;
        }

        public List<clsItemsProduction> traeListaItemsDevolucion(Company tCom, string Docnum, string almacen)
        {
            string v_almacen = "";
            if (almacen.Length <= 2)
            {
                v_almacen = " AND 0=1 ";
            }
            else
            {
                v_almacen = " AND T1.WHSCODE IN (" + almacen + ") ";
            }

            List<clsItemsProduction> cls_ItemsProduction = new List<clsItemsProduction>();
            try
            {
                //"SELECT DistNumber,ExpDate FROM OBTN WHERE STATUS =0 and itemCode = '04100101' order by expdate desc"

                Company tmpCompany;

                tmpCompany = tCom;

                SAPbobsCOM.Recordset rs;
                String sSQL = "";
                sSQL = "SELECT T0.DOCNUM, T1.ITEMCODE,T2.[ItemName], T1.[Quantity],T1.WHSCODE warehouse, ISNULL(T1.LINENUM,0)LINENUM,T1.U_LOTE, T1.U_VENCE,T2.invntryUom, T2.ISCOMMITED ,(ISNULL(T2.ONORDER,0)+ISNULL(T2.ONHAND,0)- ISNULL(T2.ISCOMMITED,0))DISP, T2.MANBTCHNUM  FROM OIGN T0  INNER JOIN IGN1 T1 ON T0.DocEntry = T1.DocEntry INNER JOIN OITM T2 ON T1.ItemCode = T2.ItemCode WHERE T0.DOCNUM = '" + Docnum + "' " + v_almacen;
                rs = (SAPbobsCOM.Recordset)tmpCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                rs.DoQuery(sSQL);
                // Set the Customer Name and Customer Code Combo Boxes 
                rs.MoveFirst();
                int contador = 1;
                while (!(rs.EoF))
                {
                    clsItemsProduction a = new clsItemsProduction(rs.Fields.Item(0).Value.ToString(), rs.Fields.Item(1).Value.ToString(), rs.Fields.Item(2).Value.ToString(), "", rs.Fields.Item(3).Value.ToString(), "", rs.Fields.Item(4).Value.ToString(), rs.Fields.Item(5).Value.ToString(), rs.Fields.Item(6).Value.ToString(), "", rs.Fields.Item(7).Value.ToString(), "", "", "", "", rs.Fields.Item(8).Value.ToString(), rs.Fields.Item(9).Value.ToString(), rs.Fields.Item(10).Value.ToString(), rs.Fields.Item(11).Value.ToString(),"","","");
                    contador = contador + 1;

                    cls_ItemsProduction.Add(a);
                    //cmbCustomer.Items.Add(a); 
                    rs.MoveNext();
                }
                tmpCompany.Disconnect();
            }
            catch (Exception e)
            {
                string error = e.Message;
            }
            return cls_ItemsProduction;
        }
    }
}