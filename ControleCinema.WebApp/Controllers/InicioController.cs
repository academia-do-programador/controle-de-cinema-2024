using ControleCinema.Aplicacao.Servicos;
using ControleCinema.Dominio.ModuloSessao;
using ControleCinema.WebApp.Extensions;
using ControleCinema.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace ControleCinema.WebApp.Controllers;

public class InicioController : WebControllerBase
{
    private readonly SessaoService servicoSessao;
    private readonly FilmeService servicoFilme;
    private readonly GeneroService servicoGenero;
    private readonly SalaService servicoSala;

    public InicioController(
        SessaoService servicoSessao,
        FilmeService servicoFilme,
        GeneroService servicoGenero,
        SalaService servicoSala
    )
    {
        this.servicoSessao = servicoSessao;
        this.servicoFilme = servicoFilme;
        this.servicoGenero = servicoGenero;
        this.servicoSala = servicoSala;
    }

    public ViewResult Index()
    {
        var resultadoAgrupamentos = servicoSessao.ObterSessoesAgrupadasPorFilme();

        var agrupamentos = resultadoAgrupamentos.Value;
        
        var agrupamentosSessoesVm = agrupamentos.Select(MapearAgrupamentoSessoes);

        ViewBag.Agrupamentos = agrupamentosSessoesVm;

        if (UsuarioId.HasValue)
        {
            ViewBag.QuantidadeFilmes = servicoFilme.SelecionarTodos(UsuarioId.Value).Value.Count;
            ViewBag.QuantidadeGeneros = servicoGenero.SelecionarTodos(UsuarioId.Value).Value.Count;
            ViewBag.QuantidadeSalas = servicoSala.SelecionarTodos(UsuarioId.Value).Value.Count;
            ViewBag.QuantidadeSessoes = servicoSessao.SelecionarTodos(UsuarioId.Value).Value.Count;
            ViewBag.QuantidadeIngressos = servicoSessao.SelecionarTodosIngressos(UsuarioId.Value).Value.Count;
        }

        ViewBag.Mensagem = TempData.DesserializarMensagemViewModel();

        return View();
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
            .OrderBy(s => s.Inicio)
        };
    }
}