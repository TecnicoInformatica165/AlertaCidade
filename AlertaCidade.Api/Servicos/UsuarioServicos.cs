using AlertaCidade.Api.Dados;
using AlertaCidade.Core.Entidades;
using AlertaCidade.Core.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace AlertaCidade.Api.Servicos;

public class UsuarioServicos(DbContexto contexto)
{
    public async Task AdicionaAsync(Usuario modelo)
    {
        var jaExiste = await contexto.Usuarios.AnyAsync(u => u.Email == modelo.Email);
        if (jaExiste)
            throw new CoreException("Já existe um usuário cadastrado com esse e-mail!");

        var hash = BCrypt.Net.BCrypt.HashPassword(modelo.Senha);
        modelo.DefinirSenhaHash(hash);

        await contexto.Usuarios.AddAsync(modelo);
        await contexto.SaveChangesAsync();
    }

    public async Task<Usuario?> BuscarPorEmailAsync(string email)
    {
        return await contexto.Usuarios
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    // Devolve o usuário se e-mail e senha estiverem certos, ou null se estiverem errados
    public async Task<Usuario?> AutenticarAsync(string email, string senha)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(senha))
            return null;

        var usuario = await BuscarPorEmailAsync(email.Trim());
        if (usuario is null)
            return null;

        try
        {
            // Compara a senha digitada com o hash salvo no banco
            var senhaCorreta = BCrypt.Net.BCrypt.Verify(senha.Trim(), usuario.Senha);
            return senhaCorreta ? usuario : null;
        }
        catch (Exception)
        {
            // Acontece se a senha no banco não for um hash (usuário antigo, cadastrado antes do BCrypt)
            return null;
        }
    }
}