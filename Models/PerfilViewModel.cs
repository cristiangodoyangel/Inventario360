using System.ComponentModel.DataAnnotations;

namespace Inventario360.Models.ViewModels
{
    public class PerfilViewModel
    {
        public string Id { get; set; }

        [Required]
        public string NombreCompleto { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Telefono { get; set; }
    }
}
