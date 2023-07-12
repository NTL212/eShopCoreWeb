using eShopCoreWeb.AdminApp.Services;
using eShopCoreWeb.ViewModels.System.Users;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;

namespace eShopCoreWeb.AdminApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserApiClient _userApiClient;
        public UsersController(IUserApiClient userApiClient)
        {
            _userApiClient = userApiClient;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
                return View(ModelState);
            var token = await _userApiClient.Authenticate(request);
            return View(token);
        }
    }
}
