using ControleCinema.Dominio.Compartilhado;
using ControleCinema.Dominio.ModuloFuncionario;
using ControleCinema.WebApp.Extensions;
using ControleCinema.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace ControleCinema.WebApp.Controllers;

public class FuncionarioController : Controller
{
    private readonly IRepositorio<Funcionario> repositorioFuncionario;

    public FuncionarioController(IRepositorio<Funcionario> repositorioFuncionario)
    {
        this.repositorioFuncionario = repositorioFuncionario;
    }

    public IActionResult Listar()
    {
        var funcionarios = repositorioFuncionario.SelecionarTodos();

        var listarFuncionariosVm = funcionarios
            .Select(f => new ListarFuncionarioViewModel
            {
                Id = f.Id,
                Nome = f.Nome
            });

        ViewBag.Mensagem = TempData.DesserializarMensagemViewModel();

        return View(listarFuncionariosVm);
    }

    public IActionResult Inserir()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult Inserir(InserirFuncionarioViewModel inserirFuncionarioVm)
    {
        if (!ModelState.IsValid)
            return View(inserirFuncionarioVm);

        var funcionario = new Funcionario()
        {
            Nome = inserirFuncionarioVm.Nome,
            Login = inserirFuncionarioVm.Login,
            Senha = inserirFuncionarioVm.Senha,
        };
        
        repositorioFuncionario.Inserir(funcionario);

        TempData.SerializarMensagemViewModel(new MensagemViewModel
        {
            Titulo = "Sucesso",
            Mensagem = $"O registro ID [{funcionario.Id}] foi inserido com sucesso!"
        });
        
        return RedirectToAction(nameof(Listar));
    }
    
    public IActionResult Editar(int id)
    {
        var funcionario = repositorioFuncionario.SelecionarPorId(id);

        if (funcionario is null)
            return MensagemRegistroNaoEncontrado(id);
        
        var editarFuncionarioVm = new EditarFuncionarioViewModel
        {
            Id = id,
            Nome = funcionario.Nome
        };
        
        return View(editarFuncionarioVm);
    }
    
    [HttpPost]
    public IActionResult Editar(EditarFuncionarioViewModel editarFuncionarioVm)
    {
        if (!ModelState.IsValid)
            return View(editarFuncionarioVm);

        var funcionario = repositorioFuncionario.SelecionarPorId(editarFuncionarioVm.Id);

        funcionario!.Nome = editarFuncionarioVm.Nome;
        
        repositorioFuncionario.Editar(funcionario);

        TempData.SerializarMensagemViewModel(new MensagemViewModel
        {
            Titulo = "Sucesso",
            Mensagem = $"O registro ID [{funcionario.Id}] foi editado com sucesso!"
        });
        
        return RedirectToAction(nameof(Listar));
    }
    
    public IActionResult Excluir(int id)
    {
        var funcionario = repositorioFuncionario.SelecionarPorId(id);

        if (funcionario is null)
            return MensagemRegistroNaoEncontrado(id);
        
        var detalhesFuncionarioViewModel = new DetalhesFuncionarioViewModel
        {
            Id = id,
            Nome = funcionario.Nome,
            Login = funcionario.Login
        };
        
        return View(detalhesFuncionarioViewModel);
    }
    
    [HttpPost]
    public IActionResult Excluir(DetalhesFuncionarioViewModel detalhesFuncionarioViewModel)
    {
        var funcionario = repositorioFuncionario.SelecionarPorId(detalhesFuncionarioViewModel.Id);

        if (funcionario is null)
            return MensagemRegistroNaoEncontrado(detalhesFuncionarioViewModel.Id);
        
        repositorioFuncionario.Excluir(funcionario);

        TempData.SerializarMensagemViewModel(new MensagemViewModel
        {
            Titulo = "Sucesso",
            Mensagem = $"O registro ID [{funcionario.Id}] foi excluído com sucesso!"
        });
        
        return RedirectToAction(nameof(Listar));
    }
    
    public IActionResult Detalhes(int id)
    {
        var funcionario = repositorioFuncionario.SelecionarPorId(id);

        if (funcionario is null)
            return MensagemRegistroNaoEncontrado(id);
        
        var detalhesFuncionarioViewModel = new DetalhesFuncionarioViewModel
        {
            Id = id,
            Nome = funcionario.Nome,
            Login = funcionario.Login
        };

        return View(detalhesFuncionarioViewModel);
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
}