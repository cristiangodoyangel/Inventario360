using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.Linq;

namespace Inventario360.Controllers
{
    public class BibliotecaController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public BibliotecaController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            var categorias = new List<string> { "Camionetas", "Proyectos", "Trabajadores", "Hsec" };
            var archivosPorCategoria = new Dictionary<string, List<string>>();

            foreach (var categoria in categorias)
            {
                string rutaCategoria = Path.Combine(_hostingEnvironment.WebRootPath, "documentos", categoria);
                if (!Directory.Exists(rutaCategoria))
                {
                    Directory.CreateDirectory(rutaCategoria);
                }

                var archivos = Directory.GetFiles(rutaCategoria).Select(Path.GetFileName).ToList();
                archivosPorCategoria[categoria] = archivos;
            }

            ViewBag.ArchivosPorCategoria = archivosPorCategoria;
            return View();
        }

        public IActionResult Descargar(string categoria, string nombreArchivo)
        {
            string rutaArchivo = Path.Combine(_hostingEnvironment.WebRootPath, "documentos", categoria, nombreArchivo);
            if (System.IO.File.Exists(rutaArchivo))
            {
                return PhysicalFile(rutaArchivo, "application/octet-stream", nombreArchivo);
            }
            return NotFound();
        }

        public IActionResult Camionetas()
        {
            string rutaCategoria = Path.Combine(_hostingEnvironment.WebRootPath, "documentos", "Camionetas");

            if (!Directory.Exists(rutaCategoria))
            {
                Directory.CreateDirectory(rutaCategoria);
            }

            var archivos = Directory.GetFiles(rutaCategoria).Select(Path.GetFileName).ToList();
            ViewBag.Archivos = archivos;
            return View();
        }

        [HttpPost]
        public IActionResult SubirDocumentoCamionetas(IFormFile archivo, string titulo)
        {
            if (archivo == null || archivo.Length == 0 || string.IsNullOrEmpty(titulo))
            {
                return Json(new { success = false, message = "Debe proporcionar un archivo y un título." });
            }

            string carpetaDestino = Path.Combine(_hostingEnvironment.WebRootPath, "documentos", "Camionetas");

            if (!Directory.Exists(carpetaDestino))
            {
                Directory.CreateDirectory(carpetaDestino);
            }

            string nombreArchivo = $"{titulo}{Path.GetExtension(archivo.FileName)}";
            string rutaArchivo = Path.Combine(carpetaDestino, nombreArchivo);

            using (var stream = new FileStream(rutaArchivo, FileMode.Create))
            {
                archivo.CopyTo(stream);
            }

            return Json(new { success = true });
        }

        [HttpDelete]
        public IActionResult EliminarDocumentoCamionetas(string archivo)
        {
            if (string.IsNullOrEmpty(archivo))
            {
                return Json(new { success = false, message = "El nombre del archivo es inválido." });
            }

            string rutaArchivo = Path.Combine(_hostingEnvironment.WebRootPath, "documentos", "Camionetas", archivo);

            if (System.IO.File.Exists(rutaArchivo))
            {
                try
                {
                    System.IO.File.Delete(rutaArchivo);
                    return Json(new { success = true });
                }
                catch
                {
                    return Json(new { success = false, message = "Error al eliminar el archivo." });
                }
            }

            return Json(new { success = false, message = "El archivo no existe." });
        }
    }
}
