using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTaxAPI.Models
{
    public class TaxResponse<T>
    {
        public string ErrorMessage { get; set; }
        public bool IsSucess { get; set; }
        public T Result { get; set; }
    }
}
