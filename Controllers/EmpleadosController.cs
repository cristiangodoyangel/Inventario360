using Microsoft.AspNetCore.Mvc;
using Inventario360.Services;
using Inventario360.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Inventario360.Data;
using Microsoft.EntityFrameworkCore;

namespace Inventario360.Controllers
{
    public class EmpleadosController : Controller
    {
        private readonly IEmpleadoService _empleadoService;
        private readonly IFichaEmpleadoService _fichaEmpleadoService;
        private readonly InventarioDbContext _context;

        public EmpleadosController(IEmpleadoService empleadoService, IFichaEmpleadoService fichaEmpleadoService, InventarioDbContext context)
        {
            _empleadoService = empleadoService;
            _fichaEmpleadoService = fichaEmpleadoService;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var empleados = await _empleadoService.ObtenerTodos();
            return View(empleados);
        }

        public async Task<IActionResult> Detalle(int id)
        {
            var empleado = await _empleadoService.ObtenerPorId(id);
            if (empleado == null) return NotFound();
            return View(empleado);
        }

        public IActionResult Crear()
        {
            return View(new Empleado());
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Empleado empleado)
        {
            if (!ModelState.IsValid)
            {
                return View(empleado);
            }

            await _empleadoService.Agregar(empleado);
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Editar(int id)
        {
            var empleado = await _empleadoService.ObtenerPorId(id);
            if (empleado == null) return NotFound();
            return View(empleado);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Empleado empleado)
        {
            if (!ModelState.IsValid) return View(empleado);

            await _empleadoService.Actualizar(empleado);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Eliminar(int id)
        {
            var empleado = await _empleadoService.ObtenerPorId(id);
            if (empleado == null) return NotFound();
            return View(empleado);
        }

        [HttpPost, ActionName("Eliminar")]
        public async Task<IActionResult> ConfirmarEliminar(int id)
        {
            try
            {
                var empleado = await _empleadoService.ObtenerPorId(id);
                if (empleado == null)
                {
                    TempData["Error"] = "No se encontró el empleado.";
                    return RedirectToAction(nameof(Index));
                }

                var tieneSalidas = await _context.SalidaDeBodega
                    .AnyAsync(s => s.Solicitante == id || s.ResponsableEntrega == id);
                if (tieneSalidas)
                {
                    TempData["Error"] = "No se puede eliminar el empleado porque está asociado a una Salida de Bodega.";
                    return RedirectToAction(nameof(Index));
                }

                _context.Empleado.Remove(empleado);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Empleado eliminado correctamente.";
            }
            catch
            {
                TempData["Error"] = "Error interno al intentar eliminar el empleado.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
