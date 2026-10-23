using AlertaCidade.Api;
using AlertaCidade.Api.Servicos;
using AlertaCidade.Core.Entidades;
using AlertaCidade.Core.Exceptions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var stringConexao = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddMySql<DbContexto>(stringConexao, ServerVersion.AutoDetect(stringConexao));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) app.MapOpenApi();

app.UseHttpsRedirection();

app.MapGet("/status", () =>
{
    return Results.Ok(new
    {
        status = "Online",
        Codigo = "5186"
    });
});
app.MapPost("/usuario", (CadastroUsuarioDto requisicao) =>
{
    try
    {
        var novoUsuario = new Usuario(requisicao.Nome, requisicao.Email, requisicao.Senha);
        UsuarioServicos.AdicionaAsync();

        return Results.Created();
    }
    catch (CoreException ex)
    {
        return Results.BadRequest(new { mensagem = ex.Message });
    }
});


app.Run();