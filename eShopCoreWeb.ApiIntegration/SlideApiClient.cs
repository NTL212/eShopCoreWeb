using eShopCoreWeb.ViewModels.Catalog.Categories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using eShopCoreWeb.ViewModels.Utilities.Slides;

namespace eShopCoreWeb.ApiIntegration
{
    public class SlideApiClient : ISlideApiClient
    {
        private readonly IHttpClientFactory _httpClientFactor;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SlideApiClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactor = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<SlideViewModel>> GetAll()
        {
            var client = _httpClientFactor.CreateClient();
            client.BaseAddress = new Uri("https://localhost:44321");
            var response = await client.GetAsync($"/api/slides");

            var body = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<SlideViewModel>>(body);
        }
    }
}
