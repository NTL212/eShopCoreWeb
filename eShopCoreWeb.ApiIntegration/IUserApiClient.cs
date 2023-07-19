using eShopCoreWeb.ViewModels.Common;
using eShopCoreWeb.ViewModels.System.Users;

namespace eShopCoreWeb.ApiIntegration
{
    public interface IUserApiClient
    {
        Task<ApiResult<string>> Authenticate(LoginRequest request);
        Task<ApiResult<bool>> Register(RegisterRequest request);
        Task<ApiResult<bool>> Update(Guid id, UserUpdateRequest request);
        Task<ApiResult<bool>> RoleAssign(Guid id, RoleAssignRequest request);
        Task<ApiResult<bool>> Delete(Guid id);
        Task<ApiResult<UserViewModel>> GetUserById(Guid id);
        Task<ApiResult<PagedResult<UserViewModel>>> GetUserPaging(GetUserPagingRequest request);
    }
}
