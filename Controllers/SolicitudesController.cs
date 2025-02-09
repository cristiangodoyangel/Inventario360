using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Inventario360.Models;

namespace Inventario360.Controllers
{
    public class SolicitudesController : Controller
    {
        // Simulación de datos (esto se reemplazará con la BD)
        private static List<SolicitudDeMaterial> solicitudes = new List<SolicitudDeMaterial>
        {
            new SolicitudDeMaterial { ID = 1, ITEM = 1, Cantidad = 10, NombreTecnico = "Cable UTP", Medida = "10", UnidadMedida = "mts", Marca = "TP-Link", Descripcion = "Cable de red categoría 6", Imagen = "cable.jpg", Producto = 1 },
            new SolicitudDeMaterial { ID = 2, ITEM = 2, Cantidad = 50, NombreTecnico = "Tornillos 3mm", Medida = "3", UnidadMedida = "mm", Marca = "Fischer", Descripcion = "Tornillos de acero inoxidable", Imagen = "tornillos.jpg", Producto = 2 }
        };

        public IActionResult Index()
        {
            return View(solicitudes);
        }

        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Crear(SolicitudDeMaterial solicitud)
        {
            if (ModelState.IsValid)
            {
                solicitud.ID = solicitudes.Count + 1;
                solicitudes.Add(solicitud);
                return RedirectToAction("Index");
            }
            return View(solicitud);
        }

        public IActionResult Detalle(int id)
        {
            var solicitud = solicitudes.Find(s => s.ID == id);
            if (solicitud == null)
                return NotFound();

            return View(solicitud);
        }
    }
}

