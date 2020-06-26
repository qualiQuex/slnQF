using slnQF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace slnQF.Controllers
{
    public class OperacionStockController : Controller
    {
        // GET: OperacionStock
        public ActionResult Index()
        {
            List<MyDrop> listaVacia = new List<MyDrop>();
            ViewBag.cmbSeries = new SelectList(listaVacia, "id", "value");
            return View();
        }
    }
}