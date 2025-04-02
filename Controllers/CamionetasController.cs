using Inventario360.Data;
using Inventario360.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Inventario360.ViewModels;

public class CamionetasController : Controller
{
    private readonly ICamionetaService _camionetaService;
    private readonly InventarioDbContext _context;

    public CamionetasController(ICamionetaService camionetaService, InventarioDbContext context)
    {
        _camionetaService = camionetaService;
        _context = context;
    }

    // 📌 Listar todas las camionetas
    public async Task<IActionResult> Index()
    {
        var camionetas = await _camionetaService.ObtenerCamionetas();
        return View(camionetas);
    }

    // 📌 Crear nueva camioneta (Vista)
    public IActionResult Crear()
    {
        var viewModel = new CamionetaConFichaCamioneta
        {
            Empleados = new SelectList(_context.Empleado, "ID", "Nombre")
        };
        return View(viewModel);
    }

    // 📌 Guardar nueva camioneta
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(CamionetaConFichaCamioneta viewModel)
    {
        if (!ModelState.IsValid)
        {
            viewModel.Empleados = new SelectList(_context.Empleado, "ID", "Nombre", viewModel.Camioneta.ResponsableID);
            return View(viewModel);
        }

        await _camionetaService.CrearCamioneta(viewModel.Camioneta);
        return RedirectToAction(nameof(Index));
    }

    // 📌 Editar (Vista)
    public async Task<IActionResult> Editar(int id)
    {
        var camioneta = await _camionetaService.ObtenerCamionetaPorId(id);
        if (camioneta == null) return NotFound();

        var viewModel = new CamionetaConFichaCamioneta
        {
            Camioneta = camioneta,
            Empleados = new SelectList(_context.Empleado, "ID", "Nombre", camioneta.ResponsableID)
        };

        return View(viewModel);
    }

    // 📌 Guardar cambios en la BD
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, CamionetaConFichaCamioneta viewModel)
    {
        if (id != viewModel.Camioneta.ID) return NotFound();

        // ⚠️ Eliminar validaciones para FichaCamioneta (ya que no se edita en esta vista)
        ModelState.Remove("FichaCamioneta.Patente");
        ModelState.Remove("FichaCamioneta.Marca");
        ModelState.Remove("FichaCamioneta.Modelo");
        ModelState.Remove("FichaCamioneta.Año");
        ModelState.Remove("FichaCamioneta.Color");
        ModelState.Remove("FichaCamioneta.Kilometraje");
        ModelState.Remove("FichaCamioneta.Estado");
        ModelState.Remove("FichaCamioneta.ResponsableID");

        if (!ModelState.IsValid)
        {
            viewModel.Empleados = new SelectList(_context.Empleado, "ID", "Nombre", viewModel.Camioneta.ResponsableID);
            return View(viewModel);
        }

        await _camionetaService.EditarCamioneta(viewModel.Camioneta);
        return RedirectToAction(nameof(Index));
    }

    // 📌 Eliminar camioneta
    public async Task<IActionResult> Eliminar(int id)
    {
        var eliminado = await _camionetaService.EliminarCamioneta(id);
        if (!eliminado) return NotFound();
        return RedirectToAction(nameof(Index));
    }
}
