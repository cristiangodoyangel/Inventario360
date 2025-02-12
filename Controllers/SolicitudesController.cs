using Microsoft.AspNetCore.Mvc;
using Inventario360.Models;
using Inventario360.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inventario360.Controllers
{
    public class SolicitudesController : Controller
    {
        private readonly ISolicitudService _solicitudService;
        private readonly IProductoService _productoService;

        public SolicitudesController(ISolicitudService solicitudService, IProductoService productoService)
        {
            _solicitudService = solicitudService;
            _productoService = productoService;
        }

        public async Task<IActionResult> Index()
        {
            var solicitudes = await _solicitudService.GetAllSolicitudesAsync();
            return View(solicitudes);
        }

        [HttpPost]
        public async Task<IActionResult> AgregarDesdeInventario(int productoId, int cantidad, string medida, string unidadMedida, string marca, string posibleProveedor)
        {
            var producto = await _productoService.GetProductoByIdAsync(productoId);
            if (producto == null)
            {
                return NotFound();
            }

            var solicitud = new SolicitudDeMaterial
            {
                NombreTecnico = producto.NombreTecnico,
                Descripcion = producto.Descripcion,
                Imagen = producto.Imagen,
                Producto = productoId,
                Cantidad = cantidad,
                Medida = medida,
                UnidadMedida = unidadMedida,
                Marca = marca,
                PosibleProveedor = posibleProveedor
            };

            await _solicitudService.AddSolicitudAsync(solicitud);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> CrearNuevoProducto(SolicitudDeMaterial solicitud)
        {
            if (!ModelState.IsValid)
            {
                return View("Index");
            }

            await _solicitudService.AddSolicitudAsync(solicitud);
            return RedirectToAction("Index");
        }
    }
}
