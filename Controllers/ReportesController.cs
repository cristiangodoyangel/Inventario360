using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Inventario360.Services;
using Inventario360.Models;

namespace Inventario360.Controllers
{
    public class ReportesController : Controller
    {
        private readonly IReporteService _reporteService;

        public ReportesController(IReporteService reporteService)
        {
            _reporteService = reporteService;
        }

        public async Task<IActionResult> Index()
        {
            var reportes = await _reporteService.ObtenerDatosReportes();
            return View(reportes);
        }

        public async Task<IActionResult> Detalles(int id)
        {
            var reporte = await _reporteService.ObtenerDetalleReporte(id);
            if (reporte == null)
            {
                return NotFound();
            }
            var reportes = await _reporteService.ObtenerDatosReportes();
            return View("~/Views/Reportes/Index.cshtml", reportes);


        }
    }
}
