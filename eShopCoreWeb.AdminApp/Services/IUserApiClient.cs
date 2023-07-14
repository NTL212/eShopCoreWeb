using eShopCoreWeb.ViewModels.Common;
using eShopCoreWeb.ViewModels.System.Users;

namespace eShopCoreWeb.AdminApp.Services
{
    public interface IUserApiClient
    {
        Task<string> Authenticate(LoginRequest request);
        Task<bool> Register(RegisterRequest request);
        Task<bool> Udpate(Guid id, UserUpdateRequest request);
        Task<bool> Delete(Guid id);
        Task<UserViewModel> GetUserById(Guid id);
        Task<PagedResult<UserViewModel>> GetUserPaging(GetUserPagingRequest request);
    }
}
