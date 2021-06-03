using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SalesTaxAPI.DAL.Contracts;
using SalesTaxAPI.Helpers;
using SalesTaxAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SalesTaxAPI.DAL.Services
{
    public class SalesTaxService: ISalesTaxService
    {
        private IHttpClientHelper httpClient;
        private ITaxCalculator taxCalculator;
        private IConfiguration _configs;
        private List<Customer> Customers;
        public SalesTaxService(IConfiguration configs)
        {            
            _configs = configs;
            Customers = new List<Customer>();
            configs.GetSection("Customers").Bind(Customers);
        }
        
        public TaxResponse CalculateTaxesForOrder(CustomerRequest request)
        {
            TaxResponse response = new TaxResponse();
            this.CreateTaxCalculator(request.CustomerId);
            try
            {
                response = taxCalculator.GetTaxRatesForLocation(request.TaxRequest);
            }
            catch (Exception ex)
            {
                //
            }
            return null;
        }

        public TaxResponse GetTaxRatesForLocation(CustomerRequest request)
        {
            TaxResponse response = new TaxResponse();
            this.CreateTaxCalculator(request.CustomerId);
            try
            {
                response= taxCalculator.GetTaxRatesForLocation(request.TaxRequest);
            }
            catch(Exception ex)
            {
                //
            }
            return response;
        }


        private void CreateTaxCalculator(int customerId)
        {
            var customer = Customers.FirstOrDefault(x => x.Id == customerId);

            if (httpClient == null)
            {
                httpClient = new HttpClientHelper(customer);
            }
            
            switch(customerId)
            {
                case 1:

                    taxCalculator = new C1TaxCalculator(httpClient);
                    break;
                case 2:

                    taxCalculator = new C2TaxCalculator(httpClient);
                    break;
                default:
                    throw new Exception("Invalid Customer");
            }           
        }


        public IHttpClientHelper clientHelper
        {
            get { return httpClient; }
            set { this.httpClient = value; }
        }       
    }
}
