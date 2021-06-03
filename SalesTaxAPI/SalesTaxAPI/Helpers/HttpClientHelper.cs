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
        string baseAddress;

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

            httpClient = new HttpClient()
            {
                BaseAddress = new Uri(customer.apiUrl)
            };
            baseAddress = customer.apiUrl;
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", customer.apiKey);
        }

        public string ExecuteGet(LocationReqest request)
        {            
            try
            {
                string queryUrl = GetQueryUrl(request);
                 
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var res = httpClient.GetAsync(queryUrl);

                res.Result.EnsureSuccessStatusCode();

                var jsonResult = res.Result.Content.ReadAsStringAsync();

                return jsonResult.Result;
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

                var res = httpClient.PostAsync(httpClient.BaseAddress, content);

                res.Result.EnsureSuccessStatusCode();

                var jsonResult = res.Result.Content.ReadAsStringAsync();

                return jsonResult.Result;
            }
            catch(Exception ex)
            {
                // Logging
                throw new Exception($"API Call failed with error: {ex.Message}");
            }
        }


        private string GetQueryUrl(LocationReqest req)
        {
            var parameters = new Dictionary<string, string>();
            string url = $"{baseAddress}?zip={req.zip}";

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
