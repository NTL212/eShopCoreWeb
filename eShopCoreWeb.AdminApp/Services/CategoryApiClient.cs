using eShopCoreWeb.ViewModels.Catalog.Categories;
using eShopCoreWeb.ViewModels.Catalog.Products;
using eShopCoreWeb.ViewModels.Common;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using eShopCoreWeb.ViewModels.System.Users;
using System.Text;

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
        public async Task<bool> CreateCategory(CategoryCreateRequest request)
        {
            var client = _httpClientFactor.CreateClient();
            client.BaseAddress = new Uri("https://localhost:44321");

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/categories", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateCategory(CategoryUpdateRequest request)
        {
            var client = _httpClientFactor.CreateClient();
            client.BaseAddress = new Uri("https://localhost:44321");
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync("/api/categories", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            return response.IsSuccessStatusCode;
        }
        public async Task<bool> DeleteCategory(int id)
        {
            var client = _httpClientFactor.CreateClient();
            client.BaseAddress = new Uri("https://localhost:44321");
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
            var response = await client.DeleteAsync($"/api/categories/{id}");
            var result = await response.Content.ReadAsStringAsync();
            return response.IsSuccessStatusCode;
        }

        public async Task<CategoryViewModel> GetCategoryById(int id, string languageId)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactor.CreateClient();
            client.BaseAddress = new Uri("https://localhost:44321");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
            var response = await client.GetAsync($"/api/categories/{id}/{languageId}");
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<CategoryViewModel>(body);

            return JsonConvert.DeserializeObject<CategoryViewModel>(body);
        }
    }
}
