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
        private string errorMsg = string.Empty;
        public SalesTaxService(IConfiguration configs)
        {            
            _configs = configs;
            Customers = new List<Customer>();
            configs.GetSection("Customers").Bind(Customers);
        }

        public TaxResponse<TaxOrderModel> CalculateTaxesForOrder(OrderTaxRequest request)
        {
            TaxResponse<TaxOrderModel> response = new TaxResponse<TaxOrderModel>();
            this.CreateTaxCalculator(request.CustomerId);

            if (!string.IsNullOrEmpty(this.errorMsg))
            {
                response.ErrorMessage = errorMsg;
                response.IsSucess = false;

                return response;
            }

            response.Result = taxCalculator.CalculateTaxesForOrder(request.OrderRequest);
            response.ErrorMessage = errorMsg;
            response.IsSucess = string.IsNullOrEmpty(errorMsg) ? true : false;

            return response;
        }

        public TaxResponse<TaxRateModel> GetTaxRatesForLocation(LocationTaxRequest request)
        {
            TaxResponse<TaxRateModel> response = new TaxResponse<TaxRateModel>();
            this.CreateTaxCalculator(request.CustomerId);

            if(!string.IsNullOrEmpty(this.errorMsg))
            {
                response.ErrorMessage = errorMsg;
                response.IsSucess = false;

                return response;
            }

            response.Result = taxCalculator.GetTaxRatesForLocation(request.LocationRequest);
            
            response.ErrorMessage = errorMsg;
            response.IsSucess = string.IsNullOrEmpty(errorMsg) ? true : false;

            return response;
        }

        private void CreateTaxCalculator(int customerId)
        {
            var customer = Customers.FirstOrDefault(x => x.Id == customerId);

            if (customer != null)
            {               
                if (httpClient == null)
                {
                    httpClient = new HttpClientHelper(customer);
                }

                switch (customerId)
                {
                    case 1:

                        taxCalculator = new C1TaxCalculator(httpClient);
                        break;
                    case 2:

                        taxCalculator = new C2TaxCalculator(httpClient);
                        break;
                    default:
                        errorMsg = "Invalid Customer";
                        return;
                }
            }
            else
            {
                // Logging error
                errorMsg = "TaxAPI not available for the given customer";
            }
        }

        public IHttpClientHelper clientHelper
        {
            get { return httpClient; }
            set { this.httpClient = value; }
        }       
    }
}
