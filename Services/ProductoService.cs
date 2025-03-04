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

        public async Task<IEnumerable<Producto>> ObtenerTodosAsync()
        {
            return await _context.Producto.ToListAsync();
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
            var productoExistente = await _context.Producto.FindAsync(producto.ITEM);
            if (productoExistente != null)
            {
                // Actualizar todos los campos, incluyendo la categoría
                productoExistente.Cantidad = producto.Cantidad;
                productoExistente.NombreTecnico = producto.NombreTecnico;
                productoExistente.Medida = producto.Medida;
                productoExistente.UnidadMedida = producto.UnidadMedida;
                productoExistente.Marca = producto.Marca;
                productoExistente.Descripcion = producto.Descripcion;
                productoExistente.Imagen = producto.Imagen;
                productoExistente.Proveedor = producto.Proveedor;
                productoExistente.Ubicacion = producto.Ubicacion;
                productoExistente.Estado = producto.Estado;
                productoExistente.Categoria = producto.Categoria; // ✅ Se agregó la categoría

                _context.Entry(productoExistente).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
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
