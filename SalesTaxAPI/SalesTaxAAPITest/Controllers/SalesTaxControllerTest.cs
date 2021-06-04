using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using SalesTaxAPI.Controllers;
using SalesTaxAPI.DAL.Contracts;
using SalesTaxAPI.DAL.Services;
using SalesTaxAPI.Helpers;
using SalesTaxAPI.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Xunit;

namespace SalesTaxAAPITest.Controllers
{
    public class SalesTaxControllerTest
    {
        Mock<ISalesTaxService> mockService;
        Mock<IConfiguration> configuration;
        SalesTaxController salesTaxController;

        public SalesTaxControllerTest()
        {
            mockService = new Mock<ISalesTaxService>();
            configuration = new Mock<IConfiguration>();

            salesTaxController = new SalesTaxController(configuration.Object, mockService.Object);
        }

        [Fact]
        public void EmptyZipCodeTest()
        {
            // Setup
            LocationTaxRequest request = new LocationTaxRequest()
            {
                CustomerId = 1,
                LocationRequest = new LocationReqest()
            };

            // Action
            var response =  salesTaxController.GetTaxRatesForLocation(request);

            // Assertion
            Assert.NotNull(response);
            Assert.False(response.IsSucess);
            Assert.Equal("zip is mandatory, Please check input.", response.ErrorMessage);
        }

        [Fact]
        public void InvalidCustomerIdForRateLocationTest()
        {
            // Setup
            LocationTaxRequest request = new LocationTaxRequest()
            {
                CustomerId = -1,
                LocationRequest = new LocationReqest()
        };

            // Action
            var response = salesTaxController.GetTaxRatesForLocation(request);

            // Assertion
            Assert.NotNull(response);
            Assert.False(response.IsSucess);
            Assert.Equal("Invalid customerId.", response.ErrorMessage);
        }

        [Fact]
        public void InvalidCustomerIdForCalTaxForOrderTest()
        {
            // Setup
            OrderTaxRequest request = new OrderTaxRequest()
            {
                CustomerId = -1,
                OrderRequest = new OrderRequest()
            };

            // Action
            var response = salesTaxController.CalulateTaxForOrder(request);

            // Assertion
            Assert.NotNull(response);
            Assert.False(response.IsSucess);
            Assert.Equal("Invalid customerId.", response.ErrorMessage);
        }

        [Fact]       
        public void GetTaxRatesForLocation()
        {
            // Setup
            LocationTaxRequest request = new LocationTaxRequest()
            {
                CustomerId = 11,
                LocationRequest = new LocationReqest() { zip= "90404", country="US"}
            };

            TaxResponse<TaxRateModel> taxRes = new TaxResponse<TaxRateModel>()
            {
                IsSucess = true,
                Result= new TaxRateModel()
            };

            mockService.Setup(x => x.GetTaxRatesForLocation(request)).Returns(taxRes);

            // Action
            var response = salesTaxController.GetTaxRatesForLocation(request);

            // Assertion
            Assert.NotNull(response);
            Assert.True(response.IsSucess);            
        }

        [Fact]
        public void CalulateTaxForOrder()
        {
            // Setup
             OrderTaxRequest request = new OrderTaxRequest()
            {
                CustomerId = 1,
                OrderRequest = new OrderRequest()
            };

            TaxResponse<TaxOrderModel> taxRes = new TaxResponse<TaxOrderModel>()
            {
                IsSucess = true,
                Result = new TaxOrderModel()
            };

            mockService.Setup(x => x.CalculateTaxesForOrder(request)).Returns(taxRes);

            // Action
            var response = salesTaxController.CalulateTaxForOrder(request);

            // Assertion
            Assert.NotNull(response);
            Assert.True(response.IsSucess);            
        }
    }
}
