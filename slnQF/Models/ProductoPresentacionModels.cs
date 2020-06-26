using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace slnQF.Models
{
    public class ProductoPresentacionModels
    {
        public int ID_PRODUCTO;

        public int ID_PRESENTACION;

        [Required(ErrorMessage = "Ingresar estado!")]
        [Range(1, 10000, ErrorMessage = "Product price must be greater than 0!")]
        [Display(Name = "ESTADO")]
        public int ESTADO;

        [Required(ErrorMessage = "Ingresar sino!")]
        [Range(1, 10000, ErrorMessage = "Product price must be greater than 0!")]
        [Display(Name = "SI/NO")]
        public int SWSI_NO;

        [Display(Name = "FECHA_CREACION")]
        public System.DateTime FECHA_CREACION;

        [Display(Name = "ID_USUARIO")]
        public int ID_USUARIO;
    }
}