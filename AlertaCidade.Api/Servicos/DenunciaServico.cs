using AlertaCidade.Api.Dados;
using AlertaCidade.Core.Entidades;

namespace AlertaCidade.Api.Servicos;

public class DenunciaServico(DbContexto contexto)
{
    public async Task AdicionaAsync(Denuncia denuncia)
    {
        await contexto.Denuncias.AddAsync(denuncia);
        await contexto.SaveChangesAsync();
    }
}