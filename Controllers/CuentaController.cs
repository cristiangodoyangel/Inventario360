using Microsoft.AspNetCore.Mvc;

public class CuentaController : Controller
{
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(string email, string password)
    {
        // Simulación de usuario válido (reemplazar con la autenticación real)
        if (email == "admin@example.com" && password == "123456")
        {
            return RedirectToAction("Index", "Home");
        }

        ViewBag.Error = "Correo o contraseña incorrectos.";
        return View();
    }

}
