using Inventario360.Models;
using Inventario360.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Inventario360.Controllers
{
    public class PerfilController : Controller
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;

        public PerfilController(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index()
        {
            var usuario = await _userManager.GetUserAsync(User);
            if (usuario == null) return NotFound();

            var model = new PerfilViewModel
            {
                Id = usuario.Id,
                NombreCompleto = usuario.NombreCompleto,
                Email = usuario.Email,
                Telefono = usuario.PhoneNumber
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ActualizarPerfil(PerfilViewModel model)
        {
            if (!ModelState.IsValid) return View("Index", model);

            var usuario = await _userManager.FindByIdAsync(model.Id);
            if (usuario == null) return NotFound();

            usuario.NombreCompleto = model.NombreCompleto;
            usuario.PhoneNumber = model.Telefono;

            var resultado = await _userManager.UpdateAsync(usuario);
            if (resultado.Succeeded)
            {
                ViewBag.Mensaje = "Perfil actualizado con éxito.";
                return RedirectToAction("Index");
            }

            return View("Index", model);
        }

        public IActionResult CambiarContrasena()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CambiarContrasena(CambiarContrasenaViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var usuario = await _userManager.GetUserAsync(User);
            if (usuario == null) return NotFound();

            var resultado = await _userManager.ChangePasswordAsync(usuario, model.ContrasenaActual, model.NuevaContrasena);
            if (resultado.Succeeded)
            {
                await _signInManager.SignInAsync(usuario, isPersistent: false);
                ViewBag.Mensaje = "Contraseña cambiada con éxito.";
                return RedirectToAction("Index");
            }

            foreach (var error in resultado.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }
    }
}
