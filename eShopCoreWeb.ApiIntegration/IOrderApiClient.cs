using eShopCoreWeb.ViewModels.Catalog.Categories;
using eShopCoreWeb.ViewModels.Common;
using eShopCoreWeb.ViewModels.Sales;
using eShopCoreWeb.ViewModels.System.Users;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace eShopCoreWeb.ApiIntegration
{
    public interface IOrderApiClient
    {
        Task<bool> CreateOrder(Guid userId, CheckoutRequest request);
        Task<List<OrderViewModel>> GetAllByUserName(string username);
        Task<OrderViewModel> GetOrderById(int orderId);
        Task<ApiResult<PagedResult<OrderViewModel>>> GetOrderPaging(GetOrderPagingRequest request);
        Task<bool> UpdateStatus(int orderId, int status);
    }
}
