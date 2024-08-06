using System.ComponentModel.DataAnnotations;

namespace ControleCinema.WebApp.Models;

public class InserirSalaViewModel
{
    [Required(ErrorMessage = "O número é obrigatório")]
    [Range(0, 1000, ErrorMessage = "Informe um número maior que zero")]
    public int Numero { get; set; }

    [Required(ErrorMessage = "A capacidade é obrigatória")]
    [Range(0, 1000, ErrorMessage = "Informe um número de capacidade maior que zero")]
    public int Capacidade { get; set; }
}

public class EditarSalaViewModel
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "O número é obrigatório")]
    [Range(0, 1000, ErrorMessage = "Informe um número maior que zero")]
    public int Numero { get; set; }

    [Required(ErrorMessage = "A capacidade é obrigatória")]
    [Range(0, 1000, ErrorMessage = "Informe um número de capacidade maior que zero")]
    public int Capacidade { get; set; }
}

public class ListarSalaViewModel
{
    public int Id { get; set; }
    public int Numero { get; set; }
    public int Capacidade { get; set; }
}

public class DetalhesSalaViewModel
{
    public int Id { get; set; }
    public int Numero { get; set; }
    public int Capacidade { get; set; }
}