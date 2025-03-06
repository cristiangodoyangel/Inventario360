﻿using Microsoft.AspNetCore.Mvc;
using Inventario360.Services;
using Inventario360.Models;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Inventario360.Data;

namespace Inventario360.Controllers
{
    [Authorize] // 🔹 Aplica autenticación a todas las acciones del controlador
    public class SalidaDeBodegaController : Controller
    {
        private readonly InventarioDbContext _context;
        private readonly ISalidaBodegaService _salidaBodegaService;
        private readonly IProductoService _productoService;
        private readonly IEmpleadoService _empleadoService;
        private readonly IProyectoService _proyectoService;

        // ✅ Constructor con inyección de dependencias
        public SalidaDeBodegaController(
            InventarioDbContext context,
            ISalidaBodegaService salidaBodegaService,
            IProductoService productoService,
            IEmpleadoService empleadoService,
            IProyectoService proyectoService)
        {
            _context = context;
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
            if (salida == null) return NotFound();

            ViewBag.Detalles = await _salidaBodegaService.ObtenerDetallesPorSalida(id);
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
                return Json(new { success = false, message = "Datos inválidos." });

            if (string.IsNullOrEmpty(ProductosJson))
                return Json(new { success = false, message = "Debe agregar al menos un producto." });

            try
            {
                var productos = JsonConvert.DeserializeObject<List<DetalleSalidaDeBodega>>(ProductosJson);
                if (productos == null || productos.Count == 0)
                    return Json(new { success = false, message = "Error al procesar productos." });

                salida.Fecha = DateTime.Now;
                bool operacionExitosa = await _salidaBodegaService.RegistrarSalidaConProductos(salida, productos);

                return Json(new { success = operacionExitosa });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error interno: " + ex.Message });
            }
        }

        // 📌 Obtener Resumen de Salidas de Bodega
        public async Task<IActionResult> ObtenerResumenSalidas(int mes, int año)
        {
            var resumen = new
            {
                proyectosMasSolicitados = await _context.SalidaDeBodega
                    .Where(s => s.Fecha.HasValue && s.Fecha.Value.Month == mes && s.Fecha.Value.Year == año)
                    .GroupBy(s => s.ProyectoAsignado)
                    .OrderByDescending(g => g.Count())
                    .Select(g => new
                    {
                        nombreProyecto = _context.Proyecto
                            .Where(p => p.ID == g.Key)
                            .Select(p => p.Nombre)
                            .FirstOrDefault(),
                        total = g.Count()
                    })
                    .FirstOrDefaultAsync(),

                empleadosMasSolicitantes = await _context.SalidaDeBodega
                    .Where(s => s.Fecha.HasValue && s.Fecha.Value.Month == mes && s.Fecha.Value.Year == año)
                    .GroupBy(s => s.Solicitante)
                    .OrderByDescending(g => g.Count())
                    .Select(g => new
                    {
                        nombreEmpleado = _context.Empleado
                            .Where(e => e.ID == g.Key)
                            .Select(e => e.Nombre)
                            .FirstOrDefault(),
                        total = g.Count()
                    })
                    .FirstOrDefaultAsync(),

                materialesMasSolicitados = await _context.DetalleSalidaDeBodega
                    .Where(d => d.SalidaDeBodega.Fecha.HasValue && d.SalidaDeBodega.Fecha.Value.Month == mes && d.SalidaDeBodega.Fecha.Value.Year == año)
                    .Include(d => d.Producto)
                    .GroupBy(d => d.Producto.NombreTecnico)
                    .OrderByDescending(g => g.Sum(d => d.Cantidad))
                    .Select(g => new { material = g.Key, totalCantidad = g.Sum(d => d.Cantidad) })
                    .Take(3)
                    .ToListAsync()
            };

            return Json(resumen);
        }

        // 📌 Método para eliminar una salida de bodega
        [HttpPost]
        public async Task<IActionResult> Eliminar(int id)
        {
            var salida = await _salidaBodegaService.ObtenerPorId(id);
            if (salida == null)
                return Json(new { success = false, message = "No se encontró la salida." });

            try
            {
                await _salidaBodegaService.EliminarDetalles(id);
                await _salidaBodegaService.Eliminar(id);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al eliminar: " + ex.Message });
            }
        }

        // 📌 Método para eliminar salida y revertir stock
        [HttpPost]
        public async Task<IActionResult> EliminarSalida(int id)
        {
            var salida = await _salidaBodegaService.ObtenerPorId(id);
            if (salida == null)
                return Json(new { success = false, message = "No se encontró la salida." });

            try
            {
                await _salidaBodegaService.RevertirStock(id);
                await _salidaBodegaService.EliminarDetalles(id);
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
