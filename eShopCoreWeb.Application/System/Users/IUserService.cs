using eShopCoreWeb.ViewModels.Common;
using eShopCoreWeb.ViewModels.System.Users;
using Microsoft.AspNetCore.Http;
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
        Task<ApiResult<string>> Authenticate(LoginRequest request);
        Task<ApiResult<bool>> Register(RegisterRequest request);
        Task<ApiResult<bool>> CreateGoogleUser(CreateGoogleUserRequest request);
        Task<ApiResult<bool>> UpdateUser(Guid id, UserUpdateRequest request);
        Task<ApiResult<bool>> RoleAssign(Guid id, RoleAssignRequest request);
        Task<ApiResult<bool>> DeleteUser(Guid id);
        Task<ApiResult<UserViewModel>> GetById(Guid id);
        Task<ApiResult<UserViewModel>> GetByUserName(string username);
        Task<ApiResult<UserViewModel>> GetByUserEmail(string email);
        Task<ApiResult<PagedResult<UserViewModel>>> GetUserPaging(GetUserPagingRequest request);
    }
}
