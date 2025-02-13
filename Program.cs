using Inventario360.Data;
using Inventario360.Models;
using Inventario360.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configurar la conexión a SQL Server
builder.Services.AddDbContext<InventarioDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configurar Identity (esto debe ir antes de registrar los servicios que lo usan)
builder.Services.AddIdentity<Usuario, IdentityRole>()
    .AddEntityFrameworkStores<InventarioDbContext>()
    .AddDefaultTokenProviders();

// Configurar servicios de dependencias (después de Identity)
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<ISalidaBodegaService, SalidaBodegaService>();
builder.Services.AddScoped<ISolicitudService, SolicitudService>();
builder.Services.AddScoped<IEmpleadoService, EmpleadoService>();
builder.Services.AddScoped<IProyectoService, ProyectoService>();
builder.Services.AddScoped<ICuentaService, CuentaService>();
builder.Services.AddControllersWithViews();// Agregado después de Identity

// Configurar reglas de contraseña para Identity
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
});

// Configurar cookie de autenticación
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Cuenta/Login"; // Ruta de login
    options.LogoutPath = "/Cuenta/Logout"; // Ruta de logout
    options.AccessDeniedPath = "/Cuenta/AccessDenied"; // Ruta de acceso denegado
});

// Agregar servicios MVC

var app = builder.Build();

// Inicializar el usuario de prueba al iniciar la aplicación
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<Usuario>>();

    // Llamar al inicializador de usuarios
    await UsuarioInitializer.Initialize(services, userManager);
}

// Configuración del entorno
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseRouting();

// Middleware para autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

// Configuración de rutas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
