using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using slnQF.Models;
using Newtonsoft.Json;

namespace slnQF.Controllers
{
    public class CatalogoController : Controller
    {
        CatalogoModel.ModelCatalogos model = new CatalogoModel.ModelCatalogos();
        clsDB cDB = new clsDB();
        // GET: Perfiles
        public ActionResult Perfiles()
        {
            model.tituloPagina = "Perfiles";
            model.Catalogo = "Perfil";
            model.ItemsCatalogo = trae_Perfiles();
            model.VerAcciones = "0";
            model.VerPermisos = "1";
            model.ItemsAcceso = traeAccesoAsocia("0");
            model.ItemsPermisoPerfil = traePermisosPerfil("0");
            ViewBag.sel_estados = new SelectList(traeEstados(), "id", "value");

            List<CatalogoModel.MyDropList> x = new List<CatalogoModel.MyDropList>();
            x = traeSelPermisos();
            ViewBag.sel_permisos = new SelectList(x, "id", "value");
            int v_id_permiso;
            if (x.Count() > 0)
            {
                v_id_permiso = Convert.ToInt32(x[0].id);
            }
            else
            {
                v_id_permiso = 0;
            }

            ViewBag.sel_accesos = new SelectList(traeSelAcessoPermiso(v_id_permiso), "id", "value");

            return View("ViewCatalogo", model);
        }

        public ActionResult Permisos()
        {
            model.tituloPagina = "Permisos";
            model.Catalogo = "Permiso";
            model.ItemsCatalogo = trae_Permisos();
            model.ItemsAcceso = traeAccesoAsocia("0");
            model.ItemsPermisoPerfil = traePermisosPerfil("0");
            model.VerAcciones = "1";
            model.VerPermisos = "0";
            ViewBag.sel_estados = new SelectList(traeEstados(), "id", "value");
            List<CatalogoModel.MyDropList> x = new List<CatalogoModel.MyDropList>();
            x = traeSelPermisos();
            ViewBag.sel_permisos = new SelectList(x, "id", "value");
            int v_id_permiso;
            if (x.Count() > 0)
            {
                v_id_permiso = Convert.ToInt32(x[0].id);
            }
            else
            {
                v_id_permiso = 0;
            }

            ViewBag.sel_accesos = new SelectList(traeSelAcessoPermiso(v_id_permiso), "id", "value");

            return View("ViewCatalogo", model);
        }

        public ActionResult Accesos()
        {

            model.tituloPagina = "Accesos";
            model.Catalogo = "Acceso";
            model.ItemsCatalogo = trae_Accesos();
            model.ItemsAcceso = traeAccesoAsocia("0");
            model.ItemsPermisoPerfil = traePermisosPerfil("0");
            model.VerAcciones = "0";
            model.VerPermisos = "0";
            ViewBag.sel_estados = new SelectList(traeEstados(), "id", "value");
            List<CatalogoModel.MyDropList> x = new List<CatalogoModel.MyDropList>();
            x = traeSelPermisos();
            ViewBag.sel_permisos = new SelectList(x, "id", "value");
            int v_id_permiso;
            if (x.Count() > 0)
            {
                v_id_permiso = Convert.ToInt32(x[0].id);
            }
            else
            {
                v_id_permiso = 0;
            }

            ViewBag.sel_accesos = new SelectList(traeSelAcessoPermiso(v_id_permiso), "id", "value");
            return View("ViewCatalogo", model);
        }

        public ActionResult Bodegas()
        {

            model.tituloPagina = "Bodegas";
            model.Catalogo = "Bodega";
            model.ItemsCatalogo = trae_Bodega();
            model.ItemsAcceso = traeAccesoAsocia("0");
            model.ItemsPermisoPerfil = traePermisosPerfil("0");
            model.VerAcciones = "0";
            model.VerPermisos = "0";
            ViewBag.sel_estados = new SelectList(traeEstados(), "id", "value");
            List<CatalogoModel.MyDropList> x = new List<CatalogoModel.MyDropList>();
            x = traeSelPermisos();
            ViewBag.sel_permisos = new SelectList(x, "id", "value");
            int v_id_permiso;
            if (x.Count() > 0)
            {
                v_id_permiso = Convert.ToInt32(x[0].id);
            }
            else
            {
                v_id_permiso = 0;
            }

            ViewBag.sel_accesos = new SelectList(traeSelAcessoPermiso(v_id_permiso), "id", "value");
            return View("ViewCatalogo", model);
        }


