using ControleCinema.Dominio.ModuloFilme;
using ControleCinema.Dominio.ModuloFuncionario;
using ControleCinema.Dominio.ModuloGenero;
using ControleCinema.Dominio.ModuloSala;
using ControleCinema.Dominio.ModuloSessao;
using ControleCinema.Dominio.ModuloUsuario;
using ControleCinema.Infra.Orm.ModuloFilme;
using ControleCinema.Infra.Orm.ModuloGenero;
using ControleCinema.Infra.Orm.ModuloSala;
using ControleCinema.Infra.Orm.ModuloSessao;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ControleCinema.Infra.Orm.Compartilhado;

public class ControleCinemaDbContext : IdentityDbContext<Usuario, Perfil, int>
{
    public DbSet<Funcionario> Funcionarios { get; set; }
    public DbSet<Genero> Generos { get; set; }
    public DbSet<Filme> Filmes { get; set; }
    public DbSet<Sala> Salas { get; set; }
    public DbSet<Sessao> Sessoes { get; set; }
    public DbSet<Ingresso> Ingressos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = config
            .GetConnectionString("SqlServer");

        optionsBuilder.UseSqlServer(connectionString);

        optionsBuilder.LogTo(Console.WriteLine).EnableSensitiveDataLogging();

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<Funcionario>();
        modelBuilder.ApplyConfiguration(new MapeadorGeneroEmOrm());
        modelBuilder.ApplyConfiguration(new MapeadorFilmeEmOrm());
        modelBuilder.ApplyConfiguration(new MapeadorSalaEmOrm());
        modelBuilder.ApplyConfiguration(new MapeadorSessaoEmOrm());
        modelBuilder.ApplyConfiguration(new MapeadorIngressoEmOrm());

        base.OnModelCreating(modelBuilder);
    }
}