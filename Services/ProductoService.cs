using System.Collections.Generic;
using System.Threading.Tasks;
using Inventario360.Models;
using Microsoft.EntityFrameworkCore;
using Inventario360.Data;

namespace Inventario360.Services
{
    public class ProductoService : IProductoService
    {
        private readonly InventarioDbContext _context;

        public ProductoService(InventarioDbContext context)
        {
            _context = context;
        }

        public async Task<List<Producto>> ObtenerTodos()
        {
            return await _context.Producto.ToListAsync();
        }

        public async Task<IEnumerable<object>> ObtenerTodosAsync()
        {
            return await _context.Producto
                .Select(p => new
                {
                    p.ITEM,
                    p.NombreTecnico
                })
                .ToListAsync();
        }



        public async Task<Producto?> ObtenerPorId(int id)
        {
            return await _context.Producto.FindAsync(id);
        }

        public async Task<Producto?> GetProductoByIdAsync(int id)
        {
            return await _context.Producto.FindAsync(id);
        }


        public async Task Agregar(Producto producto)
        {
            await _context.Producto.AddAsync(producto);
            await _context.SaveChangesAsync();
        }

        public async Task Actualizar(Producto producto)
        {
            _context.Entry(producto).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task Eliminar(int id)
        {
            var producto = await _context.Producto.FindAsync(id);
            if (producto != null)
            {
                _context.Producto.Remove(producto);
                await _context.SaveChangesAsync();
            }
        }
    }
}
