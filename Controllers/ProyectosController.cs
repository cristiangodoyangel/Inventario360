using Microsoft.AspNetCore.Mvc;
using Inventario360.Services;
using Inventario360.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;


namespace Inventario360.Controllers
{
    public class ProyectosController : Controller
    {
        private readonly IProyectoService _proyectoService;

        public ProyectosController(IProyectoService proyectoService)
        {
            _proyectoService = proyectoService;
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

        [HttpPost, ActionName("Eliminar")]
        public async Task<IActionResult> ConfirmarEliminar(int id)
        {
            await _proyectoService.Eliminar(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
