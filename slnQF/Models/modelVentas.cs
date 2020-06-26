using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace slnQF.Models
{
    public class modelVentas
    {
        public string txt_referencia;
        public String txt_referencia2;
    }
    public class modelCliente
    {
        public string id_Cliente;
        public String nombre_cliente;
    }

    public class modelContacto
    {
        public string id_Contacto;
        public string nombre_Contacto;
    }

    public class modelFormaVenta {
        public modelCliente cliente;
        public List<modelContacto> contactos;
    }

}