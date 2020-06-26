using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace slnQF.Controllers
{
    public class LoteController : Controller
    {
        public ActionResult loteInfo(string lote)
        {
            string idUsuarioLog = "-1";
            int intIdUsuarioLog = 0;
            try
            {
                idUsuarioLog = Session["LogedUserID"].ToString();
                intIdUsuarioLog = Convert.ToInt32(idUsuarioLog);
            }
            catch (Exception Ex)
            {
                intIdUsuarioLog = 0;
            }
            DC_CALIDADDataContext db = new DC_CALIDADDataContext();
            SP_CONSULTA_SAP_LOTE_INFOResult loteDataDeta = db.SP_CONSULTA_SAP_LOTE_INFO(lote).FirstOrDefault();

            return PartialView(loteDataDeta);
        }

    }
}