using eShopCoreWeb.ViewModels.Catalog.Products;
using eShopCoreWeb.ViewModels.Utilities.Slides;

namespace eShopCoreWeb.WebApp.Models
{
    public class HomeViewModel
    {
        public List<SlideViewModel> Slides { get; set; }
        public List<ProductViewModel> FeaturedProducts { get; set; }
        public List<ProductViewModel> LastestProduct { get; set; }
    }
}