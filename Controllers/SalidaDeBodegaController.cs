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

        // Acción para mostrar todas las salidas de bodega
        public async Task<IActionResult> Index()
        {
            var salidas = await _salidaBodegaService.ObtenerTodas();
            return View(salidas);
        }

        // Acción para ver el detalle de una salida específica
        public async Task<IActionResult> Details(int id)
        {
            var salida = await _salidaBodegaService.ObtenerPorId(id);
            if (salida == null)
            {
                return NotFound();
            }
            return View(salida);
        }

        // Acción para mostrar el formulario de creación
        public IActionResult Create()
        {
            return View();
        }

        // Acción para procesar la creación de una nueva salida de bodega
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SalidaDeBodega salida)
        {
            if (ModelState.IsValid)
            {
                await _salidaBodegaService.Agregar(salida);
                return RedirectToAction(nameof(Index));
            }
            return View(salida);
        }
    }
}
