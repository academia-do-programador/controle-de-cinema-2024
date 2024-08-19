using ControleCinema.Aplicacao.Servicos;
using ControleCinema.WebApp.Extensions;
using ControleCinema.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControleCinema.WebApp.Controllers;

[Authorize(Roles = "Empresa")]
public class SalaController : WebControllerBase
{
    private readonly SalaService servicoSala;

    public SalaController(SalaService servicoSala)
    {
        this.servicoSala = servicoSala;
    }

    public IActionResult Listar()
    {
        var resultado = servicoSala
            .SelecionarTodos(UsuarioId.GetValueOrDefault());

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());

            return RedirectToAction("Index", "Inicio");
        }

        var salas = resultado.Value;
        
        var listarSalasVm = salas
            .Select(f => new ListarSalaViewModel
            {
                Id = f.Id,
                Numero = f.Numero,
                Capacidade = f.Capacidade
            });

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

        var resultado = servicoSala.Inserir(
            inserirSalaVm.Numero,
            inserirSalaVm.Capacidade,
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
        var resultado = servicoSala.SelecionarPorId(id);

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());

            return RedirectToAction(nameof(Listar));
        }

        var sala = resultado.Value;

        var editarSalaVm = new EditarSalaViewModel
        {
            Id = id,
            Numero = sala.Numero,
            Capacidade = sala.Capacidade
        };

        return View(editarSalaVm);
    }

    [HttpPost]
    public IActionResult Editar(EditarSalaViewModel editarSalaVm)
    {
        if (!ModelState.IsValid)
            return View(editarSalaVm);

        var resultado = servicoSala.Editar(
            editarSalaVm.Id,
            editarSalaVm.Numero,
            editarSalaVm.Capacidade
        );
        
        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());

            return RedirectToAction(nameof(Listar));
        }
        
        var sala = resultado.Value;

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

        var detalhesSalaViewModel = new DetalhesSalaViewModel
        {
            Id = id,
            Numero = sala.Numero,
            Capacidade = sala.Capacidade
        };

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

        var detalhesSalaViewModel = new DetalhesSalaViewModel
        {
            Id = id,
            Numero = sala.Numero,
            Capacidade = sala.Capacidade
        };

        return View(detalhesSalaViewModel);
    }
}