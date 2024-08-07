using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ControleCinema.WebApp.Models;

public class InserirSessaoViewModel
{
    [Range(0, 1000, ErrorMessage = "O número máximo não pode ser menor que 0 e maior que 1000 minutos")]
    public int NumeroMaximoIngressos { get; set; }

    [Required(ErrorMessage = "A data de início é obrigatória")]
    [DataType(DataType.Date)]
    public DateTime Inicio { get; set; }

    [Required(ErrorMessage = "O filme é obrigatório")]
    public int FilmeId { get; set; }

    [Required(ErrorMessage = "A sala é obrigatória")]
    public int SalaId { get; set; }

    public IEnumerable<SelectListItem>? Salas { get; set; }
    public IEnumerable<SelectListItem>? Filmes { get; set; }
}

public class EditarSessaoViewModel
{
    public int Id { get; set; }

    [Range(0, 1000, ErrorMessage = "O número máximo não pode ser menor que 0 e maior que 1000 minutos")]
    public int NumeroMaximoIngressos { get; set; }

    [Required(ErrorMessage = "A data de início é obrigatória")]
    [DataType(DataType.DateTime)]
    public DateTime Inicio { get; set; }

    [Required(ErrorMessage = "O filme é obrigatório")]
    public int FilmeId { get; set; }

    [Required(ErrorMessage = "A sala é obrigatória")]
    public int SalaId { get; set; }

    public IEnumerable<SelectListItem>? Salas { get; set; }
    public IEnumerable<SelectListItem>? Filmes { get; set; }
}

public class ListarSessaoViewModel
{
    public int Id { get; set; }
    public string Sala { get; set; }
    public string Filme { get; set; }
    public string Inicio { get; set; }
    public string Encerrada { get; set; }
    public int IngressosDisponiveis { get; set; }
}

public class AgrupamentoSessoesPorFilmeViewModel
{
    public string Filme { get; set; }
    public IEnumerable<ListarSessaoViewModel> Sessoes { get; set; }
}

public class DetalhesSessaoViewModel
{
    public int Id { get; set; }
    public string Sala { get; set; }
    public string Filme { get; set; }
    public string Inicio { get; set; }
    public string Encerrada { get; set; }
    public int NumeroMaximoIngressos { get; set; }
    public int IngressosDisponiveis { get; set; }
}

public class ComprarIngressoViewModel
{
    public DetalhesSessaoViewModel Sessao { get; set; }

    public bool MeiaEntrada { get; set; }

    [Required(ErrorMessage = "É obrigatório selecionar um assento!")]
    public int AssentoSelecionado { get; set; }

    public IEnumerable<SelectListItem>? Assentos { get; set; }
}