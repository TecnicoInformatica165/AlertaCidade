using AlertaCidade.Api.Dados;
using AlertaCidade.Api.DTOs;
using AlertaCidade.Api.Servico;
using AlertaCidade.Core.Entidades;
using AlertaCidade.Core.Exceptions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var stringConexao = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddMySql<DbContexto>(stringConexao, ServerVersion.AutoDetect(stringConexao));

// Cadastro de Servicos Customizados
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
        status = "Online",
       
    });
});

//Criação de usuário
app.MapPost("/usuario", async (CadastroUsuarioDto body, UsuarioServicos usuarioServicos) =>
{
    try
    {


        var novoUsuario = new Usuario(body.Nome, body.Email, body.Senha);
        await usuarioServicos.AdicionaAsync(novoUsuario);
        
        return Results.Created();

    }
    
    catch (CoreException ex)
    {
        return Results.BadRequest(new { message = ex.Message });
    }
    
});

// Rota de login
app.MapPost("/login", async (LoginUsuarioDto body, UsuarioServicos usuarioServicos) =>
{
    try
    {
        var usuarioEncontrado = await usuarioServicos.BuscarPorEmailAsync(body.Email);

        if (usuarioEncontrado.Senha != body.Senha)
            return Results.BadRequest(new 
                {mensagem = "Usuario ou senha estão errados!" });

        return Results.Ok();
    }

    catch (UsuarioNaoEncontradoException ex)
    {
        return Results.NotFound(new { message = ex.Message });
    }
    catch
    {
        return Results.Problem("Ocoreu um erro desconhecido, tente novamente mais tarde.", 
            "Erro no servidor!", 500);
    }
    

});
app.Run();

