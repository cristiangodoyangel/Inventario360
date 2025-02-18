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
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Cuenta model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                     if (!await _userManager.IsEmailConfirmedAsync(user))
                    {
                        ViewBag.Error = "Correo no confirmado. Por favor verifique su correo electrónico.";
                        return View(model);
                    }

                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
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
