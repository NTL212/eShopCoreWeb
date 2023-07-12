using eShopCoreWeb.ViewModels.Catalog.Products;
using eShopCoreWeb.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopCoreWeb.Application.Catalog.Products
{
    public interface IPublicProductService
    {
        Task<PagedResult<ProductViewModel>> GetAllByCategoryId(string languageId, GetPublicProductPagingRequest request);

        Task<List<ProductViewModel>> GetAll(string languageId);
    }
}
