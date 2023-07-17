using eShopCoreWeb.Data.Entities;
using eShopCoreWeb.ViewModels.Catalog.Categories;
using eShopCoreWeb.ViewModels.Catalog.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopCoreWeb.Application.Catalog.Categories
{
    public interface ICategoryService
    {
        Task<List<CategoryViewModel>> GetAll(string languageId);
        Task<int> Create(CategoryCreateRequest request);

        Task<int> Update(CategoryUpdateRequest request);

        Task<int> Delete(int categoryId);
        Task<CategoryViewModel> GetById(int categoryId, string languageId);
    }
}
