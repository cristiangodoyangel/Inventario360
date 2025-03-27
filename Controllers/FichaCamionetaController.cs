using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Inventario360.Models;
using Inventario360.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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
            if (!ModelState.IsValid)
            {
                ViewBag.Empleados = new SelectList(await _fichaCamionetaService.ObtenerEmpleados(), "ID", "Nombre");
                return View(ficha);
            }

            // Verificar si el Responsable existe antes de actualizar
            var existeResponsable = await _fichaCamionetaService.ExisteResponsable(ficha.ResponsableID);
            if (!existeResponsable)
            {
                ModelState.AddModelError("ResponsableID", "El Responsable seleccionado no es válido.");
                ViewBag.Empleados = new SelectList(await _fichaCamionetaService.ObtenerEmpleados(), "ID", "Nombre");
                return View(ficha);
            }

            // Intentar actualizar
            bool actualizado = await _fichaCamionetaService.ActualizarFicha(ficha);
            if (!actualizado)
            {
                ModelState.AddModelError("", "No se pudo actualizar la ficha. Verifica los datos ingresados.");
                ViewBag.Empleados = new SelectList(await _fichaCamionetaService.ObtenerEmpleados(), "ID", "Nombre");
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
