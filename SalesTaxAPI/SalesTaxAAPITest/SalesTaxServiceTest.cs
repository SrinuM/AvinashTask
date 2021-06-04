using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using SalesTaxAPI.DAL.Contracts;
using SalesTaxAPI.DAL.Services;
using SalesTaxAPI.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Xunit;

namespace SalesTaxAAPITest
{
    public class SalesTaxServiceTest
    {        
        SalesTaxService salesTaxService;
        Mock<IHttpClientHelper> clientHelper;
        IConfiguration configuration;

        public SalesTaxServiceTest()
        {        
            clientHelper = new Mock<IHttpClientHelper>();            
           
            configuration = new ConfigurationBuilder()
         .AddJsonFile("appsettings.json", true, true)
         .Build();            

        }

        [Fact]
        public void CalculateTaxesForOrderTest()
        {
            // Setup
            OrderRequest taxRequest = new OrderRequest();
            OrderTaxRequest request = new OrderTaxRequest()
            {
                CustomerId = 1,
                OrderRequest = taxRequest
            };
           

            TaxOrderModel taxOrderModel = new TaxOrderModel()
            {
                tax = new Tax() { amount_to_collect = 5.0, rate = 2, taxable_amount = 20 }
            };

            var res = JsonConvert.SerializeObject(taxOrderModel);

            clientHelper.Setup(x => x.ExecutePost(taxRequest)).Returns(res);
            salesTaxService = new SalesTaxService(configuration);
         
            salesTaxService.clientHelper = clientHelper.Object;

            // Action
            var response = salesTaxService.CalculateTaxesForOrder(request);

            // Assertion
            Assert.NotNull(response);
            Assert.True(response.IsSucess);
            Assert.NotNull(response.Result);
            Assert.Equal(5.0, response.Result.tax.amount_to_collect);
        }


        [Fact]
        public void GetTaxRatesForLocation()
        {
            // Setup
            LocationReqest taxRequest = new LocationReqest();
            LocationTaxRequest request = new LocationTaxRequest()
            {
                CustomerId = 1,
                LocationRequest = taxRequest
            };

            TaxRateModel rateModel = new TaxRateModel()
            {
                Rate = new Rate()
                {
                    zip = "90404",
                    county = "LOS ANGELES",
                    county_rate = "0.1",
                    state = "CA",
                    state_rate = "0.0625",
                    combined_rate = "0.0975",
                    freight_taxable = "false"
                }
            };

            var res = JsonConvert.SerializeObject(rateModel);

            clientHelper.Setup(x => x.ExecuteGet(taxRequest)).Returns(res);
            salesTaxService = new SalesTaxService(configuration);
            salesTaxService.clientHelper = clientHelper.Object;

            // Action
            var response = salesTaxService.GetTaxRatesForLocation(request);

            // Assertion
            Assert.NotNull(response);
            Assert.True(response.IsSucess);
            Assert.NotNull(response.Result);
            Assert.NotNull(response.Result.Rate);
            Assert.Equal("CA", response.Result.Rate.state);
            Assert.Equal("0.0625", response.Result.Rate.state_rate);
            Assert.Equal("90404", response.Result.Rate.zip);
        }


        [Fact]
        public void InvalidConfigTest()
        {
            // Setup
            LocationReqest taxRequest = new LocationReqest();
            LocationTaxRequest request = new LocationTaxRequest()
            {
                CustomerId = 1,
                LocationRequest = taxRequest
            };

            configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings-invalid.json", true, true)
        .Build();
          
            salesTaxService = new SalesTaxService(configuration);
          
            // Action
            var response = salesTaxService.GetTaxRatesForLocation(request);

            // Assertion
            Assert.NotNull(response);
            Assert.False(response.IsSucess);
            Assert.Equal("TaxAPI not available for the given customer", response.ErrorMessage);
        }


        [Fact]
        public void InvalidCustomerRequestTest()
        {
            // Setup
            LocationReqest taxRequest = new LocationReqest();
            LocationTaxRequest request = new LocationTaxRequest()
            {
                CustomerId = 5,
                LocationRequest = taxRequest
            };

            salesTaxService = new SalesTaxService(configuration);

            // Action
            var response = salesTaxService.GetTaxRatesForLocation(request);

            // Assertion
            Assert.NotNull(response);
            Assert.False(response.IsSucess);
            Assert.Equal("TaxAPI not available for the given customer", response.ErrorMessage);
        }


        [Fact]
        public void TaxAPINotImplementedTest()
        {
            // Setup
            LocationReqest taxRequest = new LocationReqest();
            LocationTaxRequest request = new LocationTaxRequest()
            {
                CustomerId = 3,
                LocationRequest = taxRequest
            };

            salesTaxService = new SalesTaxService(configuration);

            // Action
            var response = salesTaxService.GetTaxRatesForLocation(request);

            // Assertion
            Assert.NotNull(response);
            Assert.False(response.IsSucess);
            Assert.Equal("Invalid Customer", response.ErrorMessage);
        }
    }
}
