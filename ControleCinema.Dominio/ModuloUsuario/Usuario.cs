using Microsoft.AspNetCore.Identity;

namespace ControleCinema.Dominio.ModuloUsuario;

public class Usuario : IdentityUser<int>
{
    public Usuario()
    {
        EmailConfirmed = true;
    }
}