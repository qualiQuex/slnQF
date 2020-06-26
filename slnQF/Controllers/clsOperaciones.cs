using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace slnQF.Controllers
{
    public class clsOperaciones
    {
        public int verificaConexionDB(string Server, string DB, string User, string Pass, out string Msj) {
            Msj = "";
            string sqlCon = "Data Source="+Server+";Initial Catalog="+DB+";User ID="+User+";Password="+ Pass + "";
            using (var connection = new SqlConnection(sqlCon))
            {
                try {
                    
                    connection.Open();
                    connection.Close();
                    return 1;
                }
                catch (Exception ex) {
                    Msj = ex.Message;
                    return 0;
                }
            }
              
        }
    }
}