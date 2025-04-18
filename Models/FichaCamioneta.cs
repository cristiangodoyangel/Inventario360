using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventario360.Models
{
    public class FichaCamioneta
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(20)]
        public string Patente { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Marca { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Modelo { get; set; } = string.Empty;

        [Required]
        public int Año { get; set; }

        [Required]
        [StringLength(20)]
        public string Color { get; set; } = string.Empty;

        [Required]
        public int Kilometraje { get; set; }

        [Required]
        [StringLength(20)]
        public string Estado { get; set; } = string.Empty;

        [ForeignKey("Responsable")]
        public int ResponsableID { get; set; }
        public Empleado? Responsable { get; set; } 

        public DateTime? FechaMantenimiento { get; set; }
        public DateTime? FechaPermisoCirculacion { get; set; }
        public DateTime? FechaSoap { get; set; }
        public DateTime? FechaAntivuelcos { get; set; }
        public DateTime? FechaLaminas { get; set; }
        public DateTime? FechaGPS { get; set; }

        [StringLength(50)]
        public string? Acreditacion { get; set; }
    }
}
