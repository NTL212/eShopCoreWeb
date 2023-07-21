using eShopCoreWeb.ApiIntegration;
using eShopCoreWeb.Utilities.Constants;
using eShopCoreWeb.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


namespace eShopCoreWeb.WebApp.Controllers
{
    public class CartController:Controller
    {
        private readonly IProductApiClient _productApiClient;
        public CartController(IProductApiClient productApiClient)
        {
            _productApiClient = productApiClient;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetListCartItem()
        {
            var session = HttpContext.Session.GetString(SystemConstants.CartSession);
            List<CartItemViewModel> currentCart = new List<CartItemViewModel>();
            if (session != null)
                currentCart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);
            return Ok(currentCart);
        }
        public async Task<IActionResult> AddToCart(int id, string languageId)
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
            int quantity = 1;
            if (currentCart.Any(x => x.ProductId == id))
            {
                currentCart.First(x => x.ProductId == id).Quantity += 1; 
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
    }
}
