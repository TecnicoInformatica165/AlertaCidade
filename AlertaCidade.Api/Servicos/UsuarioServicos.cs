using AlertaCidade.Core.Entidades;

namespace AlertaCidade.Api.Servicos;

public class UsuarioServicos(DbContexto contexto)
{
    public async Task AdicionaAsync(Usuario modelo)
    {
        await contexto.Usuarios.AddAsync(modelo);
        await contexto.SaveChangesAsync();
    }
}