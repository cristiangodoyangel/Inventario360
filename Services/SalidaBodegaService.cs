using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventario360.Data;
using Inventario360.Models;
using Microsoft.EntityFrameworkCore;

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
            return await _context.SalidaDeBodega
                .Include(s => s.SolicitanteObj)
                .Include(s => s.ResponsableEntregaObj)
                .Include(s => s.ProyectoObj)
                .ToListAsync();
        }

        public async Task<SalidaDeBodega> ObtenerPorId(int id)
        {
            return await _context.SalidaDeBodega
                .Include(s => s.SolicitanteObj)
                .Include(s => s.ResponsableEntregaObj)
                .Include(s => s.ProyectoObj)
                .Include(s => s.Detalles)
                .ThenInclude(d => d.Producto) // ✅ Incluir producto en cada detalle
                .FirstOrDefaultAsync(s => s.ID == id);
        }

        public async Task<List<DetalleSalidaDeBodega>> ObtenerDetallesPorSalida(int id)
        {
            return await _context.DetalleSalidaDeBodega
                .Where(d => d.SalidaDeBodegaID == id)
                .Include(d => d.Producto) // ✅ Solo incluir Producto
                .ToListAsync();
        }

        public async Task<bool> RegistrarSalidaConProductos(SalidaDeBodega salida, List<DetalleSalidaDeBodega> productos)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (productos == null || productos.Count == 0)
                    return false;

                salida.Fecha = DateTime.Now;
                _context.SalidaDeBodega.Add(salida);
                await _context.SaveChangesAsync();

                foreach (var detalle in productos)
                {
                    var producto = await _context.Producto.FindAsync(detalle.ProductoID);
                    if (producto == null || producto.Cantidad < detalle.Cantidad)
                    {
                        await transaction.RollbackAsync();
                        return false;
                    }

                    producto.Cantidad -= detalle.Cantidad;
                    _context.Producto.Update(producto);

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

        public async Task Eliminar(int id)
        {
            var salida = await _context.SalidaDeBodega.FindAsync(id);
            if (salida != null)
            {
                _context.SalidaDeBodega.Remove(salida);
                await _context.SaveChangesAsync();
            }
        }

        public async Task EliminarDetalles(int salidaId)
        {
            var detalles = _context.DetalleSalidaDeBodega.Where(d => d.SalidaDeBodegaID == salidaId);
            _context.DetalleSalidaDeBodega.RemoveRange(detalles);
            await _context.SaveChangesAsync();
        }

        public async Task RevertirStock(int salidaId)
        {
            Console.WriteLine($"⏳ Revertiendo stock para la salida de bodega ID: {salidaId}");

            var detalles = await _context.DetalleSalidaDeBodega
                .Where(d => d.SalidaDeBodegaID == salidaId)
                .Include(d => d.Producto)
                .ToListAsync();

            if (detalles == null || detalles.Count == 0)
            {
                Console.WriteLine("⚠️ No se encontraron detalles para esta salida.");
                return;
            }

            foreach (var detalle in detalles)
            {
                if (detalle.Producto != null)
                {
                    Console.WriteLine($"🔄 Producto ID {detalle.ProductoID}: sumando {detalle.Cantidad} unidades.");
                    detalle.Producto.Cantidad += detalle.Cantidad; // ✅ Restaurar stock
                    _context.Producto.Update(detalle.Producto);
                }
            }

            await _context.SaveChangesAsync();
            Console.WriteLine("✅ Stock revertido correctamente.");
        }

        public async Task<object> ObtenerDatosResumenSalidas(int mes, int año)
        {
            var resumen = new
            {
                proyectosMasSolicitados = await _context.SalidaDeBodega
                    .Where(s => s.Fecha.HasValue && s.Fecha.Value.Month == mes && s.Fecha.Value.Year == año)
                    .GroupBy(s => s.ProyectoAsignado)
                    .OrderByDescending(g => g.Count())
                    .Select(g => new { Proyecto = g.Key, Total = g.Count() })
                    .FirstOrDefaultAsync(),

                empleadosMasSolicitantes = await _context.SalidaDeBodega
                    .Where(s => s.Fecha.HasValue && s.Fecha.Value.Month == mes && s.Fecha.Value.Year == año)
                    .GroupBy(s => s.Solicitante)
                    .OrderByDescending(g => g.Count())
                    .Select(g => new { Empleado = g.Key, Total = g.Count() })
                    .FirstOrDefaultAsync(),

                materialesMasSolicitados = await _context.DetalleSalidaDeBodega
                    .Where(d => d.SalidaDeBodega.Fecha.HasValue && d.SalidaDeBodega.Fecha.Value.Month == mes && d.SalidaDeBodega.Fecha.Value.Year == año)
                    .Include(d => d.Producto)
                    .GroupBy(d => d.Producto.NombreTecnico)
                    .OrderByDescending(g => g.Sum(d => d.Cantidad))
                    .Select(g => new { Material = g.Key, TotalCantidad = g.Sum(d => d.Cantidad) })
                    .Take(3)
                    .ToListAsync()
            };

            return resumen;
        }


    }
}
