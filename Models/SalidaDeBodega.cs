using System;
using System.Collections.Generic;

namespace Inventario360.Models
{
    public class SalidaDeBodega
    {
        public int ID { get; set; }
        public DateTime? Fecha { get; set; }

        public int? Solicitante { get; set; }
        public Empleado? SolicitanteObj { get; set; }

        public int? ResponsableEntrega { get; set; }
        public Empleado? ResponsableEntregaObj { get; set; }

        public int? ProyectoAsignado { get; set; }
        public Proyecto? ProyectoObj { get; set; }

        
        public List<DetalleSalidaDeBodega>? Detalles { get; set; } = new List<DetalleSalidaDeBodega>();
    }
}
