using System.Runtime.CompilerServices;
using AlertaCidade.Api;
using AlertaCidade.Api.Dados;
using AlertaCidade.Api.Dados.Servicos;
using AlertaCidade.Api.DTOs;
using AlertaCidade.Core.Entidades;
using AlertaCidade.Core.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


var stringConexao = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddMySql<DbContexto>(stringConexao ,ServerVersion.AutoDetect(stringConexao));

builder.Services.AddScoped<UsuarioServicos>();
builder.Services.AddScoped<TokenServico>();
var chave = Environment.GetEnvironmentVariable("CHAVE_JWT");
var emissor = Environment.GetEnvironmentVariable("EMISSOR_JWT");
var audiencia = Environment.GetEnvironmentVariable("AUDIENCIA_JWT");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opcoes =>
    {
        opcoes.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = emissor,
            ValidAudience = audiencia,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chave))
        };
    });
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/status", () =>
{
    return Results.Ok(new
    {
        Estado = "Online",
        
    });

});

app.MapPost("/usuario", async (CadastroUsuarioDto requisicao, UsuarioServicos usuarioServicos) =>
{
    try
    {
        var novoUsuario = new Usuario(requisicao.Nome, requisicao.Email, requisicao.Senha);
        await usuarioServicos.AdicionaAsync(novoUsuario);
        return Results.Created();
        
       
    }
    catch (CoreException ex)
    {
        return Results.BadRequest(new { message = ex.Message });
        
    }
        
});

//rota de login
app.MapPost("/login", async (LoginUsuarioDto body, UsuarioServicos usuarioServicos, TokenServico tokenServico) =>
{
    try
    {
        var usuarioEncontrado = await usuarioServicos
            .BuscarPorEmailAsync(body.Email);

        if (usuarioEncontrado.Senha != body.Senha)
            return Results.BadRequest("Usuario ou senha estão errados!");

        var token = tokenServico.GerarToken(usuarioEncontrado);
        return Results.Ok(new { Token = token });
    }
    catch (UsuarioNaoEncontradoException ex)
    {
        return Results.NotFound(new { Mensagem = ex.Message });
    }
    catch
    {
        return Results.Problem("Ocorreu um erro desconhecido, tente novamente mais tarde.",
            "Erro no servidor!", 500);
    }
});
app.MapGet("/perfil", (ClaimsPrincipal usuario) =>
{
    var id = usuario.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    var email = usuario.FindFirst(ClaimTypes.Email)?.Value;
    var nome = usuario.FindFirst(ClaimTypes.Name)?.Value;

    return Results.Ok(new { id = id, email, Nome = nome });
}).RequireAuthorization();
app.Run();