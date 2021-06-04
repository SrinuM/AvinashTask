using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using SalesTaxAPI.DAL.Contracts;
using SalesTaxAPI.DAL.Services;
using SalesTaxAPI.Helpers;
using SalesTaxAPI.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Xunit;

namespace SalesTaxAAPITest.Helpers
{
    public class SalesTaxControllerTest
    {
        [Fact]
        public void NullCustomerTest()
        {
           var ex = Assert.Throws<ArgumentNullException>(() => new HttpClientHelper(null));

            Assert.Equal("Invalid Customer. (Parameter 'Customer')", ex.Message);
        }

        [Fact]       
        public void InvalidAPIUrlTest()
        {
            Customer customer = new Customer()
            {
                apiKey = "testkey",
                apiUrl = ""
            };            

            var ex= Assert.Throws<ArgumentException>(()=>new HttpClientHelper(customer));

            Assert.Equal("APIURL and APIKey must be valid. Please check.", ex.Message);
        }

        [Fact]
        public void InvalidAPIKeyTest()
        {
            Customer customer = new Customer()
            {
                apiKey = "",
                apiUrl = "testurl"
            };         

            var ex = Assert.Throws<ArgumentException>(() => new HttpClientHelper(customer));

            Assert.Equal("APIURL and APIKey must be valid. Please check.", ex.Message);
        }
    }
}
