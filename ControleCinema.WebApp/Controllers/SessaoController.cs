using ControleCinema.Dominio.ModuloFilme;
using ControleCinema.Dominio.ModuloSala;
using ControleCinema.Dominio.ModuloSessao;
using ControleCinema.WebApp.Extensions;
using ControleCinema.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ControleCinema.WebApp.Controllers;

public class SessaoController : Controller
{
    private readonly IRepositorioSessao repositorioSessao;
    private readonly IRepositorioSala repositorioSala;
    private readonly IRepositorioFilme repositorioFilme;

    public SessaoController(
        IRepositorioSessao repositorioSessao,
        IRepositorioSala repositorioSala,
        IRepositorioFilme repositorioFilme
    )
    {
        this.repositorioSessao = repositorioSessao;
        this.repositorioSala = repositorioSala;
        this.repositorioFilme = repositorioFilme;
    }

    public IActionResult Listar()
    {
        var agrupamentos =
            repositorioSessao.ObterSessoesAgrupadasPorFilme();

        var agrupamentosSessoesVm = agrupamentos
            .Select(MapearAgrupamentoSessoes);

        ViewBag.Mensagem = TempData.DesserializarMensagemViewModel();

        return View(agrupamentosSessoesVm);
    }


    public IActionResult Inserir()
    {
        var salas = repositorioSala.SelecionarTodos();
        var filmes = repositorioFilme.SelecionarTodos();

        var inserirSessaoVm = new InserirSessaoViewModel
        {
            Salas = salas.Select(s =>
                new SelectListItem(s.Numero.ToString(), s.Id.ToString())),
            Filmes = filmes.Select(f =>
                new SelectListItem(f.Titulo, f.Id.ToString())),
        };

        return View(inserirSessaoVm);
    }

    [HttpPost]
    public IActionResult Inserir(InserirSessaoViewModel inserirSessaoVm)
    {
        if (!ModelState.IsValid)
        {
            var filmes = repositorioFilme.SelecionarTodos();
            var salas = repositorioSala.SelecionarTodos();

            inserirSessaoVm.Salas = salas.Select(s =>
                new SelectListItem(s.Numero.ToString(), s.Id.ToString()));

            inserirSessaoVm.Filmes = filmes.Select(f =>
                new SelectListItem(f.Titulo, f.Id.ToString()));

            return View(inserirSessaoVm);
        }

        var salaSelecionada = repositorioSala
            .SelecionarPorId(inserirSessaoVm.SalaId);

        var filmeSelecionado =
            repositorioFilme.SelecionarPorId(inserirSessaoVm.FilmeId);

        var sessao = new Sessao()
        {
            Sala = salaSelecionada!,
            Filme = filmeSelecionado!,
            Inicio = inserirSessaoVm.Inicio,
            NumeroMaximoIngressos = inserirSessaoVm.NumeroMaximoIngressos,
        };

        repositorioSessao.Inserir(sessao);

        TempData.SerializarMensagemViewModel(new MensagemViewModel
        {
            Titulo = "Sucesso",
            Mensagem = $"O registro ID [{sessao.Id}] foi inserido com sucesso!"
        });

        return RedirectToAction(nameof(Listar));
    }

    public IActionResult Encerrar(int id)
    {
        var sessao = repositorioSessao.SelecionarPorId(id);

        if (sessao is null)
            return MensagemRegistroNaoEncontrado(id);

        var detalhesSessaoViewModel = MapearDetalhesSessao(sessao);

        return View(detalhesSessaoViewModel);
    }

    [HttpPost]
    public IActionResult Encerrar(DetalhesSessaoViewModel detalhesSessaoViewModel)
    {
        var sessao = repositorioSessao.SelecionarPorId(detalhesSessaoViewModel.Id);

        if (sessao is null)
            return MensagemRegistroNaoEncontrado(detalhesSessaoViewModel.Id);

        sessao.Encerrar();

        repositorioSessao.Editar(sessao);

        TempData.SerializarMensagemViewModel(new MensagemViewModel
        {
            Titulo = "Sucesso",
            Mensagem = $"A sessão ID [{sessao.Id}] foi encerrada com sucesso!",
        });

        return RedirectToAction(nameof(Listar));
    }

