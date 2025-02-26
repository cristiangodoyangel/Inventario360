using Microsoft.AspNetCore.Mvc;
using Inventario360.Services;
using Inventario360.Models;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

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

        // 📌 Vista principal de Salidas de Bodega
        public async Task<IActionResult> Index()
        {
            var salidas = await _salidaBodegaService.ObtenerTodas();
            return View(salidas);
        }

        // 📌 Ver detalles de una salida de bodega
        public async Task<IActionResult> Detalle(int id)
        {
            var salida = await _salidaBodegaService.ObtenerPorId(id);
            if (salida == null)
            {
                return NotFound();
            }
            return View(salida);
        }

        // 📌 Formulario para crear una nueva salida
        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            ViewBag.Productos = await _productoService.ObtenerTodos();
            ViewBag.Empleados = await _empleadoService.ObtenerTodos();
            ViewBag.Proyectos = await _proyectoService.ObtenerTodos();

            return View();
        }

        // 📌 Procesar la creación de una salida
        [HttpPost]
        public async Task<IActionResult> Crear(SalidaDeBodega salida, string ProductosJson)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(ProductosJson))
                {
                    ModelState.AddModelError("", "Debe agregar al menos un producto a la salida de bodega.");
                }
                else
                {
                    try
                    {
                        // **Deserializar la lista de productos**
                        var productos = JsonConvert.DeserializeObject<List<DetalleSalidaDeBodega>>(ProductosJson);

                        if (productos == null || productos.Count == 0)
                        {
                            ModelState.AddModelError("", "Error al procesar la lista de productos.");
                        }
                        else
                        {
                            salida.Fecha = DateTime.Now;

                            // **Registrar la salida y actualizar el stock en una transacción**
                            bool operacionExitosa = await _salidaBodegaService.RegistrarSalidaConProductos(salida, productos);

                            if (operacionExitosa)
                                return Json(new { success = true });
                            else
                                ModelState.AddModelError("", "Error al registrar la salida.");
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Error interno: " + ex.Message);
                    }
                }
            }

            ViewBag.Productos = await _productoService.ObtenerTodos();
            ViewBag.Empleados = await _empleadoService.ObtenerTodos();
            ViewBag.Proyectos = await _proyectoService.ObtenerTodos();

            return Json(new { success = false, message = "Error al registrar la salida." });
        }

        // 📌 Método para eliminar una salida de bodega
        [HttpPost]
        public async Task<IActionResult> Eliminar(int id)
        {
            var salida = await _salidaBodegaService.ObtenerPorId(id);
            if (salida == null)
            {
                return Json(new { success = false, message = "No se encontró la salida de bodega" });
            }

            try
            {
                // 🔹 **Eliminar primero los detalles de la salida**
                await _salidaBodegaService.EliminarDetalles(id);

                // 🔹 **Luego eliminar la salida**
                await _salidaBodegaService.Eliminar(id);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al eliminar: " + ex.Message });
            }
        }


    }
}
