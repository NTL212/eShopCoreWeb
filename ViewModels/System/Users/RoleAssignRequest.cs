using eShopCoreWeb.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopCoreWeb.ViewModels.System.Users
{
    public class RoleAssignRequest
    {
      public Guid RoleId { get; set; }
        public List<SelectItem> Roles { get; set; } = new List<SelectItem>(); 
    }
}
