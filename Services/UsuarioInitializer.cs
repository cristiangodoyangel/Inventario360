using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Inventario360.Models;

namespace Inventario360.Services
{
    public static class UsuarioInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider, UserManager<Usuario> userManager)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Crear todos los roles definidos
            string[] roles = {
                "Administrador",
                "Gerencia",
                "Proyectos",
                "Supervisor",
                "Usuario"
            };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Crear un usuario administrador por defecto si no existe
            var adminEmail = "admin@inventario360.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new Usuario
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    NombreCompleto = "Administrador del Sistema",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, "Admin123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Administrador");
                }
            }

            // Asegurar que el usuario tiene el rol correcto
            if (!await userManager.IsInRoleAsync(adminUser, "Administrador"))
            {
                await userManager.AddToRoleAsync(adminUser, "Administrador");
            }

            // Agregar Claim de NombreCompleto si no existe
            var claims = await userManager.GetClaimsAsync(adminUser);
            if (!claims.Any(c => c.Type == "NombreCompleto"))
            {
                await userManager.AddClaimAsync(adminUser, new Claim("NombreCompleto", adminUser.NombreCompleto));
            }
        }
    }
}
