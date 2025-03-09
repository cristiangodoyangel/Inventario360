using System.Collections.Generic;
using System.Threading.Tasks;
using Inventario360.Models;

namespace Inventario360.Services
{
    public interface ICamionetaService
    {
        Task<IEnumerable<Camioneta>> ObtenerCamionetas();
        Task<Camioneta> ObtenerCamionetaPorId(int id);
        Task<bool> CrearCamioneta(Camioneta camioneta);
        Task<bool> EditarCamioneta(Camioneta camioneta);
        Task<bool> EliminarCamioneta(int id);
    }
}
