using Microsoft.AspNetCore.Mvc;

namespace Inventario360.Controllers
{
    public class ProyectosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
