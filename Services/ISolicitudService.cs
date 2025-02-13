using Inventario360.Models;

public interface ISolicitudService
{
    Task<IEnumerable<SolicitudDeMaterial>> GetAllSolicitudesAsync();
    Task<IEnumerable<SolicitudDeMaterial>> ObtenerTodas(); // ✅ Método agregado
    Task<SolicitudDeMaterial> GetSolicitudByIdAsync(int id);
    Task AddSolicitudAsync(SolicitudDeMaterial solicitud);
    Task UpdateSolicitudAsync(SolicitudDeMaterial solicitud);
    Task DeleteSolicitudAsync(int id);
}
