using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace slnQF.Models
{
    public class CatalogoModel
    {
        public class ModelCatalogos
        {

            public int TotalCount { get; set; }
            public int PageSize { get; set; }
            public int PageNumber { get; set; }
            public int PagerCount { get; set; }

            public string tituloPagina { get; set; }

            public string Catalogo { get; set; }

            public string VerAcciones { get; set; }
            public string VerPermisos { get; set; }
            public List<clsCatalogo> ItemsCatalogo { get; set; }

            public List<clsAccesoAsocia> ItemsAcceso { get; set; }

            public List<CatalogoModel.clsPermisoPerfil> ItemsPermisoPerfil { get; set; }
        }

        public class clsAccesoAsocia
        {
            public clsAccesoAsocia(string id, string acceso, string sw_asocia)
            {
                this.id = id;
                this.acceso = acceso;
                this.sw_asocia = sw_asocia;
            }

            public string id { get; set; }
            public string acceso { get; set; }

            public string sw_asocia { get; set; }
        }

        public class clsPermisoPerfil
        {
            public clsPermisoPerfil(string id_pm, string desc_pm, string id_ac, string desc_ac)
            {
                this.id_pm = id_pm;
                this.desc_pm = desc_pm;
                this.id_ac = id_ac;
                this.desc_ac = desc_ac;
            }

            public string id_pm { get; set; }
            public string desc_pm { get; set; }
            public string id_ac { get; set; }
            public string desc_ac { get; set; }
        }
        public class MyDropList
        {
            public string id { get; set; }
            public string value { get; set; }
        }
        public class clsCatalogo
        {
            public clsCatalogo(string id, string nombre, string id_estado, string estado, string fecha, string id_responsable, string responsable)
            {
                this.id = id;
                this.nombre = nombre;
                this.id_estado = id_estado;
                this.estado = estado;
                this.fecha = fecha;
                this.id_responsable = id_responsable;
                this.responsable = responsable;
            }

            public string id { get; set; }
            public string nombre { get; set; }

            public string id_estado { get; set; }

            public string estado { get; set; }

            public string fecha { get; set; }

            public string id_responsable { get; set; }

            public string responsable { get; set; }


        }
    }
}