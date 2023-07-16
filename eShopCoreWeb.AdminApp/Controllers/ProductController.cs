using eShopCoreWeb.AdminApp.Services;
using eShopCoreWeb.ViewModels.Catalog.Products;
using eShopCoreWeb.ViewModels.System.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Globbing;

namespace eShopCoreWeb.AdminApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductApiClient _productApiClient;
        private readonly IConfiguration _configuration;
        [AllowAnonymous]
        public IActionResult Forbidden()
        {
            return View();
        }
        public ProductController(IProductApiClient productApiClient, IConfiguration configuration)
        {
            _productApiClient = productApiClient;
            _configuration = configuration;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string keyword = "default", int pageIndex = 1, int pageSize = 1, string langugeId="vi")
        {
            // Lấy token từ Session
            var sessions = HttpContext.Session.GetString("Token");

            // Tạo request để lấy danh sách sản phẩm theo trang và từ khóa tìm kiếm
            var request = new GetManageProductPagingRequest()
            {
                BearerToken = sessions,
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
                LanguageId = langugeId,
            };
            // Gọi API để lấy danh sách sản phẩm
            var data = await _productApiClient.GetProductPaging(request);
            if (data == null)
                return BadRequest("Không tìm được");
            ViewBag.ResultMsg = TempData["result"];
            return View(data);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
        {

            if (!ModelState.IsValid)
                return View();
            // Gọi API để tạo sản phẩm
            var result = await _productApiClient.CreateProduct(request);
            if (result)
            {
                TempData["result"] = "Thêm sản phẩm thành công";
                return RedirectToAction("Index");
            }
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, string languageId="vi")
        {
            // Gọi API để lấy thông tin sản phẩm theo ID và ngôn ngữ
            var product = await _productApiClient.GetProductById(id, languageId);
            var listImage = await _productApiClient.GetListImage(id);
            if (product!= null)
            {
                var updateRequest = new ProductUpdateRequest()
                {
                    SeoAlias = product.SeoAlias,
                    Description = product.Description,
                    Details = product.Details,
                    Name = product.Name,
                    SeoDescription = product.SeoDescription,
                    SeoTitle = product.SeoTitle,
                    LanguageId = languageId,
                    Id = id,
                };
                ViewBag.Image = listImage.FirstOrDefault().ImagePath;
                return View(updateRequest);
            }
            return RedirectToAction("Error", "Home");
        }
        // Xử lý việc cập nhật sản phẩm khi submit form chỉnh sửa bao gồm file ảnh
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Edit([FromForm] ProductUpdateRequest request)
        {

            if (!ModelState.IsValid)
                return View();
            // Gọi API để cập nhật sản phẩm
            var result = await _productApiClient.UpdateProduct(request);
            if (result)
            {
                TempData["result"] = "Cập nhật sản phẩm thành công";
                return RedirectToAction("Index");
            }
            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id, string languageId)
        {
            // Gọi API để lấy thông tin sản phẩm theo ID và ngôn ngữ
            var product = await _productApiClient.GetProductById(id, languageId);
            return View(product);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            // Gọi API để lấy thông tin sản phẩm theo ID và ngôn ngữ
            var result = await _productApiClient.DeleteProduct(id);
            if (result)
            {
                TempData["result"] = "Xóa sản phẩm thành công";
            }
            else
            {
                TempData["result"] = "Xóa sản phẩm thất bại";
            }
            return RedirectToAction("Index");
        }
    }
}
