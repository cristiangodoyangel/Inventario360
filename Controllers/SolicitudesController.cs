using Microsoft.AspNetCore.Mvc;

namespace Inventario360.Controllers
{
    public class SolicitudesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
