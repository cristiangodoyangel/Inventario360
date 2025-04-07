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

        public async Task<IActionResult> Index()
        {
            var fichas = await _fichaEmpleadoService.ObtenerFichas();
            return View(fichas);
        }

        
        public async Task<IActionResult> Detalle(int id)
        {
            var ficha = await _fichaEmpleadoService.ObtenerFichaPorEmpleado(id);
            if (ficha == null) return NotFound();
            return View(ficha);
        }

        
        public IActionResult Crear()
        {
            ViewData["Empleados"] = new SelectList(_context.Empleado, "ID", "Nombre");
            return View();
        }

        
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

        
        public async Task<IActionResult> Editar(int id)
        {
            var ficha = await _context.FichaEmpleado.Include(f => f.Empleado)
                .FirstOrDefaultAsync(f => f.ID == id);

            if (ficha == null) return NotFound();

            return View(ficha);
        }


        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, [Bind("ID,Empleado,FechaIngreso,FechaTerminoContrato,FechaVencimientoExamen,CursoAltura,Acreditaciones")] FichaEmpleado fichaEmpleado)
        {
            if (id != fichaEmpleado.ID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var fichaExistente = await _context.FichaEmpleado.Include(f => f.Empleado)
                        .FirstOrDefaultAsync(f => f.ID == id);

                    if (fichaExistente == null) return NotFound();

                    
                    fichaExistente.Empleado.Nombre = fichaEmpleado.Empleado.Nombre;
                    fichaExistente.FechaIngreso = fichaEmpleado.FechaIngreso;
                    fichaExistente.FechaTerminoContrato = fichaEmpleado.FechaTerminoContrato;
                    fichaExistente.FechaVencimientoExamen = fichaEmpleado.FechaVencimientoExamen;
                    fichaExistente.CursoAltura = fichaEmpleado.CursoAltura;
                    fichaExistente.Acreditaciones = fichaEmpleado.Acreditaciones;

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FichaEmpleadoExists(fichaEmpleado.ID)) return NotFound();
                    throw;
                }
            }

            return View(fichaEmpleado);
        }


        private bool FichaEmpleadoExists(int id)
        {
            return _context.FichaEmpleado.Any(e => e.ID == id);
        }

        
        public async Task<IActionResult> Eliminar(int id)
        {
            var eliminado = await _fichaEmpleadoService.EliminarFicha(id);
            if (!eliminado) return NotFound();
            return RedirectToAction(nameof(Index));
        }
    }
}
