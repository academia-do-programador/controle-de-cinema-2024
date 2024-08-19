using ControleCinema.Aplicacao.Servicos;
using ControleCinema.Dominio.ModuloFilme;
using ControleCinema.Dominio.ModuloGenero;
using ControleCinema.Dominio.ModuloSala;
using ControleCinema.Dominio.ModuloSessao;
using ControleCinema.Dominio.ModuloUsuario;
using ControleCinema.Infra.Orm.Compartilhado;
using ControleCinema.Infra.Orm.ModuloFilme;
using ControleCinema.Infra.Orm.ModuloGenero;
using ControleCinema.Infra.Orm.ModuloSala;
using ControleCinema.Infra.Orm.ModuloSessao;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using System.Reflection;

namespace ControleCinema.WebApp;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllersWithViews();

        #region Inje��o de Depend�ncia de Servi�os

        builder.Services.AddDbContext<ControleCinemaDbContext>();

        builder.Services.AddScoped<IRepositorioGenero, RepositorioGeneroEmOrm>();
        builder.Services.AddScoped<IRepositorioFilme, RepositorioFilmeEmOrm>();
        builder.Services.AddScoped<IRepositorioSala, RepositorioSalaEmOrm>();
        builder.Services.AddScoped<IRepositorioSessao, RepositorioSessaoEmOrm>();

        builder.Services.AddScoped<GeneroService>();
        builder.Services.AddScoped<FilmeService>();
        builder.Services.AddScoped<SalaService>();
        builder.Services.AddScoped<SessaoService>();

        builder.Services.AddIdentity<Usuario, Perfil>()
            .AddEntityFrameworkStores<ControleCinemaDbContext>()
            .AddDefaultTokenProviders();

        builder.Services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 3;
            options.Password.RequiredUniqueChars = 1;
        });

        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.Cookie.Name = "AspNetCore.Cookies";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                options.SlidingExpiration = true;
            });

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Usuario/Login";
            options.AccessDeniedPath = "/Usuario/AcessoNegado";
        });

        builder.Services.AddAutoMapper(cfg =>
        {
            cfg.AddMaps(Assembly.GetExecutingAssembly());
        });

        #endregion

        var app = builder.Build();

        app.UseStaticFiles();

        app.MapControllerRoute("default", "{controller=Inicio}/{action=Index}/{id:int?}");

        app.Run();
    }
}