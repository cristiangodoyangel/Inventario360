using System.Threading.Tasks;
using Inventario360.Models;

namespace Inventario360.Services
{
    public interface ISalidaBodegaService
    {
        Task<bool> RegistrarSalidaConProductos(SalidaDeBodega salida, List<DetalleSalidaDeBodega> productos);
        Task<IEnumerable<SalidaDeBodega>> ObtenerTodas();
        Task<SalidaDeBodega> ObtenerPorId(int id);
        Task Eliminar(int id);
        Task EliminarDetalles(int salidaId); // ✅ Agregar este método
    }
}
