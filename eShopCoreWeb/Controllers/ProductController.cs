using eShopCoreWeb.Application.Catalog.Products;
using eShopCoreWeb.Data.EF;
using eShopCoreWeb.Data.Entities;
using eShopCoreWeb.ViewModels.Catalog.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using System.Drawing.Printing;
using eShopCoreWeb.ApiIntegration;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.RegularExpressions;

namespace eShopCoreWeb.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductApiClient _productApiClient;
        private readonly ICategoryApiClient _categoryApiClient;
        private readonly IConfiguration _configuration;
        public ProductController(IProductApiClient productApiClient, IConfiguration configuration, ICategoryApiClient categoryApiClient)
        {
            _productApiClient = productApiClient;
            _configuration = configuration;
            _categoryApiClient = categoryApiClient;
        }
        [HttpGet]
        // GET: ProductsController
        public async Task<IActionResult> Index(int? categoryId, int? sort, string keyword = "default", int pageIndex = 1, int pageSize = 2, string languageId = "vi")
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
                LanguageId = languageId,
                CategoryId = categoryId,
                Sort =sort
            };

            // Gọi API để lấy danh sách sản phẩm
            var data = await _productApiClient.GetProductPaging(request);
            var categories = await _categoryApiClient.GetAll(languageId);
            ViewBag.Categories = categories.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString(),
                Selected = categoryId.HasValue && categoryId.Value == x.Id
            });
            if (data == null)
                return BadRequest("Không tìm được");
            ViewBag.ResultMsg = TempData["result"];
            return View(data);
        }

        // GET: ProductsController/Details/5
        public async Task<IActionResult> Detail(int id)
        {
            var product = await _productApiClient.GetProductById(id, "vi");
            var productImages = await _productApiClient.GetListImage(id);
            return View(new WebApp.Models.ProductDetailViewModel()
            {
                Product = product,
                ProductImages = productImages
            });
        }

        // GET: ProductsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProductsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProductsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
