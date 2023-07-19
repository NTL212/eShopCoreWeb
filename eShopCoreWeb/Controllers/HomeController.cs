using eShopCoreWeb.ApiIntegration;
using eShopCoreWeb.Application.Catalog.Products;
using eShopCoreWeb.Data.Entities;
using eShopCoreWeb.Utilities.Constants;
using eShopCoreWeb.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace eShopCoreWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISlideApiClient _slideApiClient;
        private readonly IProductApiClient _productApiClient;
        private readonly ICategoryApiClient _categoryApiClient;
        public HomeController(ILogger<HomeController> logger, ISlideApiClient slideApiClient, IProductApiClient productApiClient, ICategoryApiClient categoryApiClient)
        {
            _logger = logger;
            _slideApiClient = slideApiClient;
            _productApiClient = productApiClient;
            _categoryApiClient = categoryApiClient;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new HomeViewModel()
            {
                Slides = await _slideApiClient.GetAll(),
                FeaturedProducts =await _productApiClient.GetLastestProduct(SystemConstants.ProductSettings.NumberOfFeaturedProduct,"vi"),
                LastestProduct = await _productApiClient.GetLastestProduct(SystemConstants.ProductSettings.NumberOfFeaturedProduct, "vi")
            };
            var categories = await _categoryApiClient.GetAll("vi");
            categories = categories.FindAll(x=>x.ParentId!=0);
            var serializedObject = JsonConvert.SerializeObject(categories);
            HttpContext.Session.SetString("categories", serializedObject);

            var parentCategories = await _categoryApiClient.GetAllParentCategories("vi");
            serializedObject = JsonConvert.SerializeObject(parentCategories);
            HttpContext.Session.SetString("parentCategories", serializedObject);
            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}