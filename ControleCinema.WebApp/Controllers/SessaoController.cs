using AutoMapper;
using ControleCinema.Aplicacao.Servicos;
using ControleCinema.Dominio.ModuloSessao;
using ControleCinema.WebApp.Extensions;
using ControleCinema.WebApp.Models;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ControleCinema.WebApp.Controllers;

public class SessaoController : WebControllerBase
{
    private readonly FilmeService servicoFilme;
    private readonly SalaService servicoSala;
    private readonly SessaoService servicoSessao;
    private readonly IMapper mapeador;

    public SessaoController(
        FilmeService servicoFilme,
        SalaService servicoSala,
        SessaoService servicoSessao,
        IMapper mapeador
    )
    {
        this.servicoFilme = servicoFilme;
        this.servicoSala = servicoSala;
        this.servicoSessao = servicoSessao;
        this.mapeador = mapeador;
    }

    [Authorize(Roles = "Empresa")]
    public IActionResult Listar()
    {
        var resultado = 
            servicoSessao.ObterSessoesAgrupadasPorFilme(UsuarioId.GetValueOrDefault());

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());

            return RedirectToAction(nameof(Listar));
        }

        var agrupamentos = resultado.Value;

        var agrupamentosSessoesVm = agrupamentos
            .Select(MapearAgrupamentoSessoes);

        ViewBag.Mensagem = TempData.DesserializarMensagemViewModel();

        return View(agrupamentosSessoesVm);
    }

    [Authorize(Roles = "Empresa")]
    public IActionResult Inserir()
    {
        return View(CarregarInformacoes(new InserirSessaoViewModel()));
    }

    [HttpPost, Authorize(Roles = "Empresa")]
    public IActionResult Inserir(InserirSessaoViewModel inserirSessaoVm)
    {
        if (!ModelState.IsValid)
            return View(CarregarInformacoes(inserirSessaoVm));

        var sessao = mapeador.Map<Sessao>(inserirSessaoVm);

        sessao.UsuarioId = UsuarioId.GetValueOrDefault();

        var salaId = inserirSessaoVm.SalaId;
        var filmeId = inserirSessaoVm.FilmeId;

        var resultado = servicoSessao.Inserir(sessao, salaId, filmeId);

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());

            return View(CarregarInformacoes(inserirSessaoVm));
        }

        ApresentarMensagemSucesso($"O registro ID [{resultado.Value.Id}] foi inserido com sucesso!");

        return RedirectToAction(nameof(Listar));
    }

    [Authorize(Roles = "Empresa")]
    public IActionResult Encerrar(int id)
    {
        var resultado = servicoSessao.SelecionarPorId(id);

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());

            return RedirectToAction(nameof(Detalhes), new { id });
        }

        var sessao = resultado.Value;

        var detalhesSessaoViewModel = mapeador.Map<DetalhesSessaoViewModel>(sessao);

        return View(detalhesSessaoViewModel);
    }

    [HttpPost, Authorize(Roles = "Empresa")]
    public IActionResult Encerrar(DetalhesSessaoViewModel detalhesSessaoViewModel)
    {
        var resultado = servicoSessao.Encerrar(detalhesSessaoViewModel.Id);

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());

            return RedirectToAction(nameof(Listar));
        }

        ApresentarMensagemSucesso($"A sessão ID [{resultado.Value.Id}] foi encerrada com sucesso!");

        return RedirectToAction(nameof(Listar));
    }

    [Authorize(Roles = "Empresa")]
    public IActionResult Excluir(int id)
    {
        var resultado = servicoSessao.SelecionarPorId(id);

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());

            return RedirectToAction(nameof(Listar));
        }

        var sessao = resultado.Value;

        var detalhesSessaoViewModel = mapeador.Map<DetalhesSessaoViewModel>(sessao);

        return View(detalhesSessaoViewModel);
    }

    [HttpPost, Authorize(Roles = "Empresa")]
    public IActionResult Excluir(DetalhesSessaoViewModel detalhesSessaoViewModel)
    {
        var resultado = servicoSessao.Excluir(detalhesSessaoViewModel.Id);

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado);

            return RedirectToAction(nameof(Listar));
        }

        ApresentarMensagemSucesso($"A sessão ID [{detalhesSessaoViewModel.Id}] foi excluída com sucesso!");

        return RedirectToAction(nameof(Listar));
    }

    [Authorize(Roles = "Empresa,Cliente")]
    public IActionResult Detalhes(int id)
    {
        var resultado = servicoSessao.SelecionarPorId(id);

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());

            return RedirectToAction(nameof(Listar));
        }

        var sessao = resultado.Value;

        var detalhesSessaoViewModel = mapeador.Map<DetalhesSessaoViewModel>(sessao);

        return View(detalhesSessaoViewModel);
    }

    [Authorize(Roles = "Cliente")]
    [HttpGet, Route("/sessao/comprar-ingresso/{id:int}")]
    public IActionResult ComprarIngresso(int id)
    {
        var resultado = servicoSessao.SelecionarPorId(id);

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());

            return RedirectToAction(nameof(Listar));
        }

        var sessao = resultado.Value;

        var detalhesSessaoViewModel = mapeador.Map<DetalhesSessaoViewModel>(sessao);

        var comprarIngressoVm = new ComprarIngressoViewModel
        {
            Sessao = detalhesSessaoViewModel,
            Assentos = sessao.ObterAssentosDisponiveis()
                .Select(a =>
                    new SelectListItem(a.ToString(), a.ToString()))
        };

        return View(comprarIngressoVm);
    }

    [Authorize(Roles = "Cliente")]
    [HttpPost, Route("/sessao/comprar-ingresso/{id:int}")]
    public IActionResult ComprarIngresso(int id, ComprarIngressoViewModel comprarIngressoVm)
    {
        var resultado = servicoSessao.ComprarIngresso(
            id,
            comprarIngressoVm.AssentoSelecionado,
            comprarIngressoVm.MeiaEntrada,
            UsuarioId.GetValueOrDefault()
        );

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());

            return RedirectToAction(nameof(Detalhes), new { id });
        }

        ApresentarMensagemSucesso($"O ingresso para a sessão ID [{resultado.Value.Id}] foi gerado com sucesso!");

        return RedirectToAction("Index", "Inicio");
    }

    private InserirSessaoViewModel? CarregarInformacoes(InserirSessaoViewModel inserirSessaoVm)
    {
        var resultadoFilmes = servicoFilme.SelecionarTodos(UsuarioId.GetValueOrDefault());
        var resultadoSalas = servicoSala.SelecionarTodos(UsuarioId.GetValueOrDefault());

        if (resultadoFilmes.IsFailed || resultadoSalas.IsFailed)
        {
            ApresentarMensagemFalha(Result.Fail("Falha ao encontrar dados necessários!"));

            return null;
        }

        var salas = resultadoSalas.Value;
        var filmes = resultadoFilmes.Value;

        inserirSessaoVm.Salas = salas.Select(s =>
            new SelectListItem(s.Numero.ToString(), s.Id.ToString()));

        inserirSessaoVm.Filmes = filmes.Select(f =>
            new SelectListItem(f.Titulo, f.Id.ToString()));

        return inserirSessaoVm;
    }

    private AgrupamentoSessoesPorFilmeViewModel MapearAgrupamentoSessoes(IGrouping<string, Sessao> grp)
    {
        return new AgrupamentoSessoesPorFilmeViewModel
        {
            Filme = grp.Key,
            Sessoes = mapeador.Map<IEnumerable<ListarSessaoViewModel>>(grp)
                .OrderBy(s => s.Encerrada)
                .ThenBy(s => s.Inicio)
        };
    }
}