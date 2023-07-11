using eShopCoreWeb.Application.Catalog.Products;
using Microsoft.AspNetCore.Mvc;

namespace eShopCoreWeb.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly IPublicProductService _publicProductService;
        public ProductController(IPublicProductService productService)
        {
            _publicProductService = productService;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var products = await _publicProductService.GetAll();
            return Ok(products);
        }
    }
}
