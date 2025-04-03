using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Inventario360.Data;
using Inventario360.Models;

namespace Inventario360.Services
{
    public class FichaCamionetaService : IFichaCamionetaService
    {
        private readonly InventarioDbContext _context;

        public FichaCamionetaService(InventarioDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FichaCamioneta>> ObtenerFichas()
        {
            return await _context.FichaCamionetas.ToListAsync();
        }

        public async Task<FichaCamioneta> ObtenerFichaPorId(int id)
        {
            return await _context.FichaCamionetas.FindAsync(id);
        }

        public async Task CrearFicha(FichaCamioneta ficha)
        {
            _context.FichaCamionetas.Add(ficha);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ActualizarFicha(FichaCamioneta ficha)
        {
            var fichaExistente = await _context.FichaCamionetas.FindAsync(ficha.ID);
            if (fichaExistente == null) return false;

            // Verificar si el Responsable existe
            var responsableExiste = await _context.Empleado.AnyAsync(e => e.ID == ficha.ResponsableID);
            if (!responsableExiste)
            {
                return false; // Retorna falso si el Responsable no existe
            }

            _context.Entry(fichaExistente).CurrentValues.SetValues(ficha);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<bool> EliminarFicha(int id)
        {
            var ficha = await _context.FichaCamionetas.FindAsync(id);
            if (ficha == null) return false;

            _context.FichaCamionetas.Remove(ficha);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<FichaCamioneta>> ObtenerTodas()
        {
            return await _context.FichaCamionetas.ToListAsync();
        }

        public async Task<FichaCamioneta> ObtenerPorId(int id)
        {
            return await _context.FichaCamionetas.FindAsync(id);
        }

        // ✅ Agregado: Verifica si el responsable existe en la tabla Empleado
        public async Task<bool> ExisteResponsable(int responsableID)
        {
            return await _context.Empleado.AnyAsync(e => e.ID == responsableID);
        }

        // ✅ Agregado: Obtiene la lista de empleados
        public async Task<List<Empleado>> ObtenerEmpleados()
        {
            return await _context.Empleado.ToListAsync();
        }

        public async Task<List<FichaCamioneta>> ObtenerTodasConResponsableAsync()
        {
            return await _context.FichaCamionetas
                .Include(c => c.Responsable)
                .ToListAsync();
        }

        public async Task<FichaCamioneta?> ObtenerDetalleConResponsable(int id)
        {
            return await _context.FichaCamionetas
                .Include(c => c.Responsable)
                .FirstOrDefaultAsync(c => c.ID == id);
        }




    }
}
