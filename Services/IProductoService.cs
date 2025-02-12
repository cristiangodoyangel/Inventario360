using System.Collections.Generic;
using System.Threading.Tasks;
using Inventario360.Models;

namespace Inventario360.Services
{
    public interface IProductoService
    {
        Task<List<Producto>> ObtenerTodos();
        Task<Producto?> ObtenerPorId(int id);
        Task<Producto?> GetProductoByIdAsync(int id); // ✅ Método agregado
        Task Agregar(Producto producto);
        Task Actualizar(Producto producto);
        Task Eliminar(int id);
    }

}
