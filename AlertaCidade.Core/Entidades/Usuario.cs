namespace AlertaCidade.Core.Entidades;

public class Usuario
{
    public int Id { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public string Email { get; private set; }  = string.Empty;
    public string Senha { get; private set; } = string.Empty;

    protected Usuario()
    {
        
    }

    public Usuario(string nome, string email, string senha)
    {
        ValidarNome(nome);
        ValidarEmail(email);
        ValidarSenha(senha);
    }

    private void ValidarSenha(string senha)
    {
        Senha = senha;
    }

    private void ValidarEmail(string email)
    {
        Email = email;
    }

    private void ValidarNome(string nome)
    {
        Nome = nome;
    }
}