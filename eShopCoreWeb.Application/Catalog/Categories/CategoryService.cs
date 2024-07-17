using eShopCoreWeb.Application.Common;
using eShopCoreWeb.Data.EF;
using eShopCoreWeb.Data.Entities;
using eShopCoreWeb.Utilities.Exceptions;
using eShopCoreWeb.ViewModels.Catalog.Categories;
using eShopCoreWeb.ViewModels.Catalog.Products;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace eShopCoreWeb.Application.Catalog.Categories
{
    public class CategoryService:ICategoryService
    {
        private readonly EShopDbContext _context;

        public CategoryService(EShopDbContext context)
        {
            _context = context;
        }

        public async Task<List<CategoryViewModel>> GetAll(string languageId)
        {
            var query = from c in _context.Categories
                        join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId
                        where ct.LanguageId == languageId
                        select new { c, ct };
            return await query.Select(x => new CategoryViewModel()
            {
                Id = x.c.Id,
                Name = x.ct.Name,
                IsShowOnHome = x.c.IsShowOnHome,
                ParentId = x.c.ParentId,
                SortOrder = x.c.SortOrder,
                SeoAlias = x.ct.SeoAlias,
                SeoDescription = x.ct.SeoDescription,
                SeoTitle = x.ct.SeoTitle,
                LanguageId = languageId,
                Status = x.c.Status.Equals(Data.Enums.Status.Active) ? true : false
        }).ToListAsync();
        }

        public async Task<int> Create(CategoryCreateRequest request)
        {
            var active = request.Status ? Data.Enums.Status.InActive : Data.Enums.Status.Active;
            var category = new Category()
            {
                IsShowOnHome = request.IsShowOnHome,
                ParentId = request.ParentId,
                SortOrder = request.SortOrder,
                Status = active,
                CategoryTranslations = new List<CategoryTranslation>()
                {
                    new CategoryTranslation()
                    {
                        Name =  request.Name,
                        SeoDescription = request.SeoDescription,
                        SeoAlias = request.SeoAlias,
                        SeoTitle = request.SeoTitle,
                        LanguageId = request.LanguageId
                    }
                }
            };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category.Id;
        }
        public async Task<int> Update(CategoryUpdateRequest request)
        {
            var category = await _context.Categories.FindAsync(request.Id);
            var categoryTranslations = await _context.CategoryTranslations.FirstOrDefaultAsync(x => x.CategoryId == request.Id
            && x.LanguageId == request.LanguageId);

            if (category == null || categoryTranslations == null) throw new EShopException($"Cannot find a category with id: {request.Id}");

            category.Status = request.Status==true? Data.Enums.Status.Active : Data.Enums.Status.InActive;
            category.SortOrder = (int)request.SortOrder;
            category.ParentId = request.ParentId;
            category.IsShowOnHome = (bool)request.IsShowOnHome;
            categoryTranslations.Name = request.Name;
            categoryTranslations.SeoAlias = request.SeoAlias;
            categoryTranslations.SeoDescription = request.SeoDescription;
            categoryTranslations.SeoTitle = request.SeoTitle;
            categoryTranslations.LanguageId = request.LanguageId;
            _context.Categories.Update(category);
            _context.CategoryTranslations.Update(categoryTranslations);

            return await _context.SaveChangesAsync();
        }
        public async Task<int> Delete(int categoryId)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            if (category == null) throw new EShopException($"Cannot find a category with id: {categoryId}");


            _context.Categories.Remove(category);

            return await _context.SaveChangesAsync();
        }

        public async Task<CategoryViewModel> GetById(int categoryId, string languageId)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            var categoryTranslation = await _context.CategoryTranslations.FirstOrDefaultAsync(x => x.CategoryId == categoryId
            && x.LanguageId == languageId);
            var active = category.Status.Equals(1) ? true : false;
            var categoryViewModel = new CategoryViewModel()
            {
                Id = category.Id,
                Name = categoryTranslation.Name,
                SeoAlias = categoryTranslation.SeoAlias,
                IsShowOnHome = category.IsShowOnHome,
                LanguageId = languageId,
                ParentId = category.ParentId,
                SeoDescription = categoryTranslation.SeoDescription,
                SeoTitle = categoryTranslation.SeoTitle,
                SortOrder = category.SortOrder,
                Status = active
            };
            return categoryViewModel;
        }

        public async  Task<List<CategoryViewModel>> GetAllChildCategories(int parentId, string languageId)
        {
             var query = from c in _context.Categories
                        join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId
                        where ct.LanguageId == languageId
                        select new { c, ct };
            return await query.Where(x=>x.c.ParentId==parentId).Select(x => new CategoryViewModel()
            {
                Id = x.c.Id,
                Name = x.ct.Name,
                IsShowOnHome = x.c.IsShowOnHome,
                ParentId = x.c.ParentId,
                SortOrder = x.c.SortOrder,
                SeoAlias = x.ct.SeoAlias,
                SeoDescription = x.ct.SeoDescription,
                SeoTitle = x.ct.SeoTitle,
                LanguageId = languageId,
                Status = x.c.Status.Equals(Data.Enums.Status.Active) ? true : false
        }).ToListAsync();
        }

        public async Task<List<CategoryParentViewModel>> GetAllParentCategories(string languageId)
        {
            var query = from c in _context.Categories
                        join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId
                        where ct.LanguageId == languageId
                        select new { c, ct };
            var data = await query.Where(x => x.c.ParentId==0).Select(x => new CategoryParentViewModel()
            {
                Id = x.c.Id,
                Name = x.ct.Name,
                IsShowOnHome = x.c.IsShowOnHome,
                SortOrder = x.c.SortOrder,
                SeoAlias = x.ct.SeoAlias,
                SeoDescription = x.ct.SeoDescription,
                SeoTitle = x.ct.SeoTitle,
                LanguageId = languageId,
                Status = x.c.Status.Equals(Data.Enums.Status.Active) ? true : false
            }).ToListAsync();

            int totalRow = await query.CountAsync();
            return data;
        }
    }
}
