using AlertaCidade.Core.Exceptions;

namespace AlertaCidade.Core.Entidades;

public class CategoriaDenuncia
{
    protected CategoriaDenuncia()
    {
    }

    public CategoriaDenuncia(string nome)
    {
        ValidarNome(nome);
    }

    public const int TamanhoMinimoCaracteresNome = 5;
    public int Id { get; private set; }
    public string Nome { get; private set; } = string.Empty;

    private void ValidarNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new CoreException("O nome da categoria não pode ficar em branco ou conter apenas espaços!");

        nome = nome.Trim();

        if (nome.Length < TamanhoMinimoCaracteresNome)
            throw new CoreException(
                $"O nome da categoria deve conter no mínimo {TamanhoMinimoCaracteresNome} caracteres!");

        Nome = nome;
    }
}