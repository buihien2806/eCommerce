using eCommerce.Model.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace eCommerce.Model.Catalog.Products
{
    public class ProductAdminGetPagingRequest : PagingRequestBase
    {
        public string Keyword { get; set; }

        public string LanguageId { get; set; }

        public int? CategoryId { get; set; }
    }
}
