namespace Inventario360.Models
{
    public class Producto
    {
        public int ITEM { get; set; } // Clave primaria
        public int? Cantidad { get; set; } // Puede ser null según la BD
        public required string NombreTecnico { get; set; } // varchar(255)
        public string? Medida { get; set; } // varchar(50), puede ser null
        public string? UnidadMedida { get; set; } // varchar(50), puede ser null
        public string? Marca { get; set; } // varchar(255), puede ser null
        public string? Descripcion { get; set; } // text, puede ser null
        public string? Imagen { get; set; } // varchar(255), puede ser null
        public int? Proveedor { get; set; } // FK hacia Proveedor
        public string? Ubicacion { get; set; } // varchar(255), puede ser null
        public string? Estado { get; set; } // varchar(50), puede ser null (Usado/Nuevo)
    }
}

