using AutoMapper;
using ControleCinema.Aplicacao.Servicos;
using ControleCinema.Dominio.Extensions;
using ControleCinema.Dominio.ModuloFilme;
using ControleCinema.WebApp.Extensions;
using ControleCinema.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ControleCinema.WebApp.Controllers;

[Authorize(Roles = "Empresa")]
public class FilmeController : WebControllerBase
{
    private readonly FilmeService filmeService;
    private readonly GeneroService generoService;
    private readonly IMapper mapeador;

    public FilmeController(
        FilmeService filmeService,
        GeneroService generoService, 
        IMapper mapeador
    )
    {
        this.filmeService = filmeService;
        this.generoService = generoService;
        this.mapeador = mapeador;
    }

    public IActionResult Listar()
    {
        var resultado = 
            filmeService.SelecionarTodos(UsuarioId.GetValueOrDefault());

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());

            return RedirectToAction("Index", "Inicio");
        }

        var filmes = resultado.Value;

        var listarFilmesVm = mapeador.Map<IEnumerable<ListarFilmeViewModel>>(filmes);

        ViewBag.Mensagem = TempData.DesserializarMensagemViewModel();

        return View(listarFilmesVm);
    }

    public IActionResult Inserir()
    {
        return View(CarregarInformacoesFilme(new InserirFilmeViewModel()));
    }

    [HttpPost]
    public IActionResult Inserir(InserirFilmeViewModel inserirFilmeVm)
    {
        if (!ModelState.IsValid)
            return View(CarregarInformacoesFilme(inserirFilmeVm));

        var filme = mapeador.Map<Filme>(inserirFilmeVm);

        filme.UsuarioId = UsuarioId.GetValueOrDefault();

        var generoId = inserirFilmeVm.GeneroId;

        var resultado = filmeService.Inserir(filme, generoId);

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());

            return RedirectToAction(nameof(Listar));
        }

        ApresentarMensagemSucesso($"O registro ID [{filme.Id}] foi inserido com sucesso!");

        return RedirectToAction(nameof(Listar));
    }

    public IActionResult Editar(int id)
    {
        var resultadoFilme = filmeService.SelecionarPorId(id);

        if (resultadoFilme.IsFailed)
        {
            ApresentarMensagemFalha(resultadoFilme.ToResult());

            return RedirectToAction(nameof(Listar));
        }

        var resultadoGenero =
            generoService.SelecionarTodos(UsuarioId.GetValueOrDefault());

        var generos = resultadoGenero.Value;

        var filme = resultadoFilme.Value;

        var editarFilmeVm = mapeador.Map<EditarFilmeViewModel>(filme);

        editarFilmeVm.Generos = generos
            .Select(g => new SelectListItem(g.Descricao, g.Id.ToString()));

        return View(editarFilmeVm);
    }

    [HttpPost]
    public IActionResult Editar(EditarFilmeViewModel editarFilmeVm)
    {
        if (!ModelState.IsValid)
            return View(CarregarInformacoesFilme(editarFilmeVm));

        var filme = mapeador.Map<Filme>(editarFilmeVm);

        var resultado = filmeService.Editar(filme, editarFilmeVm.GeneroId);

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());

            return RedirectToAction(nameof(Listar));
        }

        ApresentarMensagemSucesso($"O registro ID [{editarFilmeVm.Id}] foi editado com sucesso!");

        return RedirectToAction(nameof(Listar));
    }

    public IActionResult Excluir(int id)
    {
        var resultado = filmeService.SelecionarPorId(id);

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());

            return RedirectToAction(nameof(Listar));
        }

        var filme = resultado.Value;

        var detalhesFilmeViewModel = mapeador.Map<DetalhesFilmeViewModel>(filme);

        return View(detalhesFilmeViewModel);
    }

    [HttpPost]
    public IActionResult Excluir(DetalhesFilmeViewModel detalhesFilmeViewModel)
    {
        var resultado = filmeService.Excluir(detalhesFilmeViewModel.Id);

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado);

            return RedirectToAction(nameof(Listar));
        }

        ApresentarMensagemSucesso($"O registro ID [{detalhesFilmeViewModel.Id}] foi excluído com sucesso!");

        return RedirectToAction(nameof(Listar));
    }

    public IActionResult Detalhes(int id)
    {
        var resultado = filmeService.SelecionarPorId(id);

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());

            return RedirectToAction(nameof(Listar));
        }

        var filme = resultado.Value;

        var detalhesFilmeViewModel = mapeador.Map<DetalhesFilmeViewModel>(filme);

        return View(detalhesFilmeViewModel);
    }

    private FormFilmeViewModel? CarregarInformacoesFilme(FormFilmeViewModel inserirFilmeVm)
    {
        var resultadoGeneros =
            generoService.SelecionarTodos(UsuarioId.GetValueOrDefault());

        if (resultadoGeneros.IsFailed)
        {
            ApresentarMensagemFalha(resultadoGeneros.ToResult());

            return null;
        }

        var generos = resultadoGeneros.Value;

        inserirFilmeVm.Generos = generos
            .Select(g => new SelectListItem(g.Descricao, g.Id.ToString()));

        return inserirFilmeVm;
    }
}