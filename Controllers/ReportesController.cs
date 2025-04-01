using Microsoft.AspNetCore.Mvc;
using Inventario360.Services;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Inventario360.Data;
using Microsoft.AspNetCore.Authorization;

namespace Inventario360.Controllers
{
    [Authorize(Roles = "Administrador,Gerencia, Proyectos")] // ✅ Solo accesible por Administrador y Gerencia
    public class ReportesController : Controller
    {
        private readonly IProductoService _productoService;
        private readonly InventarioDbContext _context;

        public ReportesController(InventarioDbContext context, IProductoService productoService)
        {
            _context = context;
            _productoService = productoService;
        }

        public async Task<IActionResult> Index()
        {
            var salidas = await _context.SalidaDeBodega
                .Include(s => s.Detalles)
                .ThenInclude(d => d.Producto)
                .Include(s => s.SolicitanteObj)
                .Include(s => s.ResponsableEntregaObj)
                .Include(s => s.ProyectoObj)
                .ToListAsync();

            return View(salidas);
        }

        public IActionResult Inventario()
        {
            return View();
        }

        public IActionResult Salidas()
        {
            return View();
        }

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

            var materialesMasSolicitados = await _context.SalidaDeBodega
                .Where(s => s.Fecha >= DateTime.Now.AddMonths(-1) && s.Detalles.Any())
                .SelectMany(s => s.Detalles)
                .Where(d => d.Producto != null)
                .GroupBy(d => new { d.Producto.NombreTecnico })
                .Select(g => new { Material = g.Key.NombreTecnico, TotalSolicitudes = g.Sum(d => d.Cantidad) })
                .OrderByDescending(g => g.TotalSolicitudes)
                .Take(3)
                .ToListAsync();

            var empleadoMasSolicitante = await _context.SalidaDeBodega
                .Where(s => s.Fecha >= DateTime.Now.AddMonths(-1) && s.SolicitanteObj != null)
                .GroupBy(s => s.SolicitanteObj.Nombre)
                .Select(g => new { Empleado = g.Key, TotalSolicitudes = g.Count() })
                .OrderByDescending(g => g.TotalSolicitudes)
                .FirstOrDefaultAsync();

            var proyectoMasSolicitado = await _context.SalidaDeBodega
                .Where(s => s.Fecha >= DateTime.Now.AddMonths(-1) && s.ProyectoObj != null)
                .GroupBy(s => s.ProyectoObj.Nombre)
                .Select(g => new { Proyecto = g.Key, TotalSolicitudes = g.Count() })
                .OrderByDescending(g => g.TotalSolicitudes)
                .FirstOrDefaultAsync();

            return Json(new
            {
                productosPorEstado,
                productosPorCategoria,
                totalInventario,
                productosStockBajo,
                productosOverstock,
                materialesMasSolicitados = materialesMasSolicitados.Any() ? materialesMasSolicitados : null,
                empleadoMasSolicitante = empleadoMasSolicitante ?? new { Empleado = "Sin datos", TotalSolicitudes = 0 },
                proyectoMasSolicitado = proyectoMasSolicitado ?? new { Proyecto = "Sin datos", TotalSolicitudes = 0 }
            });
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerInventario()
        {
            var productos = await _productoService.ObtenerTodos();

            var inventarioCompleto = productos.ToList();
            var stockCritico = productos.Where(p => p.Cantidad < 5).ToList();
            var overstock = productos.Where(p => p.Cantidad > 100).ToList();

            return Json(new
            {
                inventarioCompleto,
                stockCritico,
                overstock
            });
        }
    }
}
