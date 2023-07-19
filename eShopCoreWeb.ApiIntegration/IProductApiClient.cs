using eShopCoreWeb.ViewModels.Catalog.ProductImages;
using eShopCoreWeb.ViewModels.Catalog.Products;
using eShopCoreWeb.ViewModels.Common;
using eShopCoreWeb.ViewModels.System.Roles;
using eShopCoreWeb.ViewModels.System.Users;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eShopCoreWeb.ApiIntegration
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
        Task<List<ProductViewModel>> GetLastestProduct(int take, string languageId);
        Task<List<ProductViewModel>> GetFeaturedProduct(int take, string languageId);
        Task<bool> AddImages(int productId, ProductImageCreateRequest request);
        Task<bool> UpdateImage(int imageId, ProductImageUpdateRequest request);
        Task<bool> DeleteImage(int imageId);
        Task<ProductImageViewModel> GetImageById(int imageId);
    }
}
