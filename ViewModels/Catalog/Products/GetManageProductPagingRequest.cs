﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eShopCoreWeb.ViewModels.Common;
namespace eShopCoreWeb.ViewModels.Catalog.Products
{
    public class GetManageProductPagingRequest:PagingRequestBase
    {
        public string Keyword { get; set; }
        public string LanguageId { get; set; }
    }
}
