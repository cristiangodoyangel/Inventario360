using Microsoft.AspNetCore.Mvc;
using Inventario360.Services;
using Inventario360.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;


namespace Inventario360.Controllers
{
    public class ProductosController : Controller
    {
        private readonly IProductoService _productoService;

        public ProductosController(IProductoService productoService)
        {
            _productoService = productoService;
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
        public async Task<IActionResult> Eliminar(int id)
        {
            var producto = await _productoService.ObtenerPorId(id);
            if (producto == null)
            {
                return Json(new { success = false, message = "El producto no existe." });
            }

            await _productoService.Eliminar(id);
            return Json(new { success = true });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Producto producto, IFormFile ImagenArchivo)
        {
            if (!ModelState.IsValid) return View(producto);

            if (ImagenArchivo != null && ImagenArchivo.Length > 0)
            {
                var rutaCarpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
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
        public async Task<IActionResult> Editar(Producto producto, IFormFile ImagenArchivo)
        {
            if (ModelState.IsValid)
            {
                var productoExistente = await _productoService.ObtenerPorId(producto.ITEM);
                if (productoExistente == null)
                {
                    ModelState.AddModelError("", "El producto no existe.");
                    return View(producto);
                }

                // Actualizar datos
                productoExistente.Cantidad = producto.Cantidad;
                productoExistente.NombreTecnico = producto.NombreTecnico;
                productoExistente.Medida = producto.Medida;
                productoExistente.UnidadMedida = producto.UnidadMedida;
                productoExistente.Marca = producto.Marca;
                productoExistente.Ubicacion = producto.Ubicacion;
                productoExistente.Estado = producto.Estado;

                // Guardar nueva imagen si se subió un archivo
                if (ImagenArchivo != null && ImagenArchivo.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                    var fileName = $"{Guid.NewGuid()}_{ImagenArchivo.FileName}";
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    // Guardar archivo en servidor
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImagenArchivo.CopyToAsync(stream);
                    }

                    // Actualizar campo de imagen en la base de datos
                    productoExistente.Imagen = fileName;
                }

                await _productoService.Actualizar(productoExistente);
                return RedirectToAction("Index");
            }

            return View(producto);
        }
        [HttpGet]
        public async Task<IActionResult> ObtenerProductos()
        {
            var productos = await _productoService.ObtenerTodosAsync();
            return Json(productos);
        }



    }
}
