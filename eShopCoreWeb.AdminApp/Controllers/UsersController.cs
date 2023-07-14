using eShopCoreWeb.AdminApp.Services;
using eShopCoreWeb.ViewModels.System.Users;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace eShopCoreWeb.AdminApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IConfiguration _configuration;
        public UsersController(IUserApiClient userApiClient, IConfiguration configuration)
        { 
            _userApiClient = userApiClient;
            _configuration = configuration;
        }
        [Authorize]
        [HttpGet("danh-sach-nguoi-dung")]
        public async Task<IActionResult> Index(string keyword="default", int pageIndex = 1, int pageSize = 1)
        {
            var sessions = HttpContext.Session.GetString("Token");

            var request = new GetUserPagingRequest()
            {
                BearerToken = sessions,
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var data = await _userApiClient.GetUserPaging(request);
            if (data == null)
                return BadRequest("Khong tim duoc");
            return View(data);
        }
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var user = await _userApiClient.GetUserById(id);
            return View(user);
        }
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Users");
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
                return View(ModelState);
            var token = await _userApiClient.Authenticate(request);
            var userPrincipal = this.ValidateToken(token);
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                IsPersistent = false
            };
            HttpContext.Session.SetString("Token", token);
            await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        userPrincipal,
                        authProperties);

            return RedirectToAction("Index", "Home");
        }
        [HttpGet("tao-nguoi-dung")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost("tao-nguoi-dung")]
        public async Task<IActionResult> Create(RegisterRequest request)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            var result = await _userApiClient.Register(request);
            if(result)
            {
                return RedirectToAction("Index", "Users");
            }
            return View(request);
        }
        [HttpGet("cap-nhat")]
        public async Task<IActionResult> Update(Guid id)
        {
            var user = await _userApiClient.GetUserById(id);
            if (user == null)
                return RedirectToAction("Index", "Users");
            var updateRequest = new UserUpdateRequest()
            {
                Dob = user.Dob,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Id = id,
            };
            return View(updateRequest);
        }
        [HttpPost("cap-nhat")]
        public async Task<IActionResult> Update(UserUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var result = await _userApiClient.Udpate(request.Id, request);
            if (result)
            {
                return RedirectToAction("Index", "Users");
            }
            return View(request);
        }
        [HttpGet("xoa-nguoi-dung/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _userApiClient.Delete(id);
            return RedirectToAction("Index", "Users");
        }
        private ClaimsPrincipal ValidateToken(string jwtToken)
        {
            IdentityModelEventSource.ShowPII = true;

            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();

            validationParameters.ValidateLifetime = true;

            validationParameters.ValidAudience = _configuration["Tokens:Issuer"];
            validationParameters.ValidIssuer = _configuration["Tokens:Issuer"];
            validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));

            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);

            return principal;
        }
    }
}
