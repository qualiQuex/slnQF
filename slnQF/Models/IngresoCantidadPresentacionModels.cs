using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace slnQF.Models
{
    public class IngresoCantidadPresentacionModels
    {
        public int ID_INGRESO_CANTIDAD_PRESETNACION;

        public int ID_LOTE_INGRESO;

        public int ID_PRESENTACION;

        public int ID_NOMBRE_COMERCIAL;

        public int ID_UOM;

        public int CANTIDAD;

        public int ID_CLIENTE;

        public string LOTE;

        public string ID_ESTADO;

        [Display(Name = "FECHA_CREACION")]
        public System.DateTime FECHA_CREACION;

        [Display(Name = "ID_USUARIO")]
        public int ID_USUARIO;
    }
}