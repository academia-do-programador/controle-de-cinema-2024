using ControleCinema.Dominio.ModuloUsuario;

namespace ControleCinema.Dominio.Compartilhado;

public abstract class EntidadeBase
{
    public int Id { get; set; }

    public Usuario Usuario { get; set; }
}

