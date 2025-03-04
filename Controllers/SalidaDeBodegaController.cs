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

            // ✅ Obtener los detalles de la salida
            var detalles = await _salidaBodegaService.ObtenerDetallesPorSalida(id);

            ViewBag.Detalles = detalles;

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
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Datos inválidos." });
            }

            if (string.IsNullOrEmpty(ProductosJson))
            {
                return Json(new { success = false, message = "Debe agregar al menos un producto a la salida de bodega." });
            }

            try
            {
                // **Deserializar la lista de productos**
                var productos = JsonConvert.DeserializeObject<List<DetalleSalidaDeBodega>>(ProductosJson);

                if (productos == null || productos.Count == 0)
                {
                    return Json(new { success = false, message = "Error al procesar la lista de productos." });
                }

                salida.Fecha = DateTime.Now;

                // **Registrar la salida y actualizar el stock en una transacción**
                bool operacionExitosa = await _salidaBodegaService.RegistrarSalidaConProductos(salida, productos);

                return Json(new { success = operacionExitosa });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error interno: " + ex.Message });
            }
        }

        // 📌 Método para eliminar una salida de bodega
        [HttpPost]
        public async Task<IActionResult> Eliminar(int id)
        {
            var salida = await _salidaBodegaService.ObtenerPorId(id);
            if (salida == null)
            {
                return Json(new { success = false, message = "No se encontró la salida de bodega." });
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

        [HttpPost]
        public async Task<IActionResult> EliminarSalida(int id)
        {
            Console.WriteLine($"🗑️ Intentando eliminar salida de bodega ID: {id}");

            var salida = await _salidaBodegaService.ObtenerPorId(id);
            if (salida == null)
            {
                Console.WriteLine("⚠️ No se encontró la salida de bodega.");
                return Json(new { success = false, message = "No se encontró la salida de bodega." });
            }

            try
            {
                await _salidaBodegaService.RevertirStock(id); // ✅ Reponer stock antes de eliminar
                await _salidaBodegaService.EliminarDetalles(id);
                await _salidaBodegaService.Eliminar(id);

                Console.WriteLine("✅ Salida eliminada correctamente.");
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al eliminar salida: {ex.Message}");
                return Json(new { success = false, message = "Error al eliminar: " + ex.Message });
            }
        }

    }
}
