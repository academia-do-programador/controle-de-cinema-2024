using ControleCinema.Dominio.ModuloGenero;
using ControleCinema.WebApp.Extensions;
using ControleCinema.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace ControleCinema.WebApp.Controllers;

public class GeneroController : Controller
{
    private readonly IRepositorioGenero repositorioGenero;

    public GeneroController(IRepositorioGenero repositorioGenero)
    {
        this.repositorioGenero = repositorioGenero;
    }

    public IActionResult Listar()
    {
        var generos = repositorioGenero.SelecionarTodos();

        var listarGenerosVm = generos
            .Select(f => new ListarGeneroViewModel
            {
                Id = f.Id,
                Descricao = f.Descricao
            });

        ViewBag.Mensagem = TempData.DesserializarMensagemViewModel();

        return View(listarGenerosVm);
    }

    public IActionResult Inserir()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Inserir(InserirGeneroViewModel inserirGeneroVm)
    {
        if (!ModelState.IsValid)
            return View(inserirGeneroVm);

        var genero = new Genero()
        {
            Descricao = inserirGeneroVm.Descricao
        };

        repositorioGenero.Inserir(genero);

        TempData.SerializarMensagemViewModel(new MensagemViewModel
        {
            Titulo = "Sucesso",
            Mensagem = $"O registro ID [{genero.Id}] foi inserido com sucesso!"
        });

        return RedirectToAction(nameof(Listar));
    }

    public IActionResult Editar(int id)
    {
        var funcionario = repositorioGenero.SelecionarPorId(id);

        if (funcionario is null)
            return MensagemRegistroNaoEncontrado(id);

        var editarGeneroVm = new EditarGeneroViewModel
        {
            Id = id,
            Descricao = funcionario.Descricao
        };

        return View(editarGeneroVm);
    }

    [HttpPost]
    public IActionResult Editar(EditarGeneroViewModel editarGeneroVm)
    {
        if (!ModelState.IsValid)
            return View(editarGeneroVm);

        var genero = repositorioGenero.SelecionarPorId(editarGeneroVm.Id);

        genero!.Descricao = editarGeneroVm.Descricao;

        repositorioGenero.Editar(genero);

        TempData.SerializarMensagemViewModel(new MensagemViewModel
        {
            Titulo = "Sucesso",
            Mensagem = $"O registro ID [{genero.Id}] foi editado com sucesso!"
        });

        return RedirectToAction(nameof(Listar));
    }

    public IActionResult Excluir(int id)
    {
        var genero = repositorioGenero.SelecionarPorId(id);

        if (genero is null)
            return MensagemRegistroNaoEncontrado(id);

        var detalhesGeneroViewModel = new DetalhesGeneroViewModel
        {
            Id = id,
            Descricao = genero.Descricao
        };

        return View(detalhesGeneroViewModel);
    }

    [HttpPost]
    public IActionResult Excluir(DetalhesGeneroViewModel detalhesGeneroViewModel)
    {
        var genero = repositorioGenero.SelecionarPorId(detalhesGeneroViewModel.Id);

        if (genero is null)
            return MensagemRegistroNaoEncontrado(detalhesGeneroViewModel.Id);

        repositorioGenero.Excluir(genero);

        TempData.SerializarMensagemViewModel(new MensagemViewModel
        {
            Titulo = "Sucesso",
            Mensagem = $"O registro ID [{genero.Id}] foi exclúido com sucesso!"
        });

        return RedirectToAction(nameof(Listar));
    }

    public IActionResult Detalhes(int id)
    {
        var genero = repositorioGenero.SelecionarPorId(id);

        if (genero is null)
            return MensagemRegistroNaoEncontrado(id);

        var detalhesGeneroViewModel = new DetalhesGeneroViewModel
        {
            Id = id,
            Descricao = genero.Descricao
        };

        return View(detalhesGeneroViewModel);
    }

    private IActionResult MensagemRegistroNaoEncontrado(int idRegistro)
    {
        TempData.SerializarMensagemViewModel(new MensagemViewModel
        {
            Titulo = "Erro",
            Mensagem = $"Não foi possível encontrar o registro ID [{idRegistro}]!",
        });

        return RedirectToAction(nameof(Listar));
    }
}