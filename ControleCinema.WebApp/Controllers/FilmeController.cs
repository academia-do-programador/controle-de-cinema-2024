using ControleCinema.Dominio.Extensions;
using ControleCinema.Dominio.ModuloFilme;
using ControleCinema.Infra.Orm.Compartilhado;
using ControleCinema.Infra.Orm.ModuloFilme;
using ControleCinema.Infra.Orm.ModuloGenero;
using ControleCinema.WebApp.Extensions;
using ControleCinema.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ControleCinema.WebApp.Controllers;

public class FilmeController : Controller
{

    public IActionResult Listar()
    {
        var db = new ControleCinemaDbContext();
        var repositorioFilme = new RepositorioFilmeEmOrm(db);

        var filmes = repositorioFilme.SelecionarTodos();

        var listarFilmesVm = filmes
            .Select(f => new ListarFilmeViewModel
            {
                Id = f.Id,
                Titulo = f.Titulo,
                Duracao = f.Duracao.FormatarEmHorasEMinutos(),
                Lancamento = f.Lancamento ? "Lançamento" : "Re-Exibição",
                Genero = f.Genero.Descricao
            });

        ViewBag.Mensagem = TempData.DesserializarMensagemViewModel();

        return View(listarFilmesVm);
    }

    public IActionResult Inserir()
    {
        var db = new ControleCinemaDbContext();
        var repositorioGenero = new RepositorioGeneroEmOrm(db);

        var generosDeFilme = repositorioGenero.SelecionarTodos();

        var inserirFilmeVm = new InserirFilmeViewModel
        {
            Generos = generosDeFilme
                .Select(g => new SelectListItem(g.Descricao, g.Id.ToString()))
        };

        return View(inserirFilmeVm);
    }

    [HttpPost]
    public IActionResult Inserir(InserirFilmeViewModel inserirFilmeVm)
    {
        var db = new ControleCinemaDbContext();
        var repositorioGenero = new RepositorioGeneroEmOrm(db);
        var repositorioFilme = new RepositorioFilmeEmOrm(db);

        if (!ModelState.IsValid)
        {
            var generosDeFilme = repositorioGenero.SelecionarTodos();

            inserirFilmeVm.Generos = generosDeFilme
                .Select(g => new SelectListItem(g.Descricao, g.Id.ToString()));

            return View(inserirFilmeVm);
        }

        var generoSelecionado =
            repositorioGenero.SelecionarPorId(inserirFilmeVm.GeneroId.GetValueOrDefault());

        var filme = new Filme()
        {
            Titulo = inserirFilmeVm.Titulo,
            Lancamento = inserirFilmeVm.Lancamento,
            Duracao = inserirFilmeVm.Duracao,
            Genero = generoSelecionado
        };

        repositorioFilme.Inserir(filme);

        TempData.SerializarMensagemViewModel(new MensagemViewModel
        {
            Titulo = "Sucesso",
            Mensagem = $"O registro ID [{filme.Id}] foi inserido com sucesso!"
        });

        return RedirectToAction(nameof(Listar));
    }

    public IActionResult Editar(int id)
    {
        var db = new ControleCinemaDbContext();
        var repositorioGenero = new RepositorioGeneroEmOrm(db);
        var repositorioFilme = new RepositorioFilmeEmOrm(db);

        var filme = repositorioFilme.SelecionarPorId(id);

        if (filme is null)
            return MensagemRegistroNaoEncontrado(id);

        var generosDeFilme = repositorioGenero.SelecionarTodos();

        var editarFilmeVm = new EditarFilmeViewModel
        {
            Id = id,
            Titulo = filme.Titulo,
            Duracao = filme.Duracao,
            Lancamento = filme.Lancamento,
            Generos = generosDeFilme
                .Select(g => new SelectListItem(g.Descricao, g.Id.ToString())),
            GeneroId = filme.Genero.Id
        };

        return View(editarFilmeVm);
    }

