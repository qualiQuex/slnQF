using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace slnQF.Models
{


    public class clsItemDatosModProd
    {
        public clsItemDatosModProd(string linenum, string qUANTITY, string wHSHOUSE, string rENGLON, string tARA)
        {
            this.linenum = linenum;
            QUANTITY = qUANTITY;
            WHSHOUSE = wHSHOUSE;
            RENGLON = rENGLON;
            TARA = tARA;
        }

        public string linenum { set; get; }
        public string QUANTITY { set; get; }
        public string WHSHOUSE { set; get; }

        public string RENGLON { set; get; }

        public string TARA { set; get; }
    }

    public class clsItemData {
        public clsItemData(string whsCode, string onHand, string isCommited, string onOrder) {
            WhsCode = whsCode;
            OnHand = onHand;
            IsCommited = isCommited;
            OnOrder = onOrder;
        }
        public string WhsCode { set; get; }
        public string OnHand { set; get; }
        public string IsCommited { set; get; }
        public string OnOrder { set; get; }

    }
    public class clsItemLoteTotal
    {
        public clsItemLoteTotal(string iTEMCODE, string wHSTOTAL, string bATCHNUM, string qUANTITY, string wHSCODE, string fECHA_VENCIM, string mNF_SERIAL, string lOT_NUMBER, string nOTES)
        {
            ITEMCODE = iTEMCODE;
            WHSTOTAL = wHSTOTAL;
            BATCHNUM = bATCHNUM;
            QUANTITY = qUANTITY;
            WHSCODE = wHSCODE;
            FECHA_VENCIM = fECHA_VENCIM;
            MNF_SERIAL = mNF_SERIAL;
            LOT_NUMBER = lOT_NUMBER;
            NOTES = nOTES;
        }

        /*
   * ITEMCODE
   * WHSTOTAL
   * BATCHNUM
   * QUANTITY
  */
        public string ITEMCODE { get; set; }
        public string WHSTOTAL { get; set; }
        public string BATCHNUM { get; set; }
        public string QUANTITY { get; set; }
        public string WHSCODE { get; set; }

        public string FECHA_VENCIM { get; set; }
        public string MNF_SERIAL { get; set; }

        public string LOT_NUMBER { get; set; }
        public string NOTES { get; set; }
    }

    public class clsLoteSeleccionadoRecibo
    {
        public clsLoteSeleccionadoRecibo(string iTEMCODE, string wHSTOTAL, string bATCHNUM, string qUANTITY, string tTOTAL, string fECHA_EXP, string nUEVO, string mNFSERIAL, string lOTNUMBER,string lOTNOTE)
        {
            ITEMCODE = iTEMCODE;
            WHSTOTAL = wHSTOTAL;
            BATCHNUM = bATCHNUM;
            QUANTITY = qUANTITY;
            TOTAL = tTOTAL;
            FECHA_EXP = fECHA_EXP;
            NUEVO = nUEVO;
            MNFSERIAL = mNFSERIAL;
            LOTNUMBER = lOTNUMBER;
            LOTNOTE = lOTNOTE;
        }

        /*
   * ITEMCODE 
   * WHSTOTAL
   * BATCHNUM
   * QUANTITY
  */
        public string ITEMCODE { get; set; }
        public string WHSTOTAL { get; set; }
        public string BATCHNUM { get; set; }
        public string QUANTITY { get; set; }
        public string TOTAL { get; set; }
        public string FECHA_EXP { get; set; }

        public string NUEVO { get; set; }

        public string MNFSERIAL { get; set; }
        public string LOTNUMBER { get; set; }
        public string LOTNOTE { get; set; }
    }
    public class clsLoteSeleccionado
    {
        public clsLoteSeleccionado(string iTEMCODE, string wHSTOTAL, string bATCHNUM, string qUANTITY, string tTOTAL, string fECHA_EXP, string nUEVO, string mNFSERIAL)
        {
            ITEMCODE = iTEMCODE;
            WHSTOTAL = wHSTOTAL;
            BATCHNUM = bATCHNUM;
            QUANTITY = qUANTITY;
            TOTAL = tTOTAL;
            FECHA_EXP = fECHA_EXP;
            NUEVO = nUEVO;
            MNFSERIAL = mNFSERIAL;
        }

        /*
   * ITEMCODE
   * WHSTOTAL
   * BATCHNUM
   * QUANTITY
  */
        public string ITEMCODE { get; set; }
        public string WHSTOTAL { get; set; }
        public string BATCHNUM { get; set; }
        public string QUANTITY { get; set; }
        public string TOTAL { get; set; }
        public string FECHA_EXP { get; set; }

        public string NUEVO { get; set; }

        public string MNFSERIAL { get; set; }
    }
    public class clsTarasRenglon
    {
        public clsTarasRenglon(string lINENUM, string tARA, string pESO_NETO, string dOCNUM, string iD_RESPONSABLE)
        {
            LINENUM = lINENUM;
            TARA = tARA;
            PESO_NETO = pESO_NETO;
            DOCNUM = dOCNUM;
            ID_RESPONSABLE = iD_RESPONSABLE;

        }


        public string LINENUM { get; set; }
        public string TARA { get; set; }
        public string PESO_NETO { get; set; }
        public string DOCNUM { get; set; }
        public string ID_RESPONSABLE { get; set; }

    }
    public class linenumPAR {
        public linenumPAR(int line1, int line2) {
            this.linenum1 = line1;
            this.linenum2 = line2;
        }
        public int linenum1 { get; set; }
        public int linenum2 { get; set; }

    }
    public class clsItemsProduction
    {


        public clsItemsProduction(string noOrden, string itemCode, string itemName, string type, string quantity, string precio, string warehouse, string linenum, string lote, string renglon, string vence, string whsname, string cantidadbase, string cantidadrequerida, string tara, string uniMedida,string cantComprometida, string cantDisponible,string swlote,string comprometido,string stock,string solicitado)
        {
            this.noOrden = noOrden;
            this.itemCode = itemCode;
            this.itemName = itemName;
            this.type = type;
            this.quantity = quantity;
            this.precio = precio;
            this.warehouse = warehouse;
            this.linenum = linenum;
            this.lote = lote;
            this.renglon = renglon;
            this.vence = vence;
            this.whsname = whsname;
            this.cantidadbase = cantidadbase;
            this.cantidadrequerida = cantidadrequerida;
            this.TARA = tara;
            this.UniMedida = uniMedida;
            this.CantComprometida = cantComprometida;
            this.CantDisponible = cantDisponible;
            this.SwLote = swlote;
            this.Comprometido = comprometido;
            this.Stock = stock;
            this.Solicitado = solicitado;
        }

        public string noOrden { get; set; }
        public string itemCode { get; set; }
        public string itemName { get; set; }
        public string type { get; set; }
        public string quantity { get; set; }
        public string precio { get; set; }
        public string warehouse { get; set; }
        public string linenum { get; set; }

        public string lote { get; set; }

        public string renglon { get; set; }
        public string vence { get; set; }

        public string whsname { get; set; }

        public string cantidadbase { get; set; }

        public string cantidadrequerida { get; set; }

        public string TARA { get; set; }

        public string UniMedida { get; set; }

        public string CantComprometida { get; set; }

        public string CantDisponible { get; set; }

        public string SwLote { get; set; }

        public string Comprometido { get; set; }

        public string Stock { get; set; }

        public string Solicitado { get; set; }
    }


    public class clsProduccionModel
    {
        public clsProduccionModel(string docentry, string tipo, string estado, string descripcionProd, string cantidadPlani, string almacen, string serie, string docNum, string fechaFabricacion, string fechaFinalizacion, string usuario, string pedidoCliente, string cliente, string normaReparto, string proyecto, string comentario, string tipoFabricacion, string formulaMaestra, string formaFarmaceutica, string unidadesFabrica, string lote, string vence, string supervisor, string itemcode, string itemname, string precio, string almacen_desc, string origen, string dispensador, string observaciones, string verificador, string fechaentrega, string cantidad, string unidadmedida)
        {
            DocEntry = docentry;
            Tipo = tipo;
            Estado = estado;
            DescripcionProd = descripcionProd;
            CantidadPlani = cantidadPlani;
            Almacen = almacen;
            Serie = serie;
            DocNum = docNum;
            FechaFabricacion = fechaFabricacion;
            FechaFinalizacion = fechaFinalizacion;
            Usuario = usuario;
            PedidoCliente = pedidoCliente;
            Cliente = cliente;
            NormaReparto = normaReparto;
            Proyecto = proyecto;
            Comentario = comentario;
            TipoFabricacion = tipoFabricacion;
            FormulaMaestra = formulaMaestra;
            FormaFarmaceutica = formaFarmaceutica;
            UnidadesFabrica = unidadesFabrica;
            Lote = lote;
            Vence = vence;
            Supervisor = supervisor;
            ItemCode = itemcode;
            ItemName = itemname;
            Precio = precio;
            Almacen_Desc = almacen_desc;

            Origen = origen;

            Dispensador = dispensador;

            Observaciones = observaciones;

            Verificador = verificador;

            FechaEntrega = fechaentrega;

            Cantidad = cantidad;

            UnidadMedida = unidadmedida;

        }

        public string Tipo { get; set; }
        public string Estado { get; set; }
        public string DescripcionProd { get; set; }

        public string CantidadPlani { get; set; }
        public string Almacen { get; set; }

        public string Serie { get; set; }

        public string DocNum { get; set; }

        public string FechaFabricacion { get; set; }

        public string FechaFinalizacion { get; set; }

        public string Usuario { get; set; }

        public string PedidoCliente { get; set; }

        public string Cliente { get; set; }

        public string NormaReparto { get; set; }

        public string Proyecto { get; set; }

        public string Comentario { get; set; }

        public string TipoFabricacion { get; set; }

        public string FormulaMaestra { get; set; }

        public string FormaFarmaceutica { get; set; }

        public string UnidadesFabrica { get; set; }

        public string Lote { get; set; }

        public string Vence { get; set; }

        public string Supervisor { get; set; }

        public string DocEntry { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public string Precio { get; set; }

        public string Almacen_Desc { get; set; }
        public string Origen { get; set; }

        public string Dispensador { get; set; }

        public string Observaciones { get; set; }

        public string Verificador { get; set; }

        public string FechaEntrega { get; set; }

        public string Cantidad { get; set; }

        public string UnidadMedida { get; set; }

        public static List<clsProduccionModel> GetSampleclsItem()
        {
            return new List<clsProduccionModel>
            {
                /*
                           new clsItem(Id:"1", Nombre: "Remote Car", Precio:"9.99", Impuesto:"IVA"),
                           new clsItem(Id:"2", Nombre: "Boll Pen", Precio:"2.99", Impuesto:"IVA"),
                           new clsItem(Id:"3", Nombre: "Teddy Bear", Precio:"6.99", Impuesto:"IVA"),
                           new clsItem(Id:"4", Nombre: "Tennis Boll", Precio:"6.99", Impuesto:"IVA"),
                           new clsItem(Id:"5", Nombre: "Super Man", Precio:"6.99", Impuesto:"IVA"),
                           new clsItem(Id:"6", Nombre: "Bikes", Precio:"4.99", Impuesto:"IVA"),
                           new clsItem(Id:"7", Nombre: "Books", Precio:"7.99", Impuesto:"EXE"),
                           new clsItem(Id:"8", Nombre: "Mobiles", Precio:"5.99", Impuesto:"EXE"),
                           new clsItem(Id:"9", Nombre: "Laptops", Precio:"15.99", Impuesto:"EXE"),
                           new clsItem(Id:"10", Nombre: "Note Books", Precio:"2.99", Impuesto:"EXE")
                           */
            };
        }
    }
}