﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace slnQF.Models
{
    public class PruebaModels
    {
        public int ID_PRUEBA;

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