using eShopCoreWeb.ApiIntegration;
using eShopCoreWeb.ViewModels.System.Users;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace eShopCoreWeb.WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IConfiguration _configuration;

        public AccountController(IUserApiClient userApiClient,
            IConfiguration configuration)
        {
            _userApiClient = userApiClient;
            _configuration = configuration;
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
    {
        // Request a redirect to the external login provider
        var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });
        var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
        return Challenge(properties, provider);
    }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            // Handle the external login callback
            if (remoteError != null)
            {
                // Handle external login failure
                // You can customize the error handling here
                return RedirectToAction("Index");
            }

            var info = await HttpContext.AuthenticateAsync();
            if (info?.Principal != null)
            {
                // External login successful, process the user data
                // You can access the user's information through 'info.Principal'
                // External login successful, process the user data
                var userEmail = info.Principal.FindFirstValue(ClaimTypes.Email);
                var userName = info.Principal.FindFirstValue(ClaimTypes.Name);
                var providerKey = info.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
                // Here, you can use the userEmail and userName to create a user account
                // in your application's database or perform any other required actions.
                var user = await _userApiClient.GetUserByEmail(userEmail);
                if (user.IsSuccessed == false)
                {
                    var createGoogleUserRequest = new CreateGoogleUserRequest()
                    {
                        UserName = userName,
                        Email = userEmail,
                        ProviderKey = providerKey,
                    };
                    await _userApiClient.CreateGoogleUser(createGoogleUserRequest);
                }
                // After creating the user account, you can sign in the user using cookies:
               
            }

                // Redirect back to the original URL after successful login
                if (returnUrl != null)
                {
                    return LocalRedirect(returnUrl);
                }

                return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Đăng xuất người dùng hiện tại (nếu có)
            //await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
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
            // Kiểm tra xem có TempData "ReturnUrl" hay không
            if (TempData["ReturnUrl"] != null)
            {
                // Nếu có, redirect đến đường dẫn trong TempData "ReturnUrl"
                var returnUrl = TempData["ReturnUrl"].ToString();
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Account");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _userApiClient.Register(request);
            if (result.IsSuccessed)
            {
                TempData["result"] = "Thêm người dùng thành công";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", result.Message);
            return View(request);
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
