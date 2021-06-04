using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using SalesTaxAPI.DAL;
using SalesTaxAPI.DAL.Contracts;
using SalesTaxAPI.DAL.Services;
using SalesTaxAPI.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Xunit;

namespace SalesTaxAAPITest
{
    public class C1TaxCalculatorTest
    {
        ITaxCalculator taxCalculator;
        Mock<IHttpClientHelper> clientHelper;
        IConfiguration configuration;

        public C1TaxCalculatorTest()
        {        
            clientHelper = new Mock<IHttpClientHelper>();            
           
            configuration = new ConfigurationBuilder()
         .AddJsonFile("appsettings.json", true, true)
         .Build();            

        }

        [Fact]
        public void InvalidClientTest()
        {
            var ex= Assert.Throws<ArgumentNullException>(() => new C1TaxCalculator(null));

            Assert.Equal("httpclient is null. (Parameter 'client')", ex.Message);
        }

        [Fact]
        public void CalculateTaxesForOrderTest()
        {
            OrderRequest taxRequest = new OrderRequest();
            TaxOrderModel taxOrderModel = new TaxOrderModel()
            {
                tax = new Tax() { amount_to_collect = 5.0, rate = 2, taxable_amount = 20 }
            };

            var res= JsonConvert.SerializeObject(taxOrderModel);
            
            clientHelper.Setup(x => x.ExecutePost(taxRequest)).Returns(res);
            taxCalculator = new C1TaxCalculator(clientHelper.Object);

            var result = taxCalculator.CalculateTaxesForOrder(taxRequest);

            // Assertion
            Assert.NotNull(result);
            Assert.NotNull(result.tax);
            Assert.Equal(20, result.tax.taxable_amount);           
        }

        [Fact]
        public void GetTaxRatesForLocationTest()
        {
            // Setup
            LocationReqest request = new LocationReqest();
            TaxRateModel rateModel = new TaxRateModel()
            {
              Rate= new Rate() { zip= "90404", county= "LOS ANGELES"
              , county_rate="0.1", state="CA", state_rate= "0.0625", combined_rate= "0.0975",
              freight_taxable="false"
              }  
            };

            var res = JsonConvert.SerializeObject(rateModel);

            clientHelper.Setup(x => x.ExecuteGet(request)).Returns(res);
            taxCalculator = new C1TaxCalculator(clientHelper.Object);

            // Action
            var result = taxCalculator.GetTaxRatesForLocation(request);

            // Assertion
            Assert.NotNull(result);
            Assert.NotNull(result.Rate);
            Assert.Equal("LOS ANGELES", result.Rate.county);
            Assert.Equal("CA", result.Rate.state);
            Assert.Equal("false", result.Rate.freight_taxable);
        }
    }
}
