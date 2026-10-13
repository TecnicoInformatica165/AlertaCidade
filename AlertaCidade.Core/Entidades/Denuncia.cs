using AlertaCidade.Core.Exceptions;

namespace AlertaCidade.Core.Entidades;

public class Denuncia
{
    private const int TamanhoMinimoCaracteresTitulo = 10;
    private const int TamanhoMinimoCaracteresDescricao = 20;

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

    public int Id { get; private set; }
    public string Titulo { get; private set; } = string.Empty;
    public string Descricao { get; private set; } = string.Empty;
    public Localizacao Localizacao { get; private set; } = null!;
    public CategoriaDenuncia Categoria { get; private set; } = null!;
    public DateTimeOffset DataCriacao { get; private set; } = DateTimeOffset.UtcNow;

    private void ValidarDescricao(string descricao)
    {
        if (string.IsNullOrWhiteSpace(descricao))
            throw new CoreException("A descrição da denúncia não pode ficar em branco ou conter apenas espaços!");

        descricao = descricao.Trim();

        if (descricao.Length < TamanhoMinimoCaracteresDescricao)
            throw new CoreException(
                $"A descrição da denúncia deve conter no mínimo {TamanhoMinimoCaracteresDescricao} caracteres!");

        Descricao = descricao;
    }

    private void ValidarTitulo(string titulo)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            throw new CoreException("O titulo da denúncia não pode ficar em branco ou conter apenas espaços!");

        titulo = titulo.Trim();

        if (titulo.Length < TamanhoMinimoCaracteresTitulo)
            throw new CoreException(
                $"O título da denúncia deve conter no mínimo {TamanhoMinimoCaracteresTitulo} caracteres!");

        Titulo = titulo;
    }
}