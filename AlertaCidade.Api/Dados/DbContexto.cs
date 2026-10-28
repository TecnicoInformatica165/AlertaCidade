using AlertaCidade.Core.Entidades;
using Microsoft.EntityFrameworkCore;

namespace AlertaCidade.Api.Dados;

public class DbContexto(DbContextOptions<DbContexto> opcoesContexto) : DbContext(opcoesContexto)
{
    public DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>(usuario =>
        {
            usuario.Property(u => u.Nome).HasMaxLength(100);
            usuario.Property(u => u.Senha).HasMaxLength(100);

            // E-mail único: não deixa cadastrar o mesmo e-mail duas vezes
            usuario.Property(u => u.Email).HasMaxLength(150);
            usuario.HasIndex(u => u.Email).IsUnique();
        });
    }
}