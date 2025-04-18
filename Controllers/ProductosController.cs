﻿using Microsoft.AspNetCore.Mvc;
using Inventario360.Services;
using Inventario360.Models;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.AspNetCore.Http;
using System;
using Inventario360.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using OfficeOpenXml;
using OfficeOpenXml;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace Inventario360.Controllers
{
    [Authorize]
    public class ProductosController : Controller
    {
        private readonly IProductoService _productoService;
        private readonly InventarioDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductosController(IProductoService productoService, InventarioDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _productoService = productoService;
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var productos = await _productoService.ObtenerTodos();
            return View(productos);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Detalle(int id)
        {
            var producto = await _productoService.ObtenerPorId(id);
            if (producto == null) return NotFound();

            var proveedor = await _context.Proveedor.FindAsync(producto.Proveedor);
            ViewBag.NombreProveedor = proveedor != null ? proveedor.Nombre : "Proveedor no definido";

            return View(producto);
        }

        [Authorize(Roles = "Administrador,Proyectos")]
        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            ViewBag.Proveedores = await _context.Proveedor
                                               .Select(p => new SelectListItem
                                               {
                                                   Value = p.ID.ToString(),
                                                   Text = p.Nombre
                                               })
                                               .ToListAsync();
            return View();
        }

        [Authorize(Roles = "Administrador,Proyectos")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Producto producto, IFormFile ImagenArchivo)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Proveedores = await _context.Proveedor
                                                   .Select(p => new SelectListItem
                                                   {
                                                       Value = p.ID.ToString(),
                                                       Text = p.Nombre
                                                   })
                                                   .ToListAsync();
                return View(producto);
            }

            if (ImagenArchivo != null && ImagenArchivo.Length > 0)
            {
                var rutaCarpeta = Path.Combine(_hostEnvironment.WebRootPath, "images");
                var nombreArchivo = $"{producto.ITEM}_{Path.GetFileName(ImagenArchivo.FileName)}";
                var rutaArchivo = Path.Combine(rutaCarpeta, nombreArchivo);

                using (var stream = new FileStream(rutaArchivo, FileMode.Create))
                {
                    await ImagenArchivo.CopyToAsync(stream);
                }

                producto.Imagen = nombreArchivo;
            }

            await _productoService.Agregar(producto);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Administrador,Proyectos")]
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var producto = await _productoService.ObtenerPorId(id);
            if (producto == null) return NotFound();

            ViewBag.Proveedores = await _context.Proveedor
                                               .Select(p => new SelectListItem
                                               {
                                                   Value = p.ID.ToString(),
                                                   Text = p.Nombre
                                               })
                                               .ToListAsync();
            return View(producto);
        }

        [Authorize(Roles = "Administrador,Proyectos")]
        [HttpPost]
        public async Task<IActionResult> Editar(int id, Producto producto, IFormFile ImagenArchivo)
        {
            if (id != producto.ITEM) return NotFound();

            var productoExistente = await _context.Producto.FindAsync(id);
            if (productoExistente == null) return NotFound();

            if (ImagenArchivo != null && ImagenArchivo.Length > 0)
            {
                var uploadsPath = Path.Combine(_hostEnvironment.WebRootPath, "images");
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImagenArchivo.FileName);
                var filePath = Path.Combine(uploadsPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImagenArchivo.CopyToAsync(stream);
                }

                if (!string.IsNullOrEmpty(productoExistente.Imagen))
                {
                    var oldImagePath = Path.Combine(uploadsPath, productoExistente.Imagen);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                productoExistente.Imagen = fileName;
            }

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

        [Authorize(Roles = "Administrador,Proyectos")]
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

                var tieneDependencias = await _context.DetalleSalidaDeBodega.AnyAsync(d => d.ProductoID == id);
                if (tieneDependencias)
                {
                    return Json(new { success = false, message = "No se puede eliminar el producto porque está asociado a una Entrega de Materiales." });
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

        [AllowAnonymous]
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

        [HttpGet]
        public IActionResult Subir()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcesarExcel(IFormFile archivoExcel)
        {
            if (archivoExcel == null || archivoExcel.Length == 0)
            {
                TempData["Error"] = "Por favor, selecciona un archivo válido.";
                return RedirectToAction("Subir");
            }

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            int nuevos = 0;
            int actualizados = 0;

            using (var stream = new MemoryStream())
            {
                await archivoExcel.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    if (worksheet == null)
                    {
                        TempData["Error"] = "El archivo no contiene hojas.";
                        return RedirectToAction("Subir");
                    }

                    int rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        string nombreTecnico = worksheet.Cells[row, 2].Text.Trim();
                        string medida = worksheet.Cells[row, 3].Text.Trim();

                        var productoExistente = await _context.Producto
                            .FirstOrDefaultAsync(p => p.NombreTecnico == nombreTecnico && p.Medida == medida);

                        if (productoExistente != null)
                        {
                            int cantidadExcel = int.Parse(worksheet.Cells[row, 1].Text);
                            productoExistente.Cantidad = (productoExistente.Cantidad ?? 0) + cantidadExcel;
                            productoExistente.UnidadMedida = worksheet.Cells[row, 4].Text;
                            productoExistente.Marca = worksheet.Cells[row, 5].Text;
                            productoExistente.Descripcion = worksheet.Cells[row, 6].Text;
                            productoExistente.Imagen = string.IsNullOrWhiteSpace(worksheet.Cells[row, 7].Text) ? "no-image.png" : worksheet.Cells[row, 7].Text;
                            productoExistente.Proveedor = int.TryParse(worksheet.Cells[row, 8].Text, out int provId) ? provId : (int?)null;
                            productoExistente.Ubicacion = worksheet.Cells[row, 9].Text;
                            productoExistente.Estado = worksheet.Cells[row, 10].Text;
                            productoExistente.Categoria = worksheet.Cells[row, 11].Text;

                            actualizados++;
                        }
                        else
                        {
                            var nuevoProducto = new Producto
                            {
                                Cantidad = int.Parse(worksheet.Cells[row, 1].Text),
                                NombreTecnico = nombreTecnico,
                                Medida = medida,
                                UnidadMedida = worksheet.Cells[row, 4].Text,
                                Marca = worksheet.Cells[row, 5].Text,
                                Descripcion = worksheet.Cells[row, 6].Text,
                                Imagen = string.IsNullOrWhiteSpace(worksheet.Cells[row, 7].Text) ? "no-image.png" : worksheet.Cells[row, 7].Text,
                                Proveedor = int.TryParse(worksheet.Cells[row, 8].Text, out int provIdNuevo) ? provIdNuevo : (int?)null,
                                Ubicacion = worksheet.Cells[row, 9].Text,
                                Estado = worksheet.Cells[row, 10].Text,
                                Categoria = worksheet.Cells[row, 11].Text
                            };

                            _context.Producto.Add(nuevoProducto);
                            nuevos++;
                        }
                    }

                    await _context.SaveChangesAsync();
                    TempData["Mensaje"] = "Inventario actualizado correctamente.";
                    TempData["Nuevos"] = nuevos;
                    TempData["Actualizados"] = actualizados;
                }
            }

            return RedirectToAction("Index");
        }



    }
}
