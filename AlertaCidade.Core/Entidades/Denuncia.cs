namespace AlertaCidade.Core.Entidades;

public class Denuncia
{
    public int Id { get; private set; }

    // Conteudo
    public string Titulo { get; private set; } = string.Empty;
    public string Descricao { get; private set; } = string.Empty;
    public Localizacao Localizacao { get; private set; } = null!;
    public CategoriaDenuncia Categoria { get; private set; } = null!;

    // Rastreabilidade
    public DateTimeOffset DataCriacao { get; private set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? DataAtualizacao { get; private set; }

    protected Denuncia()
    {
    }

    public Denuncia(string titulo, string descricao, Localizacao localizacao, CategoriaDenuncia categoriaDenuncia)
    {
        ValidarTitulo(titulo);
        ValidarDescricao(descricao);
        Localizacao = localizacao;
        Categoria = categoriaDenuncia;
    }

    private void ValidarDescricao(string descricao)
    {
        Descricao = descricao;
    }

    private void ValidarTitulo(string titulo)
    {
        Titulo = titulo;
    }
}