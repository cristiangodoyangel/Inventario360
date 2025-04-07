namespace Inventario360.Models
{
    public class AspNetUserTokens
    {
        public required string UserId { get; set; } 
        public required string LoginProvider { get; set; } 
        public required string Name { get; set; } 
        public string? Value { get; set; }
    }
}
