using Newtonsoft.Json;
using SalesTaxAPI.DAL.Contracts;
using SalesTaxAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SalesTaxAPI.Helpers
{
    public class HttpClientHelper: IHttpClientHelper
    {
        HttpClient httpClient;

        public HttpClientHelper(Customer customer)
        {
            if(customer == null)
            {
                throw new ArgumentNullException("Customer", "Invalid Customer.");
            }

            httpClient = new HttpClient()
            {
                BaseAddress = new Uri(customer.apiUrl)
            };

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", customer.apiKey);
        }

        public void ExecuteGet(TaxRequest request)
        {
            TaxResponse response = new TaxResponse();
            var content = new StringContent(JsonConvert.SerializeObject(request));
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

            var res = httpClient.GetAsync(httpClient.BaseAddress);

            res.Result.EnsureSuccessStatusCode();

            var jsonResult = res.Result.Content.ReadAsStringAsync();
        }

        public void ExecutePost(TaxRequest request)
        {
            TaxResponse response = new TaxResponse();
            var content = new StringContent(JsonConvert.SerializeObject(request));
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

            var res = httpClient.PostAsync(httpClient.BaseAddress, content);

            res.Result.EnsureSuccessStatusCode();

            var jsonResult = res.Result.Content.ReadAsStringAsync();

            //return response;
        }
      
    }
}
