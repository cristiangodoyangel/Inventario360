using Microsoft.AspNetCore.Mvc;
using Inventario360.Services;
using Inventario360.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Inventario360.Controllers
{
    public class SalidasBodegaController : Controller
    {
        private readonly ISalidaBodegaService _salidaBodegaService;

        public SalidasBodegaController(ISalidaBodegaService salidaBodegaService)
        {
            _salidaBodegaService = salidaBodegaService;
        }

        public async Task<IActionResult> Index()
        {
            var salidas = await _salidaBodegaService.ObtenerTodas();
            return View(salidas);
        }

        public async Task<IActionResult> Detalle(int id)
        {
            var salida = await _salidaBodegaService.ObtenerPorId(id);
            if (salida == null) return NotFound();
            return View(salida);
        }

        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(SalidaDeBodega salida)
        {
            if (!ModelState.IsValid) return View(salida);

            await _salidaBodegaService.Agregar(salida);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Editar(int id)
        {
            var salida = await _salidaBodegaService.ObtenerPorId(id);
            if (salida == null) return NotFound();
            return View(salida);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(SalidaDeBodega salida)
        {
            if (!ModelState.IsValid) return View(salida);

            await _salidaBodegaService.Actualizar(salida);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Eliminar(int id)
        {
            var salida = await _salidaBodegaService.ObtenerPorId(id);
            if (salida == null) return NotFound();
            return View(salida);
        }

        [HttpPost, ActionName("Eliminar")]
        public async Task<IActionResult> ConfirmarEliminar(int id)
        {
            await _salidaBodegaService.Eliminar(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
