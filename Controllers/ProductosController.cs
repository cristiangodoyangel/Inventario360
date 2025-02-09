using Microsoft.AspNetCore.Mvc;
using Inventario360.Services;
using Inventario360.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Inventario360.Controllers
{
    public class ProductosController : Controller
    {
        private readonly IProductoService _productoService;

        public ProductosController(IProductoService productoService)
        {
            _productoService = productoService;
        }

        public async Task<IActionResult> Index()
        {
            var productos = await _productoService.ObtenerTodos();
            return View(productos);
        }

        public async Task<IActionResult> Detalle(int id)
        {
            var producto = await _productoService.ObtenerPorId(id);
            if (producto == null) return NotFound();
            return View(producto);
        }

        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Producto producto)
        {
            if (!ModelState.IsValid) return View(producto);

            await _productoService.Agregar(producto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Editar(int id)
        {
            var producto = await _productoService.ObtenerPorId(id);
            if (producto == null) return NotFound();
            return View(producto);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Producto producto)
        {
            if (!ModelState.IsValid) return View(producto);

            await _productoService.Actualizar(producto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Eliminar(int id)
        {
            var producto = await _productoService.ObtenerPorId(id);
            if (producto == null) return NotFound();
            return View(producto);
        }

        [HttpPost, ActionName("Eliminar")]
        public async Task<IActionResult> ConfirmarEliminar(int id)
        {
            await _productoService.Eliminar(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
