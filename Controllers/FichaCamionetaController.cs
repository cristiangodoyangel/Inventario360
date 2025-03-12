using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Inventario360.Models;
using Inventario360.Services;

namespace Inventario360.Controllers
{
    public class FichaCamionetaController : Controller
    {
        private readonly IFichaCamionetaService _fichaCamionetaService;

        public FichaCamionetaController(IFichaCamionetaService fichaCamionetaService)
        {
            _fichaCamionetaService = fichaCamionetaService;
        }

        public async Task<IActionResult> Index()
        {
            var fichas = await _fichaCamionetaService.ObtenerTodas();
            return View(fichas);
        }


        public async Task<IActionResult> Detalle(int id)
        {
            var ficha = await _fichaCamionetaService.ObtenerFichaPorId(id);
            if (ficha == null) return NotFound();
            return View(ficha);
        }

        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(FichaCamioneta ficha)
        {
            if (!ModelState.IsValid)
            {
                return View(ficha);
            }

            await _fichaCamionetaService.CrearFicha(ficha);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Editar(int id)
        {
            var ficha = await _fichaCamionetaService.ObtenerFichaPorId(id);
            if (ficha == null) return NotFound();
            return View(ficha);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(FichaCamioneta ficha)
        {
            if (!ModelState.IsValid) return View(ficha);

            bool actualizado = await _fichaCamionetaService.ActualizarFicha(ficha);
            if (!actualizado) return NotFound();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Eliminar(int id)
        {
            var ficha = await _fichaCamionetaService.ObtenerFichaPorId(id);
            if (ficha == null) return NotFound();
            return View(ficha);
        }

        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmarEliminar(int id)
        {
            bool eliminado = await _fichaCamionetaService.EliminarFicha(id);
            if (!eliminado) return NotFound();

            return RedirectToAction(nameof(Index));
        }
    }
}
