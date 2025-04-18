using Inventario360.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Inventario360.Models.ViewModels
{
    public class CamionetaConFichaViewModel
    {
        public Camioneta Camioneta { get; set; }
        public FichaCamioneta FichaCamioneta { get; set; }

        
        public SelectList Empleados { get; set; }
    }
}
