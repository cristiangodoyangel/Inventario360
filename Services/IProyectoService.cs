using System.Collections.Generic;
using System.Threading.Tasks;
using Inventario360.Models;

namespace Inventario360.Services
{
    public interface IProyectoService
    {
        Task<List<Proyecto>> ObtenerTodos(); // Obtener lista de proyectos
        Task<Proyecto?> ObtenerPorId(int id); // Obtener un proyecto por ID
        Task Agregar(Proyecto proyecto); // Agregar un nuevo proyecto
        Task Actualizar(Proyecto proyecto); // Actualizar un proyecto existente
        Task Eliminar(int id); // Eliminar un proyecto por ID
    }
}
