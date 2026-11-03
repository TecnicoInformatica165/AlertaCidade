using AlertaCidade.Core.Entidades;
using Microsoft.EntityFrameworkCore;

namespace AlertaCidade.Api.Dados;

public class DbContexto(DbContextOptions<DbContexto> opcoesContexto) : DbContext(opcoesContexto)
{
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Denuncia> Denuncias { get; set; }
    public DbSet<CategoriaDenuncia> CategoriaDenuncias { get; set; }

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

        modelBuilder.Entity<CategoriaDenuncia>(categoria =>
        {
            categoria.Property(c => c.Nome).HasMaxLength(100);

            // Categorias iniciais, para a rota de denúncias já ter o que usar
            categoria.HasData(
                new { Id = 1, Nome = "Buraco na via" },
                new { Id = 2, Nome = "Iluminação pública" },
                new { Id = 3, Nome = "Lixo e entulho" },
                new { Id = 4, Nome = "Vazamento de água" }
            );
        });

        modelBuilder.Entity<Denuncia>(denuncia =>
        {
            denuncia.HasOne(d => d.Categoria)
                .WithMany()
                .IsRequired();

            // Localizacao é um objeto de valor: vira colunas dentro de Denuncias
            denuncia.OwnsOne(d => d.Localizacao, localizacao =>
            {
                localizacao.Property(l => l.Latitude).HasPrecision(10, 7);
                localizacao.Property(l => l.Longitude).HasPrecision(10, 7);
                localizacao.Property(l => l.Precisao).HasPrecision(10, 2);
            });

            denuncia.Navigation(d => d.Localizacao).IsRequired();
        });
    }
}