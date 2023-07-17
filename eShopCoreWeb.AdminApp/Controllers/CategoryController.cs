using eShopCoreWeb.AdminApp.Services;
using eShopCoreWeb.ViewModels.Catalog.Categories;
using eShopCoreWeb.ViewModels.Catalog.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eShopCoreWeb.AdminApp.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryApiClient _categoryApiClient;
        private readonly IConfiguration _configuration;
        public CategoryController(IConfiguration configuration, ICategoryApiClient categoryApiClient)
        {
            _configuration = configuration;
            _categoryApiClient = categoryApiClient;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string languageId = "vi")
        {
            // Lấy token từ Session
            var sessions = HttpContext.Session.GetString("Token");


            // Gọi API để lấy danh sách danh muc
            var categories = await _categoryApiClient.GetAll(languageId);
            foreach(var category in categories)
            {
                category.BearerToken = sessions;
            }
            if (categories == null)
                return BadRequest("Không tìm được");
            ViewBag.ResultMsg = TempData["result"];
            return View(categories);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CategoryCreateRequest request)
        {

            if (!ModelState.IsValid)
                return View();
            // Gọi API để tạo sản phẩm
            var result = await _categoryApiClient.CreateCategory(request);
            if (result)
            {
                TempData["result"] = "Thêm danh mục thành công";
                return RedirectToAction("Index");
            }
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, string languageId = "vi")
        {
            // Gọi API để lấy thông tin danh muc theo ID và ngôn ngữ
            var category = await _categoryApiClient.GetCategoryById(id, languageId);
            if (category != null)
            {
                var updateRequest = new CategoryUpdateRequest()
                {
                    SeoAlias = category.SeoAlias,
                    Name = category.Name,
                    SeoDescription = category.SeoDescription,
                    SeoTitle = category.SeoTitle,
                    LanguageId = languageId,
                    Id = category.Id,
                    IsShowOnHome = category.IsShowOnHome,
                    ParentId = category.ParentId,
                    SortOrder =category.SortOrder,
                    Status = category.Status,
                };
                return View(updateRequest);
            }
            return RedirectToAction("Error", "Home");
        }
        // Xử lý việc cập nhật danh muc khi submit form chỉnh sửa =
        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] CategoryUpdateRequest request)
        {

            if (!ModelState.IsValid)
                return View();
            // Gọi API để cập nhật danh muc
            var result = await _categoryApiClient.UpdateCategory(request);
            if (result)
            {
                TempData["result"] = "Cập nhật danh muc thành công";
                return RedirectToAction("Index");
            }
            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            // Gọi API để lấy thông tin sản phẩm theo ID và ngôn ngữ
            var result = await _categoryApiClient.DeleteCategory(id);
            if (result)
            {
                TempData["result"] = "Xóa danh muc thành công";
            }
            else
            {
                TempData["result"] = "Xóa danh muc  thất bại";
            }
            return RedirectToAction("Index");
        }

    }
}
