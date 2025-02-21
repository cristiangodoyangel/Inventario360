using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Inventario360.Models;
using Microsoft.EntityFrameworkCore;
using Inventario360.Data;
using Newtonsoft.Json;
using System.Linq;


namespace Inventario360.Services
{
    public class SalidaBodegaService : ISalidaBodegaService
    {
        private readonly InventarioDbContext _context;

        public SalidaBodegaService(InventarioDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SalidaDeBodega>> ObtenerTodas()
        {
            return await _context.SalidasDeBodega
                .Include(s => s.SolicitanteObj)
                .Include(s => s.ResponsableEntregaObj)
                .Include(s => s.ProyectoObj)
                .ToListAsync();
        }

        public async Task<SalidaDeBodega> ObtenerPorId(int id)
        {
            return await _context.SalidasDeBodega
                .Include(s => s.Detalles)
                .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(s => s.ID == id);
        }

        public async Task<bool> RegistrarSalidaConProductos(SalidaDeBodega salida, List<DetalleSalidaDeBodega> productos)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Validar que haya al menos un producto
                if (productos == null || productos.Count == 0)
                    return false;

                // Registrar la salida de bodega
                salida.Fecha = DateTime.Now;
                _context.SalidaDeBodega.Add(salida);
                await _context.SaveChangesAsync(); // Se guarda para obtener el ID

                // Procesar cada producto en la salida
                foreach (var detalle in productos)
                {
                    var producto = await _context.Producto.FindAsync(detalle.ProductoID);
                    if (producto == null || producto.Cantidad < detalle.Cantidad)
                    {
                        await transaction.RollbackAsync();
                        return false;
                    }

                    // Reducir stock del producto
                    producto.Cantidad -= detalle.Cantidad;
                    _context.Producto.Update(producto);

                    // Asociar el detalle con la salida creada
                    detalle.SalidaDeBodegaID = salida.ID;
                    _context.DetalleSalidaDeBodega.Add(detalle);
                }

                // Guardar cambios y confirmar la transacción
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

        public async Task Eliminar(int id)
        {
            var salida = await _context.SalidasDeBodega.FindAsync(id);
            if (salida != null)
            {
                _context.SalidasDeBodega.Remove(salida);
                await _context.SaveChangesAsync();
            }
        }

        public async Task EliminarDetalles(int salidaId) // ✅ Agregar implementación
        {
            var detalles = _context.DetallesSalidasDeBodega.Where(d => d.SalidaDeBodegaID == salidaId);
            _context.DetallesSalidasDeBodega.RemoveRange(detalles);
            await _context.SaveChangesAsync();
        }
        
    
    }
        
   
    
}
