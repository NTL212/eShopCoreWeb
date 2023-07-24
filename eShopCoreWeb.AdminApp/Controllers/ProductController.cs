
using eShopCoreWeb.ApiIntegration;
using eShopCoreWeb.ViewModels.Catalog.ProductImages;
using eShopCoreWeb.ViewModels.Catalog.Products;
using eShopCoreWeb.ViewModels.Common;
using eShopCoreWeb.ViewModels.System.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Globbing;

namespace eShopCoreWeb.AdminApp.Controllers
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
        public async Task<IActionResult> Index(int? categoryId,string keyword = "default", int pageIndex = 1, int pageSize = 2, string languageId="vi")
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
                CategoryId = categoryId
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
                if(listImage!=null)
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
        [HttpPost]
        public async Task<IActionResult> UpdateFeature(int productId)
        {
            var product = await _productApiClient.GetProductById(productId, "vi");
            bool isFeatured;
            if (product.IsFeatured==true)
            {
                isFeatured=false;   
            }
            else
            {
                isFeatured = true;
            }    
            var result = await _productApiClient.UpdateFeature(productId, isFeatured);
            if (result)
            {
                TempData["result"] = $"Cập nhật feature của {product.Name} thành {isFeatured} thành công";
            }
            else
            {
                TempData["result"] = $"Cập nhật status của {product.Name}  thành {isFeatured} thất bại";
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> UpdateStock(int productId, int addedQuantity)
        {
            var product = await _productApiClient.GetProductById(productId, "vi");
            var result = await _productApiClient.UpdateStock(productId, addedQuantity);
            if (result)
            {
                TempData["result"] = $"Cập nhật số lượng của {product.Name} thành công";
            }
            else
            {
                TempData["result"] = $"Cập nhật số lượng của {product.Name} thất bại";
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> UpdatePrice(int productId, decimal newPrice)
        {
            var product = await _productApiClient.GetProductById(productId, "vi");
            var result = await _productApiClient.UpdatePrice(productId, newPrice);
            if (result)
            {
                TempData["result"] = $"Cập nhật giá của {product.Name} thành công";
            }
            else
            {
                TempData["result"] = $"Cập nhật giá của {product.Name} thất bại";
            }
            return RedirectToAction("Index");
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
            // Gọi API để xóa sản phẩm theo Id
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

        [HttpGet("cap-nhat-danh-muc")]
        public async Task<IActionResult> CategoryAssign(int id,string languageId)
        {
            var roleAssignRequsest = await GetCategoryAssignRequest(id, languageId);
            return View(roleAssignRequsest);
        }

        [HttpPost("cap-nhat-danh-muc")]
        public async Task<IActionResult> CategoryAssign(CategoryAssignRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _productApiClient.CategoryAssign(request.Id, request);

            if (result.IsSuccessed)
            {
                TempData["result"] = "Cập nhật danh mục thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", result.Message);
            var categoryAssignRequest = await GetCategoryAssignRequest(request.Id, "vi");

            return View(categoryAssignRequest);
        }
        public async Task<CategoryAssignRequest> GetCategoryAssignRequest(int id, string languageId)
        {
            var product = await _productApiClient.GetProductById(id, languageId);
            var categories = await _categoryApiClient.GetAll(languageId);
            categories = categories.FindAll(x=>x.ParentId==0);
            var categoryAssignRequest = new CategoryAssignRequest();
            categoryAssignRequest.Id = product.Id;
            foreach (var category in categories)
            {
                categoryAssignRequest.Categories.Add(new SelectItem()
                {
                    Id = category.Id.ToString(),
                    Name = category.Name,
                    
                    Selected = product.Categories.Contains(category.Name)
                });
            }
            return categoryAssignRequest;
        }

        [HttpGet]
        public async Task<IActionResult> AddImages(int productId)
        {
            var productImages = await _productApiClient.GetListImage(productId);
            ViewBag.ProductId = productId;
            if (productImages != null)
                ViewBag.Images = productImages;
            ViewBag.ResultMsg = TempData["result"];
            return View();
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddImages(int productId, ProductImageCreateRequest request)
        {

            if (!ModelState.IsValid)
                return View();
            // Gọi API để tạo sản phẩm
            var result = await _productApiClient.AddImages(productId,request);
            if (result)
            {
                TempData["result"] = "Thêm hình thành công";
                return RedirectToAction("Index");
            }
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateImage(int imageId)
        {
            // Gọi API để lấy thông tin sản phẩm theo ID và ngôn ngữ
            var image = await _productApiClient.GetImageById(imageId);
            if (image != null)
            {
                var updateRequest = new ProductImageUpdateRequest()
                {
                   Id = image.Id,
                   Caption = image.Caption,
                   ImagePath = image.ImagePath,
                   IsDefault = image.IsDefault,
                   SortOrder = image.SortOrder
                };
                return View(updateRequest);
            }

            return RedirectToAction("Error", "Home");
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateImage(ProductImageUpdateRequest request)
        {

            if (!ModelState.IsValid)
                return View();
            // Gọi API để lay imgae
            var image = await _productApiClient.GetImageById(request.Id);
            // Gọi API để update imgae
            var result = await _productApiClient.UpdateImage(request.Id, request);
            if (result)
            {
                TempData["result"] = "Cập nhật hình ảnh thành công";
                return RedirectToAction("AddImages", new {productId = image.ProductId});
            }
            TempData["result"] = "Cập nhật hình ảnh thất bại";
            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteImage(int imageId)
        {
            // Gọi API để lay imgae
            var image = await _productApiClient.GetImageById(imageId);
            // Gọi API xóa ảnh theo Id
            var result = await _productApiClient.DeleteImage(imageId);
            if (result)
            {
                TempData["result"] = "Xóa ảnh thành công";
            }
            else
            {
                TempData["result"] = "Xóa ảnh thất bại";
            }
            return RedirectToAction("AddImages", new { productId = image.ProductId });
        }
    }
}
