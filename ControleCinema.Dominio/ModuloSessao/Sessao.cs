using ControleCinema.Dominio.Compartilhado;
using ControleCinema.Dominio.ModuloFilme;
using ControleCinema.Dominio.ModuloSala;

namespace ControleCinema.Dominio.ModuloSessao;

public class Sessao : EntidadeBase
{
    public Filme Filme { get; set; }
    public Sala Sala { get; set; }

    public bool Encerrada { get; set; }

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

    public Ingresso GerarIngresso(int assentoSelecionado, bool meiaEntrada, int usuarioId)
    {
        var ingresso = new Ingresso(assentoSelecionado, meiaEntrada)
        {
            UsuarioId = usuarioId
        };

        Ingressos.Add(ingresso);

        return ingresso;
    }

    public void Encerrar()
    {
        Encerrada = true;
    }

    public List<string> Validar()
    {
        List<string> erros = [];

        if (Inicio < DateTime.Now)
            erros.Add("A sessão precisa iniciar em uma data futura!");

        if (NumeroMaximoIngressos < 1)
            erros.Add("Ao menos um ingresso precisa estar disponível!");

        return erros;
    }
}

