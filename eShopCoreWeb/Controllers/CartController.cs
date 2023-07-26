using eShopCoreWeb.ApiIntegration;
using eShopCoreWeb.Utilities.Constants;
using eShopCoreWeb.ViewModels.Sales;
using eShopCoreWeb.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace eShopCoreWeb.WebApp.Controllers
{
    public class CartController:Controller
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IProductApiClient _productApiClient;
        private readonly IOrderApiClient _orderApiClient;
        public CartController(IProductApiClient productApiClient, IOrderApiClient orderApiClient, IUserApiClient userApiClient)
        {
            _productApiClient = productApiClient;
            _orderApiClient = orderApiClient;
            _userApiClient = userApiClient;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Checkout()
        {
            return View(GetCheckoutViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutViewModel request)
        {
            var userName = User.Identity.Name;
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var userObj = await _userApiClient.GetUserByUserName(userName);
            if(userObj==null)
            {
                userObj = await _userApiClient.GetUserByEmail(userEmail);
            }
            var model = GetCheckoutViewModel();
            var orderDetails = new List<OrderDetailViewModel>();
            foreach (var item in model.CartItems)
            {
                orderDetails.Add(new OrderDetailViewModel()
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price,
                });
            }
            var checkoutRequest = new CheckoutRequest()
            {
                Address = request.CheckoutModel.Address,
                Name = request.CheckoutModel.Name,
                Email = request.CheckoutModel.Email,
                PhoneNumber = request.CheckoutModel.PhoneNumber,
                OrderDetails = orderDetails
            };
            var result = await _orderApiClient.CreateOrder(userObj.ResultObj.Id, checkoutRequest);
            if (result)
            {
                TempData["SuccessMsg"] = "Order puschased successful";
                return View(model);
            }
            TempData["SuccessMsg"] = "Order puschased fail";
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> GetListCartItem()
        {
            var order = await _orderApiClient.GetOrderById(3);
            var session = HttpContext.Session.GetString(SystemConstants.CartSession);
            List<CartItemViewModel> currentCart = new List<CartItemViewModel>();
            if (session != null)
                currentCart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);
            return Ok(currentCart);
        }
        public async Task<IActionResult> AddToCart(int id, string languageId, int cartQuantity)
        {
            //if (!User.Identity.IsAuthenticated)
            //{
            //    // Lưu đường dẫn hiện tại vào TempData với tên "ReturnUrl"
            //    TempData["ReturnUrl"] = Url.Action("AddToCart", "Cart", new { Id = id, languageId = languageId });

            //    // Redirect đến trang đăng nhập
            //    return RedirectToAction("Index", "Account");
            //}
            var product = await _productApiClient.GetProductById(id, languageId);
            var session = HttpContext.Session.GetString(SystemConstants.CartSession);
            List<CartItemViewModel> currentCart = new List<CartItemViewModel>();
            if (session != null)
                currentCart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);  
            int quantity;
            if(cartQuantity!=null || cartQuantity>0)
            {
                quantity = cartQuantity;
            }
            else
            {
                quantity = 1;
            }
            if (currentCart.Any(x => x.ProductId == id))
            {
                currentCart.First(x => x.ProductId == id).Quantity += quantity; 
                HttpContext.Session.SetString(SystemConstants.CartSession, JsonConvert.SerializeObject(currentCart));
                return Ok(currentCart);
            }

            var cartItem = new CartItemViewModel()
            {
                ProductId = id,
                Description = product.Description,
                Image = product.ThumnailImage,
                Name = product.Name,
                Price = product.Price,
                Quantity = quantity,
                UserName = User.Identity.Name,
            };

            currentCart.Add(cartItem);

            HttpContext.Session.SetString(SystemConstants.CartSession, JsonConvert.SerializeObject(currentCart));
            return Ok(currentCart);
        }

        public  IActionResult UpdateCart(int id, int quantity)
        {
            var session = HttpContext.Session.GetString(SystemConstants.CartSession);
            List<CartItemViewModel> currentCart = new List<CartItemViewModel>();
            if (session != null)
                currentCart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);
            foreach(var item in currentCart)
            {
                if (item.ProductId == id)
                {
                    if (quantity == 0)
                    {
                        currentCart.Remove(item);
                        break;
                    }    
                    item.Quantity = quantity;
                };
            }    
            HttpContext.Session.SetString(SystemConstants.CartSession, JsonConvert.SerializeObject(currentCart));
            return Ok(currentCart);
        }
        private CheckoutViewModel GetCheckoutViewModel()
        {
            var session = HttpContext.Session.GetString(SystemConstants.CartSession);
            List<CartItemViewModel> currentCart = new List<CartItemViewModel>();
            if (session != null)
                currentCart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);
            var checkoutVm = new CheckoutViewModel()
            {
                CartItems = currentCart,
                CheckoutModel = new CheckoutRequest()
            };
            return checkoutVm;
        }
    }
}
