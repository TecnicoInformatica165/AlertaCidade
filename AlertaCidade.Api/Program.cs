using System.Runtime.CompilerServices;
using AlertaCidade.Api;
using AlertaCidade.Api.Dados;
using AlertaCidade.Api.Dados.Servicos;
using AlertaCidade.Api.DTOs;
using AlertaCidade.Core.Entidades;
using AlertaCidade.Core.Exceptions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


var stringConexao = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddMySql<DbContexto>(stringConexao, ServerVersion.AutoDetect(stringConexao));

builder.Services.AddScoped<UsuarioServicos>();
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

app.MapPost("/login", async (LoginUsuarioDto body, UsuarioServicos usuarioServicos) =>
{
    try
    {
        var usuarioEncontrado = await usuarioServicos
            .BuscarPorEmailAsync(body.Email);

        if (usuarioEncontrado.Senha != body.Senha)
            return Results.BadRequest("Usuario ou senha estão errados!");

        return Results.Ok();

    }
    catch (UsuarioNaoEncontradoException ex)
    {
        return Results.NotFound(new { message = ex.Message });
    }
    catch
    {
        return Results.Problem("Ocorreu um erro desconhecido, tente novamente mais tarde.", "Erro no servidor!", 500);
    }

});


app.Run();