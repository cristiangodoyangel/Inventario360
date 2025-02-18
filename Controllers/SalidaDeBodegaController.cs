using Microsoft.AspNetCore.Mvc;
using Inventario360.Services;
using Inventario360.Models;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace Inventario360.Controllers
{
    [Authorize] // Protege todas las acciones del controlador
    public class SalidasBodegaController : Controller
    {
        private readonly IProductoService _productoService;
        private readonly IEmpleadoService _empleadoService;
        private readonly IProyectoService _proyectoService;
        private readonly ISalidaBodegaService _salidaBodegaService;

        public SalidasBodegaController(
            ISalidaBodegaService salidaBodegaService,
            IProductoService productoService,
            IEmpleadoService empleadoService,
            IProyectoService proyectoService)
        {
            _salidaBodegaService = salidaBodegaService;
            _productoService = productoService;
            _empleadoService = empleadoService;
            _proyectoService = proyectoService;
        }

        public async Task<IActionResult> Index()
        {
            var salidas = await _salidaBodegaService.ObtenerTodas();
            return View(salidas);
        }

        public async Task<IActionResult> Details(int id)
        {
            var salida = await _salidaBodegaService.ObtenerPorId(id);
            if (salida == null)
            {
                return NotFound();
            }
            return View(salida);
        }

        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            await CargarDatosVista();
            return View(new SalidaDeBodega { Detalles = new List<DetalleSalidaDeBodega>() });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(SalidaDeBodega salida, List<DetalleSalidaDeBodega> detalles)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Debe completar todos los campos obligatorios.");
                await CargarDatosVista();
                return View(salida);
            }

            if (detalles == null || detalles.Count == 0)
            {
                ModelState.AddModelError("", "Debe seleccionar al menos un producto para la salida.");
                await CargarDatosVista();
                return View(salida);
            }

            salida.Detalles = detalles; // Asignar la lista de detalles a la salida

            bool resultado = await _salidaBodegaService.RegistrarSalida(salida, salida.Detalles);


            if (!resultado)
            {
                ModelState.AddModelError("", "Error al registrar la salida, verifique el stock.");
                await CargarDatosVista();
                return View(salida);
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task CargarDatosVista()
        {
            ViewBag.Productos = await _productoService.ObtenerTodos();
            ViewBag.Empleados = await _empleadoService.ObtenerTodos();
            ViewBag.Proyectos = await _proyectoService.ObtenerTodos();
        }
    }
}
