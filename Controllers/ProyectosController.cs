using Microsoft.AspNetCore.Mvc;
using Inventario360.Services;
using Inventario360.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Inventario360.Data;


namespace Inventario360.Controllers
{
    public class ProyectosController : Controller
    {
        private readonly InventarioDbContext _context;
        private readonly IProyectoService _proyectoService;

        public ProyectosController(IProyectoService proyectoService, InventarioDbContext context)
        {
            _proyectoService = proyectoService;
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            var proyectos = await _proyectoService.ObtenerTodos();
            return View(proyectos);
        }

        public async Task<IActionResult> Detalle(int id)
        {
            var proyecto = await _proyectoService.ObtenerPorId(id);
            if (proyecto == null) return NotFound();
            return View(proyecto);
        }

        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Proyecto proyecto)
        {
            if (!ModelState.IsValid) return View(proyecto);

            await _proyectoService.Agregar(proyecto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Editar(int id)
        {
            var proyecto = await _proyectoService.ObtenerPorId(id);
            if (proyecto == null) return NotFound();
            return View(proyecto);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Proyecto proyecto)
        {
            if (!ModelState.IsValid) return View(proyecto);

            await _proyectoService.Actualizar(proyecto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Eliminar(int id)
        {
            var proyecto = await _proyectoService.ObtenerPorId(id);
            if (proyecto == null) return NotFound();
            return View(proyecto);
        }

        [Authorize(Roles = "Administrador, Proyectos")]
        [HttpPost, ActionName("Eliminar")]
        public async Task<IActionResult> ConfirmarEliminar(int id)
        {
            try
            {
                var proyecto = await _proyectoService.ObtenerPorId(id);
                if (proyecto == null)
                {
                    TempData["Error"] = "No se encontró el proyecto.";
                    return RedirectToAction(nameof(Index));
                }

                var tieneSalidas = await _context.SalidaDeBodega.AnyAsync(s => s.ProyectoAsignado == id);
                if (tieneSalidas)
                {
                    TempData["Error"] = "No se puede eliminar el proyecto porque está asociado a una salida de bodega.";
                    return RedirectToAction(nameof(Index));
                }

                _context.Proyecto.Remove(proyecto);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Proyecto eliminado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error interno: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

    }
}
