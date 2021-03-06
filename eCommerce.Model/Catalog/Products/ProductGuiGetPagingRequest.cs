using eCommerce.Model.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace eCommerce.Model.Catalog.Products
{
    public class ProductGuiGetPagingRequest : PagingRequestBase
    {
        public int? CategoryId { get; set; }
    }
}
