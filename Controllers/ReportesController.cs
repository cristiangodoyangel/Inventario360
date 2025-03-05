using Microsoft.AspNetCore.Mvc;
using Inventario360.Services;
using System.Linq;
using System.Threading.Tasks;

namespace Inventario360.Controllers
{
    public class ReportesController : Controller
    {
        private readonly IProductoService _productoService;

        public ReportesController(IProductoService productoService)
        {
            _productoService = productoService;
        }

        // 📌 MÉTODO QUE DEVUELVE LA VISTA (NO JSON)
        public IActionResult Index()
        {
            return View(); // ✅ Ahora solo devuelve la vista, sin datos JSON
        }

        // 📌 MÉTODO QUE DEVUELVE LOS DATOS JSON
        [HttpGet]
        public async Task<IActionResult> ObtenerDatosReportes()
        {
            var productos = await _productoService.ObtenerTodos();

            var productosPorEstado = productos
                .GroupBy(p => p.Estado)
                .Select(g => new { Estado = g.Key, Cantidad = g.Count() })
                .ToList();

            var productosPorCategoria = productos
                .GroupBy(p => p.Categoria)
                .Select(g => new { Categoria = g.Key ?? "Sin Categoría", Cantidad = g.Count() })
                .ToList();

            int totalInventario = productos.Count();
            int productosStockBajo = productos.Count(p => p.Cantidad < 10);
            int productosOverstock = productos.Count(p => p.Cantidad > 300);

            return Json(new
            {
                productosPorEstado,
                productosPorCategoria,
                totalInventario,
                productosStockBajo,
                productosOverstock
            });
        }
    }
}
