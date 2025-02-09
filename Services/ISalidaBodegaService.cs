using System.Collections.Generic;
using System.Threading.Tasks;
using Inventario360.Models;

namespace Inventario360.Services
{
    public interface ISalidaBodegaService
    {
        Task<List<SalidaDeBodega>> ObtenerTodas(); // Obtener todas las salidas de bodega
        Task<SalidaDeBodega?> ObtenerPorId(int id); // Obtener una salida por ID
        Task Agregar(SalidaDeBodega salida); // Agregar una nueva salida
        Task Actualizar(SalidaDeBodega salida); // Actualizar una salida existente
        Task Eliminar(int id); // Eliminar una salida por ID
    }
}
