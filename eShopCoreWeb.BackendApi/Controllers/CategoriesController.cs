using eShopCoreWeb.Application.Catalog.Categories;
using Microsoft.AspNetCore.Mvc;

namespace eShopCoreWeb.BackendApi.Controllers
{
        [Route("api/[controller]")]
        [ApiController]
        public class CategoriesController : ControllerBase
        {
            private readonly ICategoryService _categoryService;

            public CategoriesController(
                ICategoryService categoryService)
            {
                _categoryService = categoryService;
            }

            [HttpGet]
            public async Task<IActionResult> GetAll(string languageId)
            {
                var categories = await _categoryService.GetAll(languageId);
                return Ok(categories);
            }
        }
    }
