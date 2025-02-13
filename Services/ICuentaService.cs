using Microsoft.AspNetCore.Identity;
using Inventario360.Models;
using System.Threading.Tasks;

namespace Inventario360.Services
{
    public interface ICuentaService
    {
        Task<SignInResult> LoginAsync(Cuenta model);
    }
}
