using ControleCinema.Dominio.ModuloGenero;
using FluentResults;

namespace ControleCinema.Aplicacao.Servicos;

public class GeneroService
{
    private readonly IRepositorioGenero repositorioGenero;

    public GeneroService(IRepositorioGenero repositorioGenero)
    {
        this.repositorioGenero = repositorioGenero;
    }

    public Result<Genero> Inserir(Genero genero)
    {
        repositorioGenero.Inserir(genero);

        return Result.Ok(genero);
    }

    public Result<Genero> Editar(Genero generoAtualizado)
    {
        var genero = repositorioGenero.SelecionarPorId(generoAtualizado.Id);

        if (genero is null)
            return Result.Fail("O genero não foi encontrado!");

        genero.Descricao = generoAtualizado.Descricao;

        repositorioGenero.Editar(genero);

        return Result.Ok(genero);
    }

    public Result Excluir(int generoId)
    {
        var genero = repositorioGenero.SelecionarPorId(generoId);

        if (genero is null)
            return Result.Fail("O genero não foi encontrado!");

        repositorioGenero.Excluir(genero);

        return Result.Ok();
    }

    public Result<Genero> SelecionarPorId(int generoId)
    {
        var genero = repositorioGenero.SelecionarPorId(generoId);

        if (genero is null)
            return Result.Fail("O genero não foi encontrado!");

        return Result.Ok(genero);
    }

    public Result<List<Genero>> SelecionarTodos(int usuarioId)
    {
        var generos = repositorioGenero
            .Filtrar(f => f.UsuarioId == usuarioId);

        return Result.Ok(generos);
    }
}