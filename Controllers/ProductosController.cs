using Microsoft.AspNetCore.Mvc;

namespace Inventario360.Controllers
{
    public class ProductosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
