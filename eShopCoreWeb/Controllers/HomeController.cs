using eShopCoreWeb.ApiIntegration;
using eShopCoreWeb.Application.Catalog.Products;
using eShopCoreWeb.Utilities.Constants;
using eShopCoreWeb.WebApp.Models;

using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace eShopCoreWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISlideApiClient _slideApiClient;
        private readonly IProductApiClient _productApiClient;
        public HomeController(ILogger<HomeController> logger, ISlideApiClient slideApiClient, IProductApiClient productApiClient)
        {
            _logger = logger;
            _slideApiClient = slideApiClient;
            _productApiClient = productApiClient;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new HomeViewModel()
            {
                Slides = await _slideApiClient.GetAll(),
                FeaturedProducts =await _productApiClient.GetLastestProduct(SystemConstants.ProductSettings.NumberOfFeaturedProduct,"vi"),
                LastestProduct = await _productApiClient.GetLastestProduct(SystemConstants.ProductSettings.NumberOfFeaturedProduct, "vi")
            };
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