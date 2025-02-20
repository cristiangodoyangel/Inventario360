using Microsoft.AspNetCore.Mvc;
using Inventario360.Services;
using Inventario360.Models;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace Inventario360.Controllers
{
    [Authorize] // 🔹 Aplica autenticación a todas las acciones del controlador
    public class SalidaDeBodegaController : Controller
    {
        private readonly IProductoService _productoService;
        private readonly IEmpleadoService _empleadoService;
        private readonly IProyectoService _proyectoService;
        private readonly ISalidaBodegaService _salidaBodegaService;

        public SalidaDeBodegaController(
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
            ViewBag.Productos = await _productoService.ObtenerTodos();
            ViewBag.Empleados = await _empleadoService.ObtenerTodos();
            ViewBag.Proyectos = await _proyectoService.ObtenerTodos();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(SalidaDeBodega salida)
        {
            if (ModelState.IsValid)
            {
                if (!salida.Producto.HasValue)
                {
                    ModelState.AddModelError("", "Debe seleccionar un producto válido.");
                }
                else
                {
                    var producto = await _productoService.ObtenerPorId(salida.Producto.Value);
                    if (producto != null)
                    {
                        if (producto.Cantidad >= salida.Cantidad)
                        {
                            // **Corrección: Se establece la fecha automáticamente**
                            salida.Fecha = DateTime.Now;

                            // **Corrección: Se actualiza el stock y se guarda la salida en una sola transacción**
                            bool operacionExitosa = await _salidaBodegaService.RegistrarSalida(salida, producto);

                            if (operacionExitosa)
                                return RedirectToAction("Index");
                            else
                                ModelState.AddModelError("", "Error al registrar la salida.");
                        }
                        else
                        {
                            ModelState.AddModelError("", "No hay suficiente stock disponible.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "El producto seleccionado no existe.");
                    }
                }
            }

            ViewBag.Productos = await _productoService.ObtenerTodos();
            ViewBag.Empleados = await _empleadoService.ObtenerTodos();
            ViewBag.Proyectos = await _proyectoService.ObtenerTodos();

            return View(salida);
        }
    }
}
