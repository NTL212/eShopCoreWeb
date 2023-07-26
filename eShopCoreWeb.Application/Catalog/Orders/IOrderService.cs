using eShopCoreWeb.Data.Enums;
using eShopCoreWeb.ViewModels.Catalog.Categories;
using eShopCoreWeb.ViewModels.Common;
using eShopCoreWeb.ViewModels.Sales;
using eShopCoreWeb.ViewModels.System.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopCoreWeb.Application.Catalog.Orders
{
    public interface IOrderService
    {
        Task<List<OrderViewModel>> GetAllByUserName(string username);
        Task<int> Create(Guid userId, CheckoutRequest request);

        //Task<int> Update(CategoryUpdateRequest request);

        //Task<int> Delete(int categoryId);
        Task<OrderViewModel> GetById(int orderId);
        Task<ApiResult<PagedResult<OrderViewModel>>> GetOrderPaging(GetOrderPagingRequest request);
        Task<int> UpdateStatus(int orderId, int status);
    }
}
