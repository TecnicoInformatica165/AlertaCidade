using AlertaCidade.Api.Dados;
using AlertaCidade.Api.DTOs;
using AlertaCidade.Core.Entidades;
using AlertaCidade.Core.Exceptions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var stringConexao = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddMySql<ApiContext>(stringConexao, ServerVersion.AutoDetect(stringConexao));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/status", () => Results.Ok(new { Message = "Online" }));

app.MapPost("/usuario", (CadastroUsuarioDto requisicao) =>
{
    try
    {
        var novoUsuario = new Usuario(requisicao.Nome, requisicao.Email, requisicao.Senha);
        return Results.Created(string.Empty, new { Usuario = requisicao });
    }
    catch(CoreException ex)
    {
        return Results.BadRequest(new { Mensagem = ex.Message }); 
    }
});




app.Run();