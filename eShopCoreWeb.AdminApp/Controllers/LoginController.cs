using eShopCoreWeb.AdminApp.Services;
using eShopCoreWeb.ViewModels.System.Users;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace eShopCoreWeb.AdminApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IConfiguration _configuration;
        //Constructor của LoginController nhận vào các dependency IUserApiClient và IConfiguration thông qua dependency injection.
        public LoginController(IUserApiClient userApiClient,
            IConfiguration configuration)
        {
            _userApiClient = userApiClient;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Đăng xuất người dùng hiện tại (nếu có)
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);
            // Gọi API để xác thực thông tin đăng nhập
            var result = await _userApiClient.Authenticate(request);
            if (result.ResultObj == null)
            {
                // Nếu xác thực không thành công, hiển thị thông báo lỗi và trở lại trang đăng nhập
                ModelState.AddModelError("", result.Message);
                return View(request);
            }
            // Xác thực thành công, tạo ClaimsPrincipal từ JWT token
            var userPrincipal = this.ValidateToken(result.ResultObj);
            
            // Thiết lập các thuộc tính cho việc đăng nhập bằng cookie
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                IsPersistent = false
            };
            // Lưu JWT token vào Session để sử dụng trong các request sau này
            HttpContext.Session.SetString("Token", result.ResultObj);

            // Đăng nhập người dùng bằng cookie và chuyển hướng đến trang chủ
            await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        userPrincipal,
                        authProperties);

            return RedirectToAction("Index", "Home");
        }
        // Xác thực JWT token và trả về ClaimsPrincipal
        private ClaimsPrincipal ValidateToken(string jwtToken)
        {
            // Cho phép hiển thị thông tin cá nhân (PII) trong quá trình xác thực
            IdentityModelEventSource.ShowPII = true;
            
            // Xác thực token và trả về principal
            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();
            
            // Xác thực hạn sử dụng token
            validationParameters.ValidateLifetime = true;

            // Xác thực audience và issuer của token
            validationParameters.ValidAudience = _configuration["Tokens:Issuer"];
            validationParameters.ValidIssuer = _configuration["Tokens:Issuer"];
            
            // Xác thực chữ ký của token bằng SymmetricSecurityKey
            validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));

            // Xác thực token và trả về principal
            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);

            return principal;
        }
    }
}