using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace slnQF.Models
{
    public class clsJson
    {
    }
    public class reciboclsLoteSeleccionado /* transferencia de stock*/
    {
        public string ITEMCODE;
        public string BATCHNUM;
        public string QUANTITY;
        public string TTOTAL;
        public string FECHA_EXP;
        public string NUEVO;
        public string MNFSERIAL;
        public string LOTNUMBER;

        public string LOTNOTES;
    }

    public class emisionclsLoteSeleccionado /*tambien para deovlucion y transferencia de stock*/
    {
        public string ITEMCODE;
        public string BATCHNUM;
        public string QUANTITY;
        public string TTOTAL;
        public string FECHA_EXP;
        public string NUEVO;
    }
    public class emisionclsTara
    {
        public string LINENUM;
        public string ITEMCODE;
        public string TARA;
        public string PESONETO;
    }

    public class emisionclsDatosModProd  
    {
        public string linenum;
        public string QUANTITY;
        public string WHSHOUSE;
        public string RENGLON;
        public string TARA;

    }

    public class devolucionclsDatosModProd
    {
        public string linenum;
        public string QUANTITY;
        public string WHSHOUSE;

    }
    public class itemTransferenciaStock{
        public string ITEMCODE;
        public string CANTIDAD;
        public string ALMACEN_DESTINO;
        public string UOM;

    }
}