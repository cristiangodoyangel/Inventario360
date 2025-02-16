using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventario360.Models
{
    public class DetalleSalidaDeBodega
    {
        [Key]
        public int ID { get; set; } // Clave primaria

        [ForeignKey("SalidaDeBodega")]
        public int SalidaDeBodegaID { get; set; }
        public virtual SalidaDeBodega? SalidaDeBodega { get; set; } // Relación con SalidaDeBodega

        [ForeignKey("Producto")]
        public int ProductoID { get; set; }
        public virtual Producto? Producto { get; set; } // Relación con Producto

        [Required]
        public int Cantidad { get; set; } // Cantidad de cada producto en la salida
    }
}
