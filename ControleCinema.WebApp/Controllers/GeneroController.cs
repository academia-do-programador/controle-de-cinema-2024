using AutoMapper;
using ControleCinema.Aplicacao.Servicos;
using ControleCinema.Dominio.ModuloGenero;
using ControleCinema.WebApp.Extensions;
using ControleCinema.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControleCinema.WebApp.Controllers;

[Authorize(Roles = "Empresa")]
public class GeneroController : WebControllerBase
{
    private readonly GeneroService servicoGenero;
    private readonly IMapper mapeador;

    public GeneroController(
        GeneroService servicoGenero,
        IMapper mapeador
    )
    {
        this.servicoGenero = servicoGenero;
        this.mapeador = mapeador;
    }

    public IActionResult Listar()
    {
        var resultado = servicoGenero.SelecionarTodos(UsuarioId.GetValueOrDefault());

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());

            return RedirectToAction("Index", "Inicio");
        }

        var generos = resultado.Value;

        var listarGenerosVm = mapeador.Map<IEnumerable<ListarGeneroViewModel>>(generos);

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

        var genero = mapeador.Map<Genero>(inserirGeneroVm);

        genero.UsuarioId = UsuarioId.GetValueOrDefault();

        var resultado = servicoGenero.Inserir(genero);

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());

            return RedirectToAction(nameof(Listar));
        }

        ApresentarMensagemSucesso($"O registro ID [{genero.Id}] foi inserido com sucesso!");

        return RedirectToAction(nameof(Listar));
    }

    public IActionResult Editar(int id)
    {
        var resultado = servicoGenero.SelecionarPorId(id);

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());

            return RedirectToAction(nameof(Listar));
        }

        var genero = resultado.Value;

        var editarGeneroVm = mapeador.Map<EditarGeneroViewModel>(genero);

        return View(editarGeneroVm);
    }

    [HttpPost]
    public IActionResult Editar(EditarGeneroViewModel editarGeneroVm)
    {
        if (!ModelState.IsValid)
            return View(editarGeneroVm);

        var genero = mapeador.Map<Genero>(editarGeneroVm);

        var resultado = servicoGenero.Editar(genero);

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());

            return RedirectToAction(nameof(Listar));
        }

        ApresentarMensagemSucesso($"O registro ID [{editarGeneroVm.Id}] foi editado com sucesso!");

        return RedirectToAction(nameof(Listar));
    }

    public IActionResult Excluir(int id)
    {
        var resultado = servicoGenero.SelecionarPorId(id);

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());

            return RedirectToAction(nameof(Listar));
        }

        var genero = resultado.Value;

        var detalhesGeneroViewModel = mapeador.Map<DetalhesGeneroViewModel>(genero);

        return View(detalhesGeneroViewModel);
    }

    [HttpPost]
    public IActionResult Excluir(DetalhesGeneroViewModel detalhesGeneroViewModel)
    {
        var resultado = servicoGenero.Excluir(detalhesGeneroViewModel.Id);

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado);

            return RedirectToAction(nameof(Listar));
        }

        ApresentarMensagemSucesso($"O registro ID [{detalhesGeneroViewModel.Id}] foi excluído com sucesso!");

        return RedirectToAction(nameof(Listar));
    }

    public IActionResult Detalhes(int id)
    {
        var resultado = servicoGenero.SelecionarPorId(id);

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());

            return RedirectToAction(nameof(Listar));
        }

        var genero = resultado.Value;

        var detalhesGeneroViewModel = mapeador.Map<DetalhesGeneroViewModel>(genero);

        return View(detalhesGeneroViewModel);
    }
}