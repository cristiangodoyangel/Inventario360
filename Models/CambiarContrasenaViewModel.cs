using System.ComponentModel.DataAnnotations;

namespace Inventario360.Models.ViewModels
{
    public class CambiarContrasenaViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string ContrasenaActual { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NuevaContrasena { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("NuevaContrasena", ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmarContrasena { get; set; }
    }
}
