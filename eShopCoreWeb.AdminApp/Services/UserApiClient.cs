﻿using eShopCoreWeb.ViewModels.Common;
using eShopCoreWeb.ViewModels.System.Users;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace eShopCoreWeb.AdminApp.Services
{
    public class UserApiClient : IUserApiClient
    {
        private readonly IHttpClientFactory _httpClientFactor;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserApiClient(IHttpClientFactory httpClientFactory,IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactor = httpClientFactory;  
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<string> Authenticate(LoginRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json,Encoding.UTF8, "application/json");
            var client = _httpClientFactor.CreateClient();
            client.BaseAddress = new Uri("https://localhost:44321");
            var response = await client.PostAsync("/api/users/authenticate", httpContent);
            var token = await response.Content.ReadAsStringAsync();
            return token;
        }

        public async Task<UserViewModel> GetUserById(Guid id)
        {
            var client = _httpClientFactor.CreateClient();
            client.BaseAddress = new Uri("https://localhost:44321");
            var response = await client.GetAsync($"/api/users/{id}");
            var body = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<UserViewModel>(body);
            return users;
        }

        public async Task<PagedResult<UserViewModel>> GetUserPaging(GetUserPagingRequest request)
        {
            var client = _httpClientFactor.CreateClient();
            client.BaseAddress = new Uri("https://localhost:44321");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", request.BearerToken);
            var response = await client.GetAsync($"/api/users/paging?pageIndex=" +
                $"{request.PageIndex}&pageSize={request.PageSize}&keyword={request.Keyword}&bearerToken={request.BearerToken}");
            var body = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<PagedResult<UserViewModel>>(body);
            return users;
        }

        public async Task<bool> Register(RegisterRequest request)
        {
            var client = _httpClientFactor.CreateClient();
            client.BaseAddress = new Uri("https://localhost:44321");

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/users", httpContent);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Udpate(Guid id, UserUpdateRequest request)
        {
            var client = _httpClientFactor.CreateClient();
            client.BaseAddress = new Uri("https://localhost:44321");
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/users/{id}", httpContent);
            return response.IsSuccessStatusCode;
        }
        public async Task<bool> Delete(Guid id)
        {
            var client = _httpClientFactor.CreateClient();
            client.BaseAddress = new Uri("https://localhost:44321");
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
            var response = await client.DeleteAsync($"/api/users/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
