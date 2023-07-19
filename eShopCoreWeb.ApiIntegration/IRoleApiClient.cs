using eShopCoreWeb.ViewModels.Common;
using eShopCoreWeb.ViewModels.System.Roles;

namespace eShopCoreWeb.ApiIntegration
{
    public interface IRoleApiClient
    {
        Task<ApiResult<List<RoleViewModel>>> GetAll();
    }
}