    [HttpPost]
    public IActionResult Editar(EditarFilmeViewModel editarFilmeVm)
    {
        var db = new ControleCinemaDbContext();
        var repositorioGenero = new RepositorioGeneroEmOrm(db);
        var repositorioFilme = new RepositorioFilmeEmOrm(db);

        if (!ModelState.IsValid)
        {
            var generosDeFilme = repositorioGenero.SelecionarTodos();

            editarFilmeVm.Generos = generosDeFilme
                .Select(g => new SelectListItem(g.Descricao, g.Id.ToString()));

            return View(editarFilmeVm);
        }

        var generoSelecionado =
            repositorioGenero.SelecionarPorId(editarFilmeVm.GeneroId);

        var filme = repositorioFilme.SelecionarPorId(editarFilmeVm.Id);

        filme!.Titulo = editarFilmeVm.Titulo;
        filme!.Duracao = editarFilmeVm.Duracao;
        filme!.Lancamento = editarFilmeVm.Lancamento;
        filme!.Genero = generoSelecionado;

        repositorioFilme.Editar(filme);

        TempData.SerializarMensagemViewModel(new MensagemViewModel
        {
            Titulo = "Sucesso",
            Mensagem = $"O registro ID [{filme.Id}] foi editado com sucesso!"
        });

        return RedirectToAction(nameof(Listar));
    }

    public IActionResult Excluir(int id)
    {
        var db = new ControleCinemaDbContext();
        var repositorioFilme = new RepositorioFilmeEmOrm(db);

        var filme = repositorioFilme.SelecionarPorId(id);

        if (filme is null)
            return MensagemRegistroNaoEncontrado(id);

        var detalhesFilmeViewModel = new DetalhesFilmeViewModel
        {
            Id = id,
            Titulo = filme.Titulo,
            Duracao = filme.Duracao.FormatarEmHorasEMinutos(),
            Lancamento = filme.Lancamento ? "Lançamento" : "Re-Exibição",
            Genero = filme.Genero.Descricao
        };

        return View(detalhesFilmeViewModel);
    }

    [HttpPost]
    public IActionResult Excluir(DetalhesFilmeViewModel detalhesFilmeViewModel)
    {
        var db = new ControleCinemaDbContext();
        var repositorioFilme = new RepositorioFilmeEmOrm(db);

        var filme = repositorioFilme.SelecionarPorId(detalhesFilmeViewModel.Id);

        if (filme is null)
            return MensagemRegistroNaoEncontrado(detalhesFilmeViewModel.Id);

        repositorioFilme.Excluir(filme);

        TempData.SerializarMensagemViewModel(new MensagemViewModel
        {
            Titulo = "Sucesso",
            Mensagem = $"O registro ID [{filme.Id}] foi excluído com sucesso!"
        });

        return RedirectToAction(nameof(Listar));
    }

    public IActionResult Detalhes(int id)
    {
        var db = new ControleCinemaDbContext();
        var repositorioFilme = new RepositorioFilmeEmOrm(db);

        var filme = repositorioFilme.SelecionarPorId(id);

        if (filme is null)
            return MensagemRegistroNaoEncontrado(id);

        var detalhesFilmeViewModel = new DetalhesFilmeViewModel
        {
            Id = id,
            Titulo = filme.Titulo,
            Duracao = filme.Duracao.FormatarEmHorasEMinutos(),
            Lancamento = filme.Lancamento ? "Lançamento" : "Re-Exibição",
            Genero = filme.Genero.Descricao
        };

        return View(detalhesFilmeViewModel);
    }

    private IActionResult MensagemRegistroNaoEncontrado(int idRegistro)
    {
        TempData.SerializarMensagemViewModel(new MensagemViewModel
        {
            Titulo = "Erro",
            Mensagem = $"Não foi possível encontrar o registro ID [{idRegistro}]!"
        });

        return RedirectToAction(nameof(Listar));
    }
}