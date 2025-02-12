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

        // **Corrección: Método seguro para actualizar stock y registrar salida en una sola transacción**
        public async Task<bool> RegistrarSalida(SalidaDeBodega salida, Producto producto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Verificar si el producto existe y tiene suficiente stock
                if (producto == null || producto.Cantidad < salida.Cantidad)
                    return false;

                // Actualizar el stock del producto
                producto.Cantidad -= salida.Cantidad;
                _context.Producto.Update(producto);

                // Registrar la salida con la fecha actual
                salida.Fecha = DateTime.Now;
                _context.SalidaDeBodega.Add(salida);

                // Guardar ambos cambios en la base de datos
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
            var salida = await _context.SalidaDeBodega.FindAsync(id);
            if (salida != null)
            {
                _context.SalidaDeBodega.Remove(salida);
                await _context.SaveChangesAsync();
            }
        }
    }
}
