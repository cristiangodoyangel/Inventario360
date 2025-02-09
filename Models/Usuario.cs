using Microsoft.AspNetCore.Identity;

namespace Inventario360.Models
{
    public class Usuario : IdentityUser
    {
        public required string NombreCompleto { get; set; }
    }
}
