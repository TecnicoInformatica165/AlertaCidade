using System.Text.RegularExpressions;
using AlertaCidade.Core.Exceptions;

namespace AlertaCidade.Core.Entidades;

public class Usuario
{
    public const int TamanhoMinimoCaracteresNome = 3;
    public const int TamanhoMinimoCaracteresSenha = 8;

    // Construtor protegido para o EFCore
    protected Usuario()
    {
    }

    public Usuario(string nome, string email, string senha)
    {
        ValidarNome(nome);
        ValidarEmail(email);
        ValidarSenha(senha);
    }

    public int Id { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Senha { get; private set; } = string.Empty;

    // Usado pelo serviço para trocar a senha pelo hash antes de salvar
    public void DefinirSenhaHash(string hash)
    {
        if (string.IsNullOrWhiteSpace(hash))
            throw new CoreException("O hash da senha não pode ficar em branco!");

        Senha = hash;
    }

    private void ValidarSenha(string senha)
    {
        if (string.IsNullOrWhiteSpace(senha))
            throw new CoreException("A senha do usuário não pode ficar em branco ou conter apenas espaços!");

        senha = senha.Trim();

        if (senha.Length < TamanhoMinimoCaracteresSenha)
            throw new CoreException(
                $"A senha do usuário deve conter no mínimo {TamanhoMinimoCaracteresSenha} caracteres!");

        Senha = senha;
    }

    private void ValidarEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new CoreException("O e-mail do usuário não pode ficar em branco ou conter apenas espaços!");

        email = email.Trim();

        // ^ e $ garantem que o e-mail inteiro seja válido, e não só um pedaço dele
        if (!Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", RegexOptions.IgnoreCase))
            throw new CoreException("O e-mail do usuário é inválido!");

        Email = email;
    }

    private void ValidarNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new CoreException("O nome do usuário não pode ficar em branco ou conter apenas espaços!");

        nome = nome.Trim();

        if (nome.Length < TamanhoMinimoCaracteresNome)
            throw new CoreException(
                $"O nome do usuário deve possuir no mínimo {TamanhoMinimoCaracteresNome} caracteres!");

        if (!Regex.IsMatch(nome, @"^[A-Za-zÀ-ÖØ-öø-ÿ\s]+$", RegexOptions.IgnoreCase))
            throw new CoreException("O nome do usuário contém caracteres inválidos!");

        Nome = nome;
    }
}