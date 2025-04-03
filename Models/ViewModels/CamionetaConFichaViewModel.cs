using Inventario360.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Inventario360.Models.ViewModels
{
    public class CamionetaConFichaViewModel
    {
        public Camioneta Camioneta { get; set; }
        public FichaCamioneta FichaCamioneta { get; set; }

        // Opcional: si quieres seleccionar responsable
        public SelectList Empleados { get; set; }
    }
}
