using Newtonsoft.Json;
using slnQF.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace slnQF.Controllers
{

    public class UsuarioController : Controller
    {
         
        UsuarioModel.ModelUsuarios model = new UsuarioModel.ModelUsuarios();
        clsDB cDB = new clsDB();
        // GET: Usuario
        public ActionResult Usuario()
        {
 

            model.ItemsUsuarios = traeUsuarios();

            model.ItemsUsersSuper = traeUsuarioAsocia("0");

            model.ItemsUsersBodega = traeBodegaAsocia("0");

            model.ItemsPerfil = traePerfilUsuario(0);

            ViewBag.cmbPerfil = new SelectList(traePerfilUsuario(0), "id", "value");
            ViewBag.cmbTipoUser = new SelectList(traeCatalogo("TIPO_USUARIO"), "id", "value");

            ViewBag.cmbMODTipoUser = new SelectList(traeCatalogo("TIPO_USUARIO"), "id", "value");
            return View("ViewUsuario", model);
        }

        public ActionResult AtributoUsuario(string idUsuario)
        {
            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            List<TUSUARIOATRIBUTO> loteData = db.TUSUARIOATRIBUTOs.Where(x => x.ID_USUARIO == Convert.ToInt32(idUsuario)).ToList<TUSUARIOATRIBUTO>();

            SP_CONSULTA_USUARIOResult vUSER = db.SP_CONSULTA_USUARIO().Where(X => X.ID_USUARIO == Convert.ToInt32(idUsuario)).First();

            ViewBag.nombreUsuario = vUSER.NOMBRE;
            ViewBag.idUsuario = vUSER.ID_USUARIO;
            return View(loteData);
        }

        public PartialViewResult CreateAtributo(string idUsuario)
        {
            TUSUARIOATRIBUTO tUs = new TUSUARIOATRIBUTO();
            tUs.ID_USUARIO = Convert.ToInt32(idUsuario);
            return PartialView(tUs);
        }

        [HttpPost]
        public JsonResult createAtr(slnQF.TUSUARIOATRIBUTO product)
        {
            string error = "";
            string errorStr = "";
            string idUsuarioLog = "-1";
            try
            {
                idUsuarioLog = Session["LogedUserID"].ToString();

            }
            catch (Exception Ex)
            {
                error = "1";
                errorStr = "Su sesion ha caducado, ingrese nuevamente";
            }
            if (Convert.ToInt32(idUsuarioLog) > 0)
            {
                DC_CALIDADDataContext db = new DC_CALIDADDataContext();
                try
                {
                    db.Connection.Open();
                    db.Transaction = db.Connection.BeginTransaction();
                    db.TUSUARIOATRIBUTOs.InsertOnSubmit(product);
                    db.SubmitChanges();
                    db.Transaction.Commit();
                    error = "0";
                    errorStr = "Informacion guardada con exito";
                    db.Connection.Close();
                    db.Dispose();
                }
                catch (Exception e)
                {
                    db.Transaction.Rollback();
                    error = "1";
                    errorStr = "Error en el guardado de informacion, " + e.Message;
                    db.Connection.Close();
                    db.Dispose();
                }
            }
            else
            {
                error = "1";
                errorStr = "Su sesion ha caducado, ingrese nuevamente";
            }
            return Json(new { ID = error, MENSAJE = errorStr });
        }

        
        [HttpPost]
        public JsonResult EditAtrib(slnQF.TUSUARIOATRIBUTO product)
        {
            string error = "";
            string idUsuarioLog = "-1";
            string errorStr = "";
            try
            {
                idUsuarioLog = Session["LogedUserID"].ToString();

            }
            catch (Exception Ex)
            {
                error = "1";
                errorStr = "Su sesion ha caducado, ingrese nuevamente";
            }
            if (Convert.ToInt32(idUsuarioLog) > 0)
            {
                DC_CALIDADDataContext db = new DC_CALIDADDataContext();
                try
                {
                    db.Connection.Open();
                    db.Transaction = db.Connection.BeginTransaction();
                    TUSUARIOATRIBUTO tblProd = db.TUSUARIOATRIBUTOs.Where(x => x.ID_USUARIOATRIBUTO == product.ID_USUARIOATRIBUTO)
                                           .FirstOrDefault();

                    tblProd.ID_USUARIO = product.ID_USUARIO;
                    tblProd.NOMBRE = product.NOMBRE;
                    tblProd.VALOR = product.VALOR;

                    db.SubmitChanges();
                    db.Transaction.Commit();
                    error = "0";
                    errorStr = "Informacion guardada con exito";
                    db.Connection.Close();
                    db.Dispose();
                }

                catch (Exception e)
                {
                    db.Transaction.Rollback();
                    error = "1";
                    errorStr = "Error en el guardado de informacion, " + e.Message;
                    db.Connection.Close();
                    db.Dispose();
                }
            }
            else
            {
                error = "1";
                errorStr = "Su sesion ha caducado, ingrese nuevamente";
            }
            return Json(new { ID = error, MENSAJE = errorStr });

        }

        public ActionResult ListaAtributoUsuario(string idAtributo)
        {

            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            TUSUARIOATRIBUTO loteData = db.TUSUARIOATRIBUTOs.Where(x => x.ID_USUARIOATRIBUTO == Convert.ToInt32(idAtributo)).First();
               

            return PartialView(loteData);
        }

        public ActionResult Login()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(ModelUser u)
        {
            if (ModelState.IsValid)
            {

                string p_msg1 = "";
                string p_msg2 = "";
                int resultado = cDB.VALIDA_USUARIO(u.UserName, u.Password, out p_msg1, out p_msg2);
                if (resultado >= 0)
                {
                    try
                    {
                        DataSet ds = new DataSet();
                        DataTable dt = new DataTable();
                        ds = cDB.CONSULTA_USUARIO(resultado);
                        dt = ds.Tables[0];
                        foreach (DataRow dr in dt.Rows)
                        {

                            Session["LogedUserID"] = resultado.ToString();
                            Session["LogedUser"] = dr["USUARIO"].ToString();
                            Session["OrdenProduccion"] = null;
                            Session["LogedUserPerfil"] = dr["ID_PERFIL"].ToString();
                            Session["LogedMessage"] = null;
                        }

                    }
                    catch (Exception e)
                    {
                        Session["LogedMessage"] = e.Message;
                    }

                    return RedirectToAction("AfterLogin");
                }
                else
                {

                    Session["LogedMessage"] = "Intente de Nuevo, " + p_msg1 + " " + p_msg2;
                }
            }

            return View(u);
        }

        public ActionResult Login2(ModelUser u)
        {
            if (ModelState.IsValid)
            {

                string p_msg1 = "";
                string p_msg2 = "";
                int resultado = cDB.VALIDA_USUARIO(u.UserName, u.Password, out p_msg1, out p_msg2);
                if (resultado >= 0)
                {
                    try
                    {
                        DataSet ds = new DataSet();
                        DataTable dt = new DataTable();
                        ds = cDB.CONSULTA_USUARIO(resultado);
                        dt = ds.Tables[0];
                        foreach (DataRow dr in dt.Rows)
                        {

                            Session["LogedUserID"] = resultado.ToString();
                            Session["LogedUser"] = dr["USUARIO"].ToString();
                            Session["OrdenProduccion"] = null;
                            Session["LogedUserPerfil"] = dr["ID_PERFIL"].ToString();
                            Session["LogedMessage"] = null;
                        }

                    }
                    catch (Exception e)
                    {
                        Session["LogedMessage"] = e.Message;
                    }

                    return RedirectToAction("AfterLogin2");
                }
                else
                {

                    Session["LogedMessage"] = "Intente de Nuevo, " + p_msg1 + " " + p_msg2;
                }
            }

            return View(u);
        }
        public ActionResult LogOff()
        {
            Session["LogedUserID"] = null;
            Session["LogedUser"] = null;
            Session["LogedUserCorreo"] = null;
            Session["LogedMessage"] = null;
            Session["OrdenProduccion"] = null;


            Session.Abandon();
            Session.Clear();
            return RedirectToAction("AfterLogin2");
        }
        public ActionResult LogOff2()
        {
            Session["LogedUserID"] = null;
            Session["LogedUser"] = null;
            Session["LogedUserCorreo"] = null;
            Session["LogedMessage"] = null;
            Session["OrdenProduccion"] = null;

            Session.Abandon();
            Session.Clear();
            return RedirectToAction("AfterLogin2");
        }
        public ActionResult AfterLogin()
        {
            if (Session["LogedUserID"] != null)
            {
                return RedirectToAction("Index2", "App");
            }
            else
            {

                return RedirectToAction("Login");
            }
        }

        public ActionResult AfterLogin2()
        {
            if (Session["LogedUserID"] != null)
            {
                return RedirectToAction("Index2", "App");
            }
            else
            {

                return RedirectToAction("Login2");
            }
        }
        public JsonResult grabaUsuario(string nombre, string apellido, string correo, string usuario, string tipoUser, string SerieRecibo, string SerieEmision, string SerieTranStock, string Bodega, string UserSap)
        {

            string p_msg1;
            string p_msg2;
            int boo_supervisor = -1;


            int resultado = cDB.INGRESA_USUARIO(0, usuario, nombre, apellido, correo, boo_supervisor, 1, 1, Convert.ToInt32(tipoUser), SerieRecibo, SerieEmision, SerieTranStock, Convert.ToInt32(Bodega), UserSap, out p_msg1, out p_msg2);

            return Json(new { ID = resultado, MENSAJE1 = p_msg1, MENSAJE2 = p_msg2 });

        }

        public JsonResult modificaUsuario(string nombre, string apellido, string correo, string usuario, string tipoUser, string SerieRecibo, string SerieEmision,string SerieTranStock, string Bodega, string UserSap, string user)
        {

            string p_msg1;
            string p_msg2;
            int boo_supervisor = -1;


            int resultado = cDB.MODIFICA_USUARIO(Convert.ToInt32(usuario), user, nombre, apellido, correo, boo_supervisor, 1, 1, Convert.ToInt32(tipoUser), SerieRecibo, SerieEmision, SerieTranStock, Convert.ToInt32(Bodega), UserSap, out p_msg1, out p_msg2);

            return Json(new { ID = resultado, MENSAJE1 = p_msg1, MENSAJE2 = p_msg2 });

        }
        public JsonResult grabaSupervUser(string idSuperv, string listastr)
        {
            int contadorCambio = 0;
            int contadorNoCambio = 0;
            int contador = 0;
            string p_msg1 = "";
            string p_msg2 = "";
            int resultado = -1;
            List<UsuarioModel.cls_supervisa_asocia> lista = new List<UsuarioModel.cls_supervisa_asocia>();
            UsuarioModel.cls_supervisa_asocia item;
            dynamic dynJson = JsonConvert.DeserializeObject(listastr);
            foreach (var item2 in dynJson)
            {
                item = new UsuarioModel.cls_supervisa_asocia(item2.id_usuario.Value, item2.nombre_usuario.Value, item2.sw_asocia.Value);
                lista.Add(item);


                resultado = cDB.MODIFICA_SUPERV_USER(Convert.ToInt32(idSuperv), Convert.ToInt32(item2.id_usuario), Convert.ToInt32(item2.sw_asocia), 1, out p_msg1, out p_msg2);
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
        public JsonResult grabaBodegaUser(string id_usuario, string listastr)
        {
            int contadorCambio = 0;
            int contadorNoCambio = 0;
            int contador = 0;
            string p_msg1 = "";
            string p_msg2 = "";
            int resultado = 0;
            List<UsuarioModel.cls_bodega_asocia> lista = new List<UsuarioModel.cls_bodega_asocia>();
            UsuarioModel.cls_bodega_asocia item;
            dynamic dynJson = JsonConvert.DeserializeObject(listastr);
            foreach (var item2 in dynJson)
            {
                item = new UsuarioModel.cls_bodega_asocia(item2.id_bodega.Value, item2.nombre_bodega.Value, item2.sw_asocia.Value);
                lista.Add(item);
            }

            foreach (UsuarioModel.cls_bodega_asocia item3 in lista) {

                resultado = cDB.MODIFICA_BODEGA_USER(Convert.ToInt32(item3.id_bodega), Convert.ToInt32(id_usuario), Convert.ToInt32(item3.sw_asocia), 1, out p_msg1, out p_msg2);
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

        public JsonResult grabaContrasena(string id_user, string contrasena)
        {

            string p_msg1 = "";
            string p_msg2 = "";
            int resultado = 0;
            if (contrasena.Length > 0)
            {
                resultado = cDB.CAMBIA_PASS_USUARIO(Convert.ToInt32(id_user), contrasena, out p_msg1, out p_msg2);
            }
            else
            {
                p_msg1 = "Ingrese la contraseña";
            }
            return Json(new { ID = resultado, MENSAJE1 = p_msg1, MENSAJE2 = p_msg2 });

        }

        public JsonResult jsTraeAccesosUser()
        {
            List<accesos_user> tmp = new List<accesos_user>();
            tmp = traeAccesosUser();

            return Json(tmp, JsonRequestBehavior.AllowGet);
        }

        public JsonResult jsTraeUserSession()
        {
            int idUsuarioLog = 0;
            string errorStr;
            try
            {
               idUsuarioLog = Convert.ToInt32(Session["LogedUserID"].ToString());
                errorStr = "";
            }
            catch {
                idUsuarioLog = 0;
                errorStr= "/Usuario/Login2";
            }

            return Json(new { ID = idUsuarioLog, MENSAJE = errorStr }, JsonRequestBehavior.AllowGet);
        }

        public List<accesos_user> traeAccesosUser()
        {
            List<accesos_user> listaAccesos = new List<accesos_user>();

            try
            {
                int idUsuarioLog = Convert.ToInt32(Session["LogedUserID"].ToString());
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds = cDB.CONSULTA_ACCESO_X_USUARIOS(Convert.ToInt32(idUsuarioLog));
                dt = ds.Tables[0];


                foreach (DataRow dr in dt.Rows)
                {
                    accesos_user fila = new accesos_user();
                    fila.IDPERMISO = dr["ID_PERM"].ToString();
                    fila.IDACCESO = dr["ID_ACC"].ToString();

                    listaAccesos.Add(fila);
                }

            }
            catch (Exception e)
            {
                accesos_user fila = new accesos_user();
                fila.IDPERMISO = "-1";
                fila.IDACCESO = "-1";
                listaAccesos.Add(fila);
            }
            return listaAccesos;
        }

        public List<UsuarioModel.cls_bodega_asocia> traeBodegaAsocia(string p_bodega)
        {
            List<UsuarioModel.cls_bodega_asocia> listaUsers = new List<UsuarioModel.cls_bodega_asocia>();

            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds = cDB.CONSULTA_BODEGA_USUARIOS(Convert.ToInt32(p_bodega));
                dt = ds.Tables[0];


                foreach (DataRow dr in dt.Rows)
                {
                    UsuarioModel.cls_bodega_asocia fila = new UsuarioModel.cls_bodega_asocia(dr["ID_BODEGA"].ToString(), dr["NOMBRE"].ToString(), dr["SW_ASOCIA"].ToString());


                    listaUsers.Add(fila);
                }

            }
            catch (Exception e)
            {
                System.Console.Write(e.Message);
            }
            return listaUsers;
        }
        public List<UsuarioModel.cls_supervisa_asocia> traeUsuarioAsocia(string p_supervisa)
        {
            List<UsuarioModel.cls_supervisa_asocia> listaUsers = new List<UsuarioModel.cls_supervisa_asocia>();

            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds = cDB.CONSULTA_SUPERVISA_USUARIOS(Convert.ToInt32(p_supervisa));
                dt = ds.Tables[0];


                foreach (DataRow dr in dt.Rows)
                {
                    UsuarioModel.cls_supervisa_asocia fila = new UsuarioModel.cls_supervisa_asocia(dr["ID_USUARIO"].ToString(), dr["NOMBRE"].ToString(), dr["SW_ASOCIA"].ToString());


                    listaUsers.Add(fila);
                }

            }
            catch (Exception e)
            {
                System.Console.Write(e.Message);
            }
            return listaUsers;
        }
        public List<UsuarioModel.clsUsuario> traeUsuarios()
        {
            List<UsuarioModel.clsUsuario> listaUsuarios = new List<UsuarioModel.clsUsuario>();
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds = cDB.CONSULTA_USERS();
                dt = ds.Tables[0];


                foreach (DataRow dr in dt.Rows)
                {
                    UsuarioModel.clsUsuario fila = new UsuarioModel.clsUsuario(dr["USUARIOID"].ToString(), dr["USUARIO"].ToString(), dr["PASSWORD"].ToString(), dr["NOMBRES"].ToString(), dr["APELLIDOS"].ToString(), dr["CORREO"].ToString(), dr["BOOL_SUPERVISOR"].ToString(), dr["ESTADO"].ToString(), dr["FECHA_CREA"].ToString(), dr["ID_RESPONSABLE"].ToString(), dr["RESPONSABLE"].ToString(), dr["NOMBRE_COM"].ToString(), dr["IDPERFIL"].ToString(), dr["DESC_PERFIL"].ToString(), dr["ID_TIPOUS"].ToString(), dr["DESC_TIPOUSER"].ToString(), dr["SERIEEMISION"].ToString(), dr["SERIERECIBO"].ToString(), dr["ID_BODEGA"].ToString(), dr["BODEGA"].ToString(), dr["SAP_USER"].ToString(), dr["SERIE_TRANSTOCK"].ToString());


                    listaUsuarios.Add(fila);
                }
            }
            catch (Exception e)
            {
                System.Console.Write(e.Message);
            }
            return listaUsuarios;
        }
        public List<UsuarioModel.clsUsuarioPerfil> traePerfilUsuario(int p_usuario)
        {
            List<UsuarioModel.clsUsuarioPerfil> LISTA = new List<UsuarioModel.clsUsuarioPerfil>();
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds = cDB.CONSULTA_PERFIL_USERS(p_usuario);
                dt = ds.Tables[0];


                foreach (DataRow dr in dt.Rows)
                {
                    UsuarioModel.clsUsuarioPerfil fila = new UsuarioModel.clsUsuarioPerfil(dr["ID"].ToString(), dr["NOMBRE"].ToString(), dr["BAND"].ToString());

                    LISTA.Add(fila);
                }
            }
            catch (Exception e)
            {
                System.Console.Write(e.Message);
            }
            return LISTA;
        }
        public List<CatalogoModel.MyDropList> traeCatalogo(string p_catalogo)
        {
            List<CatalogoModel.MyDropList> LISTA = new List<CatalogoModel.MyDropList>();
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds = cDB.traeCatalogo(p_catalogo, 1, 1, "");
                dt = ds.Tables[0];


                foreach (DataRow dr in dt.Rows)
                {
                    CatalogoModel.MyDropList fila = new CatalogoModel.MyDropList();
                    fila.id = dr["ID"].ToString();
                    fila.value = dr["NOMBRE"].ToString();

                    LISTA.Add(fila);
                }
            }
            catch (Exception e)
            {
                System.Console.Write(e.Message);
            }
            return LISTA;
        }
        public JsonResult traeSupervUser(string id)
        {
            List<UsuarioModel.cls_supervisa_asocia> tmp = new List<UsuarioModel.cls_supervisa_asocia>();
            tmp = traeUsuarioAsocia(id);

            return Json(tmp, JsonRequestBehavior.AllowGet);
        }

        public JsonResult traeBodegaUser(string id)
        {
            List<UsuarioModel.cls_bodega_asocia> tmp = new List<UsuarioModel.cls_bodega_asocia>();
            tmp = traeBodegaAsocia(id);

            return Json(tmp, JsonRequestBehavior.AllowGet);
        }

        public JsonResult grabaPerfilesUser(string idUser, string idPerfil)
        {
            string p_msgpop = "";
            string p_msg1 = "";
            string p_msg2 = "";
            int resultado = -1;



            resultado = cDB.MODIFICA_PERFIL_USUARIO(Convert.ToInt32(idUser), Convert.ToInt32(idPerfil), 1, out p_msg1, out p_msg2);
            if (resultado == 1)
            {
                p_msgpop = "Cambios guardados correctamente";
            }
            else
            {
                p_msgpop = "Problemas con los cambios " + p_msg1 + " " + p_msg2;
            }



            return Json(new { ID = resultado, MENSAJE1 = p_msg1, MENSAJE2 = p_msg2, MENSAJEPOP = p_msgpop });
        }

        public JsonResult traePerfilesUser(string id)
        {
            List<UsuarioModel.clsUsuarioPerfil> tmp = new List<UsuarioModel.clsUsuarioPerfil>();
            tmp = traePerfilUsuario(Convert.ToInt32(id));

            return Json(tmp, JsonRequestBehavior.AllowGet);
        }
    }
}