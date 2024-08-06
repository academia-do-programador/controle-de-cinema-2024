using System.ComponentModel.DataAnnotations;

namespace ControleCinema.WebApp.Models;

public class InserirGeneroViewModel
{
    [Required(ErrorMessage = "A descrição é obrigatória")]
    [MinLength(6, ErrorMessage = "A descrição deve conter ao menos 6 caracteres")]
    public string Descricao { get; set; }
}

public class EditarGeneroViewModel
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "A descrição é obrigatória")]
    [MinLength(6, ErrorMessage = "A descrição deve conter ao menos 6 caracteres")]
    public string Descricao { get; set; }
}

public class ListarGeneroViewModel
{
    public int Id { get; set; }
    public string Descricao { get; set; }
}

public class DetalhesGeneroViewModel
{
    public int Id { get; set; }
    public string Descricao { get; set; }
}