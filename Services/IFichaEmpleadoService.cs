using System.Collections.Generic;
using System.Threading.Tasks;
using Inventario360.Models;

namespace Inventario360.Services
{
    public interface IFichaEmpleadoService
    {
        Task<List<FichaEmpleado>> ObtenerFichas();
        Task<FichaEmpleado> ObtenerFichaPorEmpleado(int empleadoId);
        Task<FichaEmpleado> CrearFicha(FichaEmpleado ficha);
        Task<FichaEmpleado> ActualizarFicha(FichaEmpleado ficha);
        Task<bool> EliminarFicha(int id);
    }
}
