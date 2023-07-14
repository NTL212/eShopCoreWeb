using eShopCoreWeb.ViewModels.Common;
using eShopCoreWeb.ViewModels.System.Users;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopCoreWeb.Application.System.Users
{
    public interface IUserService
    {
        Task<string> Authenticate(LoginRequest request);
        Task<bool> Register(RegisterRequest request);
        Task<bool> UpdateUser(Guid id, UserUpdateRequest request);
        Task<bool> DeleteUser(Guid id);
        Task<UserViewModel> GetById(Guid id);
        Task<PagedResult<UserViewModel>> GetUserPaging(GetUserPagingRequest request);
    }
}
