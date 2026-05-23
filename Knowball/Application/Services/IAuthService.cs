using Fiap.Knowball.Application.DTOs.Auth;

namespace Fiap.Knowball.Application.Services
{
    public interface IAuthService
    {
        TokenDto Autenticar(LoginDto loginDto);
    }
}
