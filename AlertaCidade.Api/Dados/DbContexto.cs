using AlertaCidade.Core.Entidades;
using Microsoft.EntityFrameworkCore;
namespace AlertaCidade.Api.Dados;

public class DbContexto(DbContextOptions<DbContexto> opcoesContexto): DbContext
(opcoesContexto)
{
    public DbSet<Usuario> Usuarios { get; set; }
    
}