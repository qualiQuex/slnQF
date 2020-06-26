using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace slnQF.Controllers
{
    public class clsDB
    {
        SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings.Get("QUALISAP"));

        //PERFILES, PERMISOS Y ACCESOS, [CATALOGOS]

        #region INGRESO_DATOS

        public int INGRESA_DATOS_CATALOGOS(string p_descripcion, string p_catalogo, int p_user_resp, out string p_msg1, out string p_msg2)
        {
            
            //P_OPC 0 - TODO 1 -ACTIVO
            string strQ = "SP_NUEVO_REG_CATALOGO";
            SqlCommand cmd = new SqlCommand(strQ, cn);
            DataSet ds = new DataSet();
            try
            {
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = strQ;
                cmd.Parameters.Clear();

                SqlParameter p_descripcion_P = new SqlParameter("@P_DESCRIPCION", SqlDbType.VarChar);
                p_descripcion_P.Value = p_descripcion;
                p_descripcion_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(p_descripcion_P);

                SqlParameter p_catalogo_P = new SqlParameter("@P_CATALOGO", SqlDbType.VarChar);
                p_catalogo_P.Value = p_catalogo;
                p_catalogo_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(p_catalogo_P);


                SqlParameter p_user_p = new SqlParameter("@P_USUARIO_RESP", SqlDbType.Int);
                p_user_p.Value = p_user_resp;
                p_user_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(p_user_p);


                SqlParameter CodRes_P = new SqlParameter("@P_RESPUESTA", SqlDbType.Int);
                CodRes_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(CodRes_P);

                SqlParameter msgErr1_P = new SqlParameter("@P_MSG1", SqlDbType.NVarChar);
                msgErr1_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgErr1_P);

                SqlParameter msgErr2_P = new SqlParameter("@P_MSG2", SqlDbType.NVarChar);
                msgErr2_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgErr2_P);

                cmd.Parameters["@P_MSG1"].Size = 100;
                cmd.Parameters["@P_MSG2"].Size = 100;

                cmd.ExecuteNonQuery();
                p_msg1 = msgErr1_P.Value.ToString();
                p_msg2 = msgErr2_P.Value.ToString();
                return Convert.ToInt32(CodRes_P.Value);
            }
            catch (Exception e)
            {
                String s = e.Message.ToString();
                p_msg1 = e.Message.ToString();
                p_msg2 = "-";
                return -1;
            }
            finally
            {
                cn.Close();
            }
        }

        public DataSet traeCatalogo(string p_catalogo, int p_opcion, int p_estado, string p_descripcion)
        {
            //P_OPC 0 - TODO 1 -ACTIVO
            string strQ = "SP_CONSULTA_CATALOGO";
            SqlCommand cmd = new SqlCommand(strQ, cn);
            DataSet ds = new DataSet();
            try
            {
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = strQ;
                cmd.Parameters.Clear();
                SqlParameter p_catalogo_P = new SqlParameter("@P_CATALOGO", SqlDbType.VarChar);
                p_catalogo_P.Value = p_catalogo;
                p_catalogo_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(p_catalogo_P);

                SqlParameter p_opcion_p = new SqlParameter("@P_OPCION", SqlDbType.Int);
                p_opcion_p.Value = p_opcion;
                p_opcion_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(p_opcion_p);

                SqlParameter p_estado_p = new SqlParameter("@P_ESTADO", SqlDbType.Int);
                p_estado_p.Value = p_estado;
                p_estado_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(p_estado_p);

                SqlParameter p_descripcion_P = new SqlParameter("@P_DESCRIPCION", SqlDbType.VarChar);
                p_descripcion_P.Value = p_descripcion;
                p_descripcion_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(p_descripcion_P);

                SqlParameter CodRes_P = new SqlParameter("P_RESPUESTA", SqlDbType.Int);
                CodRes_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(CodRes_P);

                SqlParameter msgErr1_P = new SqlParameter("P_MSG1", SqlDbType.NVarChar);
                msgErr1_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgErr1_P);

                SqlParameter msgErr2_P = new SqlParameter("P_MSG2", SqlDbType.NVarChar);
                msgErr2_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgErr2_P);

                cmd.Parameters["P_MSG1"].Size = 100;
                cmd.Parameters["P_MSG2"].Size = 100;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception e)
            {
                String s = e.Message.ToString();
                return ds;
            }
            finally
            {
                cn.Close();
            }
            return ds;
        }

        public DataSet DATOS_GENERALES_USUARIO(string P_USER)
        {
            string strQ = "[SP_CONSULTA_DATOS_G_USUARIO]";
            SqlCommand cmd = new SqlCommand(strQ, cn);
            DataSet ds = new DataSet();
            try
            {
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = strQ;
                cmd.Parameters.Clear();
                SqlParameter user_P = new SqlParameter("P_USER", SqlDbType.VarChar);
                user_P.Value = P_USER;
                user_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(user_P);

                SqlParameter CodRes_P = new SqlParameter("P_RESPUESTA", SqlDbType.Int);
                CodRes_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(CodRes_P);

                SqlParameter msgErr1_P = new SqlParameter("P_MSG1", SqlDbType.NVarChar);
                msgErr1_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgErr1_P);

                SqlParameter msgErr2_P = new SqlParameter("P_MSG2", SqlDbType.NVarChar);
                msgErr2_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgErr2_P);

                cmd.Parameters["P_MSG1"].Size = 100;
                cmd.Parameters["P_MSG2"].Size = 100;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception e)
            {
                String s = e.Message.ToString();
                return ds;
            }
            finally
            {
                cn.Close();
            }
            return ds;
        }


        #endregion

        #region CONSULTA_DATPS
        public DataSet CONSULTA_DATOS_CATALOGOS(string p_catalogo, int p_opcion, int p_estado, string p_descripcion)
        {
            //P_OPC 0 - TODO 1 -ACTIVO
            string strQ = "SP_CONSULTA_CATALOGO";
            SqlCommand cmd = new SqlCommand(strQ, cn);
            DataSet ds = new DataSet();
            try
            {
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = strQ;
                cmd.Parameters.Clear();
                SqlParameter p_catalogo_P = new SqlParameter("@P_CATALOGO", SqlDbType.VarChar);
                p_catalogo_P.Value = p_catalogo;
                p_catalogo_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(p_catalogo_P);

                SqlParameter p_opcion_p = new SqlParameter("@P_OPCION", SqlDbType.Int);
                p_opcion_p.Value = p_opcion;
                p_opcion_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(p_opcion_p);

                SqlParameter p_estado_p = new SqlParameter("@P_ESTADO", SqlDbType.Int);
                p_estado_p.Value = p_estado;
                p_estado_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(p_estado_p);

                SqlParameter p_descripcion_P = new SqlParameter("@P_DESCRIPCION", SqlDbType.VarChar);
                p_descripcion_P.Value = p_descripcion;
                p_descripcion_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(p_descripcion_P);

                SqlParameter CodRes_P = new SqlParameter("P_RESPUESTA", SqlDbType.Int);
                CodRes_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(CodRes_P);

                SqlParameter msgErr1_P = new SqlParameter("P_MSG1", SqlDbType.NVarChar);
                msgErr1_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgErr1_P);

                SqlParameter msgErr2_P = new SqlParameter("P_MSG2", SqlDbType.NVarChar);
                msgErr2_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgErr2_P);

                cmd.Parameters["P_MSG1"].Size = 100;
                cmd.Parameters["P_MSG2"].Size = 100;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception e)
            {
                String s = e.Message.ToString();
                return ds;
            }
            finally
            {
                cn.Close();
            }
            return ds;
        }
        public DataSet CONSULTA_ACCESOS_PERMISO(int P_PERMISO)
        {
            //P_OPC 0 - TODO 1 -ACTIVO
            string strQ = "SP_CONSULTA_ACCESO_POR_PERMISO";
            SqlCommand cmd = new SqlCommand(strQ, cn);
            DataSet ds = new DataSet();
            try
            {
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = strQ;
                cmd.Parameters.Clear();


                SqlParameter P_PERMISO_p = new SqlParameter("@P_IDPERMISO", SqlDbType.Int);
                P_PERMISO_p.Value = P_PERMISO;
                P_PERMISO_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_PERMISO_p);


                SqlParameter CodRes_P = new SqlParameter("P_RESPUESTA", SqlDbType.Int);
                CodRes_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(CodRes_P);

                SqlParameter msgErr1_P = new SqlParameter("P_MSG1", SqlDbType.NVarChar);
                msgErr1_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgErr1_P);

                SqlParameter msgErr2_P = new SqlParameter("P_MSG2", SqlDbType.NVarChar);
                msgErr2_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgErr2_P);

                cmd.Parameters["P_MSG1"].Size = 100;
                cmd.Parameters["P_MSG2"].Size = 100;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception e)
            {
                String s = e.Message.ToString();
                return ds;
            }
            finally
            {
                cn.Close();
            }
            return ds;
        }

        public DataSet CONSULTA_PERMISOS_PERFIL(int P_PERFIL)
        {
            //P_OPC 0 - TODO 1 -ACTIVO
            string strQ = "SP_CONSULTA_PERMISOS_P";
            SqlCommand cmd = new SqlCommand(strQ, cn);
            DataSet ds = new DataSet();
            try
            {
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = strQ;
                cmd.Parameters.Clear();


                SqlParameter P_PERFIL_p = new SqlParameter("@P_PERFIL", SqlDbType.Int);
                P_PERFIL_p.Value = P_PERFIL;
                P_PERFIL_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_PERFIL_p);


                SqlParameter CodRes_P = new SqlParameter("P_RESPUESTA", SqlDbType.Int);
                CodRes_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(CodRes_P);

                SqlParameter msgErr1_P = new SqlParameter("P_MSG1", SqlDbType.NVarChar);
                msgErr1_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgErr1_P);

                SqlParameter msgErr2_P = new SqlParameter("P_MSG2", SqlDbType.NVarChar);
                msgErr2_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgErr2_P);

                cmd.Parameters["P_MSG1"].Size = 100;
                cmd.Parameters["P_MSG2"].Size = 100;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception e)
            {
                String s = e.Message.ToString();
                return ds;
            }
            finally
            {
                cn.Close();
            }
            return ds;
        }


        public DataSet CONSULTA_ACCIONES_PERMISO(int P_ID_PERMISO)
        {
            string strQ = "SP_MOV_PERMISOS";
            SqlCommand cmd = new SqlCommand(strQ, cn);
            DataSet ds = new DataSet();
            try
            {
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = strQ;
                cmd.Parameters.Clear();
                SqlParameter p_opcion_P = new SqlParameter("@P_OPCION", SqlDbType.Int);
                p_opcion_P.Value = 2;
                p_opcion_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(p_opcion_P);

                SqlParameter P_IDUSUARIO_p = new SqlParameter("@P_IDUSUARIO", SqlDbType.Int);
                P_IDUSUARIO_p.Value = 1;
                P_IDUSUARIO_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_IDUSUARIO_p);

                SqlParameter P_ID_PERMISO_p = new SqlParameter("@P_ID_PERMISO", SqlDbType.Int);
                P_ID_PERMISO_p.Value = P_ID_PERMISO;
                P_ID_PERMISO_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_ID_PERMISO_p);

                SqlParameter P_ID_ACCESO_p = new SqlParameter("@P_ID_ACCESO", SqlDbType.Int);
                P_ID_ACCESO_p.Value = 1;
                P_ID_ACCESO_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_ID_ACCESO_p);

                SqlParameter P_SW_ASIGNADO_p = new SqlParameter("@P_SW_ASIGNADO", SqlDbType.Int);
                P_SW_ASIGNADO_p.Value = 1;
                P_SW_ASIGNADO_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_SW_ASIGNADO_p);

                SqlParameter p_estado_p = new SqlParameter("@P_ESTADO", SqlDbType.Int);
                p_estado_p.Value = 1;
                p_estado_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(p_estado_p);

                SqlParameter P_ID_RESPONSABLE_p = new SqlParameter("@P_ID_RESPONSABLE", SqlDbType.Int);
                P_ID_RESPONSABLE_p.Value = 1;
                P_ID_RESPONSABLE_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_ID_RESPONSABLE_p);

                SqlParameter CodRes_P = new SqlParameter("@P_RESPUESTA", SqlDbType.Int);
                CodRes_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(CodRes_P);

                SqlParameter msgErr1_P = new SqlParameter("@P_MSG1", SqlDbType.NVarChar);
                msgErr1_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgErr1_P);

                SqlParameter msgErr2_P = new SqlParameter("@P_MSG2", SqlDbType.NVarChar);
                msgErr2_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgErr2_P);

                cmd.Parameters["@P_MSG1"].Size = 100;
                cmd.Parameters["@P_MSG2"].Size = 100;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception e)
            {
                String s = e.Message.ToString();
                return ds;
            }
            finally
            {
                cn.Close();
            }
            return ds;
        }

        public DataSet CONSULTA_PERFIL_PERMISO(int P_ID_PERFIL)
        {
            //P_OPC 0 - TODO 1 -ACTIVO
            string strQ = "SP_CONSULTA_PERMISOS_P";
            SqlCommand cmd = new SqlCommand(strQ, cn);
            DataSet ds = new DataSet();
            try
            {
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = strQ;
                cmd.Parameters.Clear();
                SqlParameter p_opcion_P = new SqlParameter("@P_PERFIL", SqlDbType.Int);
                p_opcion_P.Value = P_ID_PERFIL;
                p_opcion_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(p_opcion_P);

                SqlParameter CodRes_P = new SqlParameter("@P_RESPUESTA", SqlDbType.Int);
                CodRes_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(CodRes_P);

                SqlParameter msgErr1_P = new SqlParameter("@P_MSG1", SqlDbType.NVarChar);
                msgErr1_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgErr1_P);

                SqlParameter msgErr2_P = new SqlParameter("@P_MSG2", SqlDbType.NVarChar);
                msgErr2_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgErr2_P);

                cmd.Parameters["@P_MSG1"].Size = 100;
                cmd.Parameters["@P_MSG2"].Size = 100;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception e)
            {
                String s = e.Message.ToString();
                return ds;
            }
            finally
            {
                cn.Close();
            }
            return ds;
        }

        public DataSet CONSULTA_SERIES_USER(int P_ID_USER)
        {
            //P_OPC 0 - TODO 1 -ACTIVO
            string strQ = "SP_CONSULTA_DATA_USER";
            SqlCommand cmd = new SqlCommand(strQ, cn);
            DataSet ds = new DataSet();
            try
            {
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = strQ;
                cmd.Parameters.Clear();

                SqlParameter P_IDUSER_p = new SqlParameter("@P_IDUSUARIO", SqlDbType.Int);
                P_IDUSER_p.Value = P_ID_USER;
                P_IDUSER_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_IDUSER_p);


                SqlParameter CodRes_P = new SqlParameter("P_RESPUESTA", SqlDbType.Int);
                CodRes_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(CodRes_P);

                SqlParameter msgErr1_P = new SqlParameter("P_MSG1", SqlDbType.NVarChar);
                msgErr1_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgErr1_P);

                SqlParameter msgErr2_P = new SqlParameter("P_MSG2", SqlDbType.NVarChar);
                msgErr2_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgErr2_P);

                cmd.Parameters["P_MSG1"].Size = 100;
                cmd.Parameters["P_MSG2"].Size = 100;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception e)
            {
                String s = e.Message.ToString();
                return ds;
            }
            finally
            {
                cn.Close();
            }
            return ds;
        }


        public DataSet CONSULTA_PERMISOS_USER(int P_ID_USER, int P_IDPERMISO, int P_OPC)
        {
            //P_OPC 0 - TODO 1 -ACTIVO
            string strQ = "SP_CONSULTA_ACCESO_USER";
            SqlCommand cmd = new SqlCommand(strQ, cn);
            DataSet ds = new DataSet();
            try
            {
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = strQ;
                cmd.Parameters.Clear();

                SqlParameter P_IDUSER_p = new SqlParameter("@P_IDUSER", SqlDbType.Int);
                P_IDUSER_p.Value = P_ID_USER;
                P_IDUSER_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_IDUSER_p);

                SqlParameter P_IDPERMISO_p = new SqlParameter("@P_IDPERMISO", SqlDbType.Int);
                P_IDPERMISO_p.Value = P_IDPERMISO;
                P_IDPERMISO_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_IDPERMISO_p);

                SqlParameter P_OPC_p = new SqlParameter("@P_OPC", SqlDbType.Int);
                P_OPC_p.Value = P_OPC;
                P_OPC_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_OPC_p);

                SqlParameter CodRes_P = new SqlParameter("P_RESPUESTA", SqlDbType.Int);
                CodRes_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(CodRes_P);

                SqlParameter msgErr1_P = new SqlParameter("P_MSG1", SqlDbType.NVarChar);
                msgErr1_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgErr1_P);

                SqlParameter msgErr2_P = new SqlParameter("P_MSG2", SqlDbType.NVarChar);
                msgErr2_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgErr2_P);

                cmd.Parameters["P_MSG1"].Size = 100;
                cmd.Parameters["P_MSG2"].Size = 100;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception e)
            {
                String s = e.Message.ToString();
                return ds;
            }
            finally
            {
                cn.Close();
            }
            return ds;
        }

        #endregion

        #region MODIFICA_DATOS

        public int MODIFICA_DATOS_CATALOGOS(string p_catalogo, int p_idcatalogo, int p_idestado, string p_descripcion, int p_user_resp, out string p_msg1, out string p_msg2)
        {
            //P_OPC 0 - TODO 1 -ACTIVO
            string strQ = "SP_MODIFICA_REG_CATALOGO";
            SqlCommand cmd = new SqlCommand(strQ, cn);
            DataSet ds = new DataSet();
            try
            {
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = strQ;
                cmd.Parameters.Clear();

                SqlParameter p_catalogo_p = new SqlParameter("@P_CATALOGO", SqlDbType.VarChar);
                p_catalogo_p.Value = p_catalogo;
                p_catalogo_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(p_catalogo_p);

                SqlParameter p_idcata_p = new SqlParameter("@P_IDCATALOGO", SqlDbType.Int);
                p_idcata_p.Value = p_idcatalogo;
                p_idcata_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(p_idcata_p);

                SqlParameter p_idestado_p = new SqlParameter("@P_ESTADO", SqlDbType.Int);
                p_idestado_p.Value = p_idestado;
                p_idestado_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(p_idestado_p);

                SqlParameter p_descripcion_p = new SqlParameter("@P_DESCRIPCION", SqlDbType.VarChar);
                p_descripcion_p.Value = p_descripcion;
                p_descripcion_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(p_descripcion_p);

                SqlParameter p_user_p = new SqlParameter("@P_USUARIO_RESP", SqlDbType.Int);
                p_user_p.Value = p_user_resp;
                p_user_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(p_user_p);


                SqlParameter CodRes_P = new SqlParameter("P_RESPUESTA", SqlDbType.Int);
                CodRes_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(CodRes_P);

                SqlParameter msgErr1_P = new SqlParameter("P_MSG1", SqlDbType.NVarChar);
                msgErr1_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgErr1_P);

                SqlParameter msgErr2_P = new SqlParameter("P_MSG2", SqlDbType.NVarChar);
                msgErr2_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgErr2_P);

                cmd.Parameters["P_MSG1"].Size = 100;
                cmd.Parameters["P_MSG2"].Size = 100;

                cmd.ExecuteNonQuery();
                p_msg1 = msgErr1_P.Value.ToString();
                p_msg2 = msgErr2_P.Value.ToString();
                return Convert.ToInt32(CodRes_P.Value);
            }
            catch (Exception e)
            {
                String s = e.Message.ToString();
                p_msg1 = e.Message.ToString();
                p_msg2 = "-";
                return -1;
            }
            finally
            {
                cn.Close();
            }
        }




        public int MODIFICA_ACCIONES_PERMISO(int P_IDUSUARIO, int P_ID_PERMISO, int P_ID_ACCESO, int P_SW_ASIGNADO, int P_ID_RESPONSABLE, out string p_msg1, out string p_msg2)
        {
            //P_OPC 0 - TODO 1 -ACTIVO
            string strQ = "SP_MOV_PERMISOS";
            SqlCommand cmd = new SqlCommand(strQ, cn);
            DataSet ds = new DataSet();
            try
            {
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = strQ;
                cmd.Parameters.Clear();
                SqlParameter p_opcion_P = new SqlParameter("@P_OPCION", SqlDbType.Int);
                p_opcion_P.Value = 1;
                p_opcion_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(p_opcion_P);

                SqlParameter P_IDUSUARIO_p = new SqlParameter("@P_IDUSUARIO", SqlDbType.Int);
                P_IDUSUARIO_p.Value = P_IDUSUARIO;
                P_IDUSUARIO_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_IDUSUARIO_p);

                SqlParameter P_ID_PERMISO_p = new SqlParameter("@P_ID_PERMISO", SqlDbType.Int);
                P_ID_PERMISO_p.Value = P_ID_PERMISO;
                P_ID_PERMISO_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_ID_PERMISO_p);

                SqlParameter P_ID_ACCESO_p = new SqlParameter("@P_ID_ACCESO", SqlDbType.Int);
                P_ID_ACCESO_p.Value = P_ID_ACCESO;
                P_ID_ACCESO_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_ID_ACCESO_p);

                SqlParameter P_SW_ASIGNADO_p = new SqlParameter("@P_SW_ASIGNADO", SqlDbType.Int);
                P_SW_ASIGNADO_p.Value = P_SW_ASIGNADO;
                P_SW_ASIGNADO_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_SW_ASIGNADO_p);

                SqlParameter p_estado_p = new SqlParameter("@P_ESTADO", SqlDbType.Int);
                p_estado_p.Value = 1;
                p_estado_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(p_estado_p);

                SqlParameter P_ID_RESPONSABLE_p = new SqlParameter("@P_ID_RESPONSABLE", SqlDbType.Int);
                P_ID_RESPONSABLE_p.Value = P_ID_RESPONSABLE;
                P_ID_RESPONSABLE_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_ID_RESPONSABLE_p);

                SqlParameter CodRes_P = new SqlParameter("@P_RESPUESTA", SqlDbType.Int);
                CodRes_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(CodRes_P);

                SqlParameter msgErr1_P = new SqlParameter("@P_MSG1", SqlDbType.NVarChar);
                msgErr1_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgErr1_P);

                SqlParameter msgErr2_P = new SqlParameter("@P_MSG2", SqlDbType.NVarChar);
                msgErr2_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgErr2_P);

                cmd.Parameters["@P_MSG1"].Size = 100;
                cmd.Parameters["@P_MSG2"].Size = 100;


                cmd.ExecuteNonQuery();
                p_msg1 = msgErr1_P.Value.ToString();
                p_msg2 = msgErr2_P.Value.ToString();
                return Convert.ToInt32(CodRes_P.Value);
            }
            catch (Exception e)
            {
                String s = e.Message.ToString();
                p_msg1 = e.Message.ToString();
                p_msg2 = "-";
                return -1;
            }
            finally
            {
                cn.Close();
            }
        }

        public int MODIFICA_PERMISOS_PERFIL(int P_ID_PERMISO, int P_ID_ACCESO, int P_ID_PERFIL, int P_SW_ASIGNADO, int P_ID_RESPONSABLE, out string p_msg1, out string p_msg2)
        {
            //P_OPC 0 - TODO 1 -ACTIVO
            string strQ = "SP_ACCESO_PERMISO";
            SqlCommand cmd = new SqlCommand(strQ, cn);
            DataSet ds = new DataSet();
            try
            {
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = strQ;
                cmd.Parameters.Clear();


                SqlParameter P_ID_PERMISO_p = new SqlParameter("@P_ID_PERMISO", SqlDbType.Int);
                P_ID_PERMISO_p.Value = P_ID_PERMISO;
                P_ID_PERMISO_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_ID_PERMISO_p);

                SqlParameter P_ID_ACCESO_p = new SqlParameter("@P_ID_ACCESO", SqlDbType.Int);
                P_ID_ACCESO_p.Value = P_ID_ACCESO;
                P_ID_ACCESO_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_ID_ACCESO_p);

                SqlParameter P_ID_PERFIL_p = new SqlParameter("@P_ID_PERFIL", SqlDbType.Int);
                P_ID_PERFIL_p.Value = P_ID_PERFIL;
                P_ID_PERFIL_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_ID_PERFIL_p);

                SqlParameter P_SW_ASIGNADO_p = new SqlParameter("@P_CHECK", SqlDbType.Int);
                P_SW_ASIGNADO_p.Value = P_SW_ASIGNADO;
                P_SW_ASIGNADO_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_SW_ASIGNADO_p);


                SqlParameter P_ID_RESPONSABLE_p = new SqlParameter("@P_IDUSERRESP", SqlDbType.Int);
                P_ID_RESPONSABLE_p.Value = P_ID_RESPONSABLE;
                P_ID_RESPONSABLE_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_ID_RESPONSABLE_p);

                SqlParameter CodRes_P = new SqlParameter("@P_RESPUESTA", SqlDbType.Int);
                CodRes_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(CodRes_P);

                SqlParameter msgErr1_P = new SqlParameter("@P_MSG1", SqlDbType.NVarChar);
                msgErr1_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgErr1_P);

                SqlParameter msgErr2_P = new SqlParameter("@P_MSG2", SqlDbType.NVarChar);
                msgErr2_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgErr2_P);

                cmd.Parameters["@P_MSG1"].Size = 100;
                cmd.Parameters["@P_MSG2"].Size = 100;


                cmd.ExecuteNonQuery();
                p_msg1 = msgErr1_P.Value.ToString();
                p_msg2 = msgErr2_P.Value.ToString();
                return Convert.ToInt32(CodRes_P.Value);
            }
            catch (Exception e)
            {
                String s = e.Message.ToString();
                p_msg1 = e.Message.ToString();
                p_msg2 = "-";
                return -1;
            }
            finally
            {
                cn.Close();
            }
        }

        public int MODIFICA_PERFIL_USUARIO(int p_iduser, int p_perfil, int P_usr_resp, out string p_msg1, out string p_msg2)
        {
            string strQ = "SP_MODIFICA_PERFIL_USUARIO";
            SqlCommand cmd = new SqlCommand(strQ, cn);
            DataSet ds = new DataSet();
            try
            {
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = strQ;
                cmd.Parameters.Clear();

                SqlParameter p_iduser_P = new SqlParameter("@P_ID_USERID", SqlDbType.Int);
                p_iduser_P.Value = p_iduser;
                p_iduser_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(p_iduser_P);

                SqlParameter p_perfil_P = new SqlParameter("@P_ID_PERFIL", SqlDbType.Int);
                p_perfil_P.Value = p_perfil;
                p_perfil_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(p_perfil_P);

                SqlParameter p_user_p = new SqlParameter("@P_IDUSERRESP", SqlDbType.Int);
                p_user_p.Value = P_usr_resp;
                p_user_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(p_user_p);

                SqlParameter CodRes_P = new SqlParameter("@P_RESPUESTA", SqlDbType.Int);
                CodRes_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(CodRes_P);

                SqlParameter msgErr1_P = new SqlParameter("@P_MSG1", SqlDbType.VarChar);
                msgErr1_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgErr1_P);

                SqlParameter msgErr2_P = new SqlParameter("@P_MSG2", SqlDbType.VarChar);
                msgErr2_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgErr2_P);

                cmd.Parameters["@P_MSG1"].Size = 100;
                cmd.Parameters["@P_MSG2"].Size = 100;

                cmd.ExecuteNonQuery();
                p_msg1 = msgErr1_P.Value.ToString();
                p_msg2 = msgErr2_P.Value.ToString();
                return Convert.ToInt32(CodRes_P.Value);
            }
            catch (Exception e)
            {
                String s = e.Message.ToString();
                p_msg1 = s;
                p_msg2 = "";
                return -1;
            }
            finally
            {
                cn.Close();
            }

        }


        #endregion

        #region INGRESO_DATA_USUARIOS

        public int INGRESA_USUARIO(int P_IDUSUARIO, string P_USUARIO, string P_NOMBRES, string P_APELLIDOS, string P_CORREO, int P_BOOL_SUPERVISOR, int P_ESTADO, int P_ID_RESPONSABLE, int p_tipoUser, string SerieRecibo, string SerieEmision,string SerieTranStock, int p_Bodega, string user_sap, out string p_msg1, out string p_msg2)
        {
            //P_OPC 0 - TODO 1 -ACTIVO
            string strQ = "SP_MOV_USUARIOS";
            SqlCommand cmd = new SqlCommand(strQ, cn);
            DataSet ds = new DataSet();
            try
            {
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = strQ;
                cmd.Parameters.Clear();


                SqlParameter P_OPCION_P = new SqlParameter("@P_OPCION", SqlDbType.Int);
                P_OPCION_P.Value = 1;
                P_OPCION_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_OPCION_P);

                SqlParameter P_IDUSUARIO_P = new SqlParameter("@P_IDUSUARIO", SqlDbType.Int);
                P_IDUSUARIO_P.Value = P_IDUSUARIO;
                P_IDUSUARIO_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_IDUSUARIO_P);

                SqlParameter P_USUARIO_P = new SqlParameter("@P_USUARIO", SqlDbType.VarChar);
                P_USUARIO_P.Value = P_USUARIO;
                P_USUARIO_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_USUARIO_P);

                SqlParameter P_NOMBRES_P = new SqlParameter("@P_NOMBRES", SqlDbType.VarChar);
                P_NOMBRES_P.Value = P_NOMBRES;
                P_NOMBRES_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_NOMBRES_P);

                SqlParameter P_APELLIDOS_P = new SqlParameter("@P_APELLIDOS", SqlDbType.VarChar);
                P_APELLIDOS_P.Value = P_APELLIDOS;
                P_APELLIDOS_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_APELLIDOS_P);

                SqlParameter P_CORREO_P = new SqlParameter("@P_CORREO", SqlDbType.VarChar);
                P_CORREO_P.Value = P_CORREO;
                P_CORREO_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_CORREO_P);

                SqlParameter P_BOOL_SUPERVISOR_P = new SqlParameter("@P_BOOL_SUPERVISOR", SqlDbType.Int);
                P_BOOL_SUPERVISOR_P.Value = P_BOOL_SUPERVISOR;
                P_BOOL_SUPERVISOR_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_BOOL_SUPERVISOR_P);

                SqlParameter P_ESTADO_P = new SqlParameter("@P_ESTADO", SqlDbType.Int);
                P_ESTADO_P.Value = P_ESTADO;
                P_ESTADO_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_ESTADO_P);

                SqlParameter P_PASSWORD_P = new SqlParameter("@P_PASSWORD", SqlDbType.VarChar);
                P_PASSWORD_P.Value = " ";
                P_PASSWORD_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_PASSWORD_P);

                SqlParameter P_ID_RESPONSABLE_P = new SqlParameter("@P_ID_RESPONSABLE", SqlDbType.Int);
                P_ID_RESPONSABLE_P.Value = P_ID_RESPONSABLE;
                P_ID_RESPONSABLE_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_ID_RESPONSABLE_P);

                SqlParameter P_NUM_SERIE_EMP_P = new SqlParameter("@P_NUM_SERIE_EMP", SqlDbType.VarChar);
                P_NUM_SERIE_EMP_P.Value = SerieEmision;
                P_NUM_SERIE_EMP_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_NUM_SERIE_EMP_P);

                SqlParameter P_NUM_SERIE_REP_P = new SqlParameter("@P_NUM_SERIE_REP", SqlDbType.VarChar);
                P_NUM_SERIE_REP_P.Value = SerieRecibo;
                P_NUM_SERIE_REP_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_NUM_SERIE_REP_P);

                SqlParameter P_NUM_SERIE_SERSTOCK_P = new SqlParameter("@P_NUM_SERIE_TSTOCK", SqlDbType.VarChar);
                P_NUM_SERIE_SERSTOCK_P.Value = SerieTranStock;
                P_NUM_SERIE_SERSTOCK_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_NUM_SERIE_SERSTOCK_P);

                SqlParameter P_ID_TIPOUSER_P = new SqlParameter("@P_ID_TIPOUSER", SqlDbType.VarChar);
                P_ID_TIPOUSER_P.Value = p_tipoUser;
                P_ID_TIPOUSER_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_ID_TIPOUSER_P);

                SqlParameter P_ID_BODEGA_P = new SqlParameter("@P_ID_BODEGA", SqlDbType.Int);
                P_ID_BODEGA_P.Value = p_Bodega;
                P_ID_BODEGA_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_ID_BODEGA_P);

                SqlParameter P_SAP_USER_P = new SqlParameter("@P_SAP_USER", SqlDbType.VarChar);
                P_SAP_USER_P.Value = user_sap;
                P_SAP_USER_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_SAP_USER_P);

                SqlParameter CodRes_P = new SqlParameter("P_RESPUESTA", SqlDbType.Int);
                CodRes_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(CodRes_P);

                SqlParameter msgErr1_P = new SqlParameter("P_MSG1", SqlDbType.NVarChar);
                msgErr1_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgErr1_P);

                SqlParameter msgErr2_P = new SqlParameter("P_MSG2", SqlDbType.NVarChar);
                msgErr2_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgErr2_P);

                cmd.Parameters["P_MSG1"].Size = 100;
                cmd.Parameters["P_MSG2"].Size = 100;


                cmd.ExecuteNonQuery();
                p_msg1 = msgErr1_P.Value.ToString();
                p_msg2 = msgErr2_P.Value.ToString();
                return Convert.ToInt32(CodRes_P.Value);
            }
            catch (Exception e)
            {
                String s = e.Message.ToString();
                p_msg1 = e.Message.ToString();
                p_msg2 = "-";
                return -1;
            }
            finally
            {
                cn.Close();
            }
        }

        public int MODIFICA_USUARIO(int P_IDUSUARIO, string P_USUARIO, string P_NOMBRES, string P_APELLIDOS, string P_CORREO, int P_BOOL_SUPERVISOR, int P_ESTADO, int P_ID_RESPONSABLE, int p_tipoUser, string SerieRecibo, string SerieEmision,string SerieTranStock, int p_Bodega, string user_sap, out string p_msg1, out string p_msg2)
        {
            //P_OPC 0 - TODO 1 -ACTIVO
            string strQ = "SP_MOV_USUARIOS";
            SqlCommand cmd = new SqlCommand(strQ, cn);
            DataSet ds = new DataSet();
            try
            {
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = strQ;
                cmd.Parameters.Clear();


                SqlParameter P_OPCION_P = new SqlParameter("@P_OPCION", SqlDbType.Int);
                P_OPCION_P.Value = 3;
                P_OPCION_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_OPCION_P);

                SqlParameter P_IDUSUARIO_P = new SqlParameter("@P_IDUSUARIO", SqlDbType.Int);
                P_IDUSUARIO_P.Value = P_IDUSUARIO;
                P_IDUSUARIO_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_IDUSUARIO_P);

                SqlParameter P_USUARIO_P = new SqlParameter("@P_USUARIO", SqlDbType.VarChar);
                P_USUARIO_P.Value = P_USUARIO;
                P_USUARIO_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_USUARIO_P);

                SqlParameter P_NOMBRES_P = new SqlParameter("@P_NOMBRES", SqlDbType.VarChar);
                P_NOMBRES_P.Value = P_NOMBRES;
                P_NOMBRES_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_NOMBRES_P);

                SqlParameter P_APELLIDOS_P = new SqlParameter("@P_APELLIDOS", SqlDbType.VarChar);
                P_APELLIDOS_P.Value = P_APELLIDOS;
                P_APELLIDOS_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_APELLIDOS_P);

                SqlParameter P_CORREO_P = new SqlParameter("@P_CORREO", SqlDbType.VarChar);
                P_CORREO_P.Value = P_CORREO;
                P_CORREO_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_CORREO_P);

                SqlParameter P_BOOL_SUPERVISOR_P = new SqlParameter("@P_BOOL_SUPERVISOR", SqlDbType.Int);
                P_BOOL_SUPERVISOR_P.Value = P_BOOL_SUPERVISOR;
                P_BOOL_SUPERVISOR_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_BOOL_SUPERVISOR_P);

                SqlParameter P_ESTADO_P = new SqlParameter("@P_ESTADO", SqlDbType.Int);
                P_ESTADO_P.Value = P_ESTADO;
                P_ESTADO_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_ESTADO_P);

                SqlParameter P_PASSWORD_P = new SqlParameter("@P_PASSWORD", SqlDbType.VarChar);
                P_PASSWORD_P.Value = " ";
                P_PASSWORD_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_PASSWORD_P);

                SqlParameter P_ID_RESPONSABLE_P = new SqlParameter("@P_ID_RESPONSABLE", SqlDbType.Int);
                P_ID_RESPONSABLE_P.Value = P_ID_RESPONSABLE;
                P_ID_RESPONSABLE_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_ID_RESPONSABLE_P);

                SqlParameter P_NUM_SERIE_EMP_P = new SqlParameter("@P_NUM_SERIE_EMP", SqlDbType.VarChar);
                P_NUM_SERIE_EMP_P.Value = SerieEmision;
                P_NUM_SERIE_EMP_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_NUM_SERIE_EMP_P);

                SqlParameter P_NUM_SERIE_REP_P = new SqlParameter("@P_NUM_SERIE_REP", SqlDbType.VarChar);
                P_NUM_SERIE_REP_P.Value = SerieRecibo;
                P_NUM_SERIE_REP_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_NUM_SERIE_REP_P);

                SqlParameter P_NUM_SERIE_SERSTOCK_P = new SqlParameter("@P_NUM_SERIE_TSTOCK", SqlDbType.VarChar);
                P_NUM_SERIE_SERSTOCK_P.Value = SerieTranStock;
                P_NUM_SERIE_SERSTOCK_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_NUM_SERIE_SERSTOCK_P);

                

                SqlParameter P_ID_TIPOUSER_P = new SqlParameter("@P_ID_TIPOUSER", SqlDbType.VarChar);
                P_ID_TIPOUSER_P.Value = p_tipoUser;
                P_ID_TIPOUSER_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_ID_TIPOUSER_P);

                SqlParameter P_ID_BODEGA_P = new SqlParameter("@P_ID_BODEGA", SqlDbType.Int);
                P_ID_BODEGA_P.Value = p_Bodega;
                P_ID_BODEGA_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_ID_BODEGA_P);

                SqlParameter P_SAP_USER_P = new SqlParameter("@P_SAP_USER", SqlDbType.VarChar);
                P_SAP_USER_P.Value = user_sap;
                P_SAP_USER_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_SAP_USER_P);

                SqlParameter CodRes_P = new SqlParameter("P_RESPUESTA", SqlDbType.Int);
                CodRes_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(CodRes_P);

                SqlParameter msgErr1_P = new SqlParameter("P_MSG1", SqlDbType.NVarChar);
                msgErr1_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgErr1_P);

                SqlParameter msgErr2_P = new SqlParameter("P_MSG2", SqlDbType.NVarChar);
                msgErr2_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgErr2_P);

                cmd.Parameters["P_MSG1"].Size = 100;
                cmd.Parameters["P_MSG2"].Size = 100;


                cmd.ExecuteNonQuery();
                p_msg1 = msgErr1_P.Value.ToString();
                p_msg2 = msgErr2_P.Value.ToString();
                return Convert.ToInt32(CodRes_P.Value);
            }
            catch (Exception e)
            {
                String s = e.Message.ToString();
                p_msg1 = e.Message.ToString();
                p_msg2 = "-";
                return -1;
            }
            finally
            {
                cn.Close();
            }
        }


        public int CAMBIA_PASS_USUARIO(int P_IDUSUARIO, string P_PASSWORD, out string p_msg1, out string p_msg2)
        {
            //P_OPC 0 - TODO 1 -ACTIVO
            string strQ = "SP_MOV_USUARIOS";
            SqlCommand cmd = new SqlCommand(strQ, cn);
            DataSet ds = new DataSet();
            try
            {
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = strQ;
                cmd.Parameters.Clear();


                SqlParameter P_OPCION_P = new SqlParameter("@P_OPCION", SqlDbType.Int);
                P_OPCION_P.Value = 8;
                P_OPCION_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_OPCION_P);

                SqlParameter P_IDUSUARIO_P = new SqlParameter("@P_IDUSUARIO", SqlDbType.Int);
                P_IDUSUARIO_P.Value = P_IDUSUARIO;
                P_IDUSUARIO_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_IDUSUARIO_P);

                SqlParameter P_USUARIO_P = new SqlParameter("@P_USUARIO", SqlDbType.VarChar);
                P_USUARIO_P.Value = "";
                P_USUARIO_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_USUARIO_P);

                SqlParameter P_NOMBRES_P = new SqlParameter("@P_NOMBRES", SqlDbType.VarChar);
                P_NOMBRES_P.Value = "";
                P_NOMBRES_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_NOMBRES_P);

                SqlParameter P_APELLIDOS_P = new SqlParameter("@P_APELLIDOS", SqlDbType.VarChar);
                P_APELLIDOS_P.Value = "";
                P_APELLIDOS_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_APELLIDOS_P);

                SqlParameter P_CORREO_P = new SqlParameter("@P_CORREO", SqlDbType.VarChar);
                P_CORREO_P.Value = "";
                P_CORREO_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_CORREO_P);

                SqlParameter P_BOOL_SUPERVISOR_P = new SqlParameter("@P_BOOL_SUPERVISOR", SqlDbType.Int);
                P_BOOL_SUPERVISOR_P.Value = 0;
                P_BOOL_SUPERVISOR_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_BOOL_SUPERVISOR_P);

                SqlParameter P_ESTADO_P = new SqlParameter("@P_ESTADO", SqlDbType.Int);
                P_ESTADO_P.Value = 0;
                P_ESTADO_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_ESTADO_P);

                SqlParameter P_PASSWORD_P = new SqlParameter("@P_PASSWORD", SqlDbType.VarChar);
                P_PASSWORD_P.Value = P_PASSWORD;
                P_PASSWORD_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_PASSWORD_P);

                SqlParameter P_ID_RESPONSABLE_P = new SqlParameter("@P_ID_RESPONSABLE", SqlDbType.Int);
                P_ID_RESPONSABLE_P.Value = 0;
                P_ID_RESPONSABLE_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_ID_RESPONSABLE_P);

                SqlParameter P_NUM_SERIE_EMP_P = new SqlParameter("@P_NUM_SERIE_EMP", SqlDbType.VarChar);
                P_NUM_SERIE_EMP_P.Value = "";
                P_NUM_SERIE_EMP_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_NUM_SERIE_EMP_P);

                SqlParameter P_NUM_SERIE_REP_P = new SqlParameter("@P_NUM_SERIE_REP", SqlDbType.VarChar);
                P_NUM_SERIE_REP_P.Value = "";
                P_NUM_SERIE_REP_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_NUM_SERIE_REP_P);

                SqlParameter P_NUM_SERIE_SERSTOCK_P = new SqlParameter("@P_NUM_SERIE_TSTOCK", SqlDbType.VarChar);
                P_NUM_SERIE_SERSTOCK_P.Value = "";
                P_NUM_SERIE_SERSTOCK_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_NUM_SERIE_SERSTOCK_P);

                SqlParameter P_ID_TIPOUSER_P = new SqlParameter("@P_ID_TIPOUSER", SqlDbType.VarChar);
                P_ID_TIPOUSER_P.Value = "";
                P_ID_TIPOUSER_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_ID_TIPOUSER_P);

                SqlParameter P_ID_BODEGA_P = new SqlParameter("@P_ID_BODEGA", SqlDbType.Int);
                P_ID_BODEGA_P.Value = 0;
                P_ID_BODEGA_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_ID_BODEGA_P);

                SqlParameter P_SAP_USER_P = new SqlParameter("@P_SAP_USER", SqlDbType.VarChar);
                P_SAP_USER_P.Value = "";
                P_SAP_USER_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_SAP_USER_P);

                SqlParameter CodRes_P = new SqlParameter("P_RESPUESTA", SqlDbType.Int);
                CodRes_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(CodRes_P);

                SqlParameter msgErr1_P = new SqlParameter("P_MSG1", SqlDbType.NVarChar);
                msgErr1_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgErr1_P);

                SqlParameter msgErr2_P = new SqlParameter("P_MSG2", SqlDbType.NVarChar);
                msgErr2_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgErr2_P);

                cmd.Parameters["P_MSG1"].Size = 100;
                cmd.Parameters["P_MSG2"].Size = 100;


                cmd.ExecuteNonQuery();
                p_msg1 = msgErr1_P.Value.ToString();
                p_msg2 = msgErr2_P.Value.ToString();
                return Convert.ToInt32(CodRes_P.Value);
            }
            catch (Exception e)
            {
                String s = e.Message.ToString();
                p_msg1 = e.Message.ToString();
                p_msg2 = "-";
                return -1;
            }
            finally
            {
                cn.Close();
            }
        }

        public int MODIFICA_SUPERV_USER(int P_ID_SUPERVISOR, int P_ID_USUARIO, int P_SW_ASIGNADO, int P_ID_RESPONSABLE, out string p_msg1, out string p_msg2)
        {
            //P_OPC 0 - TODO 1 -ACTIVO
            string strQ = "SP_MOV_SUPERVISA";
            SqlCommand cmd = new SqlCommand(strQ, cn);
            DataSet ds = new DataSet();
            try
            {
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = strQ;
                cmd.Parameters.Clear();
                SqlParameter p_opcion_P = new SqlParameter("@P_OPCION", SqlDbType.Int);
                p_opcion_P.Value = 1;
                p_opcion_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(p_opcion_P);

                SqlParameter P_ID_SUPERVISA_p = new SqlParameter("@P_ID_SUPERVISA", SqlDbType.Int);
                P_ID_SUPERVISA_p.Value = P_ID_SUPERVISOR;
                P_ID_SUPERVISA_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_ID_SUPERVISA_p);

                SqlParameter P_ID_USUARIO_p = new SqlParameter("@P_ID_USUARIO", SqlDbType.Int);
                P_ID_USUARIO_p.Value = P_ID_USUARIO;
                P_ID_USUARIO_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_ID_USUARIO_p);

                SqlParameter P_SW_ASIGNADO_p = new SqlParameter("@P_SW_ASIGNADO", SqlDbType.Int);
                P_SW_ASIGNADO_p.Value = P_SW_ASIGNADO;
                P_SW_ASIGNADO_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_SW_ASIGNADO_p);

                SqlParameter P_ID_RESPONSABLE_p = new SqlParameter("@P_ID_RESPONSABLE", SqlDbType.Int);
                P_ID_RESPONSABLE_p.Value = P_ID_RESPONSABLE;
                P_ID_RESPONSABLE_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_ID_RESPONSABLE_p);

                SqlParameter CodRes_P = new SqlParameter("@P_RESPUESTA", SqlDbType.Int);
                CodRes_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(CodRes_P);

                SqlParameter msgErr1_P = new SqlParameter("@P_MSG1", SqlDbType.NVarChar);
                msgErr1_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgErr1_P);

                SqlParameter msgErr2_P = new SqlParameter("@P_MSG2", SqlDbType.NVarChar);
                msgErr2_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgErr2_P);

                cmd.Parameters["@P_MSG1"].Size = 100;
                cmd.Parameters["@P_MSG2"].Size = 100;


                cmd.ExecuteNonQuery();
                p_msg1 = msgErr1_P.Value.ToString();
                p_msg2 = msgErr2_P.Value.ToString();
                return Convert.ToInt32(CodRes_P.Value);
            }
            catch (Exception e)
            {
                String s = e.Message.ToString();
                p_msg1 = e.Message.ToString();
                p_msg2 = "-";
                return -1;
            }
            finally
            {
                cn.Close();
            }
        }
        public int MODIFICA_BODEGA_USER(int P_ID_BODEGA, int P_ID_USUARIO, int P_SW_ASIGNADO, int P_ID_RESPONSABLE, out string p_msg1, out string p_msg2)
        {
            //P_OPC 0 - TODO 1 -ACTIVO
            string strQ = "SP_MOV_BODEGA";
            SqlCommand cmd = new SqlCommand(strQ, cn);
            DataSet ds = new DataSet();
            try
            {
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = strQ;
                cmd.Parameters.Clear();
                SqlParameter p_opcion_P = new SqlParameter("@P_OPCION", SqlDbType.Int);
                p_opcion_P.Value = 1;
                p_opcion_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(p_opcion_P);

                SqlParameter P_ID_SUPERVISA_p = new SqlParameter("@P_ID_BODEGA", SqlDbType.Int);
                P_ID_SUPERVISA_p.Value = P_ID_BODEGA;
                P_ID_SUPERVISA_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_ID_SUPERVISA_p);

                SqlParameter P_ID_USUARIO_p = new SqlParameter("@P_ID_USUARIO", SqlDbType.Int);
                P_ID_USUARIO_p.Value = P_ID_USUARIO;
                P_ID_USUARIO_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_ID_USUARIO_p);

                SqlParameter P_SW_ASIGNADO_p = new SqlParameter("@P_SW_ASIGNADO", SqlDbType.Int);
                P_SW_ASIGNADO_p.Value = P_SW_ASIGNADO;
                P_SW_ASIGNADO_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_SW_ASIGNADO_p);

                SqlParameter P_ID_RESPONSABLE_p = new SqlParameter("@P_ID_RESPONSABLE", SqlDbType.Int);
                P_ID_RESPONSABLE_p.Value = P_ID_RESPONSABLE;
                P_ID_RESPONSABLE_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_ID_RESPONSABLE_p);

                SqlParameter CodRes_P = new SqlParameter("@P_RESPUESTA", SqlDbType.Int);
                CodRes_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(CodRes_P);

                SqlParameter msgErr1_P = new SqlParameter("@P_MSG1", SqlDbType.NVarChar);
                msgErr1_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgErr1_P);

                SqlParameter msgErr2_P = new SqlParameter("@P_MSG2", SqlDbType.NVarChar);
                msgErr2_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgErr2_P);

                cmd.Parameters["@P_MSG1"].Size = 100;
                cmd.Parameters["@P_MSG2"].Size = 100;


                cmd.ExecuteNonQuery();
                p_msg1 = msgErr1_P.Value.ToString();
                p_msg2 = msgErr2_P.Value.ToString();
                return Convert.ToInt32(CodRes_P.Value);
            }
            catch (Exception e)
            {
                String s = e.Message.ToString();
                p_msg1 = e.Message.ToString();
                p_msg2 = "-";
                return -1;
            }
            finally
            {
                cn.Close();
            }
        }

        public int VALIDA_USUARIO(string P_USUARIO, string P_PASSWORD, out string p_msg1, out string p_msg2)
        {
            string strQ = "SP_MOV_USUARIOS";
            SqlCommand cmd = new SqlCommand(strQ, cn);
            DataSet ds = new DataSet();
            try
            {
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                //P_OPC 0 - TODO 1 -ACTIVO

                cmd.CommandText = strQ;
                cmd.Parameters.Clear();


                SqlParameter P_OPCION_P = new SqlParameter("@P_OPCION", SqlDbType.Int);
                P_OPCION_P.Value = 6;
                P_OPCION_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_OPCION_P);

                SqlParameter P_IDUSUARIO_P = new SqlParameter("@P_IDUSUARIO", SqlDbType.Int);
                P_IDUSUARIO_P.Value = 0;
                P_IDUSUARIO_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_IDUSUARIO_P);

                SqlParameter P_USUARIO_P = new SqlParameter("@P_USUARIO", SqlDbType.VarChar);
                P_USUARIO_P.Value = P_USUARIO;
                P_USUARIO_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_USUARIO_P);

                SqlParameter P_NOMBRES_P = new SqlParameter("@P_NOMBRES", SqlDbType.VarChar);
                P_NOMBRES_P.Value = "";
                P_NOMBRES_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_NOMBRES_P);

                SqlParameter P_APELLIDOS_P = new SqlParameter("@P_APELLIDOS", SqlDbType.VarChar);
                P_APELLIDOS_P.Value = "";
                P_APELLIDOS_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_APELLIDOS_P);

                SqlParameter P_CORREO_P = new SqlParameter("@P_CORREO", SqlDbType.VarChar);
                P_CORREO_P.Value = "";
                P_CORREO_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_CORREO_P);

                SqlParameter P_BOOL_SUPERVISOR_P = new SqlParameter("@P_BOOL_SUPERVISOR", SqlDbType.Int);
                P_BOOL_SUPERVISOR_P.Value = 0;
                P_BOOL_SUPERVISOR_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_BOOL_SUPERVISOR_P);

                SqlParameter P_ESTADO_P = new SqlParameter("@P_ESTADO", SqlDbType.Int);
                P_ESTADO_P.Value = 0;
                P_ESTADO_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_ESTADO_P);

                SqlParameter P_PASSWORD_P = new SqlParameter("@P_PASSWORD", SqlDbType.VarChar);
                P_PASSWORD_P.Value = P_PASSWORD;
                P_PASSWORD_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_PASSWORD_P);

                SqlParameter P_ID_RESPONSABLE_P = new SqlParameter("@P_ID_RESPONSABLE", SqlDbType.Int);
                P_ID_RESPONSABLE_P.Value = 0;
                P_ID_RESPONSABLE_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_ID_RESPONSABLE_P);

                SqlParameter P_NUM_SERIE_EMP_P = new SqlParameter("@P_NUM_SERIE_EMP", SqlDbType.VarChar);
                P_NUM_SERIE_EMP_P.Value = "";
                P_NUM_SERIE_EMP_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_NUM_SERIE_EMP_P);

                SqlParameter P_NUM_SERIE_REP_P = new SqlParameter("@P_NUM_SERIE_REP", SqlDbType.VarChar);
                P_NUM_SERIE_REP_P.Value = "";
                P_NUM_SERIE_REP_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_NUM_SERIE_REP_P);

                SqlParameter P_NUM_SERIE_SERSTOCK_P = new SqlParameter("@P_NUM_SERIE_TSTOCK", SqlDbType.VarChar);
                P_NUM_SERIE_SERSTOCK_P.Value = "";
                P_NUM_SERIE_SERSTOCK_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_NUM_SERIE_SERSTOCK_P);

                SqlParameter P_ID_TIPOUSER_P = new SqlParameter("@P_ID_TIPOUSER", SqlDbType.VarChar);
                P_ID_TIPOUSER_P.Value = "";
                P_ID_TIPOUSER_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_ID_TIPOUSER_P);

                SqlParameter P_ID_BODEGA_P = new SqlParameter("@P_ID_BODEGA", SqlDbType.Int);
                P_ID_BODEGA_P.Value = 0;
                P_ID_BODEGA_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_ID_BODEGA_P);

                SqlParameter P_SAP_USER_P = new SqlParameter("@P_SAP_USER", SqlDbType.VarChar);
                P_SAP_USER_P.Value = "";
                P_SAP_USER_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_SAP_USER_P);

                SqlParameter CodRes_P = new SqlParameter("P_RESPUESTA", SqlDbType.Int);
                CodRes_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(CodRes_P);

                SqlParameter msgErr1_P = new SqlParameter("P_MSG1", SqlDbType.NVarChar);
                msgErr1_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgErr1_P);

                SqlParameter msgErr2_P = new SqlParameter("P_MSG2", SqlDbType.NVarChar);
                msgErr2_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgErr2_P);

                cmd.Parameters["P_MSG1"].Size = 100;
                cmd.Parameters["P_MSG2"].Size = 100;


                cmd.ExecuteNonQuery();
                p_msg1 = msgErr1_P.Value.ToString();
                p_msg2 = msgErr2_P.Value.ToString();
                return Convert.ToInt32(CodRes_P.Value);
            }
            catch (Exception e)
            {
                String s = e.Message.ToString();
                p_msg1 = e.Message.ToString();
                p_msg2 = "-";
                return -1;
            }
            finally
            {
                cn.Close();
            }
        }

        #endregion

        #region CONSULTA_DATA_USUARIO
        public DataSet CONSULTA_USERS()
        {
            //P_OPC 0 - TODO 1 -ACTIVO
            string strQ = "SP_MOV_USUARIOS";
            SqlCommand cmd = new SqlCommand(strQ, cn);
            DataSet ds = new DataSet();
            try
            {
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = strQ;
                cmd.Parameters.Clear();


                SqlParameter P_OPCION_P = new SqlParameter("@P_OPCION", SqlDbType.Int);
                P_OPCION_P.Value = 2;
                P_OPCION_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_OPCION_P);

                SqlParameter P_IDUSUARIO_P = new SqlParameter("@P_IDUSUARIO", SqlDbType.Int);
                P_IDUSUARIO_P.Value = 0;
                P_IDUSUARIO_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_IDUSUARIO_P);

                SqlParameter P_USUARIO_P = new SqlParameter("@P_USUARIO", SqlDbType.VarChar);
                P_USUARIO_P.Value = "";
                P_USUARIO_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_USUARIO_P);

                SqlParameter P_NOMBRES_P = new SqlParameter("@P_NOMBRES", SqlDbType.VarChar);
                P_NOMBRES_P.Value = "";
                P_NOMBRES_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_NOMBRES_P);

                SqlParameter P_APELLIDOS_P = new SqlParameter("@P_APELLIDOS", SqlDbType.VarChar);
                P_APELLIDOS_P.Value = "";
                P_APELLIDOS_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_APELLIDOS_P);

                SqlParameter P_CORREO_P = new SqlParameter("@P_CORREO", SqlDbType.VarChar);
                P_CORREO_P.Value = "";
                P_CORREO_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_CORREO_P);

                SqlParameter P_BOOL_SUPERVISOR_P = new SqlParameter("@P_BOOL_SUPERVISOR", SqlDbType.Int);
                P_BOOL_SUPERVISOR_P.Value = 0;
                P_BOOL_SUPERVISOR_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_BOOL_SUPERVISOR_P);

                SqlParameter P_ESTADO_P = new SqlParameter("@P_ESTADO", SqlDbType.Int);
                P_ESTADO_P.Value = 0;
                P_ESTADO_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_ESTADO_P);

                SqlParameter P_PASSWORD_P = new SqlParameter("@P_PASSWORD", SqlDbType.VarChar);
                P_PASSWORD_P.Value = " ";
                P_PASSWORD_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_PASSWORD_P);

                SqlParameter P_ID_RESPONSABLE_P = new SqlParameter("@P_ID_RESPONSABLE", SqlDbType.Int);
                P_ID_RESPONSABLE_P.Value = 0;
                P_ID_RESPONSABLE_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_ID_RESPONSABLE_P);

                SqlParameter P_NUM_SERIE_EMP_P = new SqlParameter("@P_NUM_SERIE_EMP", SqlDbType.VarChar);
                P_NUM_SERIE_EMP_P.Value = " ";
                P_NUM_SERIE_EMP_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_NUM_SERIE_EMP_P);

                SqlParameter P_NUM_SERIE_REP_P = new SqlParameter("@P_NUM_SERIE_REP", SqlDbType.VarChar);
                P_NUM_SERIE_REP_P.Value = " ";
                P_NUM_SERIE_REP_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_NUM_SERIE_REP_P);

                SqlParameter P_NUM_SERIE_SERSTOCK_P = new SqlParameter("@P_NUM_SERIE_TSTOCK", SqlDbType.VarChar);
                P_NUM_SERIE_SERSTOCK_P.Value = "";
                P_NUM_SERIE_SERSTOCK_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_NUM_SERIE_SERSTOCK_P);

                SqlParameter P_ID_TIPOUSER_P = new SqlParameter("@P_ID_TIPOUSER", SqlDbType.VarChar);
                P_ID_TIPOUSER_P.Value = 0;
                P_ID_TIPOUSER_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_ID_TIPOUSER_P);

                SqlParameter P_ID_BODEGA_P = new SqlParameter("@P_ID_BODEGA", SqlDbType.Int);
                P_ID_BODEGA_P.Value = 0;
                P_ID_BODEGA_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_ID_BODEGA_P);

                SqlParameter P_SAP_USER_P = new SqlParameter("@P_SAP_USER", SqlDbType.VarChar);
                P_SAP_USER_P.Value = "";
                P_SAP_USER_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_SAP_USER_P);


                SqlParameter CodRes_P = new SqlParameter("P_RESPUESTA", SqlDbType.Int);
                CodRes_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(CodRes_P);

                SqlParameter msgErr1_P = new SqlParameter("P_MSG1", SqlDbType.NVarChar);
                msgErr1_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgErr1_P);

                SqlParameter msgErr2_P = new SqlParameter("P_MSG2", SqlDbType.NVarChar);
                msgErr2_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgErr2_P);

                cmd.Parameters["P_MSG1"].Size = 100;
                cmd.Parameters["P_MSG2"].Size = 100;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception e)
            {
                String s = e.Message.ToString();
                return ds;
            }
            finally
            {
                cn.Close();
            }
            return ds;
        }

        public DataSet CONSULTA_USUARIO(int p_usuario)
        {
            //P_OPC 0 - TODO 1 -ACTIVO
            string strQ = "SP_MOV_USUARIOS";
            SqlCommand cmd = new SqlCommand(strQ, cn);
            DataSet ds = new DataSet();
            try
            {
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = strQ;
                cmd.Parameters.Clear();


                SqlParameter P_OPCION_P = new SqlParameter("@P_OPCION", SqlDbType.Int);
                P_OPCION_P.Value = 7;
                P_OPCION_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_OPCION_P);

                SqlParameter P_IDUSUARIO_P = new SqlParameter("@P_IDUSUARIO", SqlDbType.Int);
                P_IDUSUARIO_P.Value = p_usuario;
                P_IDUSUARIO_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_IDUSUARIO_P);

                SqlParameter P_USUARIO_P = new SqlParameter("@P_USUARIO", SqlDbType.VarChar);
                P_USUARIO_P.Value = "";
                P_USUARIO_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_USUARIO_P);

                SqlParameter P_NOMBRES_P = new SqlParameter("@P_NOMBRES", SqlDbType.VarChar);
                P_NOMBRES_P.Value = "";
                P_NOMBRES_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_NOMBRES_P);

                SqlParameter P_APELLIDOS_P = new SqlParameter("@P_APELLIDOS", SqlDbType.VarChar);
                P_APELLIDOS_P.Value = "";
                P_APELLIDOS_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_APELLIDOS_P);

                SqlParameter P_CORREO_P = new SqlParameter("@P_CORREO", SqlDbType.VarChar);
                P_CORREO_P.Value = "";
                P_CORREO_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_CORREO_P);

                SqlParameter P_BOOL_SUPERVISOR_P = new SqlParameter("@P_BOOL_SUPERVISOR", SqlDbType.Int);
                P_BOOL_SUPERVISOR_P.Value = 0;
                P_BOOL_SUPERVISOR_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_BOOL_SUPERVISOR_P);

                SqlParameter P_ESTADO_P = new SqlParameter("@P_ESTADO", SqlDbType.Int);
                P_ESTADO_P.Value = 0;
                P_ESTADO_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_ESTADO_P);

                SqlParameter P_PASSWORD_P = new SqlParameter("@P_PASSWORD", SqlDbType.VarChar);
                P_PASSWORD_P.Value = " ";
                P_PASSWORD_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_PASSWORD_P);

                SqlParameter P_ID_RESPONSABLE_P = new SqlParameter("@P_ID_RESPONSABLE", SqlDbType.Int);
                P_ID_RESPONSABLE_P.Value = 0;
                P_ID_RESPONSABLE_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_ID_RESPONSABLE_P);

                SqlParameter P_NUM_SERIE_EMP_P = new SqlParameter("@P_NUM_SERIE_EMP", SqlDbType.VarChar);
                P_NUM_SERIE_EMP_P.Value = " ";
                P_NUM_SERIE_EMP_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_NUM_SERIE_EMP_P);

                SqlParameter P_NUM_SERIE_REP_P = new SqlParameter("@P_NUM_SERIE_REP", SqlDbType.VarChar);
                P_NUM_SERIE_REP_P.Value = " ";
                P_NUM_SERIE_REP_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_NUM_SERIE_REP_P);

                SqlParameter P_NUM_SERIE_SERSTOCK_P = new SqlParameter("@P_NUM_SERIE_TSTOCK", SqlDbType.VarChar);
                P_NUM_SERIE_SERSTOCK_P.Value = "";
                P_NUM_SERIE_SERSTOCK_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_NUM_SERIE_SERSTOCK_P);

                SqlParameter P_ID_TIPOUSER_P = new SqlParameter("@P_ID_TIPOUSER", SqlDbType.VarChar);
                P_ID_TIPOUSER_P.Value = 0;
                P_ID_TIPOUSER_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_ID_TIPOUSER_P);

                SqlParameter P_ID_BODEGA_P = new SqlParameter("@P_ID_BODEGA", SqlDbType.Int);
                P_ID_BODEGA_P.Value = 0;
                P_ID_BODEGA_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_ID_BODEGA_P);

                SqlParameter P_SAP_USER_P = new SqlParameter("@P_SAP_USER", SqlDbType.VarChar);
                P_SAP_USER_P.Value = "";
                P_SAP_USER_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_SAP_USER_P);

                SqlParameter CodRes_P = new SqlParameter("P_RESPUESTA", SqlDbType.Int);
                CodRes_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(CodRes_P);

                SqlParameter msgErr1_P = new SqlParameter("P_MSG1", SqlDbType.NVarChar);
                msgErr1_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgErr1_P);

                SqlParameter msgErr2_P = new SqlParameter("P_MSG2", SqlDbType.NVarChar);
                msgErr2_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgErr2_P);

                cmd.Parameters["P_MSG1"].Size = 100;
                cmd.Parameters["P_MSG2"].Size = 100;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception e)
            {
                String s = e.Message.ToString();
                return ds;
            }
            finally
            {
                cn.Close();
            }
            return ds;
        }

        public DataSet CONSULTA_NOMBRE_USUARIO(string p_usuario)
        {
            //P_OPC 0 - TODO 1 -ACTIVO
            string strQ = "SP_MOV_USUARIOS";
            SqlCommand cmd = new SqlCommand(strQ, cn);
            DataSet ds = new DataSet();
            try
            {
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = strQ;
                cmd.Parameters.Clear();


                SqlParameter P_OPCION_P = new SqlParameter("@P_OPCION", SqlDbType.Int);
                P_OPCION_P.Value = 9;
                P_OPCION_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_OPCION_P);

                SqlParameter P_IDUSUARIO_P = new SqlParameter("@P_IDUSUARIO", SqlDbType.Int);
                P_IDUSUARIO_P.Value = 0;
                P_IDUSUARIO_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_IDUSUARIO_P);

                SqlParameter P_USUARIO_P = new SqlParameter("@P_USUARIO", SqlDbType.VarChar);
                P_USUARIO_P.Value = p_usuario;
                P_USUARIO_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_USUARIO_P);

                SqlParameter P_NOMBRES_P = new SqlParameter("@P_NOMBRES", SqlDbType.VarChar);
                P_NOMBRES_P.Value = "";
                P_NOMBRES_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_NOMBRES_P);

                SqlParameter P_APELLIDOS_P = new SqlParameter("@P_APELLIDOS", SqlDbType.VarChar);
                P_APELLIDOS_P.Value = "";
                P_APELLIDOS_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_APELLIDOS_P);

                SqlParameter P_CORREO_P = new SqlParameter("@P_CORREO", SqlDbType.VarChar);
                P_CORREO_P.Value = "";
                P_CORREO_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_CORREO_P);

                SqlParameter P_BOOL_SUPERVISOR_P = new SqlParameter("@P_BOOL_SUPERVISOR", SqlDbType.Int);
                P_BOOL_SUPERVISOR_P.Value = 0;
                P_BOOL_SUPERVISOR_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_BOOL_SUPERVISOR_P);

                SqlParameter P_ESTADO_P = new SqlParameter("@P_ESTADO", SqlDbType.Int);
                P_ESTADO_P.Value = 0;
                P_ESTADO_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_ESTADO_P);

                SqlParameter P_PASSWORD_P = new SqlParameter("@P_PASSWORD", SqlDbType.VarChar);
                P_PASSWORD_P.Value = " ";
                P_PASSWORD_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_PASSWORD_P);

                SqlParameter P_ID_RESPONSABLE_P = new SqlParameter("@P_ID_RESPONSABLE", SqlDbType.Int);
                P_ID_RESPONSABLE_P.Value = 0;
                P_ID_RESPONSABLE_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_ID_RESPONSABLE_P);

                SqlParameter P_NUM_SERIE_EMP_P = new SqlParameter("@P_NUM_SERIE_EMP", SqlDbType.VarChar);
                P_NUM_SERIE_EMP_P.Value = " ";
                P_NUM_SERIE_EMP_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_NUM_SERIE_EMP_P);

                SqlParameter P_NUM_SERIE_REP_P = new SqlParameter("@P_NUM_SERIE_REP", SqlDbType.VarChar);
                P_NUM_SERIE_REP_P.Value = " ";
                P_NUM_SERIE_REP_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_NUM_SERIE_REP_P);

                SqlParameter P_NUM_SERIE_SERSTOCK_P = new SqlParameter("@P_NUM_SERIE_TSTOCK", SqlDbType.VarChar);
                P_NUM_SERIE_SERSTOCK_P.Value = "";
                P_NUM_SERIE_SERSTOCK_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_NUM_SERIE_SERSTOCK_P);

                SqlParameter P_ID_TIPOUSER_P = new SqlParameter("@P_ID_TIPOUSER", SqlDbType.VarChar);
                P_ID_TIPOUSER_P.Value = 0;
                P_ID_TIPOUSER_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_ID_TIPOUSER_P);

                SqlParameter P_ID_BODEGA_P = new SqlParameter("@P_ID_BODEGA", SqlDbType.Int);
                P_ID_BODEGA_P.Value = 0;
                P_ID_BODEGA_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_ID_BODEGA_P);

                SqlParameter P_SAP_USER_P = new SqlParameter("@P_SAP_USER", SqlDbType.VarChar);
                P_SAP_USER_P.Value = "";
                P_SAP_USER_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_SAP_USER_P);

                SqlParameter CodRes_P = new SqlParameter("P_RESPUESTA", SqlDbType.Int);
                CodRes_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(CodRes_P);

                SqlParameter msgErr1_P = new SqlParameter("P_MSG1", SqlDbType.NVarChar);
                msgErr1_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgErr1_P);

                SqlParameter msgErr2_P = new SqlParameter("P_MSG2", SqlDbType.NVarChar);
                msgErr2_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgErr2_P);

                cmd.Parameters["P_MSG1"].Size = 100;
                cmd.Parameters["P_MSG2"].Size = 100;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception e)
            {
                String s = e.Message.ToString();
                return ds;
            }
            finally
            {
                cn.Close();
            }
            return ds;
        }
        public DataSet CONSULTA_PERFIL_USERS(int p_usuario)
        {
            //P_OPC 0 - TODO 1 -ACTIVO
            string strQ = "SP_MOV_USUARIOS";
            SqlCommand cmd = new SqlCommand(strQ, cn);
            DataSet ds = new DataSet();
            try
            {
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = strQ;
                cmd.Parameters.Clear();


                SqlParameter P_OPCION_P = new SqlParameter("@P_OPCION", SqlDbType.Int);
                P_OPCION_P.Value = 5;
                P_OPCION_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_OPCION_P);

                SqlParameter P_IDUSUARIO_P = new SqlParameter("@P_IDUSUARIO", SqlDbType.Int);
                P_IDUSUARIO_P.Value = p_usuario;
                P_IDUSUARIO_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_IDUSUARIO_P);

                SqlParameter P_USUARIO_P = new SqlParameter("@P_USUARIO", SqlDbType.VarChar);
                P_USUARIO_P.Value = "";
                P_USUARIO_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_USUARIO_P);

                SqlParameter P_NOMBRES_P = new SqlParameter("@P_NOMBRES", SqlDbType.VarChar);
                P_NOMBRES_P.Value = "";
                P_NOMBRES_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_NOMBRES_P);

                SqlParameter P_APELLIDOS_P = new SqlParameter("@P_APELLIDOS", SqlDbType.VarChar);
                P_APELLIDOS_P.Value = "";
                P_APELLIDOS_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_APELLIDOS_P);

                SqlParameter P_CORREO_P = new SqlParameter("@P_CORREO", SqlDbType.VarChar);
                P_CORREO_P.Value = "";
                P_CORREO_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_CORREO_P);

                SqlParameter P_BOOL_SUPERVISOR_P = new SqlParameter("@P_BOOL_SUPERVISOR", SqlDbType.Int);
                P_BOOL_SUPERVISOR_P.Value = 0;
                P_BOOL_SUPERVISOR_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_BOOL_SUPERVISOR_P);

                SqlParameter P_ESTADO_P = new SqlParameter("@P_ESTADO", SqlDbType.Int);
                P_ESTADO_P.Value = 0;
                P_ESTADO_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_ESTADO_P);

                SqlParameter P_PASSWORD_P = new SqlParameter("@P_PASSWORD", SqlDbType.VarChar);
                P_PASSWORD_P.Value = " ";
                P_PASSWORD_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_PASSWORD_P);

                SqlParameter P_ID_RESPONSABLE_P = new SqlParameter("@P_ID_RESPONSABLE", SqlDbType.Int);
                P_ID_RESPONSABLE_P.Value = 0;
                P_ID_RESPONSABLE_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_ID_RESPONSABLE_P);

                SqlParameter P_NUM_SERIE_EMP_P = new SqlParameter("@P_NUM_SERIE_EMP", SqlDbType.VarChar);
                P_NUM_SERIE_EMP_P.Value = " ";
                P_NUM_SERIE_EMP_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_NUM_SERIE_EMP_P);

                SqlParameter P_NUM_SERIE_REP_P = new SqlParameter("@P_NUM_SERIE_REP", SqlDbType.VarChar);
                P_NUM_SERIE_REP_P.Value = " ";
                P_NUM_SERIE_REP_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_NUM_SERIE_REP_P);

                SqlParameter P_NUM_SERIE_SERSTOCK_P = new SqlParameter("@P_NUM_SERIE_TSTOCK", SqlDbType.VarChar);
                P_NUM_SERIE_SERSTOCK_P.Value = "";
                P_NUM_SERIE_SERSTOCK_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_NUM_SERIE_SERSTOCK_P);

                SqlParameter P_ID_TIPOUSER_P = new SqlParameter("@P_ID_TIPOUSER", SqlDbType.VarChar);
                P_ID_TIPOUSER_P.Value = 0;
                P_ID_TIPOUSER_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_ID_TIPOUSER_P);

                SqlParameter P_ID_BODEGA_P = new SqlParameter("@P_ID_BODEGA", SqlDbType.Int);
                P_ID_BODEGA_P.Value = 0;
                P_ID_BODEGA_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_ID_BODEGA_P);

                SqlParameter P_SAP_USER_P = new SqlParameter("@P_SAP_USER", SqlDbType.VarChar);
                P_SAP_USER_P.Value = "";
                P_SAP_USER_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_SAP_USER_P);



                SqlParameter CodRes_P = new SqlParameter("P_RESPUESTA", SqlDbType.Int);
                CodRes_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(CodRes_P);

                SqlParameter msgErr1_P = new SqlParameter("P_MSG1", SqlDbType.NVarChar);
                msgErr1_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgErr1_P);

                SqlParameter msgErr2_P = new SqlParameter("P_MSG2", SqlDbType.NVarChar);
                msgErr2_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgErr2_P);

                cmd.Parameters["P_MSG1"].Size = 100;
                cmd.Parameters["P_MSG2"].Size = 100;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception e)
            {
                String s = e.Message.ToString();
                return ds;
            }
            finally
            {
                cn.Close();
            }
            return ds;
        }

        public DataSet CONSULTA_SUPERVISA_USUARIOS(int P_ID_SUPERVISA)
        {
            string strQ = "SP_MOV_SUPERVISA";
            SqlCommand cmd = new SqlCommand(strQ, cn);
            DataSet ds = new DataSet();
            try
            {
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = strQ;
                cmd.Parameters.Clear();
                SqlParameter p_opcion_P = new SqlParameter("@P_OPCION", SqlDbType.Int);
                p_opcion_P.Value = 2;
                p_opcion_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(p_opcion_P);

                SqlParameter P_ID_SUPERVISA_p = new SqlParameter("@P_ID_SUPERVISA", SqlDbType.Int);
                P_ID_SUPERVISA_p.Value = P_ID_SUPERVISA;
                P_ID_SUPERVISA_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_ID_SUPERVISA_p);

                SqlParameter P_ID_USUARIO_p = new SqlParameter("@P_ID_USUARIO", SqlDbType.Int);
                P_ID_USUARIO_p.Value = 1;
                P_ID_USUARIO_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_ID_USUARIO_p);

                SqlParameter P_SW_ASIGNADO_p = new SqlParameter("@P_SW_ASIGNADO", SqlDbType.Int);
                P_SW_ASIGNADO_p.Value = 1;
                P_SW_ASIGNADO_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_SW_ASIGNADO_p);

                SqlParameter P_ID_RESPONSABLE_p = new SqlParameter("@P_ID_RESPONSABLE", SqlDbType.Int);
                P_ID_RESPONSABLE_p.Value = 1;
                P_ID_RESPONSABLE_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_ID_RESPONSABLE_p);

                SqlParameter CodRes_P = new SqlParameter("@P_RESPUESTA", SqlDbType.Int);
                CodRes_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(CodRes_P);

                SqlParameter msgErr1_P = new SqlParameter("@P_MSG1", SqlDbType.NVarChar);
                msgErr1_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgErr1_P);

                SqlParameter msgErr2_P = new SqlParameter("@P_MSG2", SqlDbType.NVarChar);
                msgErr2_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgErr2_P);

                cmd.Parameters["@P_MSG1"].Size = 100;
                cmd.Parameters["@P_MSG2"].Size = 100;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception e)
            {
                String s = e.Message.ToString();
                return ds;
            }
            finally
            {
                cn.Close();
            }
            return ds;
        }

        public DataSet CONSULTA_SUPERVISA_USER(int P_ID_USUARIO)
        {
            string strQ = "SP_MOV_SUPERVISA";
            SqlCommand cmd = new SqlCommand(strQ, cn);
            DataSet ds = new DataSet();
            try
            {
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = strQ;
                cmd.Parameters.Clear();
                SqlParameter p_opcion_P = new SqlParameter("@P_OPCION", SqlDbType.Int);
                p_opcion_P.Value = 4;
                p_opcion_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(p_opcion_P);

                SqlParameter P_ID_SUPERVISA_p = new SqlParameter("@P_ID_SUPERVISA", SqlDbType.Int);
                P_ID_SUPERVISA_p.Value = 0;
                P_ID_SUPERVISA_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_ID_SUPERVISA_p);

                SqlParameter P_ID_USUARIO_p = new SqlParameter("@P_ID_USUARIO", SqlDbType.Int);
                P_ID_USUARIO_p.Value = P_ID_USUARIO;
                P_ID_USUARIO_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_ID_USUARIO_p);

                SqlParameter P_SW_ASIGNADO_p = new SqlParameter("@P_SW_ASIGNADO", SqlDbType.Int);
                P_SW_ASIGNADO_p.Value = 0;
                P_SW_ASIGNADO_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_SW_ASIGNADO_p);

                SqlParameter P_ID_RESPONSABLE_p = new SqlParameter("@P_ID_RESPONSABLE", SqlDbType.Int);
                P_ID_RESPONSABLE_p.Value = 0;
                P_ID_RESPONSABLE_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_ID_RESPONSABLE_p);

                SqlParameter CodRes_P = new SqlParameter("@P_RESPUESTA", SqlDbType.Int);
                CodRes_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(CodRes_P);

                SqlParameter msgErr1_P = new SqlParameter("@P_MSG1", SqlDbType.NVarChar);
                msgErr1_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgErr1_P);

                SqlParameter msgErr2_P = new SqlParameter("@P_MSG2", SqlDbType.NVarChar);
                msgErr2_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgErr2_P);

                cmd.Parameters["@P_MSG1"].Size = 100;
                cmd.Parameters["@P_MSG2"].Size = 100;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception e)
            {
                String s = e.Message.ToString();
                return ds;
            }
            finally
            {
                cn.Close();
            }
            return ds;
        }


        public DataSet CONSULTA_BODEGA_USUARIOS(int P_ID_USUARIO)
        {
            string strQ = "SP_MOV_BODEGA";
            SqlCommand cmd = new SqlCommand(strQ, cn);
            DataSet ds = new DataSet();
            try
            {
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = strQ;
                cmd.Parameters.Clear();
                SqlParameter p_opcion_P = new SqlParameter("@P_OPCION", SqlDbType.Int);
                p_opcion_P.Value = 2;
                p_opcion_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(p_opcion_P);

                SqlParameter P_ID_BODEGA_p = new SqlParameter("@P_ID_BODEGA", SqlDbType.Int);
                P_ID_BODEGA_p.Value = 0;
                P_ID_BODEGA_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_ID_BODEGA_p);

                SqlParameter P_ID_USUARIO_p = new SqlParameter("@P_ID_USUARIO", SqlDbType.Int);
                P_ID_USUARIO_p.Value = P_ID_USUARIO;
                P_ID_USUARIO_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_ID_USUARIO_p);

                SqlParameter P_SW_ASIGNADO_p = new SqlParameter("@P_SW_ASIGNADO", SqlDbType.Int);
                P_SW_ASIGNADO_p.Value = 1;
                P_SW_ASIGNADO_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_SW_ASIGNADO_p);

                SqlParameter P_ID_RESPONSABLE_p = new SqlParameter("@P_ID_RESPONSABLE", SqlDbType.Int);
                P_ID_RESPONSABLE_p.Value = 1;
                P_ID_RESPONSABLE_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_ID_RESPONSABLE_p);

                SqlParameter CodRes_P = new SqlParameter("@P_RESPUESTA", SqlDbType.Int);
                CodRes_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(CodRes_P);

                SqlParameter msgErr1_P = new SqlParameter("@P_MSG1", SqlDbType.NVarChar);
                msgErr1_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgErr1_P);

                SqlParameter msgErr2_P = new SqlParameter("@P_MSG2", SqlDbType.NVarChar);
                msgErr2_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgErr2_P);

                cmd.Parameters["@P_MSG1"].Size = 100;
                cmd.Parameters["@P_MSG2"].Size = 100;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception e)
            {
                String s = e.Message.ToString();
                return ds;
            }
            finally
            {
                cn.Close();
            }
            return ds;
        }

        public DataSet CONSULTA_BODEGA_X_USUARIOS(int P_ID_USUARIO)
        {
            string strQ = "SP_MOV_BODEGA";
            SqlCommand cmd = new SqlCommand(strQ, cn);
            DataSet ds = new DataSet();
            try
            {
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = strQ;
                cmd.Parameters.Clear();
                SqlParameter p_opcion_P = new SqlParameter("@P_OPCION", SqlDbType.Int);
                p_opcion_P.Value = 4;
                p_opcion_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(p_opcion_P);

                SqlParameter P_ID_BODEGA_p = new SqlParameter("@P_ID_BODEGA", SqlDbType.Int);
                P_ID_BODEGA_p.Value = 0;
                P_ID_BODEGA_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_ID_BODEGA_p);

                SqlParameter P_ID_USUARIO_p = new SqlParameter("@P_ID_USUARIO", SqlDbType.Int);
                P_ID_USUARIO_p.Value = P_ID_USUARIO;
                P_ID_USUARIO_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_ID_USUARIO_p);

                SqlParameter P_SW_ASIGNADO_p = new SqlParameter("@P_SW_ASIGNADO", SqlDbType.Int);
                P_SW_ASIGNADO_p.Value = 1;
                P_SW_ASIGNADO_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_SW_ASIGNADO_p);

                SqlParameter P_ID_RESPONSABLE_p = new SqlParameter("@P_ID_RESPONSABLE", SqlDbType.Int);
                P_ID_RESPONSABLE_p.Value = 1;
                P_ID_RESPONSABLE_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_ID_RESPONSABLE_p);

                SqlParameter CodRes_P = new SqlParameter("@P_RESPUESTA", SqlDbType.Int);
                CodRes_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(CodRes_P);

                SqlParameter msgErr1_P = new SqlParameter("@P_MSG1", SqlDbType.NVarChar);
                msgErr1_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgErr1_P);

                SqlParameter msgErr2_P = new SqlParameter("@P_MSG2", SqlDbType.NVarChar);
                msgErr2_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgErr2_P);

                cmd.Parameters["@P_MSG1"].Size = 100;
                cmd.Parameters["@P_MSG2"].Size = 100;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception e)
            {
                String s = e.Message.ToString();
                return ds;
            }
            finally
            {
                cn.Close();
            }
            return ds;
        }

        //CONSULTA CONCENTRACION DESDE SU FORMA FARMACEUTICA
        /*   public DataSet CONSULTA_CONCENTRACION_X_FF(int P_ID_FORMA_FARMACEUTICA)
           {
               string strQ = "SP_MOV_BODEGA";
               SqlCommand cmd = new SqlCommand(strQ, cn);
               DataSet ds = new DataSet();
               try
               {
                   cn.Open();
                   cmd.CommandType = CommandType.StoredProcedure;
                   cmd.CommandText = strQ;
                   cmd.Parameters.Clear();
                   SqlParameter p_opcion_P = new SqlParameter("@P_OPCION", SqlDbType.Int);
                   p_opcion_P.Value = 4;
                   p_opcion_P.Direction = ParameterDirection.Input;
                   cmd.Parameters.Add(p_opcion_P);

                   SqlParameter P_ID_BODEGA_p = new SqlParameter("@P_ID_BODEGA", SqlDbType.Int);
                   P_ID_BODEGA_p.Value = 0;
                   P_ID_BODEGA_p.Direction = ParameterDirection.Input;
                   cmd.Parameters.Add(P_ID_BODEGA_p);

                   SqlParameter P_ID_USUARIO_p = new SqlParameter("@P_ID_USUARIO", SqlDbType.Int);
                   P_ID_USUARIO_p.Value = P_ID_USUARIO;
                   P_ID_USUARIO_p.Direction = ParameterDirection.Input;
                   cmd.Parameters.Add(P_ID_USUARIO_p);

                   SqlParameter P_SW_ASIGNADO_p = new SqlParameter("@P_SW_ASIGNADO", SqlDbType.Int);
                   P_SW_ASIGNADO_p.Value = 1;
                   P_SW_ASIGNADO_p.Direction = ParameterDirection.Input;
                   cmd.Parameters.Add(P_SW_ASIGNADO_p);

                   SqlParameter P_ID_RESPONSABLE_p = new SqlParameter("@P_ID_RESPONSABLE", SqlDbType.Int);
                   P_ID_RESPONSABLE_p.Value = 1;
                   P_ID_RESPONSABLE_p.Direction = ParameterDirection.Input;
                   cmd.Parameters.Add(P_ID_RESPONSABLE_p);

                   SqlParameter CodRes_P = new SqlParameter("@P_RESPUESTA", SqlDbType.Int);
                   CodRes_P.Direction = ParameterDirection.Output;
                   cmd.Parameters.Add(CodRes_P);

                   SqlParameter msgErr1_P = new SqlParameter("@P_MSG1", SqlDbType.NVarChar);
                   msgErr1_P.Direction = ParameterDirection.Output;
                   cmd.Parameters.Add(msgErr1_P);

                   SqlParameter msgErr2_P = new SqlParameter("@P_MSG2", SqlDbType.NVarChar);
                   msgErr2_P.Direction = ParameterDirection.Output;
                   cmd.Parameters.Add(msgErr2_P);

                   cmd.Parameters["@P_MSG1"].Size = 100;
                   cmd.Parameters["@P_MSG2"].Size = 100;

                   SqlDataAdapter da = new SqlDataAdapter(cmd);
                   da.Fill(ds);
               }
               catch (Exception e)
               {
                   String s = e.Message.ToString();
                   return ds;
               }
               finally
               {
                   cn.Close();
               }
               return ds;
           }
           */
        public DataSet CONSULTA_ACCESO_X_USUARIOS(int P_ID_USUARIO)
        {
            string strQ = "SP_CONSULTA_ACCESO_USER";
            SqlCommand cmd = new SqlCommand(strQ, cn);
            DataSet ds = new DataSet();
            try
            {
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = strQ;
                cmd.Parameters.Clear();
                SqlParameter p_opcion_P = new SqlParameter("@P_OPC", SqlDbType.Int);
                p_opcion_P.Value = 1;
                p_opcion_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(p_opcion_P);

                SqlParameter P_ID_BODEGA_p = new SqlParameter("@P_IDPERMISO", SqlDbType.Int);
                P_ID_BODEGA_p.Value = 0;
                P_ID_BODEGA_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_ID_BODEGA_p);

                SqlParameter P_ID_USUARIO_p = new SqlParameter("@P_IDUSER", SqlDbType.Int);
                P_ID_USUARIO_p.Value = P_ID_USUARIO;
                P_ID_USUARIO_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_ID_USUARIO_p);



                SqlParameter CodRes_P = new SqlParameter("@P_RESPUESTA", SqlDbType.Int);
                CodRes_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(CodRes_P);

                SqlParameter msgErr1_P = new SqlParameter("@P_MSG1", SqlDbType.NVarChar);
                msgErr1_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgErr1_P);

                SqlParameter msgErr2_P = new SqlParameter("@P_MSG2", SqlDbType.NVarChar);
                msgErr2_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgErr2_P);

                cmd.Parameters["@P_MSG1"].Size = 100;
                cmd.Parameters["@P_MSG2"].Size = 100;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception e)
            {
                String s = e.Message.ToString();
                return ds;
            }
            finally
            {
                cn.Close();
            }
            return ds;
        }
        #endregion

        #region QUERYS_SAP
        public DataSet CONSULTA_ORDENES_SAP(int P_ID_USER, string P_ESTADO)
        {
            //P_OPC 0 - TODO 1 -ACTIVO
            string strQ = "SP_CONSULTA_ORDENES_PROD_SAP";
            SqlCommand cmd = new SqlCommand(strQ, cn);
            DataSet ds = new DataSet();
            try
            {
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = strQ;
                cmd.Parameters.Clear();

                SqlParameter P_IDUSER_p = new SqlParameter("@P_IDUSER", SqlDbType.Int);
                P_IDUSER_p.Value = P_ID_USER;
                P_IDUSER_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_IDUSER_p);

                SqlParameter P_OPC_p = new SqlParameter("@P_ESTADO", SqlDbType.VarChar);
                P_OPC_p.Value = P_ESTADO;
                P_OPC_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_OPC_p);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception e)
            {
                String s = e.Message.ToString();
                return ds;
            }
            finally
            {
                cn.Close();
            }
            return ds;
        }

        public DataSet CONSULTA_LISTAITEMSORDEN_SAP(int P_ID_USER, int P_DOCENTRY)
        {
            //P_OPC 0 - TODO 1 -ACTIVO
            string strQ = "SP_CONSULTA_SAP_LISTAITEMSORDEN";
            SqlCommand cmd = new SqlCommand(strQ, cn);
            DataSet ds = new DataSet();
            try
            {
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = strQ;
                cmd.Parameters.Clear();

                SqlParameter P_IDUSER_p = new SqlParameter("@P_IDUSER", SqlDbType.Int);
                P_IDUSER_p.Value = P_ID_USER;
                P_IDUSER_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_IDUSER_p);

                SqlParameter P_DOCENTRY_p = new SqlParameter("@P_DOCENTRY", SqlDbType.Int);
                P_DOCENTRY_p.Value = P_DOCENTRY;
                P_DOCENTRY_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_DOCENTRY_p);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception e)
            {
                String s = e.Message.ToString();
                return ds;
            }
            finally
            {
                cn.Close();
            }
            return ds;
        }

        public DataSet CONSULTA_SAP_LOTE_DEVOLUCION(int P_ITEMCODE, int P_BASEENTRY,string WHSCODE)
        {
            //P_OPC 0 - TODO 1 -ACTIVO
            string strQ = "SP_CONSULTA_SAP_LOTE_DEVOLUCION";
            SqlCommand cmd = new SqlCommand(strQ, cn);
            DataSet ds = new DataSet();
            try
            {
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = strQ;
                cmd.Parameters.Clear();

                SqlParameter P_ITEMCODE_p = new SqlParameter("@P_ITEMCODE", SqlDbType.Int);
                P_ITEMCODE_p.Value = P_ITEMCODE;
                P_ITEMCODE_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_ITEMCODE_p);

                SqlParameter P_BASEENTRY_p = new SqlParameter("@P_BASEENTRY", SqlDbType.Int);
                P_BASEENTRY_p.Value = P_BASEENTRY;
                P_BASEENTRY_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_BASEENTRY_p);

                SqlParameter WHSCODE_p = new SqlParameter("@P_WHSCODE", SqlDbType.VarChar);
                WHSCODE_p.Value = WHSCODE;
                WHSCODE_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(WHSCODE_p);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception e)
            {
                String s = e.Message.ToString();
                return ds;
            }
            finally
            {
                cn.Close();
            }
            return ds;
        }

        public DataSet CONSULTA_SAP_LOTE_EMISION(int P_ITEMCODE)
        {
            //P_OPC 0 - TODO 1 -ACTIVO
            string strQ = "SP_CONSULTA_SAP_LOTE_EMISION";
            SqlCommand cmd = new SqlCommand(strQ, cn);
            DataSet ds = new DataSet();
            try
            {
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = strQ;
                cmd.Parameters.Clear();

                SqlParameter P_ITEMCODE_p = new SqlParameter("@P_ITEMCODE", SqlDbType.Int);
                P_ITEMCODE_p.Value = P_ITEMCODE;
                P_ITEMCODE_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_ITEMCODE_p);
 

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception e)
            {
                String s = e.Message.ToString();
                return ds;
            }
            finally
            {
                cn.Close();
            }
            return ds;
        }

        public DataSet CONSULTA_SAP_SERIE(string P_OBJECTCODE)
        {
            //P_OPC 0 - TODO 1 -ACTIVO
            string strQ = "SP_CONSULTA_SAP_SERIE";
            SqlCommand cmd = new SqlCommand(strQ, cn);
            DataSet ds = new DataSet();
            try
            {
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = strQ;
                cmd.Parameters.Clear();

                SqlParameter P_OBJECTCODE_p = new SqlParameter("@P_OBJECTCODE", SqlDbType.VarChar);
                P_OBJECTCODE_p.Value = P_OBJECTCODE;
                P_OBJECTCODE_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_OBJECTCODE_p);


                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception e)
            {
                String s = e.Message.ToString();
                return ds;
            }
            finally
            {
                cn.Close();
            }
            return ds;
        }

        public DataSet CONSULTA_SAP_RECIBO_DOCNUM(string P_DOCNUM)
        {
            //P_OPC 0 - TODO 1 -ACTIVO
            string strQ = "SP_CONSULTA_SAP_RECIBO_DOCNUM";
            SqlCommand cmd = new SqlCommand(strQ, cn);
            DataSet ds = new DataSet();
            try
            {
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = strQ;
                cmd.Parameters.Clear();

                SqlParameter P_DOCNUM_p = new SqlParameter("@P_DOCNUM", SqlDbType.VarChar);
                P_DOCNUM_p.Value = P_DOCNUM;
                P_DOCNUM_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_DOCNUM_p);


                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception e)
            {
                String s = e.Message.ToString();
                return ds;
            }
            finally
            {
                cn.Close();
            }
            return ds;
        }

        public DataSet CONSULTA_SAP_EMISION_DOCNUM(string P_DOCNUM)
        {
            //P_OPC 0 - TODO 1 -ACTIVO
            string strQ = "SP_CONSULTA_SAP_EMISION_DOCNUM";
            SqlCommand cmd = new SqlCommand(strQ, cn);
            DataSet ds = new DataSet();
            try
            {
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = strQ;
                cmd.Parameters.Clear();

                SqlParameter P_DOCNUM_p = new SqlParameter("@P_DOCNUM", SqlDbType.VarChar);
                P_DOCNUM_p.Value = P_DOCNUM;
                P_DOCNUM_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_DOCNUM_p);


                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception e)
            {
                String s = e.Message.ToString();
                return ds;
            }
            finally
            {
                cn.Close();
            }
            return ds;
        }

        public DataSet CONSULTA_SAP_EMISION_LISTA_DOCNUM(string P_DOCNUM, int P_IDUSER)
        {
            //P_OPC 0 - TODO 1 -ACTIVO
            string strQ = "SP_CONSULTA_SAP_LISTAITEMSEMISION_DOCNUM";
            SqlCommand cmd = new SqlCommand(strQ, cn);
            DataSet ds = new DataSet();
            try
            {
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = strQ;
                cmd.Parameters.Clear();

                SqlParameter P_DOCNUM_p = new SqlParameter("@P_DOCNUM", SqlDbType.VarChar);
                P_DOCNUM_p.Value = P_DOCNUM;
                P_DOCNUM_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_DOCNUM_p);

                SqlParameter P_IDUSER_p = new SqlParameter("@P_IDUSER", SqlDbType.Int);
                P_IDUSER_p.Value = P_IDUSER;
                P_IDUSER_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_IDUSER_p);


                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception e)
            {
                String s = e.Message.ToString();
                return ds;
            }
            finally
            {
                cn.Close();
            }
            return ds;
        }

        public DataSet CONSULTA_SAP_ITEM_DATA(string ITEMCODE, string WHSCODE)
        {
            //P_OPC 0 - TODO 1 -ACTIVO
            string strQ = "SP_CONSULTA_SAP_ITEM_DATA";
            SqlCommand cmd = new SqlCommand(strQ, cn);
            DataSet ds = new DataSet();
            try
            {
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = strQ;
                cmd.Parameters.Clear();

                SqlParameter P_IDITEM_p = new SqlParameter("@P_IDITEM", SqlDbType.VarChar);
                P_IDITEM_p.Value = ITEMCODE;
                P_IDITEM_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_IDITEM_p);

                SqlParameter P_WHSCODE_p = new SqlParameter("@P_WHSCODE", SqlDbType.VarChar);
                P_WHSCODE_p.Value = WHSCODE;
                P_WHSCODE_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_WHSCODE_p);


                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception e)
            {
                String s = e.Message.ToString();
                return ds;
            }
            finally
            {
                cn.Close();
            }
            return ds;
        }

        public DataSet CONSULTA_SAP_RECIBO_LISTA_DOCNUM(string P_DOCNUM, int P_IDUSER)
        {
            //P_OPC 0 - TODO 1 -ACTIVO
            string strQ = "SP_CONSULTA_SAP_LISTAITEMSRECIBO_DOCNUM";
            SqlCommand cmd = new SqlCommand(strQ, cn);
            DataSet ds = new DataSet();
            try
            {
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = strQ;
                cmd.Parameters.Clear();

                SqlParameter P_DOCNUM_p = new SqlParameter("@P_DOCNUM", SqlDbType.VarChar);
                P_DOCNUM_p.Value = P_DOCNUM;
                P_DOCNUM_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_DOCNUM_p);

                SqlParameter P_IDUSER_p = new SqlParameter("@P_IDUSER", SqlDbType.Int);
                P_IDUSER_p.Value = P_IDUSER;
                P_IDUSER_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_IDUSER_p);


                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception e)
            {
                String s = e.Message.ToString();
                return ds;
            }
            finally
            {
                cn.Close();
            }
            return ds;
        }
        public DataSet CONSULTA_SAP_VERIFICADORES()
        {
            //P_OPC 0 - TODO 1 -ACTIVO
            string strQ = "SP_CONSULTA_SAP_VERIFICADORES";
            SqlCommand cmd = new SqlCommand(strQ, cn);
            DataSet ds = new DataSet();
            try
            {
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = strQ;
                cmd.Parameters.Clear();
 
         

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception e)
            {
                String s = e.Message.ToString();
                return ds;
            }
            finally
            {
                cn.Close();
            }
            return ds;
        }
        public DataSet CONSULTA_SAP_DISPENSADORES()
        {
            //P_OPC 0 - TODO 1 -ACTIVO
            string strQ = "SP_CONSULTA_SAP_DISPENSADORES";
            SqlCommand cmd = new SqlCommand(strQ, cn);
            DataSet ds = new DataSet();
            try
            {
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = strQ;
                cmd.Parameters.Clear();



                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception e)
            {
                String s = e.Message.ToString();
                return ds;
            }
            finally
            {
                cn.Close();
            }
            return ds;
        }

        public int NUMERO_SIG_SERIE(int P_SERIE)
        {
            string strQ = "SP_CONSULTA_SAP_SERIE_NUMERO";
            SqlCommand cmd = new SqlCommand(strQ, cn);
            DataSet ds = new DataSet();
            try
            {
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                //P_OPC 0 - TODO 1 -ACTIVO

                cmd.CommandText = strQ;
                cmd.Parameters.Clear();


                SqlParameter P_SERIE_P = new SqlParameter("@P_SERIE", SqlDbType.Int);
                P_SERIE_P.Value = P_SERIE;
                P_SERIE_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(P_SERIE_P);

                SqlParameter CodRes_P = new SqlParameter("P_RESPUESTA", SqlDbType.Int);
                CodRes_P.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(CodRes_P);
  

                cmd.ExecuteNonQuery(); 
                return Convert.ToInt32(CodRes_P.Value);
            }
            catch (Exception e)
            { 
                return -1;
            }
            finally
            {
                cn.Close();
            }
        }

        #endregion
    }

}