using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopCoreWeb.ViewModels.Sales
{
    public class OrderDetailViewModel
    {
        public int OrderId { get; set; }
        public string? ProductName { get; set; }
        public string? ThumnailImage { set; get; }
        public int ProductId { get; set; }
        public decimal Price { get; set; } 
        public int Quantity { get; set; }
    }
}
