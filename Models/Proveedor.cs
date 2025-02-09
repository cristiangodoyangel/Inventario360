namespace Inventario360.Models
{
    public class Proveedor
    {
        public int ID { get; set; } // Clave primaria
        public string? Nombre { get; set; } // varchar(255), permite NULL
        public string? Contacto { get; set; } // varchar(255), permite NULL
    }
}
