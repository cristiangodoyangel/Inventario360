using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Inventario360.Data;
using Inventario360.Models;

namespace Inventario360.Services
{
    public class FichaEmpleadoService : IFichaEmpleadoService
    {
        private readonly InventarioDbContext _context;

        public FichaEmpleadoService(InventarioDbContext context)
        {
            _context = context;
        }

        public async Task<List<FichaEmpleado>> ObtenerFichas()
        {
            return await _context.FichaEmpleado.Include(f => f.Empleado).ToListAsync();
        }

        public async Task<FichaEmpleado> ObtenerFichaPorEmpleado(int empleadoId)
        {
            return await _context.FichaEmpleado
                .Include(f => f.Empleado)
                .FirstOrDefaultAsync(f => f.EmpleadoID == empleadoId);
        }

        public async Task<FichaEmpleado> CrearFicha(FichaEmpleado ficha)
        {
            _context.FichaEmpleado.Add(ficha);
            await _context.SaveChangesAsync();
            return ficha;
        }

        public async Task<FichaEmpleado> ActualizarFicha(FichaEmpleado ficha)
        {
            _context.FichaEmpleado.Update(ficha);
            await _context.SaveChangesAsync();
            return ficha;
        }

        public async Task<bool> EliminarFicha(int id)
        {
            var ficha = await _context.FichaEmpleado.FindAsync(id);
            if (ficha == null) return false;

            _context.FichaEmpleado.Remove(ficha);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
