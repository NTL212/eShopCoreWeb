using eShopCoreWeb.ViewModels.Catalog.Categories;

namespace eShopCoreWeb.AdminApp.Services
{
    public interface ICategoryApiClient
    {
        Task<List<CategoryViewModel>> GetAll(string languageId);
    }
}
