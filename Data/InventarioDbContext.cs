﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Inventario360.Models;

namespace Inventario360.Data
{
    public class InventarioDbContext : IdentityDbContext<Usuario> // Se usa Usuario como clase de identidad
    {
        public InventarioDbContext(DbContextOptions<InventarioDbContext> options)
            : base(options) { }

        // Definir las tablas (DbSet) para que Entity Framework las reconozca
        public DbSet<Producto> Producto { get; set; }
        public DbSet<SolicitudDeMaterial> SolicitudDeMaterial { get; set; }
        public DbSet<Empleado> Empleado { get; set; }
        public DbSet<Proveedor> Proveedor { get; set; }
        public DbSet<Proyecto> Proyecto { get; set; }
        public DbSet<SalidaDeBodega> SalidaDeBodega { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); // Llamar al método base para configurar Identity

            // Definir clave primaria de Producto
            builder.Entity<Producto>()
                .HasKey(p => p.ITEM);

            // Puedes agregar más configuraciones de entidades aquí si es necesario
        }

    }
}
