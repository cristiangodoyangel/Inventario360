using Microsoft.AspNetCore.Mvc;

namespace Inventario360.Controllers
{
    public class ProveedoresController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
