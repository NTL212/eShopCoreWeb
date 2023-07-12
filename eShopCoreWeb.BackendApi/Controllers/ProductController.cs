using eShopCoreWeb.Application.Catalog.Products;
using eShopCoreWeb.ViewModels.Catalog.ProductImages;
using eShopCoreWeb.ViewModels.Catalog.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace eShopCoreWeb.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : Controller
    {
        private readonly IPublicProductService _publicProductService;
        private readonly IManageProductService _manageProductService;
        public ProductsController(IPublicProductService productService, IManageProductService manageProductService)
        {
            _publicProductService = productService;
            _manageProductService = manageProductService;
        }
        //http://localhost:port/product/paging
        //[HttpGet("paging")]
        //public async Task<IActionResult> GetAllPaging([FromQuery] GetManageProductPagingRequest request)
        //{
        //    var products = await _manageProductService.GetAllPaging(request);
        //    return Ok(products);
        //}
        ////http://localhost:port/products
        //[HttpGet("{languageId}")]
        //public async Task<IActionResult> GetAll(string languageId)
        //{
        //    var products = await _publicProductService.GetAll(languageId);
        //    return Ok(products);
        //}
        //http://localhost:port/products?pageIndex=1&pageSize=1&CategoryId=
        [HttpGet("{langugeId}")]
        public async Task<IActionResult> GetAllPaging(string languageId,[FromQuery]GetPublicProductPagingRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var products = await _publicProductService.GetAllByCategoryId(languageId, request);
            return Ok(products);
        }
        //http://localhost:port/product/1
        [HttpGet("{productId}/{languageId}")]
        public async Task<IActionResult> GetById(int productId, string languageId)
        {
            var product = await _manageProductService.GetById(productId, languageId);
            if (product == null)
                return BadRequest("Cannot find product");
            return Ok(product);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromForm]ProductCreateRequest request)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var productId = await _manageProductService.Create(request);
            if (productId == 0)
                return BadRequest();
            var product = await _manageProductService.GetById(productId, request.LanguageId);
            return CreatedAtAction(nameof(GetById), new {id = productId},product);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] ProductUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var affectedResult = await _manageProductService.Update(request);
            if (affectedResult == 0)
                return BadRequest();
            return Ok();
        }
        [HttpDelete("{productId}")]
        public async Task<IActionResult> Delete(int productId)
        {
            var affectedResult = await _manageProductService.Delete(productId);
            if (affectedResult == 0)
                return BadRequest();
            return Ok();
        }

        [HttpPatch("price/{productId}/{newprice}")]
        public async Task<IActionResult> UpdatePrice(int productId, decimal newprice)
        {
            var isSuccessful = await _manageProductService.UpdatePrice(productId, newprice);
            if (isSuccessful)
                return Ok();
            return BadRequest();
        }
        [HttpPatch("stock/{id}/{quantity}")]
        public async Task<IActionResult> UpdateStock(int id, int quantity)
        {
            var isSuccessful = await _manageProductService.UpdateStock(id, quantity);
            if (isSuccessful)
                return Ok();
            return BadRequest();
        }
        [HttpPatch("viewcount/{productId}")]
        public async Task<IActionResult> AddViewCount(int productId)
        {
            var isSuccessful = await _manageProductService.AddViewcount(productId);
            if (isSuccessful==0)
                return BadRequest("Cannot add viewcount ");
            return Ok();
        }
        //http://localhost:port/product/1
        [HttpGet("{productId}/image/{imageId}")]
        public async Task<IActionResult> GetImageById(int productId, int imageId)
        {
            var productImage = await _manageProductService.GetImageById(imageId);
            if (productImage == null)
                return BadRequest("Cannot find image");
            return Ok(productImage);
        }

        //Images
        [HttpPost("{productId}/images")]
        public async Task<IActionResult> AddImages(int productId, [FromForm] ProductImageCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var imageId = await _manageProductService.AddImages(productId, request);
            if (imageId == 0)
                return BadRequest();
            var productImage = await _manageProductService.GetImageById(imageId);
            return CreatedAtAction(nameof(GetImageById), new { id = imageId }, productImage);
        }

        [HttpPut("{productId}/images/{imageId}")]
        public async Task<IActionResult> UpdateImage(int imageId, [FromForm] ProductImageUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var affectedResult = await _manageProductService.UpdateImage(imageId,request);
            if (affectedResult == 0)
                return BadRequest();
            return Ok();
        }
        [HttpDelete("{productId}/images/{imageId}")]
        public async Task<IActionResult> DeleteImage(int imageId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var affectedResult = await _manageProductService.RemoveImages(imageId);
            if (affectedResult == 0)
                return BadRequest();
            return Ok();
        }
    }
}
