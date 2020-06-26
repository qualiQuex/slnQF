using slnQF.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace slnQF.Controllers
{
    public class OperacionesController : Controller
    {
        public JsonResult traeSerie(string value)
        {

            int vObjCodeDoc = 0;
            if (value == "ORDEN")
            {
                //59 recibo de fabricacion
                vObjCodeDoc = 59;
            }
            if (value == "EMISION")
            {
                //60 emision de fabricacion
                vObjCodeDoc = 60;
            }

            if (value == "STOCKTRAN")
            {
                //60 emision de fabricacion
                vObjCodeDoc = 67;
            }

            if (value == "ENTRADATRAN")
            {
                //59 Entrada de mercancia
                vObjCodeDoc = 59;
            }
            if (value == "SALIDATRAN")
            {
                //59 Entrada de mercancia
                vObjCodeDoc = 60;
            }

            



            clsDB db = new clsDB();
            string serie = "";
            try
            {
                string idUsuarioLog = "";
                try
                {
                    idUsuarioLog = Session["LogedUserID"].ToString();
                }
                catch {

                    return Json(new { ID = -100, MENSAJE = "Su sesion ha expirado.", OBJETO = "" });

                }


                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds = db.CONSULTA_SERIES_USER(Convert.ToInt32(idUsuarioLog));
                dt = ds.Tables[0];


                foreach (DataRow dr in dt.Rows)
                {
                    if (value == "ORDEN")
                    {
                        serie = dr["SERIE_RECIBO"].ToString();
                    }
                    else if (value == "EMISION")
                    {
                        serie = dr["SERIE_EMISION"].ToString();
                    }
                    else if (value == "STOCKTRAN")
                    {
                        serie = dr["SERIE_TRANSTOCK"].ToString();
                    }
                    else if (value == "ENTRADATRAN")
                    {
                        serie = dr["SERIE_ENTRADA_STOCK"].ToString();
                    }
                    else if (value == "SALIDATRAN")
                    {
                        serie = dr["SERIE_SALIDA_STOCK"].ToString();
                    }


                }

            }
            catch (Exception e)
            {
                return Json(new { ID = -1, MENSAJE = e.Message, OBJETO = ""});

            }

            List<MyDropList> lista = new List<MyDropList>();

            Console.WriteLine("Conectado");

            List<MyDropList> listaTMP = new List<MyDropList>();

            lista = traeListaSerie(vObjCodeDoc);

            foreach (MyDropList tmp in lista)
            {
                if (tmp.value == serie)
                {

                    listaTMP.Add(tmp);
                }
            }

            lista = listaTMP;
            return Json(new { ID = 1, MENSAJE = "", OBJETO = lista });
            
        }
        public List<MyDropList> traeListaSerie(int objec)
        {
            var GenreLst = new List<MyDropList>();
            List<clsProduccionModel> lista = new List<clsProduccionModel>();
            try
            {
                clsDB cDB = new clsDB();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds = cDB.CONSULTA_SAP_SERIE(objec.ToString());
                dt = ds.Tables[0];


                foreach (DataRow dr in dt.Rows) //dr["UNIDADESFABRICAR"].ToString()
                {

                    MyDropList fila = new MyDropList();
                    fila.id = dr["SERIES"].ToString();
                    fila.value = dr["SERIESNAME"].ToString();

                    GenreLst.Add(fila);
                }
            }
            catch (Exception e)
            {
                System.Console.Write(e.Message);
            }


            return GenreLst;
        }
        public JsonResult traeNumDocto(string value)
        {
            clsDB cDB = new clsDB();
            int vNexNumber = 0;
            if (value == null)
            {
                value = "-1";
            }
            try
            {
                vNexNumber = cDB.NUMERO_SIG_SERIE(Convert.ToInt32(value));
            }
            catch
            {
                vNexNumber = 0;
            }
            return Json(vNexNumber, JsonRequestBehavior.AllowGet);
        }
    }
}