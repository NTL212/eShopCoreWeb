using eShopCoreWeb.ApiIntegration;
using eShopCoreWeb.ViewModels.Sales;
using eShopCoreWeb.ViewModels.System.Users;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using System.Drawing.Printing;
using NuGet.Protocol;
using Microsoft.AspNetCore.Authorization;

namespace eShopCoreWeb.WebApp.Controllers
{
    [Authorize]
	public class UserController : Controller
	{
        private readonly IUserApiClient _userApiClient;
        private readonly IOrderApiClient _orderApiClient;
        private readonly IProductApiClient _productApiClient;
        private readonly IConfiguration _configuration;

        public UserController(IUserApiClient userApiClient,
            IConfiguration configuration,
            IOrderApiClient orderApiClient, IProductApiClient productApiClient)
        {
            _userApiClient = userApiClient;
            _configuration = configuration;
            _orderApiClient = orderApiClient;
            _productApiClient = productApiClient;
        }
        public IActionResult Index()
		{
			return View();
		}

        [HttpGet]
        public async Task<IActionResult> Update(string username)
        {
            var result = await _userApiClient.GetUserByUserName(username);
            if (result.IsSuccessed)
            {
                var user = result.ResultObj;
                var updateRequest = new UserUpdateRequest()
                {
                    Dob = user.Dob,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    Id = user.Id
                };
                return View(updateRequest);
            }
            return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Update(UserUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _userApiClient.Update(request.Id, request);
            if (result.IsSuccessed)
            {
                TempData["result"] = "Cập nhật người dùng thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", result.Message);
            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> ListOrder(string username)
        {
            var listOrder = await _orderApiClient.GetAllByUserName(username);
            if(listOrder!=null)
            {

                ViewBag.ResultMsg = TempData["result"];
            }    
            return View(listOrder);
        }
        [HttpGet]
        public async Task<IActionResult> OrderDetails(int orderId)
        {
            var order = await _orderApiClient.GetOrderById(orderId);
            return View(order.OrderDetails);
        }
        [HttpGet]
        public async Task<IActionResult> UpdateStatus(int orderId, string username)
        {
            // Gọi API để cập nhật trạng thái đơn hàng
            var order = _orderApiClient.GetOrderById(orderId);
            if(order.Result.Status==1)
            {
                TempData["result"] = $"Cập nhật status thất bại do đơn hàng đã xác nhận";
                return RedirectToAction("ListOrder", new { username = username });
            }    
            var result = await _orderApiClient.UpdateStatus(orderId, 4);
            if (result)
            {
                TempData["result"] = $"Cập nhật status thành công";
            }
            else
            {
                TempData["result"] = $"Cập nhật status của {order.Result.ShipName} thất bại";
            }
            return RedirectToAction("ListOrder", new {username=username});
        }
    }
}
