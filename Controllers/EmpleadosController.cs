using Microsoft.AspNetCore.Mvc;

namespace Inventario360.Controllers
{
    public class EmpleadosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
