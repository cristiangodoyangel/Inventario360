using Inventario360.Models;
using Inventario360.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventario360.Controllers
{
    [Authorize(Roles = "Administrador, Proyectos")]
    public class UsuariosController : Controller
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsuariosController(UserManager<Usuario> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var usuarios = _userManager.Users.ToList();
            var modelo = new List<UsuarioConRolesViewModel>();

            foreach (var usuario in usuarios)
            {
                var roles = await _userManager.GetRolesAsync(usuario);
                modelo.Add(new UsuarioConRolesViewModel
                {
                    Id = usuario.Id,
                    NombreCompleto = usuario.NombreCompleto,
                    Email = usuario.Email,
                    Roles = roles.ToList()
                });
            }

            return View(modelo);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            ViewBag.Roles = _roleManager.Roles.Select(r => new SelectListItem
            {
                Value = r.Name,
                Text = r.Name
            }).ToList();

            return View(new CrearUsuarioViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Crear(CrearUsuarioViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = _roleManager.Roles.Select(r => new SelectListItem
                {
                    Value = r.Name,
                    Text = r.Name
                }).ToList();

                return View(model);
            }

            var usuario = new Usuario
            {
                UserName = model.Email,
                Email = model.Email,
                NombreCompleto = model.NombreCompleto,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(usuario, model.Password);

            if (result.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(usuario, model.Rol);
                if (!roleResult.Succeeded)
                {
                    foreach (var error in roleResult.Errors)
                    {
                        ModelState.AddModelError("", $"Error al asignar rol: {error.Description}");
                    }

                    await _userManager.DeleteAsync(usuario); // rollback
                    ViewBag.Roles = _roleManager.Roles.Select(r => new SelectListItem
                    {
                        Value = r.Name,
                        Text = r.Name
                    }).ToList();

                    return View(model);
                }

                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            ViewBag.Roles = _roleManager.Roles.Select(r => new SelectListItem
            {
                Value = r.Name,
                Text = r.Name
            }).ToList();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Editar(string id)
        {
            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario == null) return NotFound();

            var rolesUsuario = await _userManager.GetRolesAsync(usuario);
            var modelo = new EditarUsuarioViewModel
            {
                Id = usuario.Id,
                NombreCompleto = usuario.NombreCompleto,
                Email = usuario.Email,
                Rol = rolesUsuario.FirstOrDefault() 
            };

            ViewBag.Roles = _roleManager.Roles.Select(r => new SelectListItem
            {
                Value = r.Name,
                Text = r.Name,
                Selected = rolesUsuario.Contains(r.Name)
            }).ToList();

            return View(modelo);
        }


        [HttpPost]
        public async Task<IActionResult> Editar(string id, EditarUsuarioViewModel model)
        {
            if (id != model.Id) return NotFound();

            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario == null) return NotFound();

            usuario.NombreCompleto = model.NombreCompleto;

            var rolesActuales = await _userManager.GetRolesAsync(usuario);
            await _userManager.RemoveFromRolesAsync(usuario, rolesActuales);

            if (!string.IsNullOrEmpty(model.Rol))
            {
                await _userManager.AddToRoleAsync(usuario, model.Rol);
            }

            await _userManager.UpdateAsync(usuario);
            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> Eliminar([FromBody] EliminarUsuarioViewModel model)
        {
            if (string.IsNullOrEmpty(model.Id))
            {
                return Json(new { success = false, message = "ID inválido." });
            }

            var usuario = await _userManager.FindByIdAsync(model.Id);
            if (usuario == null)
            {
                return Json(new { success = false, message = "Usuario no encontrado" });
            }

            var result = await _userManager.DeleteAsync(usuario);
            if (result.Succeeded)
            {
                return Json(new { success = true });
            }

            return Json(new { success = false, message = "No se pudo eliminar el usuario" });
        }



    }
}
