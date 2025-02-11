using System.Collections.Generic;
using System.Threading.Tasks;
using Inventario360.Models;

public interface ISalidaBodegaService
{
    Task<List<SalidaDeBodega>> ObtenerTodas();
    Task<SalidaDeBodega?> ObtenerPorId(int id);
    Task Agregar(SalidaDeBodega salida);
    Task Actualizar(SalidaDeBodega salida);
    Task Eliminar(int id);
}
