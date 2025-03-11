using Inventario360.Data;
using Inventario360.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace Inventario360.Services
{
    public class SolicitudService : ISolicitudService
    {
        private readonly InventarioDbContext _context;
        private readonly string _uploadsFolder;

        // Constructor único con inyección de dependencias
        public SolicitudService(InventarioDbContext context)
        {
            _context = context;
            _uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

            if (!Directory.Exists(_uploadsFolder))
            {
                Directory.CreateDirectory(_uploadsFolder);
            }
        }

        public async Task<IEnumerable<SolicitudDeMaterial>> GetAllSolicitudesAsync()
        {
            return await _context.SolicitudDeMaterial.ToListAsync();
        }

        public async Task<IEnumerable<SolicitudDeMaterial>> ObtenerTodas()
        {
            return await _context.SolicitudDeMaterial.ToListAsync();
        }

        public async Task<SolicitudDeMaterial> GetSolicitudByIdAsync(int id)
        {
            return await _context.SolicitudDeMaterial.FindAsync(id);
        }

        public async Task AddSolicitudAsync(SolicitudDeMaterial solicitud)
        {
            _context.SolicitudDeMaterial.Add(solicitud);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSolicitudAsync(SolicitudDeMaterial solicitud)
        {
            var existingSolicitud = await _context.SolicitudDeMaterial.FindAsync(solicitud.ID);
            if (existingSolicitud != null)
            {
                existingSolicitud.Cantidad = solicitud.Cantidad;
                existingSolicitud.Medida = solicitud.Medida;
                existingSolicitud.UnidadMedida = solicitud.UnidadMedida;
                existingSolicitud.Marca = solicitud.Marca;
                existingSolicitud.PosibleProveedor = solicitud.PosibleProveedor;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteSolicitudAsync(int id)
        {
            var solicitud = await _context.SolicitudDeMaterial.FindAsync(id);
            if (solicitud != null)
            {
                _context.SolicitudDeMaterial.Remove(solicitud);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<string> SubirImagenAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("No se ha proporcionado ninguna imagen.");
            }

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(_uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileName;
        }
    }
}
