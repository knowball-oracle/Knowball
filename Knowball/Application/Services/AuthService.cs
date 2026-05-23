using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Fiap.Knowball.Application.DTOs.Auth;
using Fiap.Knowball.Application.Exceptions;
using Fiap.Knowball.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Fiap.Knowball.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly JwtSettings _jwtSettings;

        private static readonly Dictionary<string, string> _usuarios = new()
    {
        { "admin", "admin123" },
        { "arbitro", "arb123" },
        { "gestor", "ges123" }
    };

        public AuthService(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public TokenDto Autenticar(LoginDto loginDto)
        {
            if (!_usuarios.TryGetValue(loginDto.Usuario, out var senhaCorreta)
                || senhaCorreta != loginDto.Senha)
            {
                throw new BusinessException("Usuário ou senha inválidos.");
            }

            var expiracao = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes);
            var token = GerarToken(loginDto.Usuario, expiracao);

            return new TokenDto
            {
                Token = token,
                Expiracao = expiracao
            };
        }

        private string GerarToken(string usuario, DateTime expiracao)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.Name, usuario),
            new Claim(ClaimTypes.Role, usuario == "admin" ? "Admin" : "Usuario"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat,
                DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                ClaimValueTypes.Integer64)
        };

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: expiracao,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
