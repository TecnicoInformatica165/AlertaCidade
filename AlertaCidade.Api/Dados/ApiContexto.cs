using AlertaCidade.Core.Entidades;
using Microsoft.EntityFrameworkCore;

namespace AlertaCidade.Api.Dados;

public class ApiContext(DbContextOptions<ApiContext> options) : DbContext(options)
{
    public DbSet<Usuario>  Usuarios { get; set; }
}