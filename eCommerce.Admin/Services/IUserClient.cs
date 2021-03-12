using eCommerce.Model.Common;
using eCommerce.Model.System.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.Admin.Services
{
    public interface IUserClient
    {
        Task<string> Authenticate(LoginRequest request);
        Task<PagedResult<UserView>> UserGetPaging(UserGetPagingRequest request);

    }
}
