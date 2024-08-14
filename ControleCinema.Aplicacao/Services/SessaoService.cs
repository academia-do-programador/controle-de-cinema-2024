using ControleCinema.Dominio.ModuloFilme;
using ControleCinema.Dominio.ModuloSala;
using ControleCinema.Dominio.ModuloSessao;
using FluentResults;

namespace ControleCinema.Aplicacao.Services;

public class SessaoService
{
    private readonly IRepositorioSala repositorioSala;
    private readonly IRepositorioFilme repositorioFilme;
    private readonly IRepositorioSessao repositorioSessao;

    public SessaoService(
        IRepositorioSala repositorioSala,
        IRepositorioFilme repositorioFilme,
        IRepositorioSessao repositorioSessao
    )
    {
        this.repositorioSala = repositorioSala;
        this.repositorioFilme = repositorioFilme;
        this.repositorioSessao = repositorioSessao;
    }

    public Result<Sessao> Inserir(DateTime inicio, int numeroMaxIngressos, int salaId, int filmeId, int usuarioId)
    {
        var salaSelecionada = repositorioSala
            .SelecionarPorId(salaId);

        if (salaSelecionada is null)
            return Result.Fail("A sala não foi selecionada!");

        var filmeSelecionado =
            repositorioFilme.SelecionarPorId(filmeId);

        if (filmeSelecionado is null)
            return Result.Fail("O filme não foi selecionado!");

        var sessao = new Sessao()
        {
            Sala = salaSelecionada,
            Filme = filmeSelecionado,
            Inicio = inicio,
            NumeroMaximoIngressos = numeroMaxIngressos,
            UsuarioId = usuarioId
        };

        var erros = sessao.Validar();

        if (erros.Count != 0)
            return Result.Fail(erros[0]);

        repositorioSessao.Inserir(sessao);

        return Result.Ok(sessao);
    }

    public Result<Sessao> Encerrar(int id)
    {
        var sessao = repositorioSessao.SelecionarPorId(id);

        if (sessao is null)
            return Result.Fail("A sessão não foi encontrada!");

        sessao.Encerrar();

        repositorioSessao.Editar(sessao);

        return Result.Ok(sessao);
    }

    public Result<Sessao> Excluir(int id)
    {
        var sessao = repositorioSessao.SelecionarPorId(id);

        if (sessao is null)
            return Result.Fail("A sessão não foi encontrada!");

        repositorioSessao.Excluir(sessao);

        return Result.Ok(sessao);
    }

    public Result<Sessao> ComprarIngresso(
        int sessaoId, int assentoSelecionado, bool meiaEntrada, int usuarioId)
    {
        var sessao = repositorioSessao.SelecionarPorId(sessaoId);

        if (sessao is null)
            return Result.Fail("A sessão não foi encontrada!");

        sessao.GerarIngresso(assentoSelecionado, meiaEntrada, usuarioId);

        repositorioSessao.Editar(sessao);

        return Result.Ok(sessao);
    }
}