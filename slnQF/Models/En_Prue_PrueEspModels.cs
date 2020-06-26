using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace slnQF.Models
{
    public class En_Prue_PrueEspModels
    {
        public int ID_ENSAYO;

        public int ID_PRUEBA;

        public int ID_PRUEBA_ESPECIFICA;

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