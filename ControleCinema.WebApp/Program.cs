using ControleCinema.Dominio.Compartilhado;
using ControleCinema.Dominio.ModuloFilme;
using ControleCinema.Dominio.ModuloFuncionario;
using ControleCinema.Dominio.ModuloGenero;
using ControleCinema.Dominio.ModuloSala;
using ControleCinema.Dominio.ModuloSessao;
using ControleCinema.Infra.Orm.Compartilhado;
using ControleCinema.Infra.Orm.ModuloFilme;
using ControleCinema.Infra.Orm.ModuloFuncionario;
using ControleCinema.Infra.Orm.ModuloGenero;
using ControleCinema.Infra.Orm.ModuloSala;
using ControleCinema.Infra.Orm.ModuloSessao;

namespace ControleCinema.WebApp;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllersWithViews();

        builder.Services.AddDbContext<ControleCinemaDbContext>();

        builder.Services.AddScoped<IRepositorio<Funcionario>, RepositorioFuncionarioEmOrm>();
        builder.Services.AddScoped<IRepositorio<Genero>, RepositorioGeneroEmOrm>();
        builder.Services.AddScoped<IRepositorio<Filme>, RepositorioFilmeEmOrm>();
        builder.Services.AddScoped<IRepositorio<Sala>, RepositorioSalaEmOrm>();
        builder.Services.AddScoped<IRepositorioSessao, RepositorioSessaoEmOrm>();

        var app = builder.Build();

        app.UseStaticFiles();

        app.MapControllerRoute("default", "{controller=Inicio}/{action=Index}/{id:int?}");

        app.Run();
    }
}