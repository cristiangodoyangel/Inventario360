using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Inventario360.Data;
using Inventario360.Models;

namespace Inventario360.Services
{
    public class CamionetaService : ICamionetaService
    {
        private readonly InventarioDbContext _context;

        public CamionetaService(InventarioDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Camioneta>> ObtenerCamionetas()
        {
            return await _context.Camionetas.Include(c => c.Responsable).ToListAsync();
        }

        public async Task<Camioneta> ObtenerCamionetaPorId(int id)
        {
            return await _context.Camionetas.Include(c => c.Responsable)
                                            .FirstOrDefaultAsync(c => c.ID == id);
        }

        public async Task<bool> CrearCamioneta(Camioneta camioneta)
        {
            _context.Camionetas.Add(camioneta);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> EditarCamioneta(Camioneta camioneta)
        {
            _context.Camionetas.Update(camioneta);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> EliminarCamioneta(int id)
        {
            var camioneta = await _context.Camionetas.FindAsync(id);
            if (camioneta == null) return false;

            _context.Camionetas.Remove(camioneta);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
