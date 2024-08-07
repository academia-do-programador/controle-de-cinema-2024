using ControleCinema.Dominio.Compartilhado;
using ControleCinema.Dominio.ModuloFilme;
using ControleCinema.Dominio.ModuloSala;

namespace ControleCinema.Dominio.ModuloSessao;

public class Sessao : EntidadeBase
{
    public Filme Filme { get; set; }
    public Sala Sala { get; set; }

    private bool _encerrada;
    public bool Encerrada
    {
        get
        {
            return _encerrada ||
                   Inicio.AddMinutes(Filme.Duracao) < DateTime.Now;
        }
        set => _encerrada = value;
    }

    public int NumeroMaximoIngressos { get; set; }
    public DateTime Inicio { get; set; }
    public List<Ingresso> Ingressos { get; set; }

    public Sessao()
    {
        Ingressos = new List<Ingresso>();
    }

    public Sessao(Filme filme, Sala sala, int numeroMaximoIngressos, DateTime inicio) : this()
    {
        Filme = filme;
        Sala = sala;
        NumeroMaximoIngressos = numeroMaximoIngressos;
        Inicio = inicio;
    }

    public int[] ObterAssentosDisponiveis()
    {
        var assentosDisponiveis = Enumerable.Range(1, NumeroMaximoIngressos);

        var assentosOcupados = Ingressos.Select(i => i.NumeroAssento).ToArray();

        return assentosDisponiveis
            .Except(assentosOcupados)
            .ToArray();
    }

    public int ObterQuantidadeIngressosDisponiveis()
    {
        return NumeroMaximoIngressos - Ingressos.Count;
    }

    public Ingresso GerarIngresso(int assentoSelecionado, bool meiaEntrada)
    {
        var ingresso = new Ingresso(assentoSelecionado, meiaEntrada);

        Ingressos.Add(ingresso);

        return ingresso;
    }

    public void Encerrar()
    {
        Encerrada = true;
    }
}

