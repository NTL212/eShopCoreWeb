using eShopCoreWeb.Data.Entities;
using eShopCoreWeb.ViewModels.Common;
using eShopCoreWeb.ViewModels.System.Roles;
using eShopCoreWeb.ViewModels.System.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopCoreWeb.Application.System.Users
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<AppRole> _roleManager;
        public RoleService(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task<List<RoleViewModel>> GetAll()
        {
            var roles = await _roleManager.Roles.Select(x => new RoleViewModel()
            {
                Name = x.Name,
                Id = x.Id,
                Description = x.Description,
            }).ToListAsync();
            return roles;
        }
    }
}
