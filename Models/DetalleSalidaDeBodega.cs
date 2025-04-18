using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventario360.Models
{
    public class DetalleSalidaDeBodega
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("SalidaDeBodega")]
        public int SalidaDeBodegaID { get; set; }
        public SalidaDeBodega? SalidaDeBodega { get; set; } 

        [ForeignKey("Producto")]
        public int ProductoID { get; set; }
        public Producto? Producto { get; set; } 

        public int Cantidad { get; set; }

       
        [NotMapped]
        public string? Categoria => Producto?.Categoria;
    }
}
