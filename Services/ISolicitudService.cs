using System.Collections.Generic;
using System.Threading.Tasks;
using Inventario360.Models;

namespace Inventario360.Services
{
    public interface ISolicitudService
    {
        Task<List<SolicitudDeMaterial>> ObtenerTodas(); // Obtener todas las solicitudes
        Task<SolicitudDeMaterial?> ObtenerPorId(int id); // Obtener una solicitud por ID
        Task Agregar(SolicitudDeMaterial solicitud); // Agregar una nueva solicitud
        Task Actualizar(SolicitudDeMaterial solicitud); // Actualizar una solicitud existente
        Task Eliminar(int id); // Eliminar una solicitud por ID
    }
}
