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

        // 📌 Página principal de reportes
        public IActionResult Index()
        {
            return View();
        }

        // 📌 Página de Inventario con Tablas
        public IActionResult Inventario()
        {
            return View();
        }

        // 📌 Datos para gráficos en Reportes/Index
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
            int productosStockBajo = productos.Count(p => p.Cantidad < 5);
            int productosOverstock = productos.Count(p => p.Cantidad > 100);

            return Json(new
            {
                productosPorEstado,
                productosPorCategoria,
                totalInventario,
                productosStockBajo,
                productosOverstock
            });
        }

        // 📌 Datos para tablas en Reportes/Inventario
        [HttpGet]
        public async Task<IActionResult> ObtenerInventario()
        {
            var productos = await _productoService.ObtenerTodos();

            var inventarioCompleto = productos.ToList();
            var stockCritico = productos.Where(p => p.Cantidad < 5).ToList();
            var overstock = productos.Where(p => p.Cantidad > 100).ToList();

            Console.WriteLine($"Inventario: {System.Text.Json.JsonSerializer.Serialize(inventarioCompleto)}");
            Console.WriteLine($"Stock Crítico: {System.Text.Json.JsonSerializer.Serialize(stockCritico)}");
            Console.WriteLine($"Overstock: {System.Text.Json.JsonSerializer.Serialize(overstock)}");

            return Json(new
            {
                inventarioCompleto,
                stockCritico,
                overstock
            });
        }


    }
}
