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

    // Construtor público para uso de desenvolvedores em outros lugares
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

    private void ValidarSenha(string senha)
    {
        // Verifica se a senha é nula, vazio ou contém apenas espaços em branco
        if (string.IsNullOrWhiteSpace(senha))
            throw new CoreException("A senha do usuário não pode ficar em branco ou conter apenas espaços!");

        // Corta os espaços vazios no começo e no final da string
        senha = senha.Trim();

        // Valida se a senha possui um tamanho mínimo de caracteres
        if (senha.Length < TamanhoMinimoCaracteresSenha)
            throw new CoreException(
                $"A senha do usuário deve conter no mínimo {TamanhoMinimoCaracteresSenha} caracteres!");

        // Atribui à propriedade 'Senha' o parâmetro 'senha' validado
        Senha = senha;
    }

    private void ValidarEmail(string email)
    {
        // Verifica se o e-mail é nulo, vazio ou contém apenas espaços em branco
        if (string.IsNullOrWhiteSpace(email))
            throw new CoreException("O e-mail do usuário não pode ficar em branco ou conter apenas espaços!");

        // Corta os espaços vazios no começo e no final da string
        email = email.Trim();

        // Encontra e-mails válidos e inverte o sinal com o ! antes da palavra REGEX para inverter a seleção de e-mails válidos para inválidos. 'RegexOptions.IgnoreCase' faz o C# ignorar minúsculas e maiúsculas como diferentes
        if (!Regex.IsMatch(email, @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}", RegexOptions.IgnoreCase))
            throw new CoreException("O e-mail do usuário é inválido!");

        // Atribui à propriedade 'Email' o parâmetro 'email' validado
        Email = email;
    }

    private void ValidarNome(string nome)
    {
        // Verifica se o nome é nulo, vazio ou contém apenas espaços em branco
        if (string.IsNullOrWhiteSpace(nome))
            throw new CoreException("O nome do usuário não pode ficar em branco ou conter apenas espaços!");

        // Corta os espaços vazios no começo e no final da string
        nome = nome.Trim();

        // Valida se o nome possui um tamanho mínimo de caracteres
        if (nome.Length < TamanhoMinimoCaracteresNome)
            throw new CoreException(
                $"O nome do usuário deve possuir no mínimo {TamanhoMinimoCaracteresNome} caracteres!");

        // Encontra nomes válidos (que contenham apenas letras, acentos e espaços em branco) e inverte o sinal com o ! antes da palavra REGEX para inverter a seleção de nomes válidos para inválidos. 'RegexOptions.IgnoreCase' faz o C# ignorar minúsculas e maiúsculas como diferentes
        if (!Regex.IsMatch(nome, @"^[A-Za-zÀ-ÖØ-öø-ÿ\s]+$", RegexOptions.IgnoreCase))
            throw new CoreException("O nome do usuário contém caracteres inválidos!");

        // Atribui à propriedade 'Nome' o parâmetro 'nome' validado
        Nome = nome;
    }
}