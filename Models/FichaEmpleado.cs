using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventario360.Models
{
    public class FichaEmpleado
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int EmpleadoID { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaIngreso { get; set; }

        [DataType(DataType.Date)]
        public DateTime? FechaTerminoContrato { get; set; }

        [DataType(DataType.Date)]
        public DateTime? FechaVencimientoExamen { get; set; }

        public bool CursoAltura { get; set; } = false;

        public string? Acreditaciones { get; set; }

        [ForeignKey("EmpleadoID")]
        public virtual Empleado Empleado { get; set; }
    }
}
