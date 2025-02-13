using Microsoft.AspNetCore.Mvc;
using Inventario360.Models;
using Inventario360.Services;
using Microsoft.AspNetCore.Identity;

namespace Inventario360.Controllers
{
    public class CuentaController : Controller
    {
        private readonly ICuentaService _cuentaService;

        public CuentaController(ICuentaService cuentaService)
        {
            _cuentaService = cuentaService;
        }

        // Vista de Login
        public IActionResult Login()
        {
            return View();
        }

        // Método para procesar el Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Cuenta model)
        {
            if (ModelState.IsValid)
            {
                var result = await _cuentaService.LoginAsync(model);

                if (result.Succeeded)
                {
                    // Redirigir al sistema (Dashboard) si el login es exitoso
                    return RedirectToAction("Index", "Home");
                }

                // Si el login falla
                ViewBag.Error = "Correo o contraseña incorrectos.";
            }
            return View(model);
        }
    }
}
