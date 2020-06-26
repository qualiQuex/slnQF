using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace slnQF.Models
{
    public class ProductoModels
    {
        public int ID_PRODUCTO;

        [Required(ErrorMessage = "Ingresar Nombre!")]
        [StringLength(150, ErrorMessage = "La nombre es muy larga!")]
        [Display(Name = "Nombre")]
        public string NOMBRE;

        [Required(ErrorMessage = "Ingresar descripcion!")]
        [StringLength(1000, ErrorMessage = "La descripcion es muy larga!")]
        [Display(Name = "Descripcion")]
        public string DESCRIPCION;

        [Required(ErrorMessage = "Ingresar forma farmaceutica!")]
        [Range(1, 10000, ErrorMessage = "Product price must be greater than 0!")]
        [Display(Name = "F. Farmaceutica")]
        public int ID_FORMA_FARMACEUTICA;


        [Required(ErrorMessage = "Ingresar Concentracion!")]
        [StringLength(1000, ErrorMessage = "La referencia es muy larga!")]
        [Display(Name = "Concentracion")]
        public string CONCENTRACION;

        [Required(ErrorMessage = "Ingresar Cantidad de muestras!")]
        [Range(1, 10000, ErrorMessage = "Product price must be greater than 0!")]
        [Display(Name = "Cantidad de muestras")]
        public int CANTIDAD_MUESTRAS;

        [Required(ErrorMessage = "Ingresar estado!")]
        [Range(1, 10000, ErrorMessage = "Product price must be greater than 0!")]
        [Display(Name = "ESTADO")]
        public int ESTADO;

        [Required(ErrorMessage = "Ingresar unidad de medida!")]
        [Range(1, 10000, ErrorMessage = "Product price must be greater than 0!")]
        [Display(Name = "UOM")]
        public int ID_UOM;

        [Required(ErrorMessage = "Ingresar referencia!")]
       
        [Display(Name = "Descripcion")]
        public string REFERENCIA;

        [Required(ErrorMessage = "Ingresar condicion de almacenamiento!")]

        [Display(Name = "Condiciones de almacenamiento ")]
        public string CONDICION_ALMACENA;

        

        [Display(Name = "FECHA_CREACION")]
        public System.DateTime FECHA_CREACION;

        [Display(Name = "ID_USUARIO")]
        public int ID_USUARIO;
    }
}