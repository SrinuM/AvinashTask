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

        public TaxOrderModel CalculateTaxesForOrder(OrderRequest request)
        {
            TaxOrderModel response = new TaxOrderModel();

            httpClient.ExecutePost(request);

            return response;
        }

        public TaxRateModel GetTaxRatesForLocation(LocationReqest request)
        {
            TaxRateModel response = new TaxRateModel();

             var result = httpClient.ExecuteGet(request);
            response = JsonConvert.DeserializeObject<TaxRateModel>(result);
           
            return response;
        }
    }
}
