using Microsoft.AspNetCore.Mvc;

namespace Inventario360.Controllers
{
    public class ReportesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
