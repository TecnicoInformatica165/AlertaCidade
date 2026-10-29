using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AlertaCidade.Core.Entidades;
using Microsoft.IdentityModel.Tokens;

namespace AlertaCidade.Api.Dados.Servicos;

public class TokenServico
{
    public string GerarToken(Usuario usuario)
    {
        var chave = Environment.GetEnvironmentVariable("CHAVE_JWT");
        var emissor = Environment.GetEnvironmentVariable("EMISSOR_JWT");
        var audiencia = Environment.GetEnvironmentVariable("AUDIENCIA_JWT");

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Email, usuario.Email),
            new Claim(ClaimTypes.Name, usuario.Nome)
        };

        var chaveSegura = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chave));
        var credenciais = new SigningCredentials(chaveSegura, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: emissor,
            audience: audiencia,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: credenciais
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}