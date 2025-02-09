using System.Collections.Generic;
using System.Threading.Tasks;
using Inventario360.Models;
using Microsoft.EntityFrameworkCore;
using Inventario360.Data;

namespace Inventario360.Services
{
    public class SolicitudService : ISolicitudService
    {
        private readonly InventarioDbContext _context;

        public SolicitudService(InventarioDbContext context)
        {
            _context = context;
        }

        public async Task<List<SolicitudDeMaterial>> ObtenerTodas()
        {
            return await _context.SolicitudDeMaterial.ToListAsync();
        }

        public async Task<SolicitudDeMaterial?> ObtenerPorId(int id)
        {
            return await _context.SolicitudDeMaterial.FindAsync(id);
        }

        public async Task Agregar(SolicitudDeMaterial solicitud)
        {
            _context.SolicitudDeMaterial.Add(solicitud);
            await _context.SaveChangesAsync();
        }

        public async Task Actualizar(SolicitudDeMaterial solicitud)
        {
            _context.SolicitudDeMaterial.Update(solicitud);
            await _context.SaveChangesAsync();
        }

        public async Task Eliminar(int id)
        {
            var solicitud = await _context.SolicitudDeMaterial.FindAsync(id);
            if (solicitud != null)
            {
                _context.SolicitudDeMaterial.Remove(solicitud);
                await _context.SaveChangesAsync();
            }
        }
    }
}
