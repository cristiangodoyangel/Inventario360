using Inventario360.Data;
using Inventario360.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Inventario360.Services
{
    public class SolicitudService : ISolicitudService
    {
        private readonly InventarioDbContext _context;

        public SolicitudService(InventarioDbContext context)
        {
            _context = context;
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
    }
}
