using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopCoreWeb.ViewModels.System.Users
{
    public class CreateGoogleUserRequest
    {
        public string? ProviderKey { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
