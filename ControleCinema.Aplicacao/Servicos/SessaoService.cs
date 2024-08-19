using ControleCinema.Dominio.ModuloFilme;
using ControleCinema.Dominio.ModuloSala;
using ControleCinema.Dominio.ModuloSessao;
using FluentResults;

namespace ControleCinema.Aplicacao.Servicos;

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

    public Result<Sessao> Inserir(Sessao sessao, int salaId, int filmeId)
    {
        var salaSelecionada = repositorioSala
            .SelecionarPorId(salaId);

        if (salaSelecionada is null)
            return Result.Fail("A sala não foi selecionada!");

        var filmeSelecionado =
            repositorioFilme.SelecionarPorId(filmeId);

        if (filmeSelecionado is null)
            return Result.Fail("O filme não foi selecionado!");

        sessao.Sala = salaSelecionada;
        sessao.Filme = filmeSelecionado;

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

    public Result Excluir(int id)
    {
        var sessao = repositorioSessao.SelecionarPorId(id);

        if (sessao is null)
            return Result.Fail("A sessão não foi encontrada!");

        repositorioSessao.Excluir(sessao);

        return Result.Ok();
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

    public Result<Sessao> SelecionarPorId(int sessaoId)
    {
        var sessao = repositorioSessao.SelecionarPorId(sessaoId);

        if (sessao is null)
            return Result.Fail("A sessão não foi encontrada.");

        return Result.Ok(sessao);
    }

    public Result<List<IGrouping<string, Sessao>>> ObterSessoesAgrupadasPorFilme(int? usuarioId = null)
    {
        if (usuarioId is not null)
            return Result.Ok(repositorioSessao.ObterSessoesAgrupadasPorFilme(usuarioId.Value));

        return Result.Ok(repositorioSessao.ObterSessoesAgrupadasPorFilme());
    }

    public Result<List<Sessao>> SelecionarTodos(int usuarioId)
    {
        var sessoes = repositorioSessao.Filtrar(s => s.UsuarioId == usuarioId);

        return Result.Ok(sessoes);
    }

    public Result<List<Ingresso>> SelecionarTodosIngressos(int usuarioId)
    {
        var ingressos = repositorioSessao.SelecionarTodosIngressos(usuarioId);

        return Result.Ok(ingressos);
    }
}