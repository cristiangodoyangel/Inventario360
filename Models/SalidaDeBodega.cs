namespace Inventario360.Models
{
    public class SalidaDeBodega
    {
        public int ID { get; set; }
        public DateTime? Fecha { get; set; }
        public int? Cantidad { get; set; }

        public int? Producto { get; set; }
        public Producto? ProductoObj { get; set; } // Propiedad de navegación

        public int? Solicitante { get; set; }
        public Empleado? SolicitanteObj { get; set; } // Propiedad de navegación

        public int? ResponsableEntrega { get; set; }
        public Empleado? ResponsableEntregaObj { get; set; } // Propiedad de navegación

        public int? ProyectoAsignado { get; set; }
        public Proyecto? ProyectoObj { get; set; } // Pr
    }
}
