using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Inventario360.Models;

namespace Inventario360.Data
{
    public class InventarioDbContext : IdentityDbContext<Usuario> // Se usa Usuario como clase de identidad
    {
        public InventarioDbContext(DbContextOptions<InventarioDbContext> options)
            : base(options) { }

        // Definir las tablas (DbSet) para que Entity Framework las reconozca

        public DbSet<SalidaDeBodega> SalidaDeBodega { get; set; }
        public DbSet<DetalleSalidaDeBodega> DetalleSalidaDeBodega { get; set; }
       
        public DbSet<Producto> Producto { get; set; }
        public DbSet<SolicitudDeMaterial> SolicitudDeMaterial { get; set; }
        public DbSet<Empleado> Empleado { get; set; }
        public DbSet<Proveedor> Proveedor { get; set; }
        public DbSet<Proyecto> Proyecto { get; set; }
       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SalidaDeBodega>()
                .HasOne(s => s.SolicitanteObj)
                .WithMany()
                .HasForeignKey(s => s.Solicitante);

            modelBuilder.Entity<SalidaDeBodega>()
                .HasOne(s => s.ResponsableEntregaObj)
                .WithMany()
                .HasForeignKey(s => s.ResponsableEntrega);

            modelBuilder.Entity<SalidaDeBodega>()
                .HasOne(s => s.ProyectoObj)
                .WithMany()
                .HasForeignKey(s => s.ProyectoAsignado);

         
        }


    }
}