    public IActionResult Excluir(int id)
    {
        var sessao = repositorioSessao.SelecionarPorId(id);

        if (sessao is null)
            return MensagemRegistroNaoEncontrado(id);

        var detalhesSessaoVm = MapearDetalhesSessao(sessao);

        return View(detalhesSessaoVm);
    }

    [HttpPost]
    public IActionResult Excluir(DetalhesSessaoViewModel detalhesSessaoVm)
    {
        var sessao = repositorioSessao.SelecionarPorId(detalhesSessaoVm.Id);

        if (sessao is null)
            return MensagemRegistroNaoEncontrado(detalhesSessaoVm.Id);

        repositorioSessao.Excluir(sessao);

        TempData.SerializarMensagemViewModel(new MensagemViewModel
        {
            Titulo = "Sucesso",
            Mensagem = $"O registro ID [{sessao.Id}] foi excluído com sucesso!",
        });

        return RedirectToAction(nameof(Listar));
    }

    public IActionResult Detalhes(int id)
    {
        var sessao = repositorioSessao.SelecionarPorId(id);

        if (sessao is null)
            return MensagemRegistroNaoEncontrado(id);

        var detalhesSessaoVm = MapearDetalhesSessao(sessao);

        return View(detalhesSessaoVm);
    }

    [HttpGet, Route("/sessao/comprar-ingresso/{sessaoId:int}")]
    public IActionResult ComprarIngresso(int sessaoId)
    {
        var sessao = repositorioSessao.SelecionarPorId(sessaoId);

        if (sessao is null)
            return MensagemRegistroNaoEncontrado(sessaoId);

        var detalhesSessaoVm = MapearDetalhesSessao(sessao);

        var comprarIngressoVm = new ComprarIngressoViewModel
        {
            Sessao = detalhesSessaoVm,
            Assentos = sessao.ObterAssentosDisponiveis()
                .Select(a => new SelectListItem(a.ToString(), a.ToString()))
        };

        return View(comprarIngressoVm);
    }

    [HttpPost, Route("/sessao/comprar-ingresso/{sessaoId:int}")]
    public IActionResult ComprarIngresso(int sessaoId, ComprarIngressoViewModel comprarIngressoVm)
    {
        var sessao = repositorioSessao.SelecionarPorId(sessaoId);

        if (sessao is null)
            return MensagemRegistroNaoEncontrado(sessaoId);

        var novoIngresso = sessao.GerarIngresso(
            comprarIngressoVm.AssentoSelecionado,
            comprarIngressoVm.MeiaEntrada
        );

        repositorioSessao.Editar(sessao);

        TempData.SerializarMensagemViewModel(new MensagemViewModel
        {
            Titulo = "Sucesso",
            Mensagem = $"O ingresso ID [{novoIngresso.Id}] foi gerado com sucesso. Obrigado por sua compra!",
        });

        return RedirectToAction(nameof(Listar));
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
            IngressosDisponiveis = sessao.ObterQuantidadeIngressosDisponiveis(),
        };
    }

    private static AgrupamentoSessoesPorFilmeViewModel MapearAgrupamentoSessoes(IGrouping<string, Sessao> agrupamento)
    {
        return new AgrupamentoSessoesPorFilmeViewModel
        {
            Filme = agrupamento.Key,
            Sessoes = agrupamento.Select(s => new ListarSessaoViewModel
            {
                Id = s.Id,
                Filme = agrupamento.Key,
                Sala = s.Sala.Numero.ToString(),
                IngressosDisponiveis = s.ObterQuantidadeIngressosDisponiveis(),
                Inicio = s.Inicio.ToString("dd/MM/yyyy HH:mm"),
                Encerrada = s.Encerrada ? "Encerrada" : "Disponível"
            })
                .OrderBy(s => s.Encerrada)
                .ThenBy(s => s.Inicio)
        };
    }
}