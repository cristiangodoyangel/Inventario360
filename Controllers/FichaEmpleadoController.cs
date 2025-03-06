using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Inventario360.Models;
using Inventario360.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Inventario360.Data;

namespace Inventario360.Controllers
{
    public class FichaEmpleadoController : Controller
    {
        private readonly IFichaEmpleadoService _fichaEmpleadoService;
        private readonly InventarioDbContext _context;

        public FichaEmpleadoController(IFichaEmpleadoService fichaEmpleadoService, InventarioDbContext context)
        {
            _fichaEmpleadoService = fichaEmpleadoService;
            _context = context;
        }

        // 📌 Listar todas las fichas de empleados
        public async Task<IActionResult> Index()
        {
            var fichas = await _fichaEmpleadoService.ObtenerFichas();
            return View(fichas);
        }

        // 📌 Mostrar detalles de una ficha
        public async Task<IActionResult> Detalle(int id)
        {
            var ficha = await _fichaEmpleadoService.ObtenerFichaPorEmpleado(id);
            if (ficha == null) return NotFound();
            return View(ficha);
        }

        // 📌 Crear nueva ficha (Vista)
        public IActionResult Crear()
        {
            ViewData["Empleados"] = new SelectList(_context.Empleado, "ID", "Nombre");
            return View();
        }

        // 📌 Guardar ficha en la BD
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(FichaEmpleado ficha)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Empleados"] = new SelectList(_context.Empleado, "ID", "Nombre", ficha.EmpleadoID);
                return View(ficha);
            }

            await _fichaEmpleadoService.CrearFicha(ficha);
            return RedirectToAction(nameof(Index));
        }

        // 📌 Editar ficha (Vista)
        public async Task<IActionResult> Editar(int id)
        {
            var ficha = await _context.FichaEmpleado.FindAsync(id);
            if (ficha == null) return NotFound();

            ViewData["Empleados"] = new SelectList(_context.Empleado, "ID", "Nombre", ficha.EmpleadoID);
            return View(ficha);
        }

        // 📌 Guardar cambios en la BD
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, [Bind("ID,EmpleadoID,FechaIngreso,FechaTerminoContrato,FechaVencimientoExamen,CursoAltura,Acreditaciones")] FichaEmpleado fichaEmpleado)
        {
            if (id != fichaEmpleado.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var fichaExistente = await _context.FichaEmpleado.FindAsync(id);
                if (fichaExistente == null)
                {
                    return NotFound();
                }

                // Adjuntar la entidad al contexto y marcarla como modificada
                _context.Attach(fichaExistente).State = EntityState.Modified;

                if (await TryUpdateModelAsync(
                    fichaExistente,
                    "",
                    f => f.EmpleadoID,
                    f => f.FechaIngreso,
                    f => f.FechaTerminoContrato,
                    f => f.FechaVencimientoExamen,
                    f => f.CursoAltura,
                    f => f.Acreditaciones))
                {
                    try
                    {
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!FichaEmpleadoExists(fichaEmpleado.ID))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }

            ViewData["Empleados"] = new SelectList(_context.Empleado, "ID", "Nombre", fichaEmpleado.EmpleadoID);
            return View(fichaEmpleado);
        }

        private bool FichaEmpleadoExists(int id)
        {
            return _context.FichaEmpleado.Any(e => e.ID == id);
        }

        // 📌 Eliminar ficha
        public async Task<IActionResult> Eliminar(int id)
        {
            var eliminado = await _fichaEmpleadoService.EliminarFicha(id);
            if (!eliminado) return NotFound();
            return RedirectToAction(nameof(Index));
        }
    }
}
