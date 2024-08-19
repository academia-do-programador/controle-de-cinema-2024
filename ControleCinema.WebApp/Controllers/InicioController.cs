using AutoMapper;
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
    private readonly IMapper mapeador;

    public InicioController(
        SessaoService servicoSessao,
        FilmeService servicoFilme,
        GeneroService servicoGenero,
        SalaService servicoSala,
        IMapper mapeador
    )
    {
        this.servicoSessao = servicoSessao;
        this.servicoFilme = servicoFilme;
        this.servicoGenero = servicoGenero;
        this.servicoSala = servicoSala;
        this.mapeador = mapeador;
    }

    public ViewResult Index()
    {
        var resultadoAgrupamentos = 
            servicoSessao.ObterSessoesAgrupadasPorFilme();

        var agrupamentos = resultadoAgrupamentos.Value;

        var agrupamentosSessoesVm = 
            agrupamentos.Select(MapearAgrupamentoSessoes);

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

    private AgrupamentoSessoesPorFilmeViewModel MapearAgrupamentoSessoes(IGrouping<string, Sessao> agrupamento)
    {
        return new AgrupamentoSessoesPorFilmeViewModel
        {
            Filme = agrupamento.Key,
            Sessoes = mapeador.Map<IEnumerable<ListarSessaoViewModel>>(agrupamento)
            .OrderBy(s => s.Inicio)
        };
    }
}