namespace Inventario360.ViewModels
{
    public class RolesViewModel
    {
        public string UserId { get; set; }
        public string NombreUsuario { get; set; }  // ← esta propiedad estaba faltando
        public string Email { get; set; }
        public List<string> RolesAsignados { get; set; } = new();
        public List<string> TodosLosRoles { get; set; } = new();
    }
}
