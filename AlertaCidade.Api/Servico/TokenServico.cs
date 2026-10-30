using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AlertaCidade.Core.Entidades;
using Microsoft.IdentityModel.Tokens;

namespace AlertaCidade.Api.Dados.Servicos;

public class TokenServico
{
    public static string Chave = Environment.GetEnvironmentVariable("CHAVE_JWT") ?? "";
    public static string Emissor = Environment.GetEnvironmentVariable("EMISSOR_JWT") ?? "";
    public static string Audiencia = Environment.GetEnvironmentVariable("AUDIENCIA_JWT") ?? "";

    public string GerarToken(Usuario usuario)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Email, usuario.Email),
            new Claim(ClaimTypes.Name, usuario.Nome)
        };

        var chaveSegura = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Chave));
        var credenciais = new SigningCredentials(chaveSegura, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: Emissor,
            audience: Audiencia,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: credenciais
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}