using System.Collections.Generic;
using System.Threading.Tasks;
using Inventario360.Models;

namespace Inventario360.Services
{
    public interface IProveedorService
    {
        Task<List<Proveedor>> ObtenerTodos(); // Obtener lista de proveedores
        Task<Proveedor?> ObtenerPorId(int id); // Obtener un proveedor por ID
        Task Agregar(Proveedor proveedor); // Agregar un nuevo proveedor
        Task Actualizar(Proveedor proveedor); // Actualizar un proveedor existente
        Task Eliminar(int id); // Eliminar un proveedor por ID
    }
}
