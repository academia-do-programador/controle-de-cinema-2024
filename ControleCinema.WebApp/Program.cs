using ControleCinema.Dominio.ModuloFilme;
using ControleCinema.Dominio.ModuloGenero;
using ControleCinema.Dominio.ModuloSala;
using ControleCinema.Dominio.ModuloSessao;
using ControleCinema.Infra.Orm.Compartilhado;
using ControleCinema.Infra.Orm.ModuloFilme;
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

        #region Injeção de Dependência de Serviços

        builder.Services.AddDbContext<ControleCinemaDbContext>();

        builder.Services.AddScoped<IRepositorioGenero, RepositorioGeneroEmOrm>();
        builder.Services.AddScoped<IRepositorioFilme, RepositorioFilmeEmOrm>();
        builder.Services.AddScoped<IRepositorioSala, RepositorioSalaEmOrm>();
        builder.Services.AddScoped<IRepositorioSessao, RepositorioSessaoEmOrm>();

        #endregion

        var app = builder.Build();

        app.UseStaticFiles();

        app.MapControllerRoute("default", "{controller=Inicio}/{action=Index}/{id:int?}");

        app.Run();
    }
}