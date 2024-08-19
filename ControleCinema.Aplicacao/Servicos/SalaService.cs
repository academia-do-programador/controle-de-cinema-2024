using ControleCinema.Dominio.ModuloSala;
using FluentResults;

namespace ControleCinema.Aplicacao.Servicos;

public class SalaService
{
    private readonly IRepositorioSala repositorioSala;

    public SalaService(IRepositorioSala repositorioSala)
    {
        this.repositorioSala = repositorioSala;
    }

    public Result<Sala> Inserir(Sala sala)
    {
        repositorioSala.Inserir(sala);

        return Result.Ok(sala);
    }

    public Result<Sala> Editar(Sala salaAtualizada)
    {
        var sala = repositorioSala.SelecionarPorId(salaAtualizada.Id);

        if (sala is null)
            return Result.Fail("A sala não foi encontrada!");

        sala.Numero = salaAtualizada.Numero;
        sala.Capacidade = salaAtualizada.Capacidade;

        repositorioSala.Editar(sala);

        return Result.Ok(sala);
    }

    public Result Excluir(int salaId)
    {
        var sala = repositorioSala.SelecionarPorId(salaId);

        if (sala is null)
            return Result.Fail("A sala não foi encontrada!");

        repositorioSala.Excluir(sala);

        return Result.Ok();
    }

    public Result<Sala> SelecionarPorId(int salaId)
    {
        var sala = repositorioSala.SelecionarPorId(salaId);

        if (sala is null)
            return Result.Fail("A sala não foi encontrada!");

        return Result.Ok(sala);
    }

    public Result<List<Sala>> SelecionarTodos(int usuarioId)
    {
        var salas = repositorioSala
            .Filtrar(f => f.UsuarioId == usuarioId);

        return Result.Ok(salas);
    }
}