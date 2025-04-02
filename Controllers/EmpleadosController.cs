using Microsoft.AspNetCore.Mvc;
using Inventario360.Services;
using Inventario360.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Inventario360.Data;
using Microsoft.EntityFrameworkCore;
using Inventario360.Models.ViewModels;

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
            var viewModel = new EmpleadoConFichaViewModel();
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(EmpleadoConFichaViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var empleado = new Empleado
            {
                Nombre = model.Nombre,
                Cargo = model.Cargo
            };

            _context.Empleado.Add(empleado);
            await _context.SaveChangesAsync();

            var ficha = new FichaEmpleado
            {
                EmpleadoID = empleado.ID,
                FechaIngreso = model.FechaIngreso,
                FechaTerminoContrato = model.FechaTerminoContrato,
                FechaVencimientoExamen = model.FechaVencimientoExamen,
                CursoAltura = model.CursoAltura,
                Acreditaciones = model.Acreditaciones
            };

            _context.FichaEmpleado.Add(ficha);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            var empleado = await _empleadoService.ObtenerPorId(id);
            if (empleado == null)
                return NotFound();

            return View(empleado);
        }


        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var empleado = await _context.Empleado.FindAsync(id);
            var ficha = await _context.FichaEmpleado.FirstOrDefaultAsync(f => f.EmpleadoID == id);

            if (empleado == null || ficha == null)
                return NotFound();

            var model = new EmpleadoConFichaViewModel
            {
                Nombre = empleado.Nombre,
                Cargo = empleado.Cargo,
                FechaIngreso = ficha.FechaIngreso,
                FechaTerminoContrato = ficha.FechaTerminoContrato,
                FechaVencimientoExamen = ficha.FechaVencimientoExamen,
                CursoAltura = ficha.CursoAltura,
                Acreditaciones = ficha.Acreditaciones
            };

            ViewBag.EmpleadoID = id;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(int id, EmpleadoConFichaViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var empleado = await _context.Empleado.FindAsync(id);
            var ficha = await _context.FichaEmpleado.FirstOrDefaultAsync(f => f.EmpleadoID == id);

            if (empleado == null || ficha == null)
                return NotFound();

            empleado.Nombre = model.Nombre;
            empleado.Cargo = model.Cargo;

            ficha.FechaIngreso = model.FechaIngreso;
            ficha.FechaTerminoContrato = model.FechaTerminoContrato;
            ficha.FechaVencimientoExamen = model.FechaVencimientoExamen;
            ficha.CursoAltura = model.CursoAltura;
            ficha.Acreditaciones = model.Acreditaciones;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }


        [HttpPost, ActionName("Eliminar")]
        public async Task<IActionResult> ConfirmarEliminar(int id)
        {
            try
            {
                var empleado = await _context.Empleado.FindAsync(id);
                if (empleado == null)
                {
                    TempData["Error"] = "Empleado no encontrado.";
                    return RedirectToAction(nameof(Index));
                }

                // Eliminar ficha si existe
                var ficha = await _context.FichaEmpleado.FirstOrDefaultAsync(f => f.EmpleadoID == id);
                if (ficha != null)
                {
                    _context.FichaEmpleado.Remove(ficha);
                }

                // Verificar salidas de bodega
                bool tieneSalidas = await _context.SalidaDeBodega
                    .AnyAsync(s => s.Solicitante == id || s.ResponsableEntrega == id);

                if (tieneSalidas)
                {
                    TempData["Error"] = "No se puede eliminar el empleado porque está asociado a una Salida de Bodega.";
                    return RedirectToAction(nameof(Index));
                }

                _context.Empleado.Remove(empleado);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Empleado y ficha eliminados correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error interno: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }


    }
}
