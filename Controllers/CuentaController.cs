using Microsoft.AspNetCore.Mvc;
using Inventario360.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

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
                // Buscar el usuario con el correo electrónico proporcionado
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    // Verificar si el correo está confirmado
                    if (!await _userManager.IsEmailConfirmedAsync(user))
                    {
                        ViewBag.Error = "Correo no confirmado. Por favor verifique su correo electrónico.";
                        return View(model);
                    }

                    // Intentar iniciar sesión con el correo y contraseña proporcionados
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

                    if (result.Succeeded)
                    {
                        // Redirigir al sistema (Dashboard) si el login es exitoso
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        // Si las credenciales son incorrectas
                        ViewBag.Error = "Correo o contraseña incorrectos.";
                    }
                }
                else
                {
                    ViewBag.Error = "El usuario no existe.";
                }
            }
            return View(model);
        }

        // Método para manejar el logout (cerrar sesión)
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync(); // Cerrar sesión
            return RedirectToAction("Index", "Home"); // Redirigir al home después de cerrar sesión
        }
    }
}
