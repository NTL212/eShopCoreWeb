using eShopCoreWeb.Application.Catalog.Categories;
using eShopCoreWeb.Application.Catalog.Orders;
using eShopCoreWeb.ViewModels.Catalog.Categories;
using eShopCoreWeb.ViewModels.Sales;
using eShopCoreWeb.ViewModels.System.Users;
using Microsoft.AspNetCore.Mvc;

namespace eShopCoreWeb.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController:Controller
    {
        private readonly IOrderService _orderService;

        public OrdersController(
           IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpGet("getbyuser/{username}")]
        public async Task<IActionResult> GetAllByUserName(string username)
        {
            var orders = await _orderService.GetAllByUserName(username);
            return Ok(orders);
        }
        [HttpGet("paging")]
        public async Task<IActionResult> GetUserPaging([FromQuery] GetOrderPagingRequest request)
        {
            var orders = await _orderService.GetOrderPaging(request);
            return Ok(orders);
        }
        [HttpPost("{userId}")]
        public async Task<IActionResult> Create(Guid userId, [FromBody] CheckoutRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var orderId = await _orderService.Create(userId,request);
            if (orderId == 0)
                return BadRequest();
            var category = await _orderService.GetById(orderId);
            return CreatedAtAction(nameof(GetById), new { id = orderId }, category);
        }
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetById(int orderId)
        {
            var category = await _orderService.GetById(orderId);
            if (category == null)
                return BadRequest("Cannot find order");
            return Ok(category);
        }
        [HttpPut("{orderId}/status")]
        public async Task<IActionResult> UpdateStatus(int orderId,[FromBody] int status)
        {
            var affectedResult = await _orderService.UpdateStatus(orderId, status);
            if (affectedResult == 0)
                return BadRequest();
            return Ok();
        }
    }
}
