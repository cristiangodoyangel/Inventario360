using System.Collections.Generic;

namespace Inventario360.Models
{
    public class SalidaDeBodega
    {
        public int ID { get; set; } // Clave primaria
        public DateTime? Fecha { get; set; }
        public int? Solicitante { get; set; }
        public int? ResponsableEntrega { get; set; }
        public int? ProyectoAsignado { get; set; }

        // Relación con DetalleSalidaDeBodega (una salida puede tener múltiples productos)
        public virtual List<DetalleSalidaDeBodega> Detalles { get; set; } = new List<DetalleSalidaDeBodega>();
    }
}
