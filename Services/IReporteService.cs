using System.Collections.Generic;
using System.Threading.Tasks;
using Inventario360.Models;

namespace Inventario360.Services
{
    public interface IReporteService
    {
        Task<IEnumerable<Reporte>> ObtenerDatosReportes();
        Task<Reporte> ObtenerDetalleReporte(int id);
    }
}
