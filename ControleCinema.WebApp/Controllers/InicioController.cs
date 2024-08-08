using ControleCinema.Dominio.ModuloSessao;
using ControleCinema.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace ControleCinema.WebApp.Controllers;

public class InicioController : Controller
{
    private readonly IRepositorioSessao repositorioSessao;

    public InicioController(IRepositorioSessao repositorioSessao)
    {
        this.repositorioSessao = repositorioSessao;
    }

    public ViewResult Index()
    {
        var agrupamentos = repositorioSessao.ObterSessoesDisponiveisAgrupadas();

        var agrupamentosSessoesVm = agrupamentos
            .Select(MapearAgrupamentoSessoes);

        ViewBag.Agrupamentos = agrupamentosSessoesVm;

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