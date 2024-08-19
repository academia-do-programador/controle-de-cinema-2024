using AutoMapper;
using ControleCinema.Aplicacao.Servicos;
using ControleCinema.Dominio.ModuloSala;
using ControleCinema.WebApp.Extensions;
using ControleCinema.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControleCinema.WebApp.Controllers;

[Authorize(Roles = "Empresa")]
public class SalaController : WebControllerBase
{
    private readonly SalaService servicoSala;
    private readonly IMapper mapeador;

    public SalaController(SalaService servicoSala, IMapper mapeador)
    {
        this.servicoSala = servicoSala;
        this.mapeador = mapeador;
    }

    public IActionResult Listar()
    {
        var resultado = 
            servicoSala.SelecionarTodos(UsuarioId.GetValueOrDefault());

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());

            return RedirectToAction("Index", "Inicio");
        }

        var salas = resultado.Value;

        var listarSalasVm = mapeador.Map<IEnumerable<ListarSalaViewModel>>(salas);

        ViewBag.Mensagem = TempData.DesserializarMensagemViewModel();

        return View(listarSalasVm);
    }

    public IActionResult Inserir()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Inserir(InserirSalaViewModel inserirSalaVm)
    {
        if (!ModelState.IsValid)
            return View(inserirSalaVm);

        var novaSala = mapeador.Map<Sala>(inserirSalaVm);

        novaSala.UsuarioId = UsuarioId.GetValueOrDefault();

        var resultado = servicoSala.Inserir(novaSala);

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());

            return RedirectToAction(nameof(Listar));
        }

        ApresentarMensagemSucesso($"O registro ID [{novaSala.Id}] foi inserido com sucesso!");

        return RedirectToAction(nameof(Listar));
    }

    public IActionResult Editar(int id)
    {
        var resultado = servicoSala.SelecionarPorId(id);

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());

            return RedirectToAction(nameof(Listar));
        }

        var sala = resultado.Value;

        var editarSalaVm = mapeador.Map<EditarSalaViewModel>(sala);

        return View(editarSalaVm);
    }

    [HttpPost]
    public IActionResult Editar(EditarSalaViewModel editarSalaVm)
    {
        if (!ModelState.IsValid)
            return View(editarSalaVm);

        var sala = mapeador.Map<Sala>(editarSalaVm);

        var resultado = servicoSala.Editar(sala);

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());

            return RedirectToAction(nameof(Listar));
        }

        ApresentarMensagemSucesso($"O registro ID [{sala.Id}] foi editado com sucesso!");

        return RedirectToAction(nameof(Listar));
    }

    public IActionResult Excluir(int id)
    {
        var resultado = servicoSala.SelecionarPorId(id);

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());

            return RedirectToAction(nameof(Listar));
        }

        var sala = resultado.Value;

        var detalhesSalaViewModel = mapeador.Map<DetalhesSalaViewModel>(sala);

        return View(detalhesSalaViewModel);
    }

    [HttpPost]
    public IActionResult Excluir(DetalhesSalaViewModel detalhesSalaViewModel)
    {
        var resultado = servicoSala.Excluir(detalhesSalaViewModel.Id);

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado);

            return RedirectToAction(nameof(Listar));
        }

        ApresentarMensagemSucesso($"O registro ID [{detalhesSalaViewModel.Id}] foi excluído com sucesso!");

        return RedirectToAction(nameof(Listar));
    }

    public IActionResult Detalhes(int id)
    {
        var resultado = servicoSala.SelecionarPorId(id);

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());

            return RedirectToAction(nameof(Listar));
        }

        var sala = resultado.Value;

        var detalhesSalaViewModel = mapeador.Map<DetalhesSalaViewModel>(sala);

        return View(detalhesSalaViewModel);
    }
}