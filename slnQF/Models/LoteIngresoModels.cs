using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace slnQF.Models
{
    public class LoteIngresoModels
    {
        [Display(Name = "ID")]
        public int ID_LOTE_INGRESO;

        [Display(Name = "NO INGRESO")]
        public string NO_INGRESO;

        [Display(Name = "CERTIFICADO")]
        public int ID_CERTIFICADO;

        [Display(Name = "PRODUCTO")]
        public int ID_PRODUCTO;

        [Display(Name = "FECHA ANALISIS")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime FECHA_ANALISIS;

        [Display(Name = "FECHA FABRICACION")]
        [DataType(DataType.DateTime)]
        public DateTime FECHA_FABRICACION;

        [Display(Name = "FECHA VENCIMIENTO")]
        [DataType(DataType.DateTime)]
         public DateTime FECHA_VENCIMIENTO;

        [Display(Name = "FECHA INICIO ANALISIS")]
        [DataType(DataType.DateTime)]
          public DateTime FECHA_INI_ANALISIS;

        [Display(Name = "FECHA FIN ANALISIS")]
        [DataType(DataType.DateTime)]
        public DateTime FECHA_FIN_ANALISIS;

        [Display(Name = "FECHA INGRESO")]
        [DataType(DataType.DateTime)]
        public DateTime FECHA_INGRESO;

        [Display(Name = "FECHA RE ANALISIS")]
        [DataType(DataType.DateTime)]
        public DateTime FECHA_REANALISIS;

        [Display(Name = "TOMO")]
        public string TOMO;

        [Display(Name = "PAGINA")]
        public string PAGINA;

        [Display(Name = "NO LOTE")]
        public string NO_LOTE;

        [Display(Name = "FABRICANTE")]
        public int ID_FABRICANTE;

        [Display(Name = "PAIS FABRICANTE")]
        public int ID_PAIS_FABRICANTE;

        [Display(Name = "OBSERVACIONES")]
        public string OBSERVACIONES;

        [Display(Name = "TECNICO ANALISTA")]
        public int ID_TECNICO_ANALISTA;

        [Display(Name = "REVISADO POR")]
        public int ID_REVISADO_POR;

        [Display(Name = "STATUS")]
        public int ID_STATUS;

        [Display(Name = "ASEGURAMIENTO")]
        public string ASEGURAMIENTO;


        [Display(Name = "CANTIDAD ENVASES")]
        public int CANTIDAD_ENVASES;

        [Display(Name = "POTENCIA PROVEEDOR")]
        public string POTENCIA_PROVEEDOR;

        [Display(Name = "ASEGURAMIENTO POR")]
        public int ID_ASEGURAMIENTO;

        [Display(Name = "FECHA CREACION")]
        public System.DateTime FECHA_CREACION;

        [Display(Name = "USUARIO")]
        public int ID_USUARIO;


        public string FECHA_ANALISIS_STR
        {
            get
            {
                return FECHA_ANALISIS.ToString("dd/MM/yyyy");
            }
        }

        public string FECHA_FABRICACION_STR
        {
            get
            {
                return FECHA_FABRICACION.ToString("dd/MM/yyyy");
            }
        }

        public string FECHA_VENCIMIENTO_STR
        {
            get
            {
                return FECHA_VENCIMIENTO.ToString("dd/MM/yyyy");
            }
        }
        public string FECHA_INI_ANALISIS_STR
        {
            get
            {
                return FECHA_INI_ANALISIS.ToString("dd/MM/yyyy");
            }
        }

        public string FECHA_FIN_ANALISIS_STR
        {
            get
            {
                return FECHA_FIN_ANALISIS.ToString("dd/MM/yyyy");
            }
        }

        public string FECHA_INGRESO_STR
        {
            get
            {
                return FECHA_INGRESO.ToString("dd/MM/yyyy");
            }
        }

        public string FECHA_REANALISIS_STR
        {
            get
            {
                return FECHA_REANALISIS.ToString("dd/MM/yyyy");
            }
        }


    }
}