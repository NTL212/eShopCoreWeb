using eShopCoreWeb.Application.Catalog.Categories;
using eShopCoreWeb.ViewModels.Catalog.Categories;
using eShopCoreWeb.ViewModels.Catalog.Products;
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
        [HttpGet("parents")]
        public async Task<IActionResult> GetAllParentCategories(string languageId)
        {
            var parentCategories = await _categoryService.GetAllParentCategories(languageId);
            foreach(var category in parentCategories)
            {
                category.ChildCategories = await _categoryService.GetAllChildCategories(category.Id, languageId);
            }
            return Ok(parentCategories);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var categoryId = await _categoryService.Create(request);
            if (categoryId == 0)
                return BadRequest();
            var category = await _categoryService.GetById(categoryId, request.LanguageId);
            return CreatedAtAction(nameof(GetById), new { id = categoryId }, category);
        }
        [HttpGet("{categoryId}/{languageId}")]
        public async Task<IActionResult> GetById(int categoryId, string languageId)
        {
            var category = await _categoryService.GetById(categoryId, languageId);
            if (category == null)
                return BadRequest("Cannot find category");
            return Ok(category);
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CategoryUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var affectedResult = await _categoryService.Update(request);
            if (affectedResult == 0)
                return BadRequest();
            return Ok();
        }
        [HttpDelete("{categoryId}")]
        public async Task<IActionResult> Delete(int categoryId)
        {
            var affectedResult = await _categoryService.Delete(categoryId);
            if (affectedResult == 0)
                return BadRequest();
            return Ok();
        }

    }
}   