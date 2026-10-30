using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using AlertaCidade.Api;
using AlertaCidade.Api.Dados;
using AlertaCidade.Api.Dados.Servicos;
using AlertaCidade.Api.DTOs;
using AlertaCidade.Api.Servico;
using AlertaCidade.Core.Entidades;
using AlertaCidade.Core.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


var stringConexao = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddMySql<DbContexto>(stringConexao ,ServerVersion.AutoDetect(stringConexao));

builder.Services.AddScoped<UsuarioServicos>();
builder.Services.AddScoped<TokenServico>();
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = TokenServico.Emissor,
            ValidAudience = TokenServico.Audiencia,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TokenServico.Chave))
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

// rota protegida
app.MapGet("/perfil", (ClaimsPrincipal usuario) =>
    {
        var id    = usuario.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var email = usuario.FindFirst(ClaimTypes.Email)?.Value;
        var nome  = usuario.FindFirst(ClaimTypes.Name)?.Value;

        return Results.Ok(new { Id = id,Email = email,Nome = nome });
    })
    .RequireAuthorization();

app.Run();

