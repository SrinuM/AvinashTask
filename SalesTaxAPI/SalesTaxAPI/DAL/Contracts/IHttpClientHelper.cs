using SalesTaxAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTaxAPI.DAL.Contracts
{
    public interface IHttpClientHelper
    {
        string ExecutePost(OrderRequest request);
        string ExecuteGet(LocationReqest request);
    }
}
