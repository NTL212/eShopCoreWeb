namespace eShopCoreWeb.WebApp.Models
{
    public class CartItemViewModel
    {
        public string? UserName { get; set; }
        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public string? Description { get; set; }

        public string? Name { get; set; }

        public string? Image { get; set; }

        public decimal Price { get; set; }
    }
}
