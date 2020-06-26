using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace slnQF.Models
{
    public class ClienteModels
    {
        public int ID_CLIENTE;

        [Required(ErrorMessage = "Ingresar nombre!")]
        [Range(1, 300, ErrorMessage = "Product price must be greater than 0!")]
        [Display(Name = "Nombre")]

        public string NOMBRE;

        [Required(ErrorMessage = "Ingresar codigo SAP!")] 
        [Display(Name = "Codigo SAP")]

        public string CODIGO_SAP;

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