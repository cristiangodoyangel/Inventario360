namespace Inventario360.Models
{
    public class AspNetUserTokens
    {
        public required string UserId { get; set; } // Identificador del usuario
        public required string LoginProvider { get; set; } // Proveedor de autenticación (Ej: Google, Facebook)
        public required string Name { get; set; } // Nombre del token
        public string? Value { get; set; } // Valor del token (puede ser NULL)
    }
}
