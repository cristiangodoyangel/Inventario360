﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventario360.Models
{
    public class DetalleSalidaDeBodega
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("SalidaDeBodega")]
        public int SalidaDeBodegaID { get; set; }
        public SalidaDeBodega? SalidaDeBodega { get; set; } // Propiedad de navegación

        [ForeignKey("Producto")]
        public int ProductoID { get; set; }
        public Producto? Producto { get; set; } // ✅ Relación con Producto

        public int Cantidad { get; set; }

        // ✅ Nueva propiedad para acceder directamente a la categoría del producto
        [NotMapped]
        public string? Categoria => Producto?.Categoria;
    }
}
