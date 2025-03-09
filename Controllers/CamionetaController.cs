using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Inventario360.Services;
using Inventario360.Models;
using Inventario360.Data;
using Microsoft.EntityFrameworkCore;

namespace Inventario360.Controllers
{
    public class CamionetaController : Controller
    {
        private readonly ICamionetaService _camionetaService;
        private readonly InventarioDbContext _context;

        public CamionetaController(ICamionetaService camionetaService, InventarioDbContext context)
        {
            _camionetaService = camionetaService;
            _context = context;
        }

        // 📌 Listar todas las camionetas
        public async Task<IActionResult> Index()
        {
            var camionetas = await _camionetaService.ObtenerCamionetas();
            return View(camionetas);
        }

        // 📌 Ver detalles
        public async Task<IActionResult> Detalle(int id)
        {
            var camioneta = await _camionetaService.ObtenerCamionetaPorId(id);
            if (camioneta == null) return NotFound();
            return View(camioneta);
        }

        // 📌 Crear nueva camioneta (Vista)
        public IActionResult Crear()
        {
            ViewData["Responsables"] = new SelectList(_context.Empleado, "ID", "Nombre");
            return View();
        }

        // 📌 Guardar nueva camioneta
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Camioneta camioneta)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Responsables"] = new SelectList(_context.Empleado, "ID", "Nombre", camioneta.ResponsableID);
                return View(camioneta);
            }

            await _camionetaService.CrearCamioneta(camioneta);
            return RedirectToAction(nameof(Index));
        }

        // 📌 Editar (Vista)
        public async Task<IActionResult> Editar(int id)
        {
            var camioneta = await _camionetaService.ObtenerCamionetaPorId(id);
            if (camioneta == null) return NotFound();

            ViewData["Responsables"] = new SelectList(_context.Empleado, "ID", "Nombre", camioneta.ResponsableID);
            return View(camioneta);
        }

        // 📌 Guardar cambios en la BD
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, Camioneta camioneta)
        {
            if (id != camioneta.ID) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewData["Responsables"] = new SelectList(_context.Empleado, "ID", "Nombre", camioneta.ResponsableID);
                return View(camioneta);
            }

            await _camionetaService.EditarCamioneta(camioneta);
            return RedirectToAction(nameof(Index));
        }

        // 📌 Eliminar camioneta
        public async Task<IActionResult> Eliminar(int id)
        {
            var eliminado = await _camionetaService.EliminarCamioneta(id);
            if (!eliminado) return NotFound();
            return RedirectToAction(nameof(Index));
        }
    }
}
