using AlertaCidade.Core.Entidades;
using Microsoft.EntityFrameworkCore;
namespace AlertaCidade.Api.Dados;

public class ApiContexto(DbContextOptions<ApiContexto> opcoesContexto): DbContext
(opcoesContexto)
{
    public DbSet<Usuario> Usuarios { get; set; }
    
}