        public List<CatalogoModel.clsAccesoAsocia> traeAccesoAsocia(string permiso)
        {
            List<CatalogoModel.clsAccesoAsocia> listaAccesoAsocia = new List<CatalogoModel.clsAccesoAsocia>();
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds = cDB.CONSULTA_ACCIONES_PERMISO(Convert.ToInt32(permiso));
                dt = ds.Tables[0];


                foreach (DataRow dr in dt.Rows)
                {
                    CatalogoModel.clsAccesoAsocia fila = new CatalogoModel.clsAccesoAsocia(dr["ID_ACCESO"].ToString(), dr["ACCESO_DESC"].ToString(), dr["SW_ASOCIA"].ToString());
                    listaAccesoAsocia.Add(fila);
                }
            }
            catch (Exception e)
            {
                System.Console.Write(e.Message);
            }
            return listaAccesoAsocia;
        }


        public List<CatalogoModel.clsPermisoPerfil> traePermisosPerfil(string id_perfil)
        {
            List<CatalogoModel.clsPermisoPerfil> listaAccesoAsocia = new List<CatalogoModel.clsPermisoPerfil>();
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds = cDB.CONSULTA_PERMISOS_PERFIL(Convert.ToInt32(id_perfil));
                dt = ds.Tables[0];


                foreach (DataRow dr in dt.Rows)
                {
                    CatalogoModel.clsPermisoPerfil fila = new CatalogoModel.clsPermisoPerfil(dr["ID_PM"].ToString(), dr["DESC_PM"].ToString(), dr["ID_AC"].ToString(), dr["DESC_AC"].ToString());
                    listaAccesoAsocia.Add(fila);
                }
            }
            catch (Exception e)
            {
                System.Console.Write(e.Message);
            }
            return listaAccesoAsocia;
        }


        public List<CatalogoModel.clsCatalogo> trae_Perfiles()
        {
            List<CatalogoModel.clsCatalogo> listaCatalogo = new List<CatalogoModel.clsCatalogo>();
            try
            {

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds = cDB.traeCatalogo("PERFIL", 1, 0, " ");
                dt = ds.Tables[0];


                foreach (DataRow dr in dt.Rows)
                {
                    CatalogoModel.clsCatalogo filaCatalogo = new CatalogoModel.clsCatalogo(dr["ID"].ToString(), dr["NOMBRE"].ToString(), dr["ID_ESTA"].ToString(), dr["ESTADO"].ToString(), dr["FECHA_CREA"].ToString(), "", dr["RESPONSABLE"].ToString());
                    listaCatalogo.Add(filaCatalogo);
                }


            }
            catch (Exception e)
            {
                System.Console.Write(e.Message);
            }
            return listaCatalogo;
        }

        public List<CatalogoModel.clsCatalogo> trae_Permisos()
        {
            List<CatalogoModel.clsCatalogo> listaCatalogo = new List<CatalogoModel.clsCatalogo>();
            try
            {

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds = cDB.traeCatalogo("PERMISO", 1, 0, " ");
                dt = ds.Tables[0];


                foreach (DataRow dr in dt.Rows)
                {
                    CatalogoModel.clsCatalogo filaCatalogo = new CatalogoModel.clsCatalogo(dr["ID"].ToString(), dr["NOMBRE"].ToString(), dr["ID_ESTA"].ToString(), dr["ESTADO"].ToString(), dr["FECHA_CREA"].ToString(), "", dr["RESPONSABLE"].ToString());
                    listaCatalogo.Add(filaCatalogo);
                }


            }
            catch (Exception e)
            {
                System.Console.Write(e.Message);
            }
            return listaCatalogo;
        }

        public List<CatalogoModel.clsCatalogo> trae_Accesos()
        {
            List<CatalogoModel.clsCatalogo> listaCatalogo = new List<CatalogoModel.clsCatalogo>();
            try
            {

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds = cDB.traeCatalogo("ACCESO", 1, 0, " ");
                dt = ds.Tables[0];


                foreach (DataRow dr in dt.Rows)
                {
                    CatalogoModel.clsCatalogo filaCatalogo = new CatalogoModel.clsCatalogo(dr["ID"].ToString(), dr["NOMBRE"].ToString(), dr["ID_ESTA"].ToString(), dr["ESTADO"].ToString(), dr["FECHA_CREA"].ToString(), "", dr["RESPONSABLE"].ToString());
                    listaCatalogo.Add(filaCatalogo);
                }


            }
            catch (Exception e)
            {
                System.Console.Write(e.Message);
            }
            return listaCatalogo;
        }

        public List<CatalogoModel.clsCatalogo> trae_Bodega()
        {
            List<CatalogoModel.clsCatalogo> listaCatalogo = new List<CatalogoModel.clsCatalogo>();
            try
            {

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds = cDB.traeCatalogo("BODEGA", 1, 0, " ");
                dt = ds.Tables[0];


                foreach (DataRow dr in dt.Rows)
                {
                    CatalogoModel.clsCatalogo filaCatalogo = new CatalogoModel.clsCatalogo(dr["ID"].ToString(), dr["NOMBRE"].ToString(), dr["ID_ESTA"].ToString(), dr["ESTADO"].ToString(), dr["FECHA_CREA"].ToString(), "", dr["RESPONSABLE"].ToString());
                    listaCatalogo.Add(filaCatalogo);
                }


            }
            catch (Exception e)
            {
                System.Console.Write(e.Message);
            }
            return listaCatalogo;
        }

        public List<CatalogoModel.MyDropList> traeEstados()
        {
            List<CatalogoModel.MyDropList> lista = new List<CatalogoModel.MyDropList>();
            try
            {

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds = cDB.CONSULTA_DATOS_CATALOGOS("ESTADOS", 1, 0, " ");
                dt = ds.Tables[0];


                foreach (DataRow dr in dt.Rows)
                {
                    CatalogoModel.MyDropList filaCatalogo = new CatalogoModel.MyDropList();
                    filaCatalogo.id = dr["ID"].ToString();
                    filaCatalogo.value = dr["NOMBRE"].ToString();

                    lista.Add(filaCatalogo);
                }


            }
            catch (Exception e)
            {
                System.Console.Write(e.Message);
            }
            return lista;
        }

        public List<CatalogoModel.MyDropList> traeSelPermisos()
        {
            List<CatalogoModel.MyDropList> lista = new List<CatalogoModel.MyDropList>();
            try
            {

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds = cDB.CONSULTA_DATOS_CATALOGOS("PERMISO", 1, 0, " ");
                dt = ds.Tables[0];


                foreach (DataRow dr in dt.Rows)
                {
                    CatalogoModel.MyDropList filaCatalogo = new CatalogoModel.MyDropList();
                    filaCatalogo.id = dr["ID"].ToString();
                    filaCatalogo.value = dr["NOMBRE"].ToString();

                    lista.Add(filaCatalogo);
                }


            }
            catch (Exception e)
            {
                System.Console.Write(e.Message);
            }
            return lista;
        }

        public List<CatalogoModel.MyDropList> traeSelAcessoPermiso(int idpermiso)
        {
            List<CatalogoModel.MyDropList> lista = new List<CatalogoModel.MyDropList>();
            try
            {

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds = cDB.CONSULTA_ACCESOS_PERMISO(idpermiso);
                dt = ds.Tables[0];


                foreach (DataRow dr in dt.Rows)
                {
                    CatalogoModel.MyDropList filaCatalogo = new CatalogoModel.MyDropList();
                    filaCatalogo.id = dr["ID"].ToString();
                    filaCatalogo.value = dr["NOMBRE"].ToString();

                    lista.Add(filaCatalogo);
                }


            }
            catch (Exception e)
            {
                System.Console.Write(e.Message);
            }
            return lista;
        }

        public JsonResult traeComboEstado()
        {

            return Json(traeEstados(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult grabaAccesos(string permiso, string listastr)
        {
            int contadorCambio = 0;
            int contadorNoCambio = 0;
            int contador = 0;
            string p_msg1 = "";
            string p_msg2 = "";
            int resultado = -1;
            List<CatalogoModel.clsAccesoAsocia> lista = new List<CatalogoModel.clsAccesoAsocia>();
            CatalogoModel.clsAccesoAsocia item;
            dynamic dynJson = JsonConvert.DeserializeObject(listastr);
            foreach (var item2 in dynJson)
            {
                item = new CatalogoModel.clsAccesoAsocia(item2.id.ToString(), item2.acceso.ToString(), item2.sw_asocia.ToString());
                lista.Add(item);



                resultado = cDB.MODIFICA_ACCIONES_PERMISO(0, Convert.ToInt32(permiso), Convert.ToInt32(item2.id.ToString()), Convert.ToInt32(item2.sw_asocia.ToString()), 1, out p_msg1, out p_msg2);

                if (resultado == 1)
                {
                    contadorCambio = contadorCambio + 1;
                }
                if (resultado == -1)
                {
                    contadorNoCambio = contadorNoCambio + 1;
                }
                contador = contador + 1;
            }
            contadorCambio = contadorCambio + 1 - 1;
            contador = contador + 1 - 1;
            string p_msgpop = "";
            if (contadorCambio == 0)
            {
                p_msgpop = "No se hizo ningun cambio";
            }
            else
            {
                p_msgpop = "Cambios guardados correctamente";
            }
            return Json(new { ID = resultado, MENSAJE1 = p_msg1, MENSAJE2 = p_msg2, MENSAJEPOP = p_msgpop });
        }
        public JsonResult traeAccesosPorPermiso(string id)
        {
            List<CatalogoModel.clsAccesoAsocia> tmp = new List<CatalogoModel.clsAccesoAsocia>();
            tmp = traeAccesoAsocia(id);

            return Json(tmp, JsonRequestBehavior.AllowGet);
        }

        public JsonResult traeSELAccesosPorPermiso(string id)
        {
            List<CatalogoModel.MyDropList> tmp = new List<CatalogoModel.MyDropList>();
            tmp = traeSelAcessoPermiso(Convert.ToInt32(id));

            return Json(tmp, JsonRequestBehavior.AllowGet);
        }


        public JsonResult asociaPermisoPerfil(string idperfil, string idpermiso, string idacceso)
        {
            string p_msg1;
            string p_msg2;
            int resultado = cDB.MODIFICA_PERMISOS_PERFIL(Convert.ToInt32(idpermiso), Convert.ToInt32(idacceso), Convert.ToInt32(idperfil), 1, 1, out p_msg1, out p_msg2);

            return Json(new { ID = resultado, MENSAJE1 = p_msg1, MENSAJE2 = p_msg2 });
        }


        public JsonResult desasociaPermisoPerfil(string idperfil, string idpermiso, string idacceso)
        {

            string p_msg1;
            string p_msg2;
            int resultado = cDB.MODIFICA_PERMISOS_PERFIL(Convert.ToInt32(idpermiso), Convert.ToInt32(idacceso), Convert.ToInt32(idperfil), 0, 1, out p_msg1, out p_msg2);

            return Json(new { ID = resultado, MENSAJE1 = p_msg1, MENSAJE2 = p_msg2 });
        }

        public JsonResult guardaCatalogo(string descripcion, string catalogo)
        {
            string p_msg1;
            string p_msg2;
            int resultado = cDB.INGRESA_DATOS_CATALOGOS(descripcion.ToUpper(), catalogo.ToUpper(), 1, out p_msg1, out p_msg2);

            if (catalogo == "Perfil")
            {
                model.ItemsCatalogo = trae_Perfiles();
            }
            else if (catalogo == "Permiso")
            {
                model.ItemsCatalogo = trae_Permisos();
            }
            else if (catalogo == "Acceso")
            {
                model.ItemsCatalogo = trae_Accesos();
            }
            else if (catalogo == "Bodega")
            {
                model.ItemsCatalogo = trae_Bodega();
            }
            return Json(new { ID = resultado, MENSAJE1 = p_msg1, MENSAJE2 = p_msg2 });
        }

        public JsonResult consultaPermisosPerfil(string idpermiso)
        {
            List<CatalogoModel.clsPermisoPerfil> tmp = new List<CatalogoModel.clsPermisoPerfil>();
            tmp = traePermisosPerfil(idpermiso);
            model.ItemsPermisoPerfil = tmp;
            return Json(tmp, JsonRequestBehavior.AllowGet);
        }
        public JsonResult editaCatalogo(string iddesc, string descripcion, string id_estado, string catalogo)
        {
            string p_msg1;
            string p_msg2;


            int resultado = cDB.MODIFICA_DATOS_CATALOGOS(catalogo.ToUpper(), Convert.ToInt32(iddesc), Convert.ToInt32(id_estado), descripcion.ToUpper(), 1, out p_msg1, out p_msg2);

            if (catalogo == "Perfil")
            {
                model.ItemsCatalogo = trae_Perfiles();
            }
            else if (catalogo == "Permiso")
            {
                model.ItemsCatalogo = trae_Permisos();
            }
            else if (catalogo == "Acceso")
            {
                model.ItemsCatalogo = trae_Accesos();
            }
            else if (catalogo == "Bodega")
            {
                model.ItemsCatalogo = trae_Bodega();
            }
            return Json(new { ID = resultado, MENSAJE1 = p_msg1, MENSAJE2 = p_msg2 });
        }

    }
}