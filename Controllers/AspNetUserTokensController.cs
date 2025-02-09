using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Collections.Generic;
using Inventario360.Models;

namespace Inventario360.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AspNetUserTokensController : ControllerBase
    {
        private readonly UserManager<Usuario> _userManager;

        public AspNetUserTokensController(UserManager<Usuario> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> ObtenerTokens(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound("Usuario no encontrado");

            var tokens = await _userManager.GetAuthenticationTokenAsync(user, "DefaultProvider", "AuthToken");
            return Ok(new { user.Id, Token = tokens });
        }

        [HttpPost("{userId}/generar")]
        public async Task<IActionResult> GenerarToken(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound("Usuario no encontrado");

            var token = await _userManager.GenerateUserTokenAsync(user, "DefaultProvider", "AuthToken");
            await _userManager.SetAuthenticationTokenAsync(user, "DefaultProvider", "AuthToken", token);

            return Ok(new { user.Id, Token = token });
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> EliminarToken(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound("Usuario no encontrado");

            await _userManager.RemoveAuthenticationTokenAsync(user, "DefaultProvider", "AuthToken");
            return Ok("Token eliminado correctamente");
        }
    }
}
