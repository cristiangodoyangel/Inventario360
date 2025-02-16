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
            return await _context.SalidaDeBodega
                .Include(s => s.Detalles) // Incluye los detalles de la salida
                .ThenInclude(d => d.Producto) // Incluye información del producto
                .ToListAsync();
        }

        public async Task<SalidaDeBodega?> ObtenerPorId(int id)
        {
            return await _context.SalidaDeBodega
                .Include(s => s.Detalles)
                .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(s => s.ID == id);
        }

        // Método corregido para registrar múltiples productos en una sola salida
        public async Task<bool> RegistrarSalida(SalidaDeBodega salida, List<DetalleSalidaDeBodega> detalles)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                foreach (var detalle in detalles)
                {
                    var producto = await _context.Producto.FindAsync(detalle.ProductoID);
                    if (producto == null || producto.Cantidad < detalle.Cantidad)
                        return false;

                    // Reducir el stock del producto
                    producto.Cantidad -= detalle.Cantidad;
                    _context.Producto.Update(producto);
                }

                // Guardar la salida y los detalles
                salida.Fecha = DateTime.Now;
                _context.SalidaDeBodega.Add(salida);
                await _context.SaveChangesAsync();

                foreach (var detalle in detalles)
                {
                    detalle.SalidaDeBodegaID = salida.ID;
                    _context.DetalleSalidaDeBodega.Add(detalle);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }


        public async Task Actualizar(SalidaDeBodega salida)
        {
            _context.Entry(salida).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task Eliminar(int id)
        {
            var salida = await _context.SalidaDeBodega
                .Include(s => s.Detalles)
                .FirstOrDefaultAsync(s => s.ID == id);

            if (salida != null)
            {
                // Restaurar stock antes de eliminar la salida
                foreach (var detalle in salida.Detalles)
                {
                    var producto = await _context.Producto.FindAsync(detalle.ProductoID);
                    if (producto != null)
                    {
                        producto.Cantidad += detalle.Cantidad; // Revertir la cantidad
                        _context.Producto.Update(producto);
                    }
                }

                _context.SalidaDeBodega.Remove(salida);
                await _context.SaveChangesAsync();
            }
        }
    }
}
