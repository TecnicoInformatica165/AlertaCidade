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

    public async Task<Usuario> BuscarPorEmailAsync(string email)
    {
        var usuarioEncontrado = await contexto.Usuarios
            .FirstOrDefaultAsync(usuario => usuario.Email == email);
        
        if (usuarioEncontrado is null)
            throw new UsuarioNaoEncontradoException("O usuario não existe!");

        return usuarioEncontrado;
    }
}
