using eShopCoreWeb.ViewModels.Catalog.Categories;
using eShopCoreWeb.ViewModels.Sales;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using eShopCoreWeb.ViewModels.Common;
using eShopCoreWeb.ViewModels.System.Users;

namespace eShopCoreWeb.ApiIntegration
{
    public class OrderApiClient : IOrderApiClient
    {
        private readonly IHttpClientFactory _httpClientFactor;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public OrderApiClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactor = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<bool> CreateOrder(Guid userId, CheckoutRequest request)
        {
            var client = _httpClientFactor.CreateClient();
            client.BaseAddress = new Uri("https://localhost:44321");

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/orders/{userId}", httpContent);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<OrderViewModel>> GetAllByUserName(string username)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactor.CreateClient();
            client.BaseAddress = new Uri("https://localhost:44321");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
            var response = await client.GetAsync($"/api/orders/getbyuser/{username}");
            var body = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<OrderViewModel>>(body);
        }

        public async Task<OrderViewModel> GetOrderById( int orderId)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactor.CreateClient();
            client.BaseAddress = new Uri("https://localhost:44321");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
            var response = await client.GetAsync($"/api/orders/{orderId}");
            var body = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<OrderViewModel>(body);
        }
        public async Task<ApiResult<PagedResult<OrderViewModel>>> GetOrderPaging(GetOrderPagingRequest request)
        {
            var client = _httpClientFactor.CreateClient();
            client.BaseAddress = new Uri("https://localhost:44321");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", request.BearerToken);
            var response = await client.GetAsync($"/api/orders/paging?pageIndex=" +
                $"{request.PageIndex}&pageSize={request.PageSize}&keyword={request.Keyword}&bearerToken={request.BearerToken}");
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<PagedResult<OrderViewModel>>>(body);
            return JsonConvert.DeserializeObject<ApiErrorResult<PagedResult<OrderViewModel>>>(body);
        }

        public async Task<bool> UpdateStatus(int orderId, int status)
        {
            var client = _httpClientFactor.CreateClient();
            client.BaseAddress = new Uri("https://localhost:44321");
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var json = JsonConvert.SerializeObject(status);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
            var response = await client.PutAsync($"/api/orders/{orderId}/status", httpContent);
            return response.IsSuccessStatusCode;
        }
    }
}
