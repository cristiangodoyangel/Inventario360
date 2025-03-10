using Microsoft.AspNetCore.Mvc;
using Inventario360.Services;
using Inventario360.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.AspNetCore.Http;
using System;
using Inventario360.Data;

namespace Inventario360.Controllers
{
    public class ProductosController : Controller
    {
        private readonly IProductoService _productoService;
        private readonly InventarioDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        // Constructor único que inyecta las dependencias necesarias
        public ProductosController(IProductoService productoService, InventarioDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _productoService = productoService;
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var productos = await _productoService.ObtenerTodos();
            return View(productos);
        }

        public async Task<IActionResult> Detalle(int id)
        {
            var producto = await _productoService.ObtenerPorId(id);
            if (producto == null) return NotFound();
            return View(producto);
        }

        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Producto producto, IFormFile ImagenArchivo)
        {
            if (!ModelState.IsValid) return View(producto);

            if (ImagenArchivo != null && ImagenArchivo.Length > 0)
            {
                var rutaCarpeta = Path.Combine(_hostEnvironment.WebRootPath, "images");
                var nombreArchivo = $"{producto.ITEM}_{Path.GetFileName(ImagenArchivo.FileName)}";
                var rutaArchivo = Path.Combine(rutaCarpeta, nombreArchivo);

                using (var stream = new FileStream(rutaArchivo, FileMode.Create))
                {
                    await ImagenArchivo.CopyToAsync(stream);
                }

                producto.Imagen = nombreArchivo; // Guardar el nombre de la imagen en la BD
            }

            await _productoService.Agregar(producto);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var producto = await _productoService.ObtenerPorId(id);
            if (producto == null)
            {
                return NotFound();
            }
            return View(producto);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(int id, Producto producto, IFormFile ImagenArchivo)
        {
            if (id != producto.ITEM)
            {
                return NotFound();
            }

            // Obtener el producto actual desde la base de datos
            var productoExistente = await _context.Producto.FindAsync(id);
            if (productoExistente == null)
            {
                return NotFound();
            }

            // Mantener la imagen existente si no se sube una nueva
            if (ImagenArchivo != null && ImagenArchivo.Length > 0)
            {
                // Guardar la nueva imagen
                var uploadsPath = Path.Combine(_hostEnvironment.WebRootPath, "images");
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImagenArchivo.FileName);
                var filePath = Path.Combine(uploadsPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImagenArchivo.CopyToAsync(stream);
                }

                // Eliminar la imagen anterior si existe
                if (!string.IsNullOrEmpty(productoExistente.Imagen))
                {
                    var oldImagePath = Path.Combine(uploadsPath, productoExistente.Imagen);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                // Guardar el nombre de la nueva imagen en la base de datos
                productoExistente.Imagen = fileName;
            }

            // Si no se sube una nueva imagen, mantener la actual
            else
            {
                producto.Imagen = productoExistente.Imagen;
            }

            // Actualizar el resto de los campos
            productoExistente.NombreTecnico = producto.NombreTecnico;
            productoExistente.Medida = producto.Medida;
            productoExistente.UnidadMedida = producto.UnidadMedida;
            productoExistente.Marca = producto.Marca;
            productoExistente.Cantidad = producto.Cantidad;
            productoExistente.Descripcion = producto.Descripcion;
            productoExistente.Ubicacion = producto.Ubicacion;
            productoExistente.Estado = producto.Estado;
            productoExistente.Proveedor = producto.Proveedor;
            productoExistente.Categoria = producto.Categoria;

            _context.Update(productoExistente);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                var producto = await _context.Producto.FindAsync(id);
                if (producto == null)
                {
                    return Json(new { success = false, message = "No se encontró el producto." });
                }

                // Verificar si el producto tiene dependencias antes de eliminarlo
                var tieneDependencias = await _context.DetalleSalidaDeBodega.AnyAsync(d => d.ProductoID == id);
                if (tieneDependencias)
                {
                    return Json(new { success = false, message = "No se puede eliminar el producto porque está asociado a una salida de bodega." });
                }

                _context.Producto.Remove(producto);
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error interno: " + ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerProductos()
        {
            try
            {
                var productos = await _productoService.ObtenerTodos();

                if (productos == null || !productos.Any())
                {
                    return StatusCode(404, new { error = "No se encontraron productos." });
                }

                return Json(productos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al obtener productos", detalle = ex.Message });
            }
        }
    }
}
