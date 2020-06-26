using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace slnQF.Models
{
    public class FFarmaceuticaModels
    {
        public int ID_FORMA_FARMACEUTICA;

        [Required(ErrorMessage = "Ingresar Nombre!")]
        [StringLength(150, ErrorMessage = "La nombre es muy larga!")]
        [Display(Name = "Nombre")]
        public string NOMBRE;

        [Required(ErrorMessage = "Ingresar descripcion!")]
        [StringLength(1000, ErrorMessage = "La descripcion es muy larga!")]
        [Display(Name = "Descripcion")]
        public string DESCRIPCION;

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