using GerenciadorTarefas.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace GerenciadorTarefas.Data;
public class GerenciadorTarefasDbContext : DbContext
{
    private readonly DbContextOptions<GerenciadorTarefasDbContext> _options;
    public GerenciadorTarefasDbContext(DbContextOptions<GerenciadorTarefasDbContext> options) : base(options)        
    {
        _options = options;
    }

    public DbContextOptions<GerenciadorTarefasDbContext> StartUpOptions {
        get {
            return _options;
        }
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Tarefa>();
    }

    public DbSet<Tarefa> Tarefas { get; set; }
}