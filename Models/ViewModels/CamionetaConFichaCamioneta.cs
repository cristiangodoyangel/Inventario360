using System;
using System.ComponentModel.DataAnnotations;
using Inventario360.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Inventario360.ViewModels
{
    public class CamionetaConFichaCamioneta
    {
        // Propiedades de Camioneta
        public Camioneta Camioneta { get; set; }

        // Lista de empleados para el select
        public IEnumerable<SelectListItem> Empleados { get; set; }

        // Validaciones para los campos
        [Required(ErrorMessage = "La patente es obligatoria")]
        [Display(Name = "Patente")]
        public string Patente { get; set; }

        [Required(ErrorMessage = "La marca es obligatoria")]
        [Display(Name = "Marca")]
        public string Marca { get; set; }

        [Required(ErrorMessage = "El modelo es obligatorio")]
        [Display(Name = "Modelo")]
        public string Modelo { get; set; }

        [Required(ErrorMessage = "El año es obligatorio")]
        [Range(1900, 2100, ErrorMessage = "Por favor ingrese un año válido")]
        [Display(Name = "Año")]
        public int Año { get; set; }

        [Required(ErrorMessage = "El color es obligatorio")]
        [Display(Name = "Color")]
        public string Color { get; set; }

        [Required(ErrorMessage = "El kilometraje es obligatorio")]
        [Range(0, int.MaxValue, ErrorMessage = "Por favor ingrese un kilometraje válido")]
        [Display(Name = "Kilometraje")]
        public int Kilometraje { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio")]
        [Display(Name = "Estado")]
        public string Estado { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un responsable")]
        [Display(Name = "Responsable")]
        public int ResponsableID { get; set; }

        // Fechas de la documentación
        [Display(Name = "Fecha de Mantenimiento")]
        [DataType(DataType.Date)]
        public DateTime FechaMantenimiento { get; set; }

        [Display(Name = "Permiso de Circulación")]
        [DataType(DataType.Date)]
        public DateTime FechaPermisoCirculacion { get; set; }

        [Display(Name = "SOAP")]
        [DataType(DataType.Date)]
        public DateTime FechaSoap { get; set; }

        [Display(Name = "Antivuelcos")]
        [DataType(DataType.Date)]
        public DateTime FechaAntivuelcos { get; set; }

        [Display(Name = "Láminas")]
        [DataType(DataType.Date)]
        public DateTime FechaLaminas { get; set; }

        [Display(Name = "GPS")]
        [DataType(DataType.Date)]
        public DateTime FechaGPS { get; set; }

        [Display(Name = "Acreditación")]
        public string Acreditacion { get; set; }
    }
}
