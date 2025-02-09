using Microsoft.AspNetCore.Mvc;
using Inventario360.Services;
using Inventario360.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Inventario360.Controllers
{
    public class SolicitudesController : Controller
    {
        private readonly ISolicitudService _solicitudService;

        public SolicitudesController(ISolicitudService solicitudService)
        {
            _solicitudService = solicitudService;
        }

        public async Task<IActionResult> Index()
        {
            var solicitudes = await _solicitudService.ObtenerTodas();
            return View(solicitudes);
        }

        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(SolicitudDeMaterial solicitud)
        {
            if (!ModelState.IsValid) return View(solicitud);

            await _solicitudService.Agregar(solicitud);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Detalle(int id)
        {
            var solicitud = await _solicitudService.ObtenerPorId(id);
            if (solicitud == null) return NotFound();
            return View(solicitud);
        }

        public async Task<IActionResult> Editar(int id)
        {
            var solicitud = await _solicitudService.ObtenerPorId(id);
            if (solicitud == null) return NotFound();
            return View(solicitud);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(SolicitudDeMaterial solicitud)
        {
            if (!ModelState.IsValid) return View(solicitud);

            await _solicitudService.Actualizar(solicitud);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Eliminar(int id)
        {
            var solicitud = await _solicitudService.ObtenerPorId(id);
            if (solicitud == null) return NotFound();
            return View(solicitud);
        }

        [HttpPost, ActionName("Eliminar")]
        public async Task<IActionResult> ConfirmarEliminar(int id)
        {
            await _solicitudService.Eliminar(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
