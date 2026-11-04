using AlertaCidade.Core.Entidades;

namespace AlertaCidade.Api.Dados.Servicos;

public class DenunciaServico
{
    private readonly DbContexto _contexto;

    public DenunciaServico(DbContexto contexto)
    {
        _contexto = contexto;
    }

    public async Task AdicionaAsync(Denuncia denuncia)
    {
        _contexto.Denuncias.Add(denuncia);
        await _contexto.SaveChangesAsync();
    }
    public async Task AdicionaCategoriaAsync(CategoriaDenuncia categoria)
    {
        _contexto.CategoriaDenuncias.Add(categoria);
        await _contexto.SaveChangesAsync();
    }
}