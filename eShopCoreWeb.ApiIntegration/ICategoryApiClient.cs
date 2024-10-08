﻿using eShopCoreWeb.ViewModels.Catalog.Categories;
using eShopCoreWeb.ViewModels.Catalog.Products;

namespace eShopCoreWeb.ApiIntegration
{
    public interface ICategoryApiClient
    {
        Task<List<CategoryViewModel>> GetAll(string languageId);
        Task<List<CategoryParentViewModel>> GetAllParentCategories(string languageId);
        Task<bool> CreateCategory(CategoryCreateRequest request);
        Task<bool> UpdateCategory(CategoryUpdateRequest request);
        Task<bool> DeleteCategory(int id);
        Task<CategoryViewModel> GetCategoryById(int id, string languageId);
    }
}
