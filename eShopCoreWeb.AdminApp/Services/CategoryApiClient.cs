using eShopCoreWeb.ViewModels.Catalog.Categories;
using eShopCoreWeb.ViewModels.Catalog.Products;
using eShopCoreWeb.ViewModels.Common;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;

namespace eShopCoreWeb.AdminApp.Services
{
    public class CategoryApiClient : ICategoryApiClient
    {
        private readonly IHttpClientFactory _httpClientFactor;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CategoryApiClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactor = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<List<CategoryViewModel>> GetAll(string languageId)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactor.CreateClient();
            client.BaseAddress = new Uri("https://localhost:44321");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
            var response = await client.GetAsync($"/api/categories?languageId={languageId}");
               
            var body = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<CategoryViewModel>>(body);
        }
    }
}
