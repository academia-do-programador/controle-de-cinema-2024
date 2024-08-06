using ControleCinema.Dominio.Compartilhado;

namespace ControleCinema.Dominio.ModuloSala;

public class Sala : EntidadeBase
{
    public int Numero { get; set; }
    public int Capacidade { get; set; }

    public Sala() { }
    
    public Sala(int numero, int capacidade)
    {
        Numero = numero;
        Capacidade = capacidade;
    }
}

