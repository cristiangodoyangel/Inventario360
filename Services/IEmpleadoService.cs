using System.Collections.Generic;
using System.Threading.Tasks;
using Inventario360.Models;

namespace Inventario360.Services
{
    public interface IEmpleadoService
    {
        Task<List<Empleado>> ObtenerTodos(); // Obtener lista de empleados
        Task<Empleado?> ObtenerPorId(int id); // Obtener un empleado por ID
        Task Agregar(Empleado empleado); // Agregar un nuevo empleado
        Task Actualizar(Empleado empleado); // Actualizar un empleado existente
        Task Eliminar(int id); // Eliminar un empleado por ID
    }
}
