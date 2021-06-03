using SalesTaxAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTaxAPI.DAL.Contracts
{
    public interface ISalesTaxService
    {
        TaxRateModel GetTaxRatesForLocation(LocationTaxRequest request);
        TaxOrderModel CalculateTaxesForOrder(OrderTaxRequest request);
    }
}
