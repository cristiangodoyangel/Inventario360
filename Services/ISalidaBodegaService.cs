using System.Collections.Generic;
using System.Threading.Tasks;
using Inventario360.Models;

namespace Inventario360.Services
{
    public interface ISalidaBodegaService
    {
        Task<List<SalidaDeBodega>> ObtenerTodas();
        Task<SalidaDeBodega?> ObtenerPorId(int id);
        Task<bool> RegistrarSalida(SalidaDeBodega salida, Producto producto); // ✅ Se actualiza aquí
        Task Actualizar(SalidaDeBodega salida);
        Task Eliminar(int id);
    }
}
