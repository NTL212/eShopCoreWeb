using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopCoreWeb.ViewModels.Catalog.Categories
{
    public class CategoryUpdateRequest
    {
        public int Id { set; get; }
        public int? SortOrder { set; get; }
        public bool? IsShowOnHome { set; get; }
        public int? ParentId { set; get; }
        public bool? Status { set; get; }
        public string? Name { set; get; }
        public string? SeoDescription { set; get; }
        public string? SeoTitle { set; get; }
        public string? LanguageId { set; get; }
        public string? SeoAlias { set; get; }
    }
}
