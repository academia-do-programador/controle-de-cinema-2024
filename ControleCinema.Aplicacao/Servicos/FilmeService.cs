using ControleCinema.Dominio.ModuloFilme;
using ControleCinema.Dominio.ModuloGenero;
using FluentResults;

namespace ControleCinema.Aplicacao.Servicos;

public class FilmeService
{
    private readonly IRepositorioFilme repositorioFilme;
    private readonly IRepositorioGenero repositorioGenero;

    public FilmeService(
        IRepositorioFilme repositorioFilme, IRepositorioGenero repositorioGenero)
    {
        this.repositorioFilme = repositorioFilme;
        this.repositorioGenero = repositorioGenero;
    }

    public Result<Filme> Inserir(Filme filme, int generoId)
    {
        var generoSelecionado = repositorioGenero.SelecionarPorId(generoId);

        if (generoSelecionado is null)
            return Result.Fail("O gênero informado não foi encontrado!");

        filme.Genero = generoSelecionado;

        repositorioFilme.Inserir(filme);

        return Result.Ok(filme);
    }

    public Result<Filme> Editar(Filme filmeAtualizado, int generoId)
    {
        var filme = repositorioFilme.SelecionarPorId(filmeAtualizado.Id);

        if (filme is null)
            return Result.Fail("O filme não foi encontrado!");

        var generoSelecionado =
            repositorioGenero.SelecionarPorId(generoId);

        if (generoSelecionado is null)
            return Result.Fail("O gênero informado não foi encontrado!");

        filme.Titulo = filmeAtualizado.Titulo;
        filme.Duracao = filmeAtualizado.Duracao;
        filme.Lancamento = filmeAtualizado.Lancamento;
        filme.Genero = generoSelecionado;

        repositorioFilme.Editar(filme);

        return Result.Ok(filme);
    }

    public Result Excluir(int filmeId)
    {
        var filme = repositorioFilme.SelecionarPorId(filmeId);

        if (filme is null)
            return Result.Fail("O filme não foi encontrado!");

        repositorioFilme.Excluir(filme);

        return Result.Ok();
    }

    public Result<Filme> SelecionarPorId(int filmeId)
    {
        var filme = repositorioFilme.SelecionarPorId(filmeId);

        if (filme is null)
            return Result.Fail("O filme não foi encontrado!");

        return Result.Ok(filme);
    }

    public Result<List<Filme>> SelecionarTodos(int usuarioId)
    {
        var filmes = repositorioFilme
            .Filtrar(f => f.UsuarioId == usuarioId);

        return Result.Ok(filmes);
    }
}