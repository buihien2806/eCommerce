using eCommerce.Model.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace eCommerce.Model.System.Users
{
    public class UserGetPagingRequest : PagingRequestBase
    {
        public string Keyword { get; set; }
    }
}
