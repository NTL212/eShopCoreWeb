using eShopCoreWeb.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopCoreWeb.ViewModels.Sales
{
    public class GetOrderPagingRequest: PagingRequestBase
    {
        public string Keyword { get; set; }
    }
}
