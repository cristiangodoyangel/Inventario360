using Inventario360.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventario360.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsuarioService(UserManager<Usuario> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<List<Usuario>> ObtenerTodos()
        {
            return _userManager.Users.ToList();
        }

        public async Task<Usuario> ObtenerPorId(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<IdentityResult> Crear(Usuario usuario, string contraseña)
        {
            return await _userManager.CreateAsync(usuario, contraseña);
        }

        public async Task<IdentityResult> Actualizar(Usuario usuario)
        {
            return await _userManager.UpdateAsync(usuario);
        }

        public async Task<IdentityResult> Eliminar(string id)
        {
            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario != null)
            {
                return await _userManager.DeleteAsync(usuario);
            }
            return IdentityResult.Failed();
        }

        public async Task<IList<string>> ObtenerRoles(Usuario usuario)
        {
            return await _userManager.GetRolesAsync(usuario);
        }

        public async Task<IList<string>> ObtenerRolesPorId(string id)
        {
            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario != null)
            {
                return await _userManager.GetRolesAsync(usuario);
            }
            return new List<string>();
        }

        public async Task AsignarRoles(string userId, List<string> roles)
        {
            var usuario = await _userManager.FindByIdAsync(userId);
            if (usuario != null)
            {
                var rolesActuales = await _userManager.GetRolesAsync(usuario);
                await _userManager.RemoveFromRolesAsync(usuario, rolesActuales);
                await _userManager.AddToRolesAsync(usuario, roles);
            }
        }

        public async Task<List<string>> ObtenerTodosLosRoles()
        {
            return _roleManager.Roles.Select(r => r.Name).ToList();
        }
    }
}
