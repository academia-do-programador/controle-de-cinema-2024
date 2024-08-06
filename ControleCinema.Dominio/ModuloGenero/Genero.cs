using ControleCinema.Dominio.Compartilhado;

namespace ControleCinema.Dominio.ModuloGenero;

public class Genero : EntidadeBase
{
    public string Descricao { get; set; }
    
    public Genero() { }

    public Genero(string descricao)
    {
        Descricao = descricao;
    }
}

