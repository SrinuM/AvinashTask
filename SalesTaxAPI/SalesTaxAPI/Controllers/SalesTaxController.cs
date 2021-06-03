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

        [HttpPost]
        public IEnumerable<string> GetTaxInfoByLocation(CustomerRequest request)
        {           
            _taxService.GetTaxRatesForLocation(request);
            return new string[] { "test123" };
        }
    }
}
