using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace slnQF.Models
{
    public class EspecificacionProductoModels
    {
        public int ID_ESPECIFICACION_PRODUCTO;

        public int ID_PRODUCTO;

        public int ID_ENSAYO;

        public int ID_PRUEBA;

        public int ID_PRUEBA_ESPECIFICA;

        [Required(ErrorMessage = "Ingresar tipo de respuesta!")]
        [Range(1, 500, ErrorMessage = "Product price must be greater than 0!")]
        [Display(Name = "TIPO_RESPUESTA")]

        public string TIPO_RESPUESTA;


        [Required(ErrorMessage = "Ingresar especifcacion!")]
        [Range(1, 500, ErrorMessage = "Product price must be greater than 0!")]
        [Display(Name = "ESPECIFICACION")]

        public string ESPECIFICACION;
         
        [Required(ErrorMessage = "Ingresar estado!")]
        [Range(1, 10000, ErrorMessage = "Product price must be greater than 0!")]
        [Display(Name = "ESTADO")]
        public int ESTADO;


        [Display(Name = "FECHA_CREACION")]
        public System.DateTime FECHA_CREACION;

        [Display(Name = "ID_USUARIO")]
        public int ID_USUARIO;
    }
}