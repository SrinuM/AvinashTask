using SalesTaxAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTaxAPI.DAL.Contracts
{
    public interface ISalesTaxService
    {
        TaxResponse GetTaxRatesForLocation(CustomerRequest request);
        TaxResponse CalculateTaxesForOrder(CustomerRequest request);
    }
}
