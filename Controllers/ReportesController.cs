using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Inventario360.Services;
using Microsoft.AspNetCore.Authorization;

namespace Inventario360.Controllers
{
    [Authorize] // Protege todas las acciones del controlador
    public class ReportesController : Controller
    {
        private readonly IProductoService _productoService;
        private readonly ISolicitudService _solicitudService;

        public ReportesController(IProductoService productoService, ISolicitudService solicitudService)
        {
            _productoService = productoService;
            _solicitudService = solicitudService;
        }

        public async Task<IActionResult> Index()
        {
            var productos = await _productoService.ObtenerTodos();
            var solicitudes = await _solicitudService.ObtenerTodas(); // Debe devolver una colección


            var reportes = new List<object>
            {
                new { Material = "Total Productos", Cantidad = productos.Count },
                new { Material = "Total Solicitudes", Cantidad = solicitudes.Count } // ✅ Se corrigió el error
            };

            return View(reportes);
        }
    }
}
