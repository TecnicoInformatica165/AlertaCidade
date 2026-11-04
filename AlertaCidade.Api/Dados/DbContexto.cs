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
        modelBuilder.Entity<Denuncia>().OwnsOne(d => d.Localizacao);
    }
}