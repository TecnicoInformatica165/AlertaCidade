using AlertaCidade.Core.Entidades;
using Microsoft.EntityFrameworkCore;

namespace AlertaCidade.Api.Dados.Servicos;

public class UsuarioServicos(DbContexto contexto)
{
    public async Task AdicionaAsync(Usuario modelo)
    {
        await contexto.Usuarios.AddAsync(modelo);
        await contexto.SaveChangesAsync();
    }

    //busca o usuário pelo email
    // Busca o usuario pelo email
    public async Task<Usuario> BuscarPorEmailAsync(string email)
    {
        //funciona como uma busca direta na tabela do banco de dados para encontrar um único registro que possua o e-mail informado.
        var usuario = await contexto.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
        if (usuario == null)
        {
            throw new Exception("Erro no sistema: Usuário inexistente.");
        }

        // // Caso o usuario exista, retornar o usuario
        return usuario;
    }
}