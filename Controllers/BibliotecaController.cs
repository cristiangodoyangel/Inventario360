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
            var categorias = new List<string> { "Camionetas", "Proyectos", "Trabajadores" };
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
    }
}
