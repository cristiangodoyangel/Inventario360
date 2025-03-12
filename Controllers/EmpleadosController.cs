using Microsoft.AspNetCore.Mvc;
using Inventario360.Services;
using Inventario360.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Inventario360.Controllers
{
    public class EmpleadosController : Controller
    {
        private readonly IEmpleadoService _empleadoService;
        private readonly IFichaEmpleadoService _fichaEmpleadoService;


        public EmpleadosController(IEmpleadoService empleadoService, IFichaEmpleadoService fichaEmpleadoService)
        {
            _empleadoService = empleadoService;
            _fichaEmpleadoService = fichaEmpleadoService;
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

        public async Task<IActionResult> Crear()
        {
            var empleados = await _empleadoService.ObtenerTodos();
            ViewBag.Empleados = new SelectList(empleados, "ID", "Nombre");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(FichaEmpleado ficha)
        {
            if (!ModelState.IsValid)
            {
                var empleados = await _empleadoService.ObtenerTodos();
                ViewBag.Empleados = new SelectList(empleados, "ID", "Nombre");
                return View(ficha);
            }

            await _fichaEmpleadoService.Agregar(ficha);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Editar(int id)
        {
            var empleado = await _empleadoService.ObtenerPorId(id);
            if (empleado == null) return NotFound();
            return View(empleado);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Empleado empleado)
        {
            if (!ModelState.IsValid) return View(empleado);

            await _empleadoService.Actualizar(empleado);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Eliminar(int id)
        {
            var empleado = await _empleadoService.ObtenerPorId(id);
            if (empleado == null) return NotFound();
            return View(empleado);
        }

        [HttpPost, ActionName("Eliminar")]
        public async Task<IActionResult> ConfirmarEliminar(int id)
        {
            await _empleadoService.Eliminar(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
