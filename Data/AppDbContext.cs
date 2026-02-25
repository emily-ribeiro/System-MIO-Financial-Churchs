using Microsoft.EntityFrameworkCore;
using SistemaIgreja.Services;
using SistemaIgreja.Data;

namespace SistemaIgreja.Data;

public class AppDbContext : DbContext
{
    public DbSet<Congregacao> Congregacoes { get; set; }
    public DbSet<Lancamento> Lancamentos { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public AppDbContext() { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=igreja.db");
    }
}
