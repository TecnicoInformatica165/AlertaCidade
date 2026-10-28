using AlertaCidade.Api.Dados;
using AlertaCidade.Core.Entidades;
using AlertaCidade.Core.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace AlertaCidade.Api.Servico;

public class UsuarioServicos (DbContexto contexto)
{
    public async Task AdicionaAsync(Usuario modelo)
    {
       await contexto.Usuarios.AddAsync(modelo);
       await contexto.SaveChangesAsync();

    }
    
    //Busca o usuário pelo email
    public async Task<Usuario> BuscarPorEmailAsync(string email)
    {
        
        var usuarioEncontrado = await contexto.Usuarios.FirstOrDefaultAsync(usuario => usuario.Email == email);

        if (usuarioEncontrado is null) throw new UsuarioNaoEncontradoException("O usuario não existe!");
        
        return usuarioEncontrado;

        // Buscar o usuario pelo email e salvar em uma variavel
        // Se o usuaio não existir, joga um erro no sistema informando que o usuario é inexitente 
        // Caso o usuario exista, retornar o usuario   

    }
    
}