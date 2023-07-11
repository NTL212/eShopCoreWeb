using eShopCoreWeb.Application.Catalog.Products;
using eShopCoreWeb.Data.EF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace eShopCoreWeb.Controllers
{
    [Route("cua-hang")]
    public class ProductController : Controller
    {
        private readonly IPublicProductService _publicProductService;
        public ProductController(IPublicProductService productService)
        {
            _publicProductService = productService;
        }
        [Route("san-pham")]
        // GET: ProductsController
        public async Task<IActionResult> Index()
        {

            var products = await _publicProductService.GetAll("vi");
            return View(products);
        }

        // GET: ProductsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProductsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProductsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProductsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
