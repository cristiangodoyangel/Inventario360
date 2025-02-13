using Microsoft.AspNetCore.Identity;
using Inventario360.Models;
using System.Threading.Tasks;

namespace Inventario360.Services
{
    public class UsuarioInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider, UserManager<Usuario> userManager)
        {
            // Verificar si el usuario ya existe
            var user = await userManager.FindByEmailAsync("admin@example.com");

            if (user == null)
            {
                // Si no existe, crear el usuario
                user = new Usuario
                {
                    UserName = "admin@example.com",
                    Email = "admin@example.com",
                    NombreCompleto = "Administrador"
                };

                // Crear el usuario con la contraseña
                var result = await userManager.CreateAsync(user, "Password123!");

                // Asignar un rol si es necesario, por ejemplo "Admin"
                // await userManager.AddToRoleAsync(user, "Admin");
            }
        }
    }
}