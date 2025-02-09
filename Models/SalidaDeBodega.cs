namespace Inventario360.Models
{
    public class SalidaDeBodega
    {
        public int ID { get; set; } // Clave primaria
        public DateTime? Fecha { get; set; } // datetime, permite NULL
        public int? Cantidad { get; set; } // int, permite NULL
        public int? Producto { get; set; } // FK hacia Producto, permite NULL
        public int? Solicitante { get; set; } // FK hacia Empleado, permite NULL
        public int? ResponsableEntrega { get; set; } // FK hacia Empleado, permite NULL
        public int? ProyectoAsignado { get; set; } // FK hacia Proyecto, permite NULL
    }
}
