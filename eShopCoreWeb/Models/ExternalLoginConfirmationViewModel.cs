﻿using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace eShopCoreWeb.WebApp.Models
{
    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
