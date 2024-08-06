﻿using ControleCinema.Dominio.ModuloSessao;
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
        var agrupamentos = repositorioSessao
            .ObterSessoesAgrupadasPorFilme();
        
        var agrupamentosSessoesVm = agrupamentos
            .Select(grp => new AgrupamentoSessoesPorFilmeViewModel
            {
                Filme = grp.Key,
                Sessoes = grp.Where(s => !s.Encerrada).Select(s => new ListarSessaoViewModel
                {
                    Id = s.Id,
                    Filme = grp.Key,
                    Sala = s.Sala.Numero.ToString(),
                    IngressosDisponiveis = s.ObterQuantidadeIngressosDisponiveis(),
                    Inicio = s.Inicio.ToString("dd/MM/yyyy HH:mm"),
                    Encerrada = s.Encerrada ? "Encerrada" : "Disponível"
                })               
                .OrderBy(s => s.Inicio)
            });

        ViewBag.Agrupamentos = agrupamentosSessoesVm;
        
        return View();
    }
}