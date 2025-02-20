using Microsoft.AspNetCore.Mvc;
using Inventario360.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Inventario360.Controllers
{
    public class CuentaController : Controller
    {
        private readonly SignInManager<Usuario> _signInManager;
        private readonly UserManager<Usuario> _userManager;

        // Inyectar SignInManager y UserManager
        public CuentaController(SignInManager<Usuario> signInManager, UserManager<Usuario> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        // ✅ Vista de Login (Permitir acceso sin autenticación)
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // ✅ Método para procesar el Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Cuenta model, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Buscar el usuario con el correo electrónico proporcionado
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ViewBag.Error = "El usuario no existe.";
                return View(model);
            }

            // Verificar si el correo está confirmado
            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                ViewBag.Error = "Correo no confirmado. Por favor, verifique su correo electrónico.";
                return View(model);
            }

            // Intentar iniciar sesión con el correo y contraseña proporcionados
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {
                // ✅ Evitar bucle infinito en redirecciones
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("Index", "Home");
            }

            // Si las credenciales son incorrectas
            ViewBag.Error = "Correo o contraseña incorrectos.";
            return View(model);
        }

        // ✅ Método para manejar el Logout (Cerrar sesión)
        [HttpPost]
        [ValidateAntiForgeryToken] // Protege contra ataques CSRF
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}
