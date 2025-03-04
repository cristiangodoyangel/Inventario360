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

        public async Task<IActionResult> Index()
        {
            var productos = await _productoService.ObtenerTodos();

            // 📊 Productos por estado (Nuevos vs. Usados)
            var productosPorEstado = productos
                .GroupBy(p => p.Estado)
                .Select(g => new { Estado = g.Key, Cantidad = g.Count() })
                .ToList();

            // 📊 Productos por categoría
            var productosPorCategoria = productos
                .GroupBy(p => p.Categoria)
                .Select(g => new { Categoria = g.Key ?? "Sin Categoría", Cantidad = g.Count() })
                .ToList();

            // 📦 Total de productos en inventario

            int total = miVariableNullable ?? 0;



            // ⚠️ Productos con stock bajo (Menos de 10)
            int productosStockBajo = productos.Count(p => p.Cantidad < 10);

            // 🔴 Productos en overstock (Más de 300)
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
