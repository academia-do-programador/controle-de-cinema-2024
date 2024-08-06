using ControleCinema.Dominio.Compartilhado;
using ControleCinema.Dominio.ModuloGenero;
using ControleCinema.Dominio.ModuloSessao;

namespace ControleCinema.Dominio.ModuloFilme;

public class Filme : EntidadeBase
{
    public string Titulo { get; set; }
    public int Duracao { get; set; }
    public Genero Genero { get; set; }
    public bool Lancamento { get; set; }
    public List<Sessao> Sessoes { get; set; }

    public Filme()
    {
        Sessoes = new List<Sessao>();
    }

    public Filme(string titulo, int duracao, Genero genero, bool lancamento) : this()
    {
        this.Titulo = titulo;
        this.Duracao = duracao;
        this.Genero = genero;
        this.Lancamento = lancamento;
    }
}

