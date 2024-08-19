using ControleCinema.Aplicacao.Servicos;
using ControleCinema.Dominio.Extensions;
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

    public FilmeController(
        FilmeService filmeService,
        GeneroService generoService
    )
    {
        this.filmeService = filmeService;
        this.generoService = generoService;
    }

    public IActionResult Listar()
    {
        var resultado = filmeService.SelecionarTodos(UsuarioId.GetValueOrDefault());

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());

            return RedirectToAction("Index", "Inicio");
        }

        var filmes = resultado.Value;
        
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
        return View(CarregarInformacoesFilme(new InserirFilmeViewModel()));
    }

    [HttpPost]
    public IActionResult Inserir(InserirFilmeViewModel inserirFilmeVm)
    {
        if (!ModelState.IsValid)
            return View(CarregarInformacoesFilme(inserirFilmeVm));

        var resultado = filmeService.Inserir(
            inserirFilmeVm.Titulo,
            inserirFilmeVm.Duracao,
            inserirFilmeVm.Lancamento,
            inserirFilmeVm.GeneroId,
            UsuarioId.GetValueOrDefault()
        );

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());

            return RedirectToAction(nameof(Listar));
        }

        var filme = resultado.Value; 

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

        var filme = resultadoFilme.Value;

        var resultadoGenero = 
            generoService.SelecionarTodos(UsuarioId.GetValueOrDefault());

        var generos = resultadoGenero.Value;

        var editarFilmeVm = new EditarFilmeViewModel
        {
            Id = id,
            Titulo = filme.Titulo,
            Duracao = filme.Duracao,
            Lancamento = filme.Lancamento,
            Generos = generos
                .Select(g => new SelectListItem(g.Descricao, g.Id.ToString())),
            GeneroId = filme.Genero.Id
        };

        return View(editarFilmeVm);
    }

    [HttpPost]
    public IActionResult Editar(EditarFilmeViewModel editarFilmeVm)
    {
        if (!ModelState.IsValid)
            return View(CarregarInformacoesFilme(editarFilmeVm));

        var resultado = filmeService.Editar(
            editarFilmeVm.Id,
            editarFilmeVm.Titulo,
            editarFilmeVm.Duracao,
            editarFilmeVm.Lancamento,
            editarFilmeVm.GeneroId
        );
        
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