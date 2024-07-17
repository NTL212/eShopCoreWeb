
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
using Microsoft.AspNetCore.Http;
using eShopCoreWeb.ViewModels.Common;
using eShopCoreWeb.ApiIntegration;

namespace eShopCoreWeb.AdminApp.Controllers
{
    [Authorize(Roles = "admin")]
    public class UsersController : Controller
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IRoleApiClient _roleApiClient;
        private readonly IConfiguration _configuration;
        [AllowAnonymous]
        public IActionResult Forbidden()
        {
            return View();
        }
        public UsersController(IUserApiClient userApiClient, IRoleApiClient roleApiClient, IConfiguration configuration)
        { 
            _userApiClient = userApiClient;
            _configuration = configuration;
            _roleApiClient = roleApiClient;
        }
        [HttpGet("danh-sach-nguoi-dung")]
        public async Task<IActionResult> Index(string keyword="default", int pageIndex = 1, int pageSize = 2)
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
            ViewBag.ResultMsg = TempData["result"];
            return View(data.ResultObj);
        }
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var user = await _userApiClient.GetUserById(id);
            return View(user.ResultObj);
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Login");
        }
        
        [HttpGet("tao-nguoi-dung")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost("tao-nguoi-dung")]
        public async Task<IActionResult> Create(RegisterRequest request)
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
        [HttpGet("cap-nhat")]
        public async Task<IActionResult> Update(Guid id)
        {
            var result = await _userApiClient.GetUserById(id);
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
                    Id = id
                };
                return View(updateRequest);
            }
            return RedirectToAction("Error", "Home");
        }

        [HttpPost("cap-nhat")]
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

        [HttpGet("xoa-nguoi-dung/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _userApiClient.Delete(id);
            if(result.IsSuccessed)
            {
                TempData["result"] = "Xóa người dùng thành công";
                return RedirectToAction("Index");
            }
            TempData["result"] = "Xóa người dùng không thành công";
            return RedirectToAction("Index");

        }
        [HttpGet("cap-nhat-role")]
        public async Task<IActionResult> RoleAssign(Guid id)
        {
            var roleAssignRequsest = await GetRoleAssignRequest(id);
            return View(roleAssignRequsest);
        }

        [HttpPost("cap-nhat-role")]
        public async Task<IActionResult> RoleAssign(RoleAssignRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _userApiClient.RoleAssign(request.RoleId, request);

            if (result.IsSuccessed)
            {
                TempData["result"] = "Cập nhật quyền thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", result.Message);
            var roleAssignRequest = await GetRoleAssignRequest(request.RoleId);

            return View(roleAssignRequest);
        }
        public async Task<RoleAssignRequest> GetRoleAssignRequest(Guid id)
        {
            var userObj = await _userApiClient.GetUserById(id);
            var roleObj = await _roleApiClient.GetAll();
            var roleAssignRequest = new RoleAssignRequest();
            roleAssignRequest.RoleId = userObj.ResultObj.Id;
            foreach (var role in roleObj.ResultObj)
            {
                roleAssignRequest.Roles.Add(new SelectItem()
                {
                    Id = role.Id.ToString(),
                    Name = role.Name,
                    Selected = userObj.ResultObj.Roles.Contains(role.Name)
                });
            }
            return roleAssignRequest;
        }
    }
}
