using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventario360.Models
{
    public class Producto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ITEM { get; set; }
        public int? Cantidad { get; set; } // Puede ser null según la BD
        public required string NombreTecnico { get; set; } // varchar(255)
        public string? Medida { get; set; } // varchar(50), puede ser null
        public string? UnidadMedida { get; set; } // varchar(50), puede ser null
        public string? Marca { get; set; } // varchar(255), puede ser null
        public string? Descripcion { get; set; } // text, puede ser null
        public string? Imagen { get; set; } // varchar(255), puede ser null
        public string? Proveedor { get; set; }
        public string? Ubicacion { get; set; } // varchar(255), puede ser null
        public string? Estado { get; set; } // varchar(50), puede ser null (Usado/Nuevo)
    }
}

