namespace Inventario360.Models
{
    public class SolicitudDeMaterial
    {
        public int ID { get; set; } // Clave primaria
        public int? ITEM { get; set; } // Puede ser null según la BD
        public int? Cantidad { get; set; } // Puede ser null según la BD
        public required string NombreTecnico { get; set; } // varchar(255)
        public string? Medida { get; set; } // varchar(50), puede ser null
        public string? UnidadMedida { get; set; } // varchar(50), puede ser null
        public string? Marca { get; set; } // varchar(255), puede ser null
        public string? Descripcion { get; set; } // text, puede ser null
        public string? Imagen { get; set; } // varchar(255), puede ser null
        public int? Producto { get; set; } // FK hacia Producto
    }
}
