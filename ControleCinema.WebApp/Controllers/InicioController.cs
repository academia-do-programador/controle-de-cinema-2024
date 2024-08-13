using ControleCinema.Dominio.ModuloFilme;
using ControleCinema.Dominio.ModuloGenero;
using ControleCinema.Dominio.ModuloSala;
using ControleCinema.Dominio.ModuloSessao;
using ControleCinema.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace ControleCinema.WebApp.Controllers;

public class InicioController : Controller
{
    private readonly IRepositorioSessao repositorioSessao;
    private readonly IRepositorioFilme repositorioFilme;
    private readonly IRepositorioGenero repositorioGenero;
    private readonly IRepositorioSala repositorioSala;

    public InicioController(
        IRepositorioSessao repositorioSessao,
        IRepositorioFilme repositorioFilme,
        IRepositorioGenero repositorioGenero,
        IRepositorioSala repositorioSala
    )
    {
        this.repositorioSessao = repositorioSessao;
        this.repositorioFilme = repositorioFilme;
        this.repositorioGenero = repositorioGenero;
        this.repositorioSala = repositorioSala;
    }

    public ViewResult Index()
    {
        var agrupamentos = repositorioSessao.ObterSessoesAgrupadasPorFilme();

        var agrupamentosSessoesVm = agrupamentos
            .Select(MapearAgrupamentoSessoes);

        ViewBag.Agrupamentos = agrupamentosSessoesVm;

        ViewBag.QuantidadeFilmes = repositorioFilme.SelecionarTodos().Count;
        ViewBag.QuantidadeGeneros = repositorioGenero.SelecionarTodos(.Count;
        ViewBag.QuantidadeSalas = repositorioSala.SelecionarTodos().Count;
        ViewBag.QuantidadeSessoes = repositorioSessao.SelecionarTodos().Count;
        ViewBag.QuantidadeIngressos = repositorioSessao.SelecionarTodosIngressos().Count;

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