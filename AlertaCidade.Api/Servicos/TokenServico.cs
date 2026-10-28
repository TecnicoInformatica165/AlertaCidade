using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AlertaCidade.Core.Entidades;
using Microsoft.IdentityModel.Tokens;

namespace AlertaCidade.Api.Servicos;

public class TokenServico(IConfiguration configuracao)
{
    public string GerarToken(Usuario usuario)
    {
        var chave = configuracao["CHAVE_JWT"]
                    ?? throw new InvalidOperationException("A configuração CHAVE_JWT não foi encontrada.");
        var emissor = configuracao["EMISSOR_JWT"];
        var audiencia = configuracao["AUDIENCIA_JWT"];

        // Claims: as informações do usuário que vão dentro do token
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Email, usuario.Email),
            new Claim(ClaimTypes.Name, usuario.Nome)
        };

        // Credenciais de assinatura: a chave secreta + o algoritmo HmacSha256
        var chaveSegura = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chave));
        var credenciais = new SigningCredentials(chaveSegura, SecurityAlgorithms.HmacSha256);

        // Monta o token com emissor, audiência, claims, expiração (2 horas) e assinatura
        var token = new JwtSecurityToken(
            issuer: emissor,
            audience: audiencia,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: credenciais
        );

        // Transforma o objeto na string "xxxxx.yyyyy.zzzzz"
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}