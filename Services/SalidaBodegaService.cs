using System.Collections.Generic;
using System.Threading.Tasks;
using Inventario360.Models;
using Microsoft.EntityFrameworkCore;
using Inventario360.Data;

namespace Inventario360.Services
{
    public class SalidaBodegaService : ISalidaBodegaService
    {
        private readonly InventarioDbContext _context;

        public SalidaBodegaService(InventarioDbContext context)
        {
            _context = context;
        }

        public async Task<List<SalidaDeBodega>> ObtenerTodas()
        {
            return await _context.SalidaDeBodega.ToListAsync();
        }

        public async Task<SalidaDeBodega?> ObtenerPorId(int id)
        {
            return await _context.SalidaDeBodega.FindAsync(id);
        }

        public async Task Agregar(SalidaDeBodega salida)
        {
            _context.SalidaDeBodega.Add(salida);
            await _context.SaveChangesAsync();
        }

        public async Task Actualizar(SalidaDeBodega salida)
        {
            _context.SalidaDeBodega.Update(salida);
            await _context.SaveChangesAsync();
        }

        public async Task Eliminar(int id)
        {
            var salida = await _context.SalidaDeBodega.FindAsync(id);
            if (salida != null)
            {
                _context.SalidaDeBodega.Remove(salida);
                await _context.SaveChangesAsync();
            }
        }
    }
}
