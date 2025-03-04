using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventario360.Models
{
    public class Producto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ITEM { get; set; }

        public int? Cantidad { get; set; }

        [Required]
        [StringLength(255)]
        public string NombreTecnico { get; set; }

        [StringLength(50)]
        public string? Medida { get; set; }

        [StringLength(50)]
        public string? UnidadMedida { get; set; }

        [StringLength(255)]
        public string? Marca { get; set; }

        public string? Descripcion { get; set; }

        [StringLength(255)]
        public string? Imagen { get; set; }

        public int? Proveedor { get; set; }

        [StringLength(255)]
        public string? Ubicacion { get; set; }

        [StringLength(50)]
        public string? Estado { get; set; }

        [StringLength(255)]
        public string? Categoria { get; set; } // Nueva columna para almacenar la categoría como texto
    }
}
