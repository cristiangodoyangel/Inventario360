﻿using Microsoft.AspNetCore.Mvc;
using Inventario360.Models;
using Inventario360.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using OfficeOpenXml;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Inventario360.Data;


namespace Inventario360.Controllers
{
    public class SolicitudesController : Controller
    {
        private readonly ISolicitudService _solicitudService;
        private readonly IProductoService _productoService;
        private readonly InventarioDbContext _context;

        public SolicitudesController(ISolicitudService solicitudService, IProductoService productoService,
                                 InventarioDbContext context)
        {
            _solicitudService = solicitudService;
            _productoService = productoService;
            _context = context; 
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Proveedores = new SelectList(await _context.Proveedor.ToListAsync(), "ID", "Nombre");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AgregarDesdeInventario(int productoId, int cantidad, string medida, string unidadMedida, string marca, string posibleProveedor)
        {
            var producto = await _productoService.GetProductoByIdAsync(productoId);
            if (producto == null)
            {
                return Json(new { success = false, message = "Producto no encontrado" });
            }

            var solicitud = new SolicitudDeMaterial
            {
                ITEM = producto.ITEM,
                NombreTecnico = producto.NombreTecnico,
                Descripcion = producto.Descripcion,
                Imagen = producto.Imagen,
                Cantidad = cantidad,
                Medida = medida,
                UnidadMedida = unidadMedida,
                Marca = marca,
                PosibleProveedor = posibleProveedor,
                Fecha = DateTime.Now,
                Estado = "Pendiente"
            };

            await _solicitudService.AddSolicitudAsync(solicitud);
            return Json(new { success = true, solicitud });
        }



        [HttpPost]
        public async Task<IActionResult> CrearNuevoProducto(SolicitudDeMaterial solicitud)
        {
            if (!ModelState.IsValid)
            {
                return View("Crear", solicitud);
            }

            solicitud.Fecha = DateTime.Now;
            solicitud.Estado = "Pendiente";

            await _solicitudService.AddSolicitudAsync(solicitud);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerProductos()
        {
           var productos = (IEnumerable<Producto>)await _productoService.ObtenerTodosAsync();

            var productosDTO = productos.Select(p => new
            {
                item = p.ITEM,
                nombreTecnico = p.NombreTecnico,
                descripcion = p.Descripcion ?? "",  
                marca = p.Marca ?? "",             
                imagen = p.Imagen ?? "",           
                posibleProveedor = p.Proveedor ?? 0 

            }).ToList();

            return Json(productosDTO);
        }


        [HttpGet]
        public async Task<IActionResult> Detalle(int id)
        {
            var solicitud = await _solicitudService.GetSolicitudByIdAsync(id);
            if (solicitud == null)
            {
                return NotFound();
            }
            return View(solicitud);
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(int id)
        {
            var solicitud = await _solicitudService.GetSolicitudByIdAsync(id);
            if (solicitud == null)
            {
                return NotFound();
            }

            await _solicitudService.DeleteSolicitudAsync(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> DescargarExcel()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; 

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Solicitudes");

               
                worksheet.Cells[1, 1].Value = "Nombre del Material";
                worksheet.Cells[1, 2].Value = "Cantidad";
                worksheet.Cells[1, 3].Value = "Medida";
                worksheet.Cells[1, 4].Value = "Unidad de Medida";
                worksheet.Cells[1, 5].Value = "Marca";
                worksheet.Cells[1, 6].Value = "Descripción";
                worksheet.Cells[1, 7].Value = "Posible Proveedor";

              
                var solicitudes = await _solicitudService.ObtenerTodas();

                if (solicitudes == null || !solicitudes.Any())
                {
                    return BadRequest("No hay datos para exportar.");
                }

                int fila = 2; 
                foreach (var solicitud in solicitudes)
                {
                    worksheet.Cells[fila, 1].Value = solicitud.NombreTecnico;
                    worksheet.Cells[fila, 2].Value = solicitud.Cantidad;
                    worksheet.Cells[fila, 3].Value = solicitud.Medida;
                    worksheet.Cells[fila, 4].Value = solicitud.UnidadMedida;
                    worksheet.Cells[fila, 5].Value = solicitud.Marca;
                    worksheet.Cells[fila, 6].Value = solicitud.Descripcion;
                    worksheet.Cells[fila, 7].Value = solicitud.PosibleProveedor;
                    fila++;
                }

             
                worksheet.Cells.AutoFitColumns();

              
                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                string nombreArchivo = $"Solicitudes_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreArchivo);
            }
        }

        [HttpPost]
        [Route("Solicitudes/DescargarPDF")]
        public IActionResult DescargarPDF([FromBody] List<SolicitudDeMaterial> solicitudes)
        {
            if (solicitudes == null || !solicitudes.Any())
            {
                return BadRequest("No hay datos para exportar.");
            }

        
            foreach (var solicitud in solicitudes)
            {
                Console.WriteLine($"Proveedor recibido: {solicitud.PosibleProveedor}");
            }

            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    Document document = new Document(PageSize.A4.Rotate());
                    PdfWriter.GetInstance(document, stream);
                    document.Open();

                    Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                    Font normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);
                    document.Add(new Paragraph("Solicitud de Materiales", titleFont));
                    document.Add(new Paragraph("\n"));
                    document.Add(new Paragraph("Solicito estos materiales para esta semana:", normalFont));
                    document.Add(new Paragraph("\n"));

                    PdfPTable table = new PdfPTable(8);
                    table.WidthPercentage = 100;
                    table.SetWidths(new float[] { 3, 1, 1, 1, 2, 3, 2, 2 });

                    string[] headers = { "Nombre", "Cantidad", "Medida", "Unidad", "Marca", "Descripción", "Proveedor", "Imagen" };
                    BaseColor headerColor = new BaseColor(255, 204, 153);

                    foreach (var header in headers)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(header, FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
                        cell.BackgroundColor = headerColor;
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.Padding = 5f;
                        table.AddCell(cell);
                    }

                
                    foreach (var solicitud in solicitudes)
                    {
                        table.AddCell(new PdfPCell(new Phrase(solicitud.NombreTecnico)) { Padding = 5f });
                        table.AddCell(new PdfPCell(new Phrase(solicitud.Cantidad.ToString())) { Padding = 5f });
                        table.AddCell(new PdfPCell(new Phrase(solicitud.Medida)) { Padding = 5f });
                        table.AddCell(new PdfPCell(new Phrase(solicitud.UnidadMedida)) { Padding = 5f });
                        table.AddCell(new PdfPCell(new Phrase(solicitud.Marca)) { Padding = 5f });
                        table.AddCell(new PdfPCell(new Phrase(solicitud.Descripcion)) { Padding = 5f });

                        
                        string proveedorFinal = !string.IsNullOrEmpty(solicitud.PosibleProveedor) ? solicitud.PosibleProveedor : "Sin proveedor";
                        table.AddCell(new PdfPCell(new Phrase(proveedorFinal, FontFactory.GetFont(FontFactory.HELVETICA, 10))) { Padding = 5f });

                        
                        PdfPCell imgCell = new PdfPCell { Padding = 5f, HorizontalAlignment = Element.ALIGN_CENTER };

                        if (!string.IsNullOrEmpty(solicitud.Imagen))
                        {
                            try
                            {
                                Image img;
                                if (solicitud.Imagen.StartsWith("data:image")) // Base64
                                {
                                    byte[] imageBytes = Convert.FromBase64String(solicitud.Imagen.Split(',')[1]);
                                    img = Image.GetInstance(imageBytes);
                                }
                                else
                                {
                                    string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", solicitud.Imagen);
                                    if (System.IO.File.Exists(imagePath))
                                    {
                                        img = Image.GetInstance(imagePath);
                                    }
                                    else
                                    {
                                        img = null;
                                    }
                                }

                                if (img != null)
                                {
                                    img.ScaleAbsolute(50f, 50f);
                                    imgCell.AddElement(img);
                                }
                                else
                                {
                                    imgCell.AddElement(new Phrase("No disponible"));
                                }
                            }
                            catch
                            {
                                imgCell.AddElement(new Phrase("Error al cargar"));
                            }
                        }
                        else
                        {
                            imgCell.AddElement(new Phrase("Sin imagen"));
                        }

                        table.AddCell(imgCell);
                    }

                    document.Add(table);
                    document.Close();

                    return File(stream.ToArray(), "application/pdf", "Solicitud_Material.pdf");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al generar el PDF: {ex.Message}");
            }
        }


        [HttpPost]
        public async Task<IActionResult> SubirImagen(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No se ha proporcionado ninguna imagen.");
            }

            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Json(new { fileName });
        }

    }



}

