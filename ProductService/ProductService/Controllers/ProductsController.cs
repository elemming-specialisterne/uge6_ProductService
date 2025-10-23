using Microsoft.AspNetCore.Mvc;

namespace ProductService.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
