using eShopCoreWeb.ViewModels.Common;
using eShopCoreWeb.ViewModels.System.Roles;
using eShopCoreWeb.ViewModels.System.Users;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopCoreWeb.Application.System.Users
{
    public interface IRoleService
    {
        Task<List<RoleViewModel>> GetAll();
    }
}
