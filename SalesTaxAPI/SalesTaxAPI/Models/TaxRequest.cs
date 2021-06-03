using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTaxAPI.Models
{
    public class OrderTaxRequest
    {
        public int CustomerId { get; set; }
        public OrderRequest OrderRequest { get; set; }        
    }

    public class LocationTaxRequest
    {
        public int CustomerId { get; set; }       
        public LocationReqest LocationRequest { get; set; }
    }

    public class OrderRequest
    {
        public double amount { get; set; }
        public double shipping { get; set; }
        public string from_city { get; set; }
        public string from_state { get; set; }
        public string from_zip { get; set; }
        public string from_country { get; set; }
        public string to_city { get; set; }
        public string to_state { get; set; }
        public string to_zip { get; set; }
        public string to_country { get; set; }
        public List<LineItem> line_items { get; set; }
    }

    public class LineItem
    {
        public int quantity { get; set; }
        public double unit_price { get; set; }
        public string product_tax_code { get; set; }
    }

    public class LocationReqest
    {
        public string country { get; set; }
        public string zip { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string street { get; set; }
    }
}
