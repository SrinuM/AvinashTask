using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SalesTaxAPI.DAL.Contracts;
using SalesTaxAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SalesTaxAPI.DAL
{
    public class C1TaxCalculator : ITaxCalculator
    {
        IHttpClientHelper httpClient;
        public C1TaxCalculator(IHttpClientHelper client)
        {
            if(client == null)
            {
                throw new ArgumentNullException("client", "httpclient is null.");
            }

            httpClient = client;            
        }     

        public TaxResponse CalculateTaxesForOrder(TaxRequest request)
        {
            TaxResponse response = new TaxResponse();

            httpClient.ExecutePost(request);

            return response;
        }

        public TaxResponse GetTaxRatesForLocation(TaxRequest request)
        {
            TaxResponse response = new TaxResponse();

             httpClient.ExecutePost(request);
           
            return response;
        }
    }
}
