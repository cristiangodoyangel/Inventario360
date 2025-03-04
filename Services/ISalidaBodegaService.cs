using System.Collections.Generic;
using System.Threading.Tasks;
using Inventario360.Models;

namespace Inventario360.Services
{
    public interface ISalidaBodegaService
    {
        Task<bool> RegistrarSalidaConProductos(SalidaDeBodega salida, List<DetalleSalidaDeBodega> productos);
        Task<IEnumerable<SalidaDeBodega>> ObtenerTodas();
        Task<SalidaDeBodega> ObtenerPorId(int id);
        Task<List<DetalleSalidaDeBodega>> ObtenerDetallesPorSalida(int id); // ✅ Nuevo método
        Task Eliminar(int id);
        Task EliminarDetalles(int salidaId);
        Task RevertirStock(int salidaId); // ✅ Nuevo método para restaurar stock
    }
}
