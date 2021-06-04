using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SalesTaxAPI.DAL.Contracts;
using SalesTaxAPI.DAL.Services;
using SalesTaxAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTaxAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesTaxController : ControllerBase
    {
        private readonly ISalesTaxService _taxService;
        IConfiguration configuration;

        public SalesTaxController(IConfiguration configs, ISalesTaxService salesTaxService)
        {
            _taxService = salesTaxService;
            configuration = configs;
        }

        [HttpGet,Route("GetTaxRatesForLocation")]
        public TaxResponse<TaxRateModel> GetTaxRatesForLocation([FromQuery]LocationTaxRequest request)
        {
            TaxResponse<TaxRateModel> response = new TaxResponse<TaxRateModel>();
            if (request.CustomerId <= 0)
            {
                response.IsSucess = false;
                response.ErrorMessage = "Invalid customerId.";
                return response;
            }
            else if (request.LocationRequest == null || string.IsNullOrWhiteSpace(request.LocationRequest.zip))
            {
                response.IsSucess = false;
                response.ErrorMessage = "zip is mandatory, Please check input.";
                return response;
            }
           

            try
            {
                response = _taxService.GetTaxRatesForLocation(request);
            }
            catch (Exception ex)
            {
                response.IsSucess = false;
                response.ErrorMessage = $"GetTaxInfo failed with error :{ex.Message}";
            }

            return response;
        }

        [HttpPost, Route("CalculateTaxForOrder")]
        public TaxResponse<TaxOrderModel> CalulateTaxForOrder(OrderTaxRequest request)
        {
            TaxResponse<TaxOrderModel> response = new TaxResponse<TaxOrderModel>();
            if (request.CustomerId <= 0)
            {
                response.IsSucess = false;
                response.ErrorMessage = "Invalid customerId.";
                return response;
            }

            try
            {
                response = _taxService.CalculateTaxesForOrder(request);                
            }
            catch (Exception ex)
            {
                response.IsSucess = false;
                response.ErrorMessage = $"CalculateTaxesForOrder failed with error :{ex.Message}";
            }

            return response;
        }
    }
}
