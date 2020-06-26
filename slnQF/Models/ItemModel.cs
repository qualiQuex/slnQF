using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace slnQF.Models
{
    public class ItemModel
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Precio { get; set; }
        public string Impuesto { get; set; }

        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int PagerCount { get; set; }

        public List<clsItem> Items { get; set; }
    }

    public class ProduccionModel
    {
     
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int PagerCount { get; set; }

        public List<clsProduccionModel> Items { get; set; }

        public List<clsItemsProduction> ItemsOrdenProduccion { get; set; }
    }

    public class ProduccionItemsModel {
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int PagerCount { get; set; }
        
        public List<clsItemsProduction> Items { get; set; }

    }
    public class modelPrecio {
        public string codArticulo { get; set; }
        public string fechaPost { get; set; }
        public string codCliente { get; set; }
    }
}