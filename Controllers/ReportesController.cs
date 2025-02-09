using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Inventario360.Controllers
{
    public class ReportesController : Controller
    {
        public IActionResult Index()
        {
            // Datos simulados para reportes
            var reportes = new List<object>
            {
                new { Material = "Cable UTP", Cantidad = 100 },
                new { Material = "Tornillos 3mm", Cantidad = 250 },
                new { Material = "Cinta adhesiva", Cantidad = 75 }
            };

            return View(reportes);
        }
    }
}
