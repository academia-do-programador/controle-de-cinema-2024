using ControleCinema.Aplicacao.Services;
using ControleCinema.Dominio.ModuloFilme;
using ControleCinema.Dominio.ModuloSala;
using ControleCinema.Dominio.ModuloSessao;
using ControleCinema.WebApp.Extensions;
using ControleCinema.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ControleCinema.WebApp.Controllers;

public class SessaoController : WebControllerBase
{
    private readonly IRepositorioSala repositorioSala;
    private readonly IRepositorioFilme repositorioFilme;
    private readonly IRepositorioSessao repositorioSessao;
    private readonly SessaoService servicoSessao;

    public SessaoController(
        IRepositorioFilme repositorioFilme,
        IRepositorioSala repositorioSala,
        IRepositorioSessao repositorioSessao,
        SessaoService servicoSessao
    )
    {
        this.repositorioFilme = repositorioFilme;
        this.repositorioSessao = repositorioSessao;
        this.servicoSessao = servicoSessao;
        this.repositorioSala = repositorioSala;
    }

    [Authorize(Roles = "Empresa")]
    public IActionResult Listar()
    {
        var agrupamentos = repositorioSessao
            .ObterSessoesAgrupadasPorFilme(UsuarioId.GetValueOrDefault());

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

        var resultado = servicoSessao.Inserir(
            inserirSessaoVm.Inicio,
            inserirSessaoVm.NumeroMaximoIngressos,
            inserirSessaoVm.SalaId,
            inserirSessaoVm.FilmeId,
            UsuarioId.GetValueOrDefault()
        );

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());

            return View(CarregarInformacoes(inserirSessaoVm));
        }

        ApresentarMensagemSucesso(
            $"O registro ID [{resultado.Value.Id}] foi inserido com sucesso!"
        );

        return RedirectToAction(nameof(Listar));
    }

    [Authorize(Roles = "Empresa")]
    public IActionResult Encerrar(int id)
    {
        var sessao = repositorioSessao.SelecionarPorId(id);

        if (sessao is null)
            return MensagemRegistroNaoEncontrado(id);

        var detalhesSessaoViewModel = MapearDetalhesSessao(sessao);

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
        var sessao = repositorioSessao.SelecionarPorId(id);

        if (sessao is null)
            return MensagemRegistroNaoEncontrado(id);

        var detalhesSessaoViewModel = MapearDetalhesSessao(sessao);

        return View(detalhesSessaoViewModel);
    }

    [HttpPost, Authorize(Roles = "Empresa")]
    public IActionResult Excluir(DetalhesSessaoViewModel detalhesSessaoViewModel)
    {
        var resultado = servicoSessao.Excluir(detalhesSessaoViewModel.Id);

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());

            return RedirectToAction(nameof(Listar));
        }

        ApresentarMensagemSucesso($"A sessão ID [{resultado.Value.Id}] foi excluída com sucesso!");

        return RedirectToAction(nameof(Listar));
    }

    [Authorize(Roles = "Empresa,Cliente")]
    public IActionResult Detalhes(int id)
    {
        var sessao = repositorioSessao.SelecionarPorId(id);

        if (sessao is null)
            return MensagemRegistroNaoEncontrado(id);

        var detalhesSessaoViewModel = MapearDetalhesSessao(sessao);

        return View(detalhesSessaoViewModel);
    }

    [Authorize(Roles = "Cliente")]
    [HttpGet, Route("/sessao/comprar-ingresso/{id:int}")]
    public IActionResult ComprarIngresso(int id)
    {
        var sessao = repositorioSessao.SelecionarPorId(id);

        if (sessao is null)
            return MensagemRegistroNaoEncontrado(id);

        var detalhesSessaoViewModel = MapearDetalhesSessao(sessao);

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

    private InserirSessaoViewModel CarregarInformacoes(InserirSessaoViewModel inserirSessaoVm)
    {
        var filmes = repositorioFilme.SelecionarTodos();
        var salas = repositorioSala.SelecionarTodos();

        inserirSessaoVm.Salas = salas.Select(s =>
            new SelectListItem(s.Numero.ToString(), s.Id.ToString()));

        inserirSessaoVm.Filmes = filmes.Select(f =>
            new SelectListItem(f.Titulo, f.Id.ToString()));

        return inserirSessaoVm;
    }

    private static AgrupamentoSessoesPorFilmeViewModel MapearAgrupamentoSessoes(IGrouping<string, Sessao> grp)
    {
        return new AgrupamentoSessoesPorFilmeViewModel
        {
            Filme = grp.Key,
            Sessoes = grp.Select(s => new ListarSessaoViewModel
            {
                Id = s.Id,
                Filme = grp.Key,
                Sala = s.Sala.Numero.ToString(),
                IngressosDisponiveis = s.ObterQuantidadeIngressosDisponiveis(),
                Inicio = s.Inicio.ToString("dd/MM/yyyy HH:mm"),
                Encerrada = s.Encerrada ? "Encerrada" : "Disponível"
            })
                .OrderBy(s => s.Encerrada)
                .ThenBy(s => s.Inicio)
        };
    }

    private static DetalhesSessaoViewModel MapearDetalhesSessao(Sessao sessao)
    {
        return new DetalhesSessaoViewModel
        {
            Id = sessao.Id,
            Sala = sessao.Sala.Numero.ToString(),
            Filme = sessao.Filme.Titulo,
            Inicio = sessao.Inicio.ToString("dd/MM/yyyy HH:mm"),
            Encerrada = sessao.Encerrada ? "Encerrada" : "Disponível",
            NumeroMaximoIngressos = sessao.NumeroMaximoIngressos,
            IngressosDisponiveis = sessao.ObterQuantidadeIngressosDisponiveis()
        };
    }
}