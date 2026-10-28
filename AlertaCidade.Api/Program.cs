using AlertaCidade.Api.Dados;
using AlertaCidade.Api.DTOs;
using AlertaCidade.Api.Servicos;
using AlertaCidade.Core.Entidades;
using AlertaCidade.Core.Exceptions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var stringConexao = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddMySql<DbContexto>(stringConexao, ServerVersion.AutoDetect(stringConexao));

builder.Services.AddScoped<UsuarioServicos>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
else
{
    app.UseHttpsRedirection();
}

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

// Login
app.MapPost("/login", async (LoginUsuarioDto requisicao, UsuarioServicos usuarioServicos) =>
{
    var usuario = await usuarioServicos.AutenticarAsync(requisicao.Email, requisicao.Senha);

    // A mensagem é a mesma para e-mail errado e senha errada (por segurança)
    if (usuario is null)
        return Results.Json(new { message = "E-mail ou senha inválidos!" }, statusCode: 401);

    return Results.Ok(new
    {
        Mensagem = "Login realizado com sucesso!",
        usuario.Id,
        usuario.Nome,
        usuario.Email
    });
});

app.Run();