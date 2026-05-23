using Fiap.Knowball.Application.DTOs.Auth;
using Fiap.Knowball.Application.Exceptions;
using Fiap.Knowball.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fiap.Knowball.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(typeof(TokenDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Login([FromBody] LoginDto dto)
        {
            try
            {
                var token = _authService.Autenticar(dto);
                _logger.LogInformation("Login realizado com sucesso para o usuário {Usuario}", dto.Usuario);
                return Ok(token);
            }
            catch (BusinessException ex)
            {
                _logger.LogWarning("Tentativa de login inválida para o usuário {Usuario}", dto.Usuario);
                return Unauthorized(new { message = ex.Message });
            }
        }
    }
}
