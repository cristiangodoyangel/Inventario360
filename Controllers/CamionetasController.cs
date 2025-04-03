using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Inventario360.Models;
using Inventario360.Services;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Inventario360.Controllers
{
    public class CamionetasController : Controller
    {
        private readonly IFichaCamionetaService _fichaCamionetaService;

        public CamionetasController(IFichaCamionetaService fichaCamionetaService)
        {
            _fichaCamionetaService = fichaCamionetaService;
        }

        public async Task<IActionResult> Index()
        {
            var camionetas = await _fichaCamionetaService.ObtenerTodasConResponsableAsync();
            return View(camionetas);
        }



        public async Task<IActionResult> Detalle(int id)
        {
            var ficha = await _fichaCamionetaService.ObtenerFichaPorId(id);
            if (ficha == null) return NotFound();
            return View(ficha);
        }

        public async Task<IActionResult> Crear()
        {
            ViewBag.Empleados = new SelectList(await _fichaCamionetaService.ObtenerEmpleados(), "ID", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(FichaCamioneta ficha)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Empleados = new SelectList(await _fichaCamionetaService.ObtenerEmpleados(), "ID", "Nombre");
                return View(ficha);
            }

            await _fichaCamionetaService.CrearFicha(ficha);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Editar(int id)
        {
            var ficha = await _fichaCamionetaService.ObtenerFichaPorId(id);
            if (ficha == null) return NotFound();

            ViewBag.Empleados = new SelectList(await _fichaCamionetaService.ObtenerEmpleados(), "ID", "Nombre", ficha.ResponsableID);
            return View(ficha);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(FichaCamioneta ficha)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Empleados = new SelectList(await _fichaCamionetaService.ObtenerEmpleados(), "ID", "Nombre", ficha.ResponsableID);
                return View(ficha);
            }

            var existeResponsable = await _fichaCamionetaService.ExisteResponsable(ficha.ResponsableID);
            if (!existeResponsable)
            {
                ModelState.AddModelError("ResponsableID", "El Responsable seleccionado no es válido.");
                ViewBag.Empleados = new SelectList(await _fichaCamionetaService.ObtenerEmpleados(), "ID", "Nombre", ficha.ResponsableID);
                return View(ficha);
            }

            bool actualizado = await _fichaCamionetaService.ActualizarFicha(ficha);
            if (!actualizado)
            {
                ModelState.AddModelError("", "No se pudo actualizar la ficha. Verifica los datos ingresados.");
                ViewBag.Empleados = new SelectList(await _fichaCamionetaService.ObtenerEmpleados(), "ID", "Nombre", ficha.ResponsableID);
                return View(ficha);
            }

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
