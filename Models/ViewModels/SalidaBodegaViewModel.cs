using System.Collections.Generic;
using Inventario360.Models;

namespace Inventario360.ViewModels
{
    public class SalidaBodegaViewModel
    {
        public SalidaDeBodega Salida { get; set; }
        public List<Producto> Productos { get; set; }
        public List<Empleado> Empleados { get; set; }
        public List<Proyecto> Proyectos { get; set; }

        public SalidaBodegaViewModel()
        {
            Salida = new SalidaDeBodega();
            Productos = new List<Producto>();
            Empleados = new List<Empleado>();
            Proyectos = new List<Proyecto>();
        }
    }
}
