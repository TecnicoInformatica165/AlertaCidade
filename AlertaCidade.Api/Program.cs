using System.Security.Claims;
using System.Text;
using AlertaCidade.Api.Dados;
using AlertaCidade.Api.DTOs;
using AlertaCidade.Api.Servicos;
using AlertaCidade.Core.Entidades;
using AlertaCidade.Core.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var stringConexao = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddMySql<DbContexto>(stringConexao, ServerVersion.AutoDetect(stringConexao));

builder.Services.AddScoped<UsuarioServicos>();
builder.Services.AddScoped<TokenServico>();

// Configurações do JWT (lidas de CHAVE_JWT, EMISSOR_JWT e AUDIENCIA_JWT)
var chaveJwt = builder.Configuration["CHAVE_JWT"]
               ?? throw new InvalidOperationException("A configuração CHAVE_JWT não foi encontrada.");
var emissorJwt = builder.Configuration["EMISSOR_JWT"];
var audienciaJwt = builder.Configuration["AUDIENCIA_JWT"];

// Ensina a API a validar o token que chega no cabeçalho "Authorization: Bearer ..."
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opcoes =>
    {
        opcoes.MapInboundClaims = true;
        opcoes.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = emissorJwt,
            ValidAudience = audienciaJwt,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chaveJwt))
        };
    });
builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
else
{
    app.UseHttpsRedirection();
}

// A ordem importa: primeiro descobre quem é o usuário, depois verifica a permissão
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/status", () => Results.Ok(new { Estado = "Online" }));

// Cadastro de usuário
app.MapPost("/usuario", async (CadastroUsuarioDto requisicao, UsuarioServicos usuarioServicos) =>
{
    try
    {
        var novoUsuario = new Usuario(requisicao.Nome, requisicao.Email, requisicao.Senha);

        await usuarioServicos.AdicionaAsync(novoUsuario);

        return Results.Created("/usuario", new
        {
            Mensagem = "Usuário cadastrado com sucesso!"
        });
    }
    catch (CoreException ex)
    {
        return Results.BadRequest(new { message = ex.Message });
    }
    catch (DbUpdateException)
    {
        return Results.BadRequest(new { message = "Não foi possível cadastrar. Esse e-mail já pode estar em uso." });
    }
});

// Login: devolve o token JWT
app.MapPost("/login", async (
    LoginUsuarioDto requisicao,
    UsuarioServicos usuarioServicos,
    TokenServico tokenServico) =>
{
    var usuario = await usuarioServicos.AutenticarAsync(requisicao.Email, requisicao.Senha);

    if (usuario is null)
        return Results.Json(new { message = "E-mail ou senha inválidos!" }, statusCode: 401);

    var token = tokenServico.GerarToken(usuario);

    return Results.Ok(new { Token = token });
});

// Rota protegida: só abre com um token válido
app.MapGet("/perfil", (ClaimsPrincipal usuario) =>
{
    // Lê cada informação que foi colocada dentro do token no login
    var id = usuario.FindFirstValue(ClaimTypes.NameIdentifier);
    var email = usuario.FindFirstValue(ClaimTypes.Email);
    var nome = usuario.FindFirstValue(ClaimTypes.Name);

    return Results.Ok(new
    {
        Mensagem = $"Olá, {nome}! Você está autenticado.",
        Id = id,
        Nome = nome,
        Email = email
    });
}).RequireAuthorization();

app.Run();