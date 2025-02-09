using System.Collections.Generic;
using System.Threading.Tasks;
using Inventario360.Models;
using Microsoft.EntityFrameworkCore;
using Inventario360.Data;

namespace Inventario360.Services
{
    public class EmpleadoService : IEmpleadoService
    {
        private readonly InventarioDbContext _context;

        public EmpleadoService(InventarioDbContext context)
        {
            _context = context;
        }

        public async Task<List<Empleado>> ObtenerTodos()
        {
            return await _context.Empleado.ToListAsync();
        }

        public async Task<Empleado?> ObtenerPorId(int id)
        {
            return await _context.Empleado.FindAsync(id);
        }

        public async Task Agregar(Empleado empleado)
        {
            _context.Empleado.Add(empleado);
            await _context.SaveChangesAsync();
        }

        public async Task Actualizar(Empleado empleado)
        {
            _context.Empleado.Update(empleado);
            await _context.SaveChangesAsync();
        }

        public async Task Eliminar(int id)
        {
            var empleado = await _context.Empleado.FindAsync(id);
            if (empleado != null)
            {
                _context.Empleado.Remove(empleado);
                await _context.SaveChangesAsync();
            }
        }
    }
}
