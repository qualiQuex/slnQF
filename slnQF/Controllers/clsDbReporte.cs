using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace slnQF.Controllers
{
    public class clsDbReporte
    {
        SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings.Get("QUALISAP_REPORTES"));
        public DataSet TRAE_ETIQUETAS(string num_orden)
        {
            string strQ = "[RPT_ETIQUETAEMISION2]";
            SqlCommand cmd = new SqlCommand(strQ, cn);
            DataSet ds = new DataSet();
            try
            {
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = strQ;
                cmd.Parameters.Clear();
                SqlParameter num_orden_P = new SqlParameter("@DOCNUMSAP", SqlDbType.Int);
                num_orden_P.Value = Convert.ToInt32(num_orden);
                num_orden_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(num_orden_P);
 
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

        public DataSet TRAE_ETIQUETASB(string num_orden)
        {
            string strQ = "[RPT_ANEXOB]";
            SqlCommand cmd = new SqlCommand(strQ, cn);
            DataSet ds = new DataSet();
            try
            {
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = strQ;
                cmd.Parameters.Clear();
                SqlParameter num_orden_P = new SqlParameter("@DOCNUMSAP", SqlDbType.Int);
                num_orden_P.Value = Convert.ToInt32(num_orden);
                num_orden_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(num_orden_P);

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

        public DataSet TRAE_ETIQUETASD(string num_orden)
        {
            //string strQ = "[RPT_ETIQUETARECIBO2]";
            string strQ = "[RPT_ANEXOE]";
            SqlCommand cmd = new SqlCommand(strQ, cn);
            DataSet ds = new DataSet();
            try
            {
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = strQ;
                cmd.Parameters.Clear();
                SqlParameter num_orden_P = new SqlParameter("@DOCNUMSAP", SqlDbType.Int);
                num_orden_P.Value = Convert.ToInt32(num_orden);
                num_orden_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(num_orden_P);

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

        public DataSet TRAE_ETIQUETASE(string num_orden)
        {
            string strQ = "[RPT_ANEXOE]";
            SqlCommand cmd = new SqlCommand(strQ, cn);
            DataSet ds = new DataSet();
            try
            {
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = strQ;
                cmd.Parameters.Clear();
                SqlParameter num_orden_P = new SqlParameter("@DOCNUMSAP", SqlDbType.Int);
                num_orden_P.Value = Convert.ToInt32(num_orden);
                num_orden_P.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(num_orden_P);

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

        public int INGRESA_DATOS(int p_linenum,int renglon,  decimal p_tara, decimal p_pesoneto,int p_docnum, int p_user_resp,out string p_msg1)
        {
            //P_OPC 0 - TODO 1 -ACTIVO
            string strQ = "P_INGRESO_TARA";
            SqlCommand cmd = new SqlCommand(strQ, cn);
            DataSet ds = new DataSet();
            try
            {
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = strQ;
                cmd.Parameters.Clear();

                SqlParameter p_linenum_p = new SqlParameter("@p_linenum", SqlDbType.Int);
                p_linenum_p.Value = p_linenum;
                p_linenum_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(p_linenum_p);

                SqlParameter p_tara_t = new SqlParameter("@p_tara", SqlDbType.Decimal);
                p_tara_t.Value = p_tara;
                p_tara_t.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(p_tara_t);

                SqlParameter p_renglon_t = new SqlParameter("@p_renglon", SqlDbType.Decimal);
                p_renglon_t.Value = renglon;
                p_renglon_t.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(p_renglon_t);

                SqlParameter p_pesoneto_p = new SqlParameter("@p_peso_neto", SqlDbType.Decimal);
                p_pesoneto_p.Value = p_pesoneto;
                p_pesoneto_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(p_pesoneto_p);

                SqlParameter p_docnum_p = new SqlParameter("@p_docnum", SqlDbType.Int);
                p_docnum_p.Value = p_docnum;
                p_docnum_p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(p_docnum_p);

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

               
                cmd.Parameters["P_MSG1"].Size = 100;
                 
                cmd.ExecuteNonQuery();
                p_msg1 = msgErr1_P.Value.ToString();
               
                return Convert.ToInt32(CodRes_P.Value);
            }
            catch (Exception e)
            {
                String s = e.Message.ToString();
                p_msg1 = e.Message.ToString();
                 
                return -1;
            }
            finally
            {
                cn.Close();
            }
        }
                
    }

}