using AlertaCidade.Core.Entidades;
using AlertaCidade.Core.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace AlertaCidade.Api.Servicos;

public class UsuarioServicos(DbContexto contexto)
{
    //Adicionar usuario no banco de dados
    public async Task AdicionaAsync(Usuario modelo)
    {
        await contexto.Usuarios.AddAsync(modelo);
        await contexto.SaveChangesAsync();
    }
    
    //Buscar o Usuario pelo email
    public async Task<Usuario> BuscarPorEmailAsync(string email)
    {
        var usuarioEncontrado = await contexto.Usuarios.FirstOrDefaultAsync(usuario => usuario.Email == email);
        
        if (usuarioEncontrado is null)
        {
            throw new UsuarioNaoEncontradoException("O usuario não existe!");
        }
        
        return usuarioEncontrado;

    }
    //buscar o usuario pelo email e salvar em uma variavel
    //Se o usuario não existir, joga um erro no sistema informando que o usuario e inexistente
    //Caso o usuario exista, retornar o usuario
}