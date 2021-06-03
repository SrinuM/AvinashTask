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
    public class C1TaxCalculatorTest
    {        
        SalesTaxService salesTaxService;
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
        public void Test1()
        {
            OrderRequest taxRequest = new OrderRequest();
            var customerReq = new OrderTaxRequest()
            {
                CustomerId = 1,
                OrderRequest = taxRequest

            };
        
            clientHelper.Setup(x => x.ExecutePost(taxRequest));
            salesTaxService = new SalesTaxService(configuration);
            salesTaxService.clientHelper = clientHelper.Object;

            salesTaxService.CalculateTaxesForOrder(customerReq);
        }
    }
}
