using Microsoft.AspNetCore.Mvc;
using Inventario360.Services;
using Inventario360.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Inventario360.Controllers
{
    public class ProveedoresController : Controller
    {
        private readonly IProveedorService _proveedorService;

        public ProveedoresController(IProveedorService proveedorService)
        {
            _proveedorService = proveedorService;
        }

        public async Task<IActionResult> Index()
        {
            var proveedores = await _proveedorService.ObtenerTodos();
            return View(proveedores);
        }

        public async Task<IActionResult> Detalle(int id)
        {
            var proveedor = await _proveedorService.ObtenerPorId(id);
            if (proveedor == null) return NotFound();
            return View(proveedor);
        }

        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Proveedor proveedor)
        {
            if (!ModelState.IsValid) return View(proveedor);

            await _proveedorService.Agregar(proveedor);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Editar(int id)
        {
            var proveedor = await _proveedorService.ObtenerPorId(id);
            if (proveedor == null) return NotFound();
            return View(proveedor);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Proveedor proveedor)
        {
            if (!ModelState.IsValid) return View(proveedor);

            await _proveedorService.Actualizar(proveedor);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Eliminar(int id)
        {
            var proveedor = await _proveedorService.ObtenerPorId(id);
            if (proveedor == null) return NotFound();
            return View(proveedor);
        }

        [HttpPost, ActionName("Eliminar")]
        public async Task<IActionResult> ConfirmarEliminar(int id)
        {
            await _proveedorService.Eliminar(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
