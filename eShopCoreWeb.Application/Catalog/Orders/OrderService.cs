using eShopCoreWeb.Data.EF;
using eShopCoreWeb.Data.Entities;
using eShopCoreWeb.Data.Enums;
using eShopCoreWeb.ViewModels.Catalog.Categories;
using eShopCoreWeb.ViewModels.Catalog.ProductImages;
using eShopCoreWeb.ViewModels.Common;
using eShopCoreWeb.ViewModels.Sales;
using eShopCoreWeb.ViewModels.System.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopCoreWeb.Application.Catalog.Orders
{
    public class OrderService : IOrderService
    {
        private readonly EShopDbContext _context;

        public OrderService(EShopDbContext context)
        {
            _context = context;
        }

        public async Task<int> Create(Guid userId, CheckoutRequest request)
        {
       
            var orderDetails =new List<OrderDetail>();
            foreach(var item in request.OrderDetails)
            {
                var orderItem = new OrderDetail()
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price,                 
                };
                orderDetails.Add(orderItem);
            }
            var order = new Order()
            {
                OrderDate = DateTime.Now,
                ShipAddress = request.Address,
                ShipEmail = request.Email,
                ShipName = request.Name,
                ShipPhoneNumber = request.PhoneNumber,
                UserId = userId,
                Status = Data.Enums.OrderStatus.InProgress,
                OrderDetails = orderDetails
            };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order.Id;
        }


        public Task<List<OrderViewModel>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<OrderViewModel> GetById(int orderId=2)
        {
            var order = await _context.Orders.FindAsync(orderId);
            var orderDetails = await _context.OrderDetails.Where(x => x.OrderId == orderId)
                .Select(i => new OrderDetailViewModel()
                {
                     ProductId = i.ProductId,
                     Quantity = i.Quantity,
                     OrderId = i.OrderId,
                     Price = i.Price,
                }).ToListAsync();
            var total = await _context.OrderDetails.SumAsync(x => x.Price * x.Quantity);
            int active;
            if(order.Status==Data.Enums.OrderStatus.InProgress)
            {
                active = 0;//Đơn hàng đang xử lí
            }
            else if(order.Status==Data.Enums.OrderStatus.Confirmed)
            {
                active = 1;//Đơn hàng đã xác nhận
            }
            else if (order.Status==Data.Enums.OrderStatus.Shipping)
            {
                active = 2;//Đơn hàng đang giao
            }
            else if (order.Status==Data.Enums.OrderStatus.Success)
            {
                active = 3;//Đơn hàng hoàn thành
            }
            else
            {
                active = 4;//Don hang bi huy
            }
            var orderViewModel = new OrderViewModel()
            {
               Id = orderId,
               OrderDate = order.OrderDate,
               ShipAddress = order.ShipAddress,
               ShipEmail = order.ShipEmail,
               ShipName = order.ShipName,
               ShipPhoneNumber = order.ShipPhoneNumber,
               Status = active,
               UserId = order.UserId,
               Total = total,
               OrderDetails = orderDetails
            };
            return orderViewModel;
        }

        public async Task<ApiResult<PagedResult<OrderViewModel>>> GetOrderPaging(GetOrderPagingRequest request)
        {
            var query = _context.Orders;

            //3.Paging
            int totalRow = await query.CountAsync();

            var orderIds = await query
            .Skip((request.PageIndex - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(x => x.Id)
            .ToListAsync();

            var data = new List<OrderViewModel>();
            foreach (var orderId in orderIds)
            {
                var order = await GetById(orderId);
                if (order != null)
                {
                    data.Add(order);
                }
            }
            //4. Select and projection
            var pagedResult = new PagedResult<OrderViewModel>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };
            return new ApiSuccessResult<PagedResult<OrderViewModel>>(pagedResult);
        }

        public async Task<int> UpdateStatus(int orderId, int status)
        {
            OrderStatus active;
            if (status == 0)
            {
                active = Data.Enums.OrderStatus.InProgress;//Đơn hàng đang xử lí
            }
            else if (status == 1)
            {
                active = Data.Enums.OrderStatus.Confirmed;//Đơn hàng đã xác nhận
            }
            else if (status == 2)
            {
                active = Data.Enums.OrderStatus.Shipping;//Đơn hàng đang giao
            }
            else if (status == 3)
            {
                active = Data.Enums.OrderStatus.Success;//Đơn hàng hoàn thành
            }
            else
            {
                active = Data.Enums.OrderStatus.Canceled;//Don hang bi huy
            }
            var order = await _context.Orders.FindAsync(orderId);
            if(order != null)                
                order.Status = active;
            _context.Orders.Update(order);
            return await _context.SaveChangesAsync();
        }
    }
}
