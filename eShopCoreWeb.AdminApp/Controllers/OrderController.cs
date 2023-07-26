using eShopCoreWeb.ApiIntegration;
using eShopCoreWeb.ViewModels.Sales;
using eShopCoreWeb.ViewModels.System.Users;
using Microsoft.AspNetCore.Mvc;

namespace eShopCoreWeb.AdminApp.Controllers
{
    public class OrderController : Controller
    {
        private IOrderApiClient _orderApiClient;
        public OrderController(IOrderApiClient orderApiClient)
        {
            _orderApiClient = orderApiClient;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Index(string keyword = "default", int pageIndex = 1, int pageSize = 2)
        {
            var sessions = HttpContext.Session.GetString("Token");

            var request = new GetOrderPagingRequest()
            {
                BearerToken = sessions,
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var data = await _orderApiClient.GetOrderPaging(request);
            if (data == null)
                return BadRequest("Khong tim duoc");
            ViewBag.ResultMsg = TempData["result"];
            return View(data.ResultObj);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int orderId, int status)
        {
            // Gọi API để cập nhật trạng thái đơn hàng
            var order = _orderApiClient.GetOrderById(orderId);  
            var result = await _orderApiClient.UpdateStatus(orderId, status);
            if (result)
            {
                TempData["result"] = $"Cập nhật status của {order.Result.ShipName} thành công";
            }
            else
            {
                TempData["result"] = $"Cập nhật status của {order.Result.ShipName} thất bại";
            }
            return RedirectToAction("Index");
        }
    }
}
