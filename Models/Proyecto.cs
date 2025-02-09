namespace Inventario360.Models
{
    public class Proyecto
    {
        public int ID { get; set; } // Clave primaria
        public string? Nombre { get; set; } // varchar(255), permite NULL
        public string? Descripcion { get; set; } // text, permite NULL
    }
}
