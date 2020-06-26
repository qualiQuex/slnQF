using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Xml.Linq;

namespace slnQF.Models
{
    public class ReporteModels
    {
    }
    public class ReporteParamValModels {

        public int ID_REPORTE_PARAMETRO { get; set; }
        public string NOMBRE_PARAMETRO { get; set; }
        public int ID_TIPO_DATO { get; set; }
        public string VALOR { get; set; }
        public int ID_REPORTE { get; set; }

    }
    public class AutenticacionWeb
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public AutenticacionWeb()
        {
            UserName = "username";
            Password = "password";
        }
    }
    public class RespuestaWS
    {
        public int Codigo { get; set; }
        public string errorMsg1 { get; set; }
        public string errorMsg2 { get; set; }
        public object Valor { get; set; }
    }

    public partial class SP_REPORTE_FACTURAMENSUAL {
        private string _CardCode;
        private string _CardName;
        private int _DocNum;
        private DateTime _DocDate;
        private string _ItemCode;
        private string _ItemName;
        private decimal _Quantity;
        private decimal _DiscPrcnt;
        private decimal _LineTotal;
        private decimal _PriceAfVAT;
        private decimal _GTotalSC;
        private decimal _GTotal;
        private string _SlpName;
        private string _GroupName;
        private string _Name;

        public string CardCode
        {
            get
            {
                return _CardCode;
            }

            set
            {
                _CardCode = value;
            }
        }

        public string CardName
        {
            get
            {
                return _CardName;
            }

            set
            {
                _CardName = value;
            }
        }

        public int DocNum
        {
            get
            {
                return _DocNum;
            }

            set
            {
                _DocNum = value;
            }
        }

        public DateTime DocDate
        {
            get
            {
                return _DocDate;
            }

            set
            {
                _DocDate = value;
            }
        }

        public string ItemCode
        {
            get
            {
                return _ItemCode;
            }

            set
            {
                _ItemCode = value;
            }
        }

        public string ItemName
        {
            get
            {
                return _ItemName;
            }

            set
            {
                _ItemName = value;
            }
        }

        public decimal Quantity
        {
            get
            {
                return _Quantity;
            }

            set
            {
                _Quantity = value;
            }
        }

        public decimal DiscPrcnt
        {
            get
            {
                return _DiscPrcnt;
            }

            set
            {
                _DiscPrcnt = value;
            }
        }

        public decimal LineTotal
        {
            get
            {
                return _LineTotal;
            }

            set
            {
                _LineTotal = value;
            }
        }

        public decimal PriceAfVAT
        {
            get
            {
                return _PriceAfVAT;
            }

            set
            {
                _PriceAfVAT = value;
            }
        }

        public decimal GTotalSC
        {
            get
            {
                return _GTotalSC;
            }

            set
            {
                _GTotalSC = value;
            }
        }

        public decimal GTotal
        {
            get
            {
                return _GTotal;
            }

            set
            {
                _GTotal = value;
            }
        }

        public string SlpName
        {
            get
            {
                return _SlpName;
            }

            set
            {
                _SlpName = value;
            }
        }

        public string GroupName
        {
            get
            {
                return _GroupName;
            }

            set
            {
                _GroupName = value;
            }
        }

        public string Name
        {
            get
            {
                return _Name;
            }

            set
            {
                _Name = value;
            }
        }
    }
    public class clsFilas
    {

        private string m_ID_MODULO;
        private string m_TIPO_GRUPO;
        private string m_ID;
        private string m_TOTAL;

        public string ID_MODULO
        {
            get { return m_ID_MODULO; }

            set { value = m_ID_MODULO; }
        }



        public string ID
        {
            get { return m_ID; }

            set { value = m_ID; }
        }

        public string TOTAL
        {
            get { return m_TOTAL; }

            set { value = m_TOTAL; }
        }

        public string TIPO_GRUPO
        {
            get { return m_TIPO_GRUPO; }

            set { value = m_TIPO_GRUPO; }
        }

        public clsFilas() { }

        public clsFilas(string ID_MODULO, string ID, string TOTAL, string TIPO_GRUPO)
        {
            m_ID_MODULO = ID_MODULO;
            m_TIPO_GRUPO = TIPO_GRUPO;
            m_ID = ID;
            m_TOTAL = TOTAL;
        }
    }

    public class ClsTipoPago
    {

        private string m_ID_MODULO;
        private string m_ID_TPAGO;
        private string m_TOTAL;

        public string ID_MODULO
        {
            get { return m_ID_MODULO; }

            set { value = m_ID_MODULO; }
        }

        public string ID_TPAGO
        {
            get { return m_ID_TPAGO; }

            set { value = m_ID_TPAGO; }
        }

        public string TOTAL
        {
            get { return m_TOTAL; }

            set { value = m_TOTAL; }
        }

        public ClsTipoPago() { }

        public ClsTipoPago(string ID_MODULO, string ID_TPAGO, string TOTAL)
        {
            m_ID_MODULO = ID_MODULO;
            m_ID_TPAGO = ID_TPAGO;
            m_TOTAL = TOTAL;
        }
    }
    public class ClsTipoLicencia
    {

        private string m_ID_MODULO;
        private string m_ID_TLICENCIA;
        private string m_TOTAL;

        public string ID_MODULO
        {
            get { return m_ID_MODULO; }

            set { value = m_ID_MODULO; }
        }

        public string ID_TLICENCIA
        {
            get { return m_ID_TLICENCIA; }

            set { value = m_ID_TLICENCIA; }
        }

        public string TOTAL
        {
            get { return m_TOTAL; }

            set { value = m_TOTAL; }
        }

        public ClsTipoLicencia() { }

        public ClsTipoLicencia(string ID_MODULO, string ID_TLICENCIA, string TOTAL)
        {
            m_ID_MODULO = ID_MODULO;
            m_ID_TLICENCIA = ID_TLICENCIA;
            m_TOTAL = TOTAL;
        }
    }

    public class ClsTipoTramite
    {

        private string m_ID_MODULO;
        private string m_ID_TTRAMITE;
        private string m_TOTAL;

        public string ID_MODULO
        {
            get { return m_ID_MODULO; }

            set { value = m_ID_MODULO; }
        }

        public string ID_TTRAMITE
        {
            get { return m_ID_TTRAMITE; }

            set { value = m_ID_TTRAMITE; }
        }

        public string TOTAL
        {
            get { return m_TOTAL; }

            set { value = m_TOTAL; }
        }

        public ClsTipoTramite() { }

        public ClsTipoTramite(string ID_MODULO, string ID_TTRAMITE, string TOTAL)
        {
            m_ID_MODULO = ID_MODULO;
            m_ID_TTRAMITE = ID_TTRAMITE;
            m_TOTAL = TOTAL;
        }
    }

    public class ClsPrueba
    {

        private string m_ID;
        private string m_VALOR;
        

        public string ID
        {
            get { return m_ID; }

            set { value = m_ID; }
        }

        public string VALOR
        {
            get { return m_VALOR; }

            set { value = m_VALOR; }
        }

       
        public ClsPrueba() { }

        public ClsPrueba(string ID, string VALOR)
        {
            m_ID = ID;
            m_VALOR = VALOR;
            
        }
    }

    public class ClsEtiquetaPesado {
        private string m_PRODUCTO;
        private string m_LOTE;
        private string m_ORDEN;
        private string m_MATERIAPRIMA;
        private string m_BULTO;
        private string m_BULTO_DE;
        private string m_NOMBRE_PRODUCTO;
        private string m_VENCE;
        private string m_LOTE2;
        private string m_NOANALISIS;
        private string m_POTENCIA;
        private string m_PESO_NETO;
        private string m_TARA;
        private string m_FECHA;
        private string m_PESO_BRUTO;
        private string m_FRACCIONADO_POR;
        private string m_VERIFICADO_POR;
        private string m_UOM;
        private string m_ORDEN_PRODUCCION;

        public ClsEtiquetaPesado(string m_PRODUCTO, string m_LOTE, string m_ORDEN, string m_MATERIAPRIMA, string m_BULTO, string m_BULTO_DE, string m_NOMBRE_PRODUCTO, string m_VENCE, string m_LOTE2, string m_NOANALISIS, string m_POTENCIA, string m_PESO_NETO, string m_TARA, string m_FECHA, string m_PESO_BRUTO, string m_FRACCIONADO_POR, string m_VERIFICADO_POR,string m_UOM, string m_ORDEN_PRODUCCION)
        {
            this.m_PRODUCTO = m_PRODUCTO;
            this.m_LOTE = m_LOTE;
            this.m_ORDEN = m_ORDEN;
            this.m_MATERIAPRIMA = m_MATERIAPRIMA;
            this.m_BULTO = m_BULTO;
            this.m_BULTO_DE = m_BULTO_DE;
            this.m_NOMBRE_PRODUCTO = m_NOMBRE_PRODUCTO;
            this.m_VENCE = m_VENCE;
            this.m_LOTE2 = m_LOTE2;
            this.m_NOANALISIS = m_NOANALISIS;
            this.m_POTENCIA = m_POTENCIA;
            this.m_PESO_NETO = m_PESO_NETO;
            this.m_TARA = m_TARA;
            this.m_FECHA = m_FECHA;
            this.m_PESO_BRUTO = m_PESO_BRUTO;
            this.m_FRACCIONADO_POR = m_FRACCIONADO_POR;
            this.m_VERIFICADO_POR = m_VERIFICADO_POR;
            this.m_UOM = m_UOM;
            this.m_ORDEN_PRODUCCION = m_ORDEN_PRODUCCION;
        }

        public string PRODUCTO
        {
            get
            {
                return m_PRODUCTO;
            }

            set
            {
                m_PRODUCTO = value;
            }
        }

        public string LOTE
        {
            get
            {
                return m_LOTE;
            }

            set
            {
                m_LOTE = value;
            }
        }
 
        public string ORDEN
        {
            get
            {
                return m_ORDEN;
            }

            set
            {
                m_ORDEN = value;
            }
        }

        public string MATERIAPRIMA
        {
            get
            {
                return m_MATERIAPRIMA;
            }

            set
            {
                m_MATERIAPRIMA = value;
            }
        }

        public string BULTO
        {
            get
            {
                return m_BULTO;
            }

            set
            {
                m_BULTO = value;
            }
        }

        public string BULTO_DE
        {
            get
            {
                return m_BULTO_DE;
            }

            set
            {
                m_BULTO_DE = value;
            }
        }

        public string NOMBRE_PRODUCTO
        {
            get
            {
                return m_NOMBRE_PRODUCTO;
            }

            set
            {
                m_NOMBRE_PRODUCTO = value;
            }
        }

        public string VENCE
        {
            get
            {
                return m_VENCE;
            }

            set
            {
                m_VENCE = value;
            }
        }

        public string LOTE2
        {
            get
            {
                return m_LOTE2;
            }

            set
            {
                m_LOTE2 = value;
            }
        }

        public string NOANALISIS
        {
            get
            {
                return m_NOANALISIS;
            }

            set
            {
                m_NOANALISIS = value;
            }
        }

        public string POTENCIA
        {
            get
            {
                return m_POTENCIA;
            }

            set
            {
                m_POTENCIA = value;
            }
        }

        public string PESO_NETO
        {
            get
            {
                return m_PESO_NETO;
            }

            set
            {
                m_PESO_NETO = value;
            }
        }

        public string TARA
        {
            get
            {
                return m_TARA;
            }

            set
            {
                m_TARA = value;
            }
        }

        public string FECHA
        {
            get
            {
                return m_FECHA;
            }

            set
            {
                m_FECHA = value;
            }
        }

        public string PESO_BRUTO
        {
            get
            {
                return m_PESO_BRUTO;
            }

            set
            {
                m_PESO_BRUTO = value;
            }
        }

        public string FRACCIONADO_POR
        {
            get
            {
                return m_FRACCIONADO_POR;
            }

            set
            {
                m_FRACCIONADO_POR = value;
            }
        }

        public string VERIFICADO_POR
        {
            get
            {
                return m_VERIFICADO_POR;
            }

            set
            {
                m_VERIFICADO_POR = value;
            }
        }
        public string UOM
        {
            get
            {
                return m_UOM;
            }

            set
            {
                m_UOM = value;
            }
        }
        public string ORDEN_PRODUCCION
        {
            get
            {
                return m_ORDEN_PRODUCCION;
            }

            set
            {
                m_ORDEN_PRODUCCION = value;
            }
        }
    }

    public class ClsAnexoB
    {
        private string m_ORDEN_PRODUCCION;
        private string m_NOMBRE_PRODUCTO;
        private string m_LOTE;
        private string m_ORDEN_EMISION;
        private string m_ITEMCODE;
        private string m_UOM;
        private string m_LOTE_ITEM;
        private string m_CANTIDAD;
        private string m_DESCRIPCION_ITEM;
        private string m_LINENUM;
        private string m_OPERADOR;
        private DateTime m_FECHA_OPERACION;
        private string m_U_VENCE;

        public string U_VENCE
        {
            get { return m_U_VENCE; }
            set { m_U_VENCE = value; }
        }
        private string m_BULTO;

        public string BULTO
        {
            get { return m_BULTO; }
            set { m_BULTO = value; }
        }
        private string m_BULTO_DE;

        public string BULTO_DE
        {
            get { return m_BULTO_DE; }
            set { m_BULTO_DE = value; }
        }
        private string m_U_U_FABRICAR;

        public string U_U_FABRICAR
        {
            get { return m_U_U_FABRICAR; }
            set { m_U_U_FABRICAR = value; }
        }

        public DateTime FECHA_OPERACION
        {
            get { return m_FECHA_OPERACION; }
            set { m_FECHA_OPERACION = value; }
        }
        private string m_VERIFICADOR;

        public string VERIFICADOR
        {
            get { return m_VERIFICADOR; }
            set { m_VERIFICADOR = value; }
        }
        private string m_REF2;

        public string REF2
        {
            get { return m_REF2; }
            set { m_REF2 = value; }
        }
        private string m_COMENTARIOS;

        public string COMENTARIOS
        {
            get { return m_COMENTARIOS; }
            set { m_COMENTARIOS = value; }
        }


        public ClsAnexoB(string oRDEN_PRODUCCION, string nOMBRE_PRODUCTO, string lOTE, string oRDEN_EMISION, string iTEMCODE, string uOM, string lOTE_ITEM, string cANTIDAD, string dESCRIPCION_ITEM, string lINENUM, string oPERADOR, DateTime fECHA_OPERACION, string vERIFICADOR, string cOMENARIOS,string rEF2,string u_VENCE, string bULTO, string bULTO_DE, string u_U_FABRICAR)
        {
            m_ORDEN_PRODUCCION = oRDEN_PRODUCCION;
            m_NOMBRE_PRODUCTO = nOMBRE_PRODUCTO;
            m_LOTE = lOTE;
            m_ORDEN_EMISION = oRDEN_EMISION;
            m_ITEMCODE = iTEMCODE;
            m_UOM = uOM;
            m_LOTE_ITEM = lOTE_ITEM;
            m_CANTIDAD = cANTIDAD;
            m_DESCRIPCION_ITEM = dESCRIPCION_ITEM;
            m_LINENUM = lINENUM;
            m_OPERADOR = oPERADOR;
            m_FECHA_OPERACION = fECHA_OPERACION;
            m_VERIFICADOR = vERIFICADOR;
            m_REF2 = rEF2;
            m_COMENTARIOS = cOMENARIOS;
            m_U_VENCE = u_VENCE;
            m_BULTO = bULTO;
            m_BULTO_DE = bULTO_DE;
            m_U_U_FABRICAR = u_U_FABRICAR;
        }

        public string ORDEN_PRODUCCION
        {
            get
            {
                return m_ORDEN_PRODUCCION;
            }

            set
            {
                m_ORDEN_PRODUCCION = value;
            }
        }
        public string NOMBRE_PRODUCTO
        {
            get
            {
                return m_NOMBRE_PRODUCTO;
            }

            set
            {
                m_NOMBRE_PRODUCTO = value;
            }
        }
        public string LOTE
        {
            get
            {
                return m_LOTE;
            }

            set
            {
                m_LOTE = value;
            }
        }
        public string ORDEN_EMISION
        {
            get
            {
                return m_ORDEN_EMISION;
            }

            set
            {
                m_ORDEN_EMISION = value;
            }
        }
        public string ITEMCODE
        {
            get
            {
                return m_ITEMCODE;
            }

            set
            {
                m_ITEMCODE = value;
            }
        }
        public string UOM
        {
            get
            {
                return m_UOM;
            }

            set
            {
                m_UOM = value;
            }
        }
        public string LOTE_ITEM
        {
            get
            {
                return m_LOTE_ITEM;
            }

            set
            {
                m_LOTE_ITEM = value;
            }
        }
        public string CANTIDAD
        {
            get
            {
                return m_CANTIDAD;
            }

            set
            {
                m_CANTIDAD = value;
            }
        }
        public string DESCRIPCION_ITEM
        {
            get
            {
                return m_DESCRIPCION_ITEM;
            }

            set
            {
                m_DESCRIPCION_ITEM = value;
            }
        }
        public string LINENUM
        {
            get
            {
                return m_LINENUM;
            }

            set
            {
                m_LINENUM = value;
            }
        }
        public string OPERADOR
        {
            get
            {
                return m_OPERADOR;
            }

            set
            {
                m_OPERADOR = value;
            }
        }

    }
    public class SearchParameterModel
    {

        [Display(Name = "Orden de produccion")]
        public string docnum
        {
            get;

            set;
        }
        public string Format
        {
            get;

            set;
        }

    }

    public static class ListConverter
    {

        /// <summary>
        /// Convert our List to a DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns>DataTable</returns>
        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            object[] values = new object[props.Count];
            using (DataTable table = new DataTable())
            {
                long _pCt = props.Count;
                for (int i = 0; i < _pCt; ++i)
                {
                    PropertyDescriptor prop = props[i];
                    table.Columns.Add(prop.Name, prop.PropertyType);
                }
                foreach (T item in data)
                {
                    long _vCt = values.Length;
                    for (int i = 0; i < _vCt; ++i)
                    {
                        values[i] = props[i].GetValue(item);
                    }
                    table.Rows.Add(values);
                }
                return table;
            }
        }

        /// <summary>
        /// Convert our List to a DataSet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns>DataSet</returns>
        public static DataSet ToDataSet<T>(this IList<T> list)
        {
            Type elementType = typeof(T);
            using (DataSet ds = new DataSet())
            {
                using (DataTable t = new DataTable())
                {
                    ds.Tables.Add(t);
                    //add a column to table for each public property on T
                    PropertyInfo[] _props = elementType.GetProperties();
                    foreach (PropertyInfo propInfo in _props)
                    {
                        Type _pi = propInfo.PropertyType;
                        Type ColType = Nullable.GetUnderlyingType(_pi) ?? _pi;
                        t.Columns.Add(propInfo.Name, ColType);
                    }
                    //go through each property on T and add each value to the table
                    foreach (T item in list)
                    {
                        DataRow row = t.NewRow();
                        foreach (PropertyInfo propInfo in _props)
                        {
                            row[propInfo.Name] = propInfo.GetValue(item, null) ?? DBNull.Value;
                        }
                        t.Rows.Add(row);
                    }
                }
                return ds;
            }
        }

    }
}