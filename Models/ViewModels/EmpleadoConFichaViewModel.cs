﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Inventario360.Models.ViewModels
{
    public class EmpleadoConFichaViewModel
    {
        
        public string Nombre { get; set; }
        public string Cargo { get; set; }

        
        [Required]
        public DateTime FechaIngreso { get; set; }

        public DateTime? FechaTerminoContrato { get; set; }

        public DateTime? FechaVencimientoExamen { get; set; }

        public bool CursoAltura { get; set; }

        public string? Acreditaciones { get; set; }
        public int Id { get; set; }

    }
}
