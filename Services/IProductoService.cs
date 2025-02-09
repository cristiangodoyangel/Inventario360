using System.Collections.Generic;
using System.Threading.Tasks;
using Inventario360.Models;

namespace Inventario360.Services
{
    public interface IProductoService
    {
        Task<List<Producto>> ObtenerTodos(); // Obtener lista de productos
        Task<Producto?> ObtenerPorId(int id); // Obtener un producto por ID
        Task Agregar(Producto producto); // Agregar un nuevo producto
        Task Actualizar(Producto producto); // Actualizar un producto existente
        Task Eliminar(int id); // Eliminar un producto por ID
    }
}
