using AlertaCidade.Api.Dados;
using AlertaCidade.Core.Entidades;

namespace AlertaCidade.Api.Servicos;

public class DenunciaServico(DbContexto contexto)
{
    public async Task AdicionaAsync(Denuncia denuncia)
    {
        contexto.Denuncias.Add(denuncia);
        await contexto.SaveChangesAsync();
    }
}