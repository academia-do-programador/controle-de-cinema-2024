using ControleCinema.Dominio.ModuloSala;
using ControleCinema.WebApp.Extensions;
using ControleCinema.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace ControleCinema.WebApp.Controllers;

public class SalaController : Controller
{
    private readonly IRepositorioSala repositorioSala;

    public SalaController(IRepositorioSala repositorioSala)
    {
        this.repositorioSala = repositorioSala;
    }

    public IActionResult Listar()
    {
        var salas = repositorioSala.SelecionarTodos();

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

        var sala = new Sala()
        {
            Numero = inserirSalaVm.Numero,
            Capacidade = inserirSalaVm.Capacidade
        };

        repositorioSala.Inserir(sala);

        TempData.SerializarMensagemViewModel(new MensagemViewModel
        {
            Titulo = "Sucesso",
            Mensagem = $"O registro ID [{sala.Id}] foi inserido com sucesso!"
        });

        return RedirectToAction(nameof(Listar));
    }

    public IActionResult Editar(int id)
    {
        var sala = repositorioSala.SelecionarPorId(id);

        if (sala is null)
            return MensagemRegistroNaoEncontrado(id);

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

        var sala = repositorioSala.SelecionarPorId(editarSalaVm.Id);

        sala!.Numero = editarSalaVm.Numero;
        sala!.Capacidade = editarSalaVm.Capacidade;

        repositorioSala.Editar(sala);

        TempData.SerializarMensagemViewModel(new MensagemViewModel
        {
            Titulo = "Sucesso",
            Mensagem = $"O registro ID [{sala.Id}] foi editado com sucesso!"
        });

        return RedirectToAction(nameof(Listar));
    }

    public IActionResult Excluir(int id)
    {
        var sala = repositorioSala.SelecionarPorId(id);

        if (sala is null)
            return MensagemRegistroNaoEncontrado(id);

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
        var sala = repositorioSala.SelecionarPorId(detalhesSalaViewModel.Id);

        if (sala is null)
            return MensagemRegistroNaoEncontrado(detalhesSalaViewModel.Id);

        repositorioSala.Excluir(sala);

        TempData.SerializarMensagemViewModel(new MensagemViewModel
        {
            Titulo = "Sucesso",
            Mensagem = $"O registro ID [{sala.Id}] foi excluído com sucesso!"
        });

        return RedirectToAction(nameof(Listar));
    }

    public IActionResult Detalhes(int id)
    {
        var sala = repositorioSala.SelecionarPorId(id);

        if (sala is null)
            return MensagemRegistroNaoEncontrado(id);

        var detalhesSalaViewModel = new DetalhesSalaViewModel
        {
            Id = id,
            Numero = sala.Numero,
            Capacidade = sala.Capacidade
        };

        return View(detalhesSalaViewModel);
    }

    private IActionResult MensagemRegistroNaoEncontrado(int idRegistro)
    {
        TempData.SerializarMensagemViewModel(new MensagemViewModel
        {
            Titulo = "Erro",
            Mensagem = $"Não foi possível encontrar o registro ID [{idRegistro}]!"
        });

        return RedirectToAction(nameof(Listar));
    }
}