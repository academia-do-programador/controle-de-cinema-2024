namespace ControleCinema.WebApp;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllersWithViews();

        var app = builder.Build();

        app.UseStaticFiles();

        app.MapControllerRoute("default", "{controller}/{action}/{id:int?}");

        app.Run();
    }
}