﻿using SalesTaxAPI.DAL.Contracts;
using SalesTaxAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SalesTaxAPI.DAL
{
    public class C2TaxCalculator : ITaxCalculator
    {
        IHttpClientHelper httpClient;
        public C2TaxCalculator(IHttpClientHelper client)
        {
            if (client == null)
            {
                throw new ArgumentNullException("client", "httpclient is null.");
            }

            httpClient = client;
        }

        public TaxOrderModel CalculateTaxesForOrder(OrderRequest request)
        {
            throw new NotImplementedException();
        }

        public TaxRateModel GetTaxRatesForLocation(LocationReqest request)
        {
            throw new NotImplementedException();
        }
    }
}
