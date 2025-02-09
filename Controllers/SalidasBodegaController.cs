using Microsoft.AspNetCore.Mvc;

namespace Inventario360.Controllers
{
    public class SalidasBodegaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
