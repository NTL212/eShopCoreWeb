﻿using eShopCoreWeb.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopCoreWeb.ViewModels.Catalog.Categories
{
    public class CategoryParentViewModel:RequestBase
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public int? SortOrder { set; get; }
        public bool? IsShowOnHome { set; get; }
        public bool? Status { set; get; }
        public string? SeoDescription { set; get; }
        public string? SeoTitle { set; get; }
        public string? LanguageId { set; get; }
        public string? SeoAlias { set; get; }
        public List<CategoryViewModel> ChildCategories { get; set; }
    }
}
