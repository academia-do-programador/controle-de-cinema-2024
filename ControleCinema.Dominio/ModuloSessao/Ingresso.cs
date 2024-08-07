using ControleCinema.Dominio.Compartilhado;

namespace ControleCinema.Dominio.ModuloSessao;

public class Ingresso : EntidadeBase
{
    public bool MeiaEntrada { get; set; }
    public int NumeroAssento { get; set; }
    public Sessao Sessao { get; set; }

    public Ingresso() { }

    public Ingresso(int numeroAssento, bool meiaEntrada)
    {
        NumeroAssento = numeroAssento;
        MeiaEntrada = meiaEntrada;
    }
}

