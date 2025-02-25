using Inventario360.Data;
using Inventario360.Models;
using Inventario360.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Configurar la conexi�n a SQL Server
builder.Services.AddDbContext<InventarioDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configurar Identity con reglas de autenticaci�n m�s seguras
builder.Services.AddIdentity<Usuario, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;

    // Configurar bloqueo de cuenta tras intentos fallidos
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
})
    .AddEntityFrameworkStores<InventarioDbContext>()
    .AddDefaultTokenProviders();

// Configurar cookie de autenticaci�n
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Cuenta/Login"; // Ruta de login
    options.LogoutPath = "/Cuenta/Logout"; // Ruta de logout
    options.AccessDeniedPath = "/Cuenta/AccessDenied"; // Ruta de acceso denegado
});

// Configurar servicios de dependencias
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<ISalidaBodegaService, SalidaBodegaService>();
builder.Services.AddScoped<ISolicitudService, SolicitudService>();
builder.Services.AddScoped<IEmpleadoService, EmpleadoService>();
builder.Services.AddScoped<IProyectoService, ProyectoService>();
builder.Services.AddScoped<ICuentaService, CuentaService>();
builder.Services.AddScoped<IReporteService, ReporteService>();



// Configurar autorizaci�n global para proteger todas las p�ginas
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new Microsoft.AspNetCore.Mvc.Authorization.AuthorizeFilter());
});

var app = builder.Build();

// Inicializar el usuario de prueba al iniciar la aplicaci�n
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<Usuario>>();

    if (userManager != null)
    {
        await UsuarioInitializer.Initialize(services, userManager);
    }
}

// Configuraci�n del entorno
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Middleware para autenticaci�n y autorizaci�n
app.UseAuthentication();
app.UseAuthorization();

// Configuraci�n de rutas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

app.UseStaticFiles();

