using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Inventario360.Models;

namespace Inventario360.Controllers
{
    public class ProductosController : Controller
    {
        // Simulación de datos (esto se reemplazará con la BD)
        private static List<Producto> productos = new List<Producto>
        {
            new Producto { ITEM = 1, Cantidad = 50, NombreTecnico = "Cable UTP", Medida = "10", UnidadMedida = "mts", Marca = "TP-Link", Descripcion = "Cable de red categoría 6", Imagen = "cable.jpg", Proveedor = 1, Ubicacion = "Bodega 1", Estado = "Nuevo" },
            new Producto { ITEM = 2, Cantidad = 200, NombreTecnico = "Tornillos 3mm", Medida = "3", UnidadMedida = "mm", Marca = "Fischer", Descripcion = "Tornillos de acero inoxidable", Imagen = "tornillos.jpg", Proveedor = 2, Ubicacion = "Bodega 2", Estado = "Usado" },
            new Producto { ITEM = 3, Cantidad = 100, NombreTecnico = "Cinta Aislante", Medida = "5", UnidadMedida = "mts", Marca = "3M", Descripcion = "Cinta aislante eléctrica negra", Imagen = "cinta.jpg", Proveedor = 3, Ubicacion = "Bodega 3", Estado = "Nuevo" }
        };

        public IActionResult Index()
        {
            return View(productos);
        }

        public IActionResult Detalle(int id)
        {
            var producto = productos.Find(p => p.ITEM == id);
            if (producto == null)
                return NotFound();

            return View(producto);
        }
    }
}
