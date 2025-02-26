using Microsoft.AspNetCore.Mvc;
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
                return View("Crear", solicitud); // ✅ Devuelve la vista en lugar de redirigir
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
                descripcion = p.Descripcion ?? "",  // ✅ Evita valores NULL
                marca = p.Marca ?? "",              // ✅ Evita valores NULL
                imagen = p.Imagen ?? "",            // ✅ Evita valores NULL
                posibleProveedor = p.Proveedor ?? 0 // Si `null`, asigna `0`

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
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // ✅ Solucionado el error de licencia

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Solicitudes");

                // Encabezados
                worksheet.Cells[1, 1].Value = "Nombre del Material";
                worksheet.Cells[1, 2].Value = "Cantidad";
                worksheet.Cells[1, 3].Value = "Medida";
                worksheet.Cells[1, 4].Value = "Unidad de Medida";
                worksheet.Cells[1, 5].Value = "Marca";
                worksheet.Cells[1, 6].Value = "Descripción";
                worksheet.Cells[1, 7].Value = "Posible Proveedor";

                // Obtener datos de la base de datos (ahora correctamente con await)
                var solicitudes = await _solicitudService.ObtenerTodas();

                if (solicitudes == null || !solicitudes.Any())
                {
                    return BadRequest("No hay datos para exportar.");
                }

                int fila = 2; // Comenzar en la fila 2 para los datos
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

                // Formatear la tabla
                worksheet.Cells.AutoFitColumns();

                // Convertir el archivo en un stream y descargarlo
                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                string nombreArchivo = $"Solicitudes_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreArchivo);
            }
        }

        [HttpPost]
        [Route("Solicitudes/DescargarPDF")]
        public async Task<IActionResult> DescargarPDF([FromBody] List<SolicitudDeMaterial> solicitudes)
        {
            if (solicitudes == null || !solicitudes.Any())
            {
                return BadRequest("No hay datos para exportar.");
            }

            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    Document document = new Document(PageSize.A4);
                    PdfWriter.GetInstance(document, stream);
                    document.Open();

                    // Agregar título
                    Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                    document.Add(new Paragraph("Lista de Solicitudes de Material", titleFont));
                    document.Add(new Paragraph("\n"));

                    // Crear tabla con columnas
                    PdfPTable table = new PdfPTable(7);
                    table.WidthPercentage = 100;
                    table.SetWidths(new float[] { 3, 1, 1, 2, 2, 3, 2 });

                    // Encabezados
                    string[] headers = { "Nombre", "Cantidad", "Medida", "Unidad", "Marca", "Descripción", "Imagen" };
                    foreach (var header in headers)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(header, FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
                        cell.BackgroundColor = new BaseColor(211, 211, 211);
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        table.AddCell(cell);
                    }

                    // Agregar datos
                    foreach (var solicitud in solicitudes)
                    {
                        table.AddCell(new Phrase(solicitud.NombreTecnico));
                        table.AddCell(new Phrase(solicitud.Cantidad.ToString()));
                        table.AddCell(new Phrase(solicitud.Medida));
                        table.AddCell(new Phrase(solicitud.UnidadMedida));
                        table.AddCell(new Phrase(solicitud.Marca));
                        table.AddCell(new Phrase(solicitud.Descripcion));

                        // Convertir imagen desde URL base64 o ruta local
                        if (!string.IsNullOrEmpty(solicitud.Imagen))
                        {
                            try
                            {
                                Image img;
                                if (solicitud.Imagen.StartsWith("data:image")) // Verificar si es base64
                                {
                                    byte[] imageBytes = Convert.FromBase64String(solicitud.Imagen.Split(',')[1]);
                                    img = Image.GetInstance(imageBytes);
                                }
                                else // Si es una ruta local
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
                                    PdfPCell imgCell = new PdfPCell(img);
                                    imgCell.HorizontalAlignment = Element.ALIGN_CENTER;
                                    table.AddCell(imgCell);
                                }
                                else
                                {
                                    table.AddCell(new Phrase("No disponible"));
                                }
                            }
                            catch
                            {
                                table.AddCell(new Phrase("Error al cargar"));
                            }
                        }
                        else
                        {
                            table.AddCell(new Phrase("Sin imagen"));
                        }
                    }

                    document.Add(table);
                    document.Close();

                    return File(stream.ToArray(), "application/pdf", "Solicitudes_Material.pdf");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al generar el PDF: {ex.Message}");
            }
        }


    }
}
