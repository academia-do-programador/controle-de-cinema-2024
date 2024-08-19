using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ControleCinema.WebApp.Models;

public abstract class FormFilmeViewModel
{
    [Required(ErrorMessage = "O nome é obrigatório")]
    [MinLength(6, ErrorMessage = "O nome deve conter ao menos 6 caracteres")]
    public string Titulo { get; set; }

    [Range(0, 1000, ErrorMessage = "A duração não pode ser menor que zero e maior que 1000 minutos")]
    public int Duracao { get; set; }

    public bool Lancamento { get; set; }

    [Required(ErrorMessage = "O gênero do filme é obrigatório")]
    public int GeneroId { get; set; }
    
    public IEnumerable<SelectListItem>? Generos { get; set; }
}

public class InserirFilmeViewModel : FormFilmeViewModel
{

}

public class EditarFilmeViewModel : FormFilmeViewModel
{
    public int Id { get; set; }
}

public class ListarFilmeViewModel
{
    public int Id { get; set; }
    public string Titulo { get; set; }
    public string Duracao { get; set; }
    public string Lancamento { get; set; }
    public string Genero { get; set; }
}

public class DetalhesFilmeViewModel
{
    public int Id { get; set; }
    public string Titulo { get; set; }
    public string Duracao { get; set; }
    public string Lancamento { get; set; }
    public string Genero { get; set; }
}