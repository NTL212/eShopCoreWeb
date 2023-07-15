using eShopCoreWeb.ViewModels.Common;
using eShopCoreWeb.ViewModels.System.Roles;

namespace eShopCoreWeb.AdminApp.Services
{
    public interface IRoleApiClient
    {
        Task<ApiResult<List<RoleViewModel>>> GetAll();
    }
}
