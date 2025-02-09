using System.Collections.Generic;
using System.Threading.Tasks;
using Inventario360.Models;
using Microsoft.EntityFrameworkCore;
using Inventario360.Data;

namespace Inventario360.Services
{
    public class ProveedorService : IProveedorService
    {
        private readonly InventarioDbContext _context;

        public ProveedorService(InventarioDbContext context)
        {
            _context = context;
        }

        public async Task<List<Proveedor>> ObtenerTodos()
        {
            return await _context.Proveedor.ToListAsync();
        }

        public async Task<Proveedor?> ObtenerPorId(int id)
        {
            return await _context.Proveedor.FindAsync(id);
        }

        public async Task Agregar(Proveedor proveedor)
        {
            _context.Proveedor.Add(proveedor);
            await _context.SaveChangesAsync();
        }

        public async Task Actualizar(Proveedor proveedor)
        {
            _context.Proveedor.Update(proveedor);
            await _context.SaveChangesAsync();
        }

        public async Task Eliminar(int id)
        {
            var proveedor = await _context.Proveedor.FindAsync(id);
            if (proveedor != null)
            {
                _context.Proveedor.Remove(proveedor);
                await _context.SaveChangesAsync();
            }
        }
    }
}
