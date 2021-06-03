using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTaxAPI.Models
{
    public class TaxRateModel
    {
        public Rate Rate { get; set; }
    }

    public class Rate
    {
        public string zip { get; set; }
        public string state { get; set; }
        public string state_rate { get; set; }
        public string county { get; set; }
        public string county_rate { get; set; }
        public string combined_district_rate { get; set; }
        public string combined_rate { get; set; }
        public string freight_taxable { get; set; }

    }
}
