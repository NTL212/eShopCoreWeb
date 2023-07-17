using eShopCoreWeb.ViewModels.Catalog.ProductImages;
using eShopCoreWeb.ViewModels.Catalog.Products;
using eShopCoreWeb.ViewModels.Common;
using eShopCoreWeb.ViewModels.System.Roles;
using eShopCoreWeb.ViewModels.System.Users;

namespace eShopCoreWeb.AdminApp.Services
{
    public interface IProductApiClient
    {
        Task<PagedResult<ProductViewModel>> GetProductPaging(GetManageProductPagingRequest request);
        Task<bool> CreateProduct(ProductCreateRequest request);
        Task<bool> UpdateProduct(ProductUpdateRequest request);
        Task<bool> DeleteProduct(int id);
        Task<ProductViewModel> GetProductById(int id, string languageId);
        Task<List<ProductImageViewModel>> GetListImage(int productId);
        Task<ApiResult<bool>> CategoryAssign(int id, CategoryAssignRequest request);
    }
}
