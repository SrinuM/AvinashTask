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
        private Customer _customer;        

        public HttpClientHelper(Customer customer)
        {
            if(customer == null)
            {
                throw new ArgumentNullException("Customer", "Invalid Customer.");
            }
            else if(string.IsNullOrWhiteSpace(customer.apiUrl) || string.IsNullOrWhiteSpace(customer.apiKey))
            {
                throw new ArgumentException("APIURL and APIKey must be valid. Please check.");
            }
            _customer = customer;            
        }

        public string ExecuteGet(LocationReqest request)
        {            
            try
            {
                string apiUrl = $"{_customer.apiUrl}v2/rates";
                string queryUrl = GetQueryUrl(request, apiUrl);

                using (var httpClient = GetClient(apiUrl))
                {
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var res = httpClient.GetAsync(queryUrl);
                    res.Result.EnsureSuccessStatusCode();

                    var jsonResult = res.Result.Content.ReadAsStringAsync();

                    return jsonResult.Result;
                }
            }
            catch (Exception ex)
            {
                // Logging
                throw new Exception($"API Call failed with error: {ex.Message}");
            }
        }

        public string ExecutePost(OrderRequest request)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(request));
                content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                string Url = $"{_customer.apiUrl}v2/taxes";
                using (var httpClient = this.GetClient(Url))
                {
                    var res = httpClient.PostAsync(httpClient.BaseAddress, content);
                    res.Result.EnsureSuccessStatusCode();

                    var jsonResult = res.Result.Content.ReadAsStringAsync();

                    return jsonResult.Result;
                }
            }
            catch(Exception ex)
            {
                // Logging
                throw new Exception($"API Call failed with error: {ex.Message}");
            }
        }

        private HttpClient GetClient(string Url)
        {
            HttpClient httpClient = new HttpClient()
            {
                BaseAddress = new Uri(Url)
            };

            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _customer.apiKey);

            return httpClient;
        }

        private string GetQueryUrl(LocationReqest req, string apiUrl)
        {
            var parameters = new Dictionary<string, string>();
            string url = $"{apiUrl}?zip={req.zip}";

            if (!string.IsNullOrEmpty(req.country))
                url = url + $"&country={req.country}";

            if (!string.IsNullOrEmpty(req.state))
                url = url + $"&state={req.state}";

            if (!string.IsNullOrEmpty(req.city))
                url = url + $"&city={req.city}";

            if (!string.IsNullOrEmpty(req.street))
                url = url + $"&street={req.street}";

            return url;
        }
      
    }
}
