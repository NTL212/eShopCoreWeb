using eShopCoreWeb.Application.Catalog.Products;
using eShopCoreWeb.ViewModels.Catalog.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace eShopCoreWeb.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly IPublicProductService _publicProductService;
        private readonly IManageProductService _manageProductService;
        public ProductController(IPublicProductService productService, IManageProductService manageProductService)
        {
            _publicProductService = productService;
            _manageProductService = manageProductService;
        }
        //http://localhost:port/product/paging
        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetManageProductPagingRequest request)
        {
            var products = await _manageProductService.GetAllPaging(request);
            return Ok(products);
        }
        //http://localhost:port/product
        [HttpGet("{languageId}")]
        public async Task<IActionResult> Get(string languageId)
        {
            var products = await _publicProductService.GetAll(languageId);
            return Ok(products);
        }
        //http://localhost:port/product/public-paging
        [HttpGet("public-paging/{langugeId}")]
        public async Task<IActionResult> Get([FromQuery]GetPublicProductPagingRequest request)
        {
            var products = await _publicProductService.GetAllByCategoryId(request);
            return Ok(products);
        }
        //http://localhost:port/product/1
        [HttpGet("{id}/{languageId}")]
        public async Task<IActionResult> GetById(int id, string languageId)
        {
            var product = await _manageProductService.GetById(id, languageId);
            if (product == null)
                return BadRequest("Cannot find product");
            return Ok(product);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromForm]ProductCreateRequest request)
        {
            var productId = await _manageProductService.Create(request);
            if (productId == 0)
                return BadRequest();
            var product = await _manageProductService.GetById(productId, request.LanguageId);
            return CreatedAtAction(nameof(GetById), new {id = productId},product);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] ProductUpdateRequest request)
        {
            var affectedResult = await _manageProductService.Update(request);
            if (affectedResult == 0)
                return BadRequest();
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var affectedResult = await _manageProductService.Delete(id);
            if (affectedResult == 0)
                return BadRequest();
            return Ok();
        }

        [HttpPut("price/{productId}/{newprice}")]
        public async Task<IActionResult> UpdatePrice(int productId, decimal newprice)
        {
            var isSuccessful = await _manageProductService.UpdatePrice(productId, newprice);
            if (isSuccessful)
                return Ok();
            return BadRequest();
        }
        [HttpPut("stock/{id}/{quantity}")]
        public async Task<IActionResult> UpdateStock(int id, int quantity)
        {
            var isSuccessful = await _manageProductService.UpdateStock(id, quantity);
            if (isSuccessful)
                return Ok();
            return BadRequest();
        }
        [HttpPut("viewcount/{id}")]
        public async Task<IActionResult> AddViewCount(int id)
        {
            var isSuccessful = await _manageProductService.AddViewcount(id);
            if (isSuccessful==0)
                return BadRequest("Cannot add viewcount ");
            return Ok();
        }
    }
}
