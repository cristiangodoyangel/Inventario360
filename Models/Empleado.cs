namespace Inventario360.Models
{
    public class Empleado
    {
        public int ID { get; set; } // Clave primaria
        public string? Nombre { get; set; } // varchar(255), permite NULL
        public string? Cargo { get; set; } // varchar(255), permite NULL
    }
}
