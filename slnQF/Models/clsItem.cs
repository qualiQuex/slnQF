using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace slnQF.Models
{
    public class MyDrop
    {
        public int id { get; set; }
        public string value { get; set; }
    }
    public class clsItemEtiquetaCantidad {
        public clsItemEtiquetaCantidad(string iDITEMEMBALAJE, string lIMITE, string cANTIDAD, string nOETIQUETAS)
        {
            IDITEMEMBALAJE = iDITEMEMBALAJE;
            LIMITE = lIMITE;
            CANTIDAD = cANTIDAD;
            NOETIQUETAS = nOETIQUETAS;
        }

        public string IDITEMEMBALAJE { get; set; }
        public string LIMITE { get; set; }
        public string CANTIDAD { get; set; }
        public string NOETIQUETAS { get; set; } 
    }
    public class clsItem
    {


        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Precio { get; set; }
        public string Impuesto { get; set; }

        public string Cantidad { get; set; }
        public string Lote{ get; set; }


        public clsItem(string Id, string Nombre, string Precio, string Impuesto, string Cantidad,string Lote)
        {
            this.Id = Id;
            this.Nombre = Nombre;
            this.Precio = Precio;
            this.Impuesto = Impuesto;
            this.Cantidad = Cantidad;
            this.Lote = Lote;
        }
        public static List<clsItem> GetSampleclsItem()
        {
            return new List<clsItem>
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