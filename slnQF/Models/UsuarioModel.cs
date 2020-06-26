using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace slnQF.Models
{
    public class UsuarioModel
    {

        public class ModelUsuarios
        {

            public int TotalCount { get; set; }
            public int PageSize { get; set; }
            public int PageNumber { get; set; }
            public int PagerCount { get; set; }

            public List<clsUsuario> ItemsUsuarios { get; set; }

            public List<cls_supervisa_asocia> ItemsUsersSuper { get; set; }

            public List<cls_bodega_asocia> ItemsUsersBodega { get; set; }

            public List<clsUsuarioPerfil> ItemsPerfil { get; set; }

        }



        public class clsUsuario
        {
            public clsUsuario(string id_usuario, string usuario, string password, string nombres, string apellidos, string correo, string bool_supervisor, string estado, string fecha_crea, string id_responsable, string desc_responsable, string nombre_completo, string id_perfil, string desc_perfil, string id_tipouser, string desc_tipouser, string serieemision, string serierecibo, string id_Bodega, string desc_Bodega, string user_sap, string serie_transferStock)
            {
                this.id_usuario = id_usuario;
                this.usuario = usuario;
                this.password = password;
                this.nombres = nombres;
                this.apellidos = apellidos;
                this.correo = correo;
                this.bool_supervisor = bool_supervisor;
                this.estado = estado;
                this.fecha_crea = fecha_crea;
                this.id_responsable = id_responsable;
                this.desc_responsable = desc_responsable;
                this.nombre_completo = nombre_completo;
                this.id_perfil = id_perfil;
                this.desc_perfil = desc_perfil;

                this.id_tipouser = id_tipouser;

                this.desc_tipouser = desc_tipouser;

                this.serieemision = serieemision;

                this.serierecibo = serierecibo;

                this.id_bodega = id_Bodega;

                this.desc_bodega = desc_Bodega;

                this.user_sap = user_sap;

                this.serietransferStock = serie_transferStock;
            }

            public string id_usuario { set; get; }
            public string usuario { set; get; }

            public string password { set; get; }
            public string nombres { set; get; }
            public string apellidos { set; get; }
            public string correo { set; get; }
            public string bool_supervisor { set; get; }
            public string estado { set; get; }
            public string fecha_crea { set; get; }
            public string id_responsable { set; get; }
            public string desc_responsable { set; get; }
            public string nombre_completo { set; get; }
            public string id_perfil { set; get; }
            public string desc_perfil { set; get; }

            public string id_tipouser { set; get; }

            public string desc_tipouser { set; get; }

            public string serieemision { set; get; }

            public string serierecibo { set; get; }

            public string serietransferStock { set; get; }


            public string id_bodega { set; get; }

            public string desc_bodega { set; get; }

            public string user_sap { set; get; }

        }
        public class clsUsuarioSerie
        {


            public string serieEmision { set; get; }

            public string serieRecibo { set; get; }

            public string serietransferStock { set; get; }

            



        }
        public class clsUsuarioPerfil
        {
            public clsUsuarioPerfil(string id, string value, string band)
            {
                this.id = id;
                this.value = value;
                this.band = band;
            }

            public string id { set; get; }

            public string value { set; get; }

            public string band { set; get; }

        }
        public class cls_supervisa_asocia
        {
            public cls_supervisa_asocia(string id_usuario, string nombre_usuario, string sw_asocia)
            {
                this.id_usuario = id_usuario;
                this.nombre_usuario = nombre_usuario;
                this.sw_asocia = sw_asocia;
            }

            public string id_usuario { set; get; }

            public string nombre_usuario { set; get; }

            public string sw_asocia { set; get; }

        }

        public class cls_bodega_asocia
        {
            public cls_bodega_asocia(string id_bodega, string nombre_bodega, string sw_asocia)
            {
                this.id_bodega = id_bodega;
                this.nombre_bodega = nombre_bodega;
                this.sw_asocia = sw_asocia;
            }

            public string id_bodega { set; get; }

            public string nombre_bodega { set; get; }

            public string sw_asocia { set; get; }

        }
        public class cls_supervisa_user
        {
            public string id_supervisa { set; get; }

            public string nombre_supervisa { set; get; }
            public string id_usuario { set; get; }

            public string nombre_usuario { set; get; }

            public string sw_asocia { set; get; }
        }
    }




}