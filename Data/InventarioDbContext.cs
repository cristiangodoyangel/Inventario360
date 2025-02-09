using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Inventario360.Models;

namespace Inventario360.Data
{
    public class InventarioDbContext : IdentityDbContext<Usuario>
    {
        public InventarioDbContext(DbContextOptions<InventarioDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